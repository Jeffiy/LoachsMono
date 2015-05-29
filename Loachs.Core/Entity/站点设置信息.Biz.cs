﻿using System;
﻿using System.ComponentModel;
﻿using System.IO;
﻿using System.Text;
﻿using System.Xml;
﻿using System.Xml.Serialization;
﻿using NewLife.Data;
﻿using NewLife.Log;
﻿using XCode;
﻿using XCode.Membership;

namespace Loachs.Entity
{
    /// <summary>站点设置信息</summary>
    public partial class Sites : LogEntity<Sites>
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
            //if (isNew || Dirtys[__.Name]) CheckExist(__.Name);
            
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
            if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}[{1}]数据……", typeof(Sites).Name, Meta.Table.DataTable.DisplayName);

            var entity = new Sites
            {
                PostCount = 0,
                CommentCount = 0,
                VisitCount = 0,
                TagCount = 0,
                Setting = "<?xml version=\"1.0\" encoding=\"utf-8\"?><SettingInfo xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"></SettingInfo>"
            };
            entity.Insert();

            if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}[{1}]数据！", typeof(Sites).Name, Meta.Table.DataTable.DisplayName);
        }


        ///// <summary>已重载。基类先调用Valid(true)验证数据，然后在事务保护内调用OnInsert</summary>
        ///// <returns></returns>
        //public override Int32 Insert()
        //{
        //    return base.Insert();
        //}

        ///// <summary>已重载。在事务保护范围内处理业务，位于Valid之后</summary>
        ///// <returns></returns>
        //protected override Int32 OnInsert()
        //{
        //    return base.OnInsert();
        //}
        #endregion

        #region 扩展属性
        /// <summary>
        /// Gets the site info.
        /// </summary>
        [XmlIgnore]
        public static Sites SiteInfo
        {
            get
            {
                var sites = FindAll(null, null, null, 0, 1);
                return sites[0];
            }
        }

        #endregion

        #region 扩展查询﻿
        /// <summary>根据Siteid查找</summary>
        /// <param name="siteid"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Sites FindBySiteid(Int32 siteid)
        {
            if (Meta.Count >= 1000)
                return Find(_.Id, siteid);
            else // 实体缓存
                return Meta.Cache.Entities.Find(__.Id, siteid);
            // 单对象缓存
            //return Meta.SingleCache[siteid];
        }
        #endregion

        #region 高级查询
        // 以下为自定义高级查询的例子

        /// <summary>查询满足条件的记录集，分页、排序</summary>
        /// <param name="userid">用户编号</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="key">关键字</param>
        /// <param name="param">分页排序参数，同时返回满足条件的总记录数</param>
        /// <returns>实体集</returns>
        public static EntityList<Sites> Search(Int32 userid, DateTime start, DateTime end, String key, PageParameter param)
        {
            // WhereExpression重载&和|运算符，作为And和Or的替代
            // SearchWhereByKeys系列方法用于构建针对字符串字段的模糊搜索，第二个参数可指定要搜索的字段
            var exp = SearchWhereByKeys(key, null, null);

            // 以下仅为演示，Field（继承自FieldItem）重载了==、!=、>、<、>=、<=等运算符
            //if (userid > 0) exp &= _.OperatorID == userid;
            //if (isSign != null) exp &= _.IsSign == isSign.Value;
            //exp &= _.OccurTime.Between(start, end); // 大于等于start，小于end，当start/end大于MinValue时有效

            return FindAll(exp, param);
        }
        #endregion

        #region 扩展操作
        #endregion

        #region 业务
        /// <summary>
        /// 更新站点配置信息
        /// </summary>
        public static int UpdateSetting(SettingInfo setting)
        {
            SiteInfo.Setting = Serialize(setting);
            return SiteInfo.Save();
        }

        /// <summary>
        ///     获取站点配置信息
        /// </summary>
        /// <returns></returns>
        public static SettingInfo GetSetting()
        {
            object obj = DeSerialize(typeof(SettingInfo), SiteInfo.Setting);
            if (obj == null)
            {
                return new SettingInfo();
            }

            return (SettingInfo)obj;
        }

        /// <summary>
        ///     更新文章数
        /// </summary>
        /// <param name="addCount">增加数，可为负数</param>
        /// <returns></returns>
        public static int UpdateStatisticsPostCount(int addCount)
        {
            SiteInfo.PostCount += addCount;
            return SiteInfo.Save();
        }

        /// <summary>
        ///     更新评论数
        /// </summary>
        /// <param name="addCount">增加数，可为负数</param>
        /// <returns></returns>
        public static int UpdateStatisticsCommentCount(int addCount)
        {
            SiteInfo.CommentCount += addCount;
            return SiteInfo.Save();
        }
        #endregion

        #region 序列化

        /// <summary>
        ///     xml序列化成字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>xml字符串</returns>
        public static string Serialize(object obj)
        {
            string returnStr = "";

            XmlSerializer serializer = new XmlSerializer(obj.GetType());


            MemoryStream ms = new MemoryStream();
            XmlTextWriter xtw = null;
            StreamReader sr = null;
            try
            {
                xtw = new XmlTextWriter(ms, Encoding.UTF8) {Formatting = Formatting.Indented};
                serializer.Serialize(xtw, obj);
                ms.Seek(0, SeekOrigin.Begin);
                sr = new StreamReader(ms);
                returnStr = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (xtw != null)
                    xtw.Close();
                if (sr != null)
                    sr.Close();
                ms.Close();
            }
            return returnStr;
        }

        /// <summary>
        ///     反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static object DeSerialize(Type type, string s)
        {
            byte[] b = Encoding.UTF8.GetBytes(s);
            try
            {
                XmlSerializer serializer = new XmlSerializer(type);

                return serializer.Deserialize(new MemoryStream(b));
            }
            catch
            {
                //  throw ex;
                return null;
            }
        }

        #endregion
    }
}