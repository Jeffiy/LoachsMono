using System;
using System.ComponentModel;
using System.Linq;
using Loachs.Common;
using NewLife.Data;
using NewLife.Log;
using XCode;
using StringHelper = Loachs.Common.StringHelper;

namespace Loachs.Entity
{
    /// <summary>标签</summary>
    public class Tags : Terms<Tags>
    {
        #region 对象操作﻿

        /// <summary>验证数据，通过抛出异常的方式提示验证失败。</summary>
        /// <param name="isNew"></param>
        public override void Valid(Boolean isNew)
        {
            // 如果没有脏数据，则不需要进行任何处理
            if (!HasDirty) return;

            // 这里验证参数范围，建议抛出参数异常，指定参数名，前端用户界面可以捕获参数异常并聚焦到对应的参数输入框
            //if (String.IsNullOrEmpty(Name)) throw new ArgumentNullException(_.Name, _.Name.DisplayName + "无效！");
            //if (!isNew && ID < 1) throw new ArgumentOutOfRangeException(_.ID, _.ID.DisplayName + "必须大于0！");

            // 建议先调用基类方法，基类方法会对唯一索引的数据进行验证
            base.Valid(isNew);

            // 在新插入数据或者修改了指定字段时进行唯一性验证，CheckExist内部抛出参数异常
//            if (isNew || Dirtys[__.Slug]) CheckExist(__.Slug);
            if (isNew && !Dirtys[__.CreateDate]) CreateDate = DateTime.Now;
            if (HasDirty) Type = (int)TermType.Tag;
        }

        /// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void InitData()
        {
            base.InitData();

            // InitData一般用于当数据表没有数据时添加一些默认数据，该实体类的任何第一次数据库操作都会触发该方法，默认异步调用
            // Meta.Count是快速取得表记录数
            if (Meta.Count > 0) return;

            // 需要注意的是，如果该方法调用了其它实体类的首次数据库操作，目标实体类的数据初始化将会在同一个线程完成
            if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}[{1}]数据……", typeof(Categorys).Name, Meta.Table.DataTable.DisplayName);

            Tags t = new Tags
            {
                Name = "默认标签",
                Type = (int)TermType.Tag,
                CreateDate = DateTime.Now,
                Description = "这是系统自动添加的默认标签",
                Slug = "default",
                DisplayOrder = 1000,
                Count = 0
            };

            t.Insert();

            if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}[{1}]数据！", typeof(Categorys).Name, Meta.Table.DataTable.DisplayName);
        }

//        /// <summary>已重载。基类先调用Valid(true)验证数据，然后在事务保护内调用OnInsert</summary>
//        /// <returns></returns>
//        public override Int32 Insert()
//        {
//            CheckSlug();
//            return base.Insert();
//        }

        ///// <summary>已重载。在事务保护范围内处理业务，位于Valid之后</summary>
        ///// <returns></returns>
        //protected override Int32 OnInsert()
        //{
        //    return base.OnInsert();
        //}
        #endregion

        #region 扩展属性﻿
        #endregion

        #region 扩展查询﻿

        /// <summary>
        /// Finds all tags.
        /// </summary>
        public new static EntityList<Tags> FindAll()
        {
            return FindAll(__.Type, (int)TermType.Tag);
        }

        /// <summary>
        /// Finds all tags with cache.
        /// </summary>
        public new static EntityList<Tags> FindAllWithCache()
        {
            return FindAllWithCache(__.Type, (int)TermType.Tag);
        }

        /// <summary>
        /// Finds the Tags count.
        /// </summary>
        public new static int FindCount()
        {
            return FindCount(__.Type, (int)TermType.Tag);
        }
        #endregion

        #region 高级查询
        // 以下为自定义高级查询的例子

        /*/// <summary>查询满足条件的记录集，分页、排序</summary>
        /// <param name="userid">用户编号</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="key">关键字</param>
        /// <param name="param">分页排序参数，同时返回满足条件的总记录数</param>
        /// <returns>实体集</returns>
        public static EntityList<Categorys> Search(Int32 userid, DateTime start, DateTime end, String key, PageParameter param)
        {
            // WhereExpression重载&和|运算符，作为And和Or的替代
            // SearchWhereByKeys系列方法用于构建针对字符串字段的模糊搜索，第二个参数可指定要搜索的字段
            var exp = SearchWhereByKeys(key, null, null);

            // 以下仅为演示，Field（继承自FieldItem）重载了==、!=、>、<、>=、<=等运算符
            //if (userid > 0) exp &= _.OperatorID == userid;
            //if (isSign != null) exp &= _.IsSign == isSign.Value;
            //exp &= _.OccurTime.Between(start, end); // 大于等于start，小于end，当start/end大于MinValue时有效

            return FindAll(exp, param);
        }*/
        #endregion

        #region 扩展操作
        #endregion

        #region 业务

        /// <summary>
        ///     检查别名是否重复
        /// </summary>
        /// <returns></returns>
        private bool CheckSlug()
        {
            while (true)
            {
                var exp = new WhereExpression();
                if (Id == 0)
                {
                    exp &= _.Slug == Slug;
                    exp &= _.Type == (int) TermType.Tag;
                }
                else
                {
                    exp &= _.Id != Id;
                    exp &= _.Type == (int)TermType.Tag;
                }
                int r = FindCount(exp);
                if (r == 0)
                {
                    return true;
                }
                Slug += "-2";
            }
        }

        /// <summary>
        ///     获取指定条数标签
        /// </summary>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static EntityList<Tags> GetTagList(int rowCount)
        {
            return FindAll(_.Type == (int)TermType.Tag, null, null, 0, rowCount);
        }

        /// <summary>
        ///     获取分页标签
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static EntityList<Tags> GetTagList(int pageSize, int pageIndex, out int recordCount)
        {
            var exp = new WhereExpression();
            exp &= _.Type == (int) TermType.Tag;
            var pageParameter = new PageParameter {PageIndex = pageIndex, PageSize = pageSize};

            var list = FindAll(exp, pageParameter);
            recordCount = pageParameter.TotalCount;
            return list;
        }

        /// <summary>
        ///     根据标签Id获取标签列表
        /// </summary>
        /// <param name="ids">标签Id,逗号隔开</param>
        /// <returns></returns>
        public static EntityList<Tags> GetTagList(string ids)
        {
            string[] idArray = ids.Split(',');

            return FindAll(_.Type == (int)TermType.Tag & _.Id.In(idArray), null);
        }

        /// <summary>
        ///     更新标签对应文章数
        /// </summary>
        /// <param name="tagids">格式:{2}{26}</param>
        /// <param name="addCount"></param>
        /// <returns></returns>
        public static bool UpdateTagUseCount(string tagids, int addCount)
        {
            if (string.IsNullOrEmpty(tagids))
            {
                return false;
            }

            string[] tagidlist = tagids.Replace("{", "").Split('}');

            foreach (Tags tag in tagidlist.Select(tagId => FindById(StringHelper.StrToInt(tagId))).Where(tag => tag != null))
            {
                tag.Count += addCount;
                tag.Save();
            }
            return true;
        }
        #endregion

        #region 非字段

        /// <summary>
        ///     地址
        /// </summary>
        public string Url
        {
            get
            {
                string url = string.Empty;

                if (Utils.IsSupportUrlRewriter == false)
                {
                    url = string.Format("{0}default.aspx?type=tag&slug={1}", ConfigHelper.SiteUrl,
                        StringHelper.UrlEncode(Slug));
                }
                else
                {
                    url = string.Format("{0}tag/{1}{2}", ConfigHelper.SiteUrl, StringHelper.UrlEncode(Slug),
                        Sites.GetSetting().RewriteExtension);
                }
                return Utils.CheckPreviewThemeUrl(url);
            }
        }

        /// <summary>
        ///     连接
        /// </summary>
        public string Link
        {
            get { return string.Format("<a href=\"{0}\" title=\"标签:{1}\">{2}</a>", Url, Name, Name); }
        }

        #endregion
    }
}
