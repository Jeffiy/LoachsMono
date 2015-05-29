﻿using System;
﻿using System.ComponentModel;
﻿using NewLife.Data;
﻿using XCode;
﻿using XCode.Membership;
﻿using StringHelper = Loachs.Common.StringHelper;

namespace Loachs.Entity
{
    /// <summary>评论</summary>
    public partial class Comments : LogEntity<Comments>
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
            
            if (isNew && !Dirtys[__.CreateDate]) CreateDate = DateTime.Now;
        }

        ///// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //protected override void InitData()
        //{
        //    base.InitData();

        //    // InitData一般用于当数据表没有数据时添加一些默认数据，该实体类的任何第一次数据库操作都会触发该方法，默认异步调用
        //    // Meta.Count是快速取得表记录数
        //    if (Meta.Count > 0) return;

        //    // 需要注意的是，如果该方法调用了其它实体类的首次数据库操作，目标实体类的数据初始化将会在同一个线程完成
        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}[{1}]数据……", typeof(Comments).Name, Meta.Table.DataTable.DisplayName);

        //    var entity = new Comments();
        //    entity.PostId = 0;
        //    entity.ParentId = 0;
        //    entity.UserId = 0;
        //    entity.Name = "abc";
        //    entity.Email = "abc";
        //    entity.SiteUrl = "abc";
        //    entity.Content = "abc";
        //    entity.IpAddress = "abc";
        //    entity.EmailNotify = 0;
        //    entity.Approved = 0;
        //    entity.CreateDate = DateTime.Now;
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}[{1}]数据！", typeof(Comments).Name, Meta.Table.DataTable.DisplayName);
        //}


        /// <summary>已重载。基类先调用Valid(true)验证数据，然后在事务保护内调用OnInsert</summary>
        /// <returns></returns>
        public override Int32 Insert()
        {
            //统计
            Sites.UpdateStatisticsCommentCount(1);

            //用户
            Users.UpdateUserCommentCount(UserId, 1);

            //文章
            Posts.UpdatePostCommentCount(PostId, 1);

            return base.Insert();
        }

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

        /// <summary>根据id查找</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Comments FindById(Int32 id)
        {
            if (Meta.Count >= 1000)
                return Find(_.Id, id);
            else // 实体缓存
                return Meta.Cache.Entities.Find(__.Id, id);
            // 单对象缓存
            //return Meta.SingleCache[id];
        }

        /// <summary>根据PostId查找</summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<Comments> FindAllByPostId(Int32 postId)
        {
            if (Meta.Count >= 1000)
                return FindAll(_.PostId, postId);
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(__.PostId, postId);
            // 单对象缓存
            //return Meta.SingleCache[id];
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
        public static EntityList<Comments> Search(Int32 userid, DateTime start, DateTime end, String key, PageParameter param)
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
        /// <summary>根据CommentId删除</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Int32 DeleteById(Int32 id)
        {
            if (id <= 0)
            {
                return 0;
            }

            Comments comment = FindById(id);


            if (comment != null)
            {
                //统计
                Sites.UpdateStatisticsCommentCount(-1);
                //用户
                Users.UpdateUserCommentCount(comment.UserId, -1);
                //文章
                Posts.UpdatePostCommentCount(comment.PostId, -1);
                
                return comment.Delete();
            }

            return 0;
        }

        /// <summary>根据PostId删除</summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Int32 DeleteByPostId(Int32 postId)
        {
            if (postId <= 0)
            {
                return 0;
            }

            EntityList<Comments> comments = FindAllByPostId(postId);

            int result = comments.Delete();

            Sites.UpdateStatisticsCommentCount(-comments.Count);

            return result;
        }

        /// <summary>
        ///     统计评论数
        /// </summary>
        /// <param name="incChild"></param>
        /// <returns></returns>
        public static int GetCommentCount(bool incChild)
        {
            return GetCommentCount(-1, incChild);
        }

        /// <summary>
        ///     统计评论数
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="incChild"></param>
        /// <returns></returns>
        public static int GetCommentCount(int postId, bool incChild)
        {
            return GetCommentCount(-1, postId, incChild);
        }

        /// <summary>
        ///     获取评论数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        /// <param name="incChild"></param>
        /// <returns></returns>
        public static int GetCommentCount(int userId, int postId, bool incChild)
        {
            var exp = new WhereExpression();

            if (userId > 0) exp &= _.UserId == userId;
            if (postId > 0) exp &= _.PostId == postId;
            if (incChild == false) exp &= _.ParentId == 0;

            return FindCount(exp);
        }
        
        /// <summary>
        ///     获取评论
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="order"></param>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        /// <param name="parentId"></param>
        /// <param name="approved"></param>
        /// <param name="emailNotify"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static EntityList<Comments> GetCommentList(int rowCount, int order, int userId, int postId, int parentId,
            int approved, int emailNotify, string keyword)
        {
            int totalRecord = 0;
            return GetCommentList(rowCount, 1, out totalRecord, order, userId, postId, parentId, approved, emailNotify,
                keyword);
        }

        /// <summary>
        ///     最近评论
        /// </summary>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static EntityList<Comments> GetCommentListByRecent(int rowCount)
        {
            return FindAll(null, null, null, 0, rowCount);
        }

        /// <summary>
        ///     获取评论
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <param name="order"></param>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        /// <param name="parentId"></param>
        /// <param name="approved"></param>
        /// <param name="emailNotify"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static EntityList<Comments> GetCommentList(int pageSize, int pageIndex, out int totalRecord, int order,
            int userId, int postId, int parentId, int approved, int emailNotify, string keyword)
        {
            var exp = new WhereExpression();
            if (userId != -1) exp &= _.UserId == userId;
            if (postId != -1) exp &= _.PostId == postId;
            if (parentId != -1) exp &= _.ParentId == parentId;
            if (approved != -1) exp &= _.Approved == approved;
            if (emailNotify != -1) exp &= _.EmailNotify == emailNotify;
            if (!string.IsNullOrEmpty(keyword)) exp &= SearchWhereByKey(keyword);

            var pageParameter = new PageParameter { PageIndex = pageIndex, PageSize = pageSize, Desc = order == 1, Sort = "Id"};
            EntityList<Comments> list = FindAll(exp, pageParameter);
            totalRecord = pageParameter.TotalCount;

            int floor = 1;
            foreach (Comments comment in list)
            {
                comment.Floor = pageSize * (pageIndex - 1) + floor;
                floor++;
            }
            return list;
        }
        #endregion

        #region 非字段

        /// <summary>
        ///     标题(实为过滤html的正文某长度文字)
        /// </summary>
        public string Title
        {
            get { return StringHelper.CutString(StringHelper.RemoveHtml(Content), 20, ""); }
        }


        /// <summary>
        ///     URl地址
        /// </summary>
        /// <remarks>
        ///     desc:考虑分页，总是跳到最后一页，需改进
        ///     date:2012.7
        /// </remarks>
        public string Url
        {
            get
            {
                Posts post = Posts.FindById(PostId);
                if (post != null)
                {
                    return string.Format("{0}#comment-{1}", post.Url, Id);
                }
                return "###";
            }
        }

        /// <summary>
        ///     评论连接
        /// </summary>
        public string Link
        {
            get { return string.Format("<a href=\"{0}\" title=\"{1} 发表于 {2}\">{3}</a>", Url, Name, CreateDate, Title); }
        }


        /// <summary>
        ///     作者连接
        /// </summary>
        public string AuthorLink
        {
            get
            {
                if (UserId > 0)
                {
                    Users user = Users.FindById(UserId);
                    if (user != null)
                    {
                        return user.Link;
                    }
                }
                else if (StringHelper.IsHttpUrl(SiteUrl))
                {
                    return string.Format("<a href=\"{0}\" target=\"_blank\" title=\"{1}\">{1}</a>", SiteUrl, Name);
                }
                return Name;
            }
        }

        /// <summary>
        ///     层次
        /// </summary>
        public int Floor { set; get; }

        /// <summary>
        ///     Gravatar 加密后的字符串
        /// </summary>
        public string GravatarCode
        {
            get { return Email.MD5().ToLower(); }
        }

        /// <summary>
        ///     评论对应文章
        /// </summary>
        public Posts Post
        {
            get
            {
                Posts post = Posts.FindById(PostId);
                if (post != null)
                {
                    return post;
                }
                return new Posts();
            }
        }

        #endregion
    }
}