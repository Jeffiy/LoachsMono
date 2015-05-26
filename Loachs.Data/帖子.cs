﻿using System;
﻿using System.ComponentModel;
﻿using XCode;
﻿using XCode.Configuration;
﻿using XCode.DataAccessLayer;

namespace Loachs.Data
{
    /// <summary>帖子</summary>
    [Serializable]
    [DataObject]
    [Description("帖子")]
    [BindIndex("PK_postsid", true, "id")]
    [BindIndex("FK_categoryid", false, "categoryid")]
    [BindIndex("FK_userid", false, "userid")]
    [BindRelation("CategoryId", false, "Categorys", "ID")]
    [BindRelation("UserId", false, "Users", "ID")]
    [BindTable("Posts", Description = "帖子", ConnName = "loachsConn", DbType = DatabaseType.SQLite)]
    public partial class Posts : IPosts
    {
        #region 属性
        private Int32 _Id;
        /// <summary>ID</summary>
        [DisplayName("ID")]
        [Description("ID")]
        [DataObjectField(true, true, false, 10)]
        [BindColumn(1, "id", "ID", null, "int", 10, 0, false)]
        public virtual Int32 Id
        {
            get { return _Id; }
            set { if (OnPropertyChanging(__.Id, value)) { _Id = value; OnPropertyChanged(__.Id); } }
        }

        private Int32 _CategoryId;
        /// <summary>分类ID</summary>
        [DisplayName("分类ID")]
        [Description("分类ID")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "categoryid", "分类ID", null, "int", 10, 0, false)]
        public virtual Int32 CategoryId
        {
            get { return _CategoryId; }
            set { if (OnPropertyChanging(__.CategoryId, value)) { _CategoryId = value; OnPropertyChanged(__.CategoryId); } }
        }

        private String _Title;
        /// <summary>标题</summary>
        [DisplayName("标题")]
        [Description("标题")]
        [DataObjectField(false, false, true, 255)]
        [BindColumn(3, "title", "标题", null, "nvarchar(255)", 0, 0, true, Master=true)]
        public virtual String Title
        {
            get { return _Title; }
            set { if (OnPropertyChanging(__.Title, value)) { _Title = value; OnPropertyChanged(__.Title); } }
        }

        private String _Slug;
        /// <summary>重复</summary>
        [DisplayName("重复")]
        [Description("重复")]
        [DataObjectField(false, false, true, 255)]
        [BindColumn(4, "slug", "重复", null, "nvarchar(255)", 0, 0, true)]
        public virtual String Slug
        {
            get { return _Slug; }
            set { if (OnPropertyChanging(__.Slug, value)) { _Slug = value; OnPropertyChanged(__.Slug); } }
        }

        private String _ImageUrl;
        /// <summary>图片地址</summary>
        [DisplayName("图片地址")]
        [Description("图片地址")]
        [DataObjectField(false, false, true, 255)]
        [BindColumn(5, "imageurl", "图片地址", null, "nvarchar(255)", 0, 0, true)]
        public virtual String ImageUrl
        {
            get { return _ImageUrl; }
            set { if (OnPropertyChanging(__.ImageUrl, value)) { _ImageUrl = value; OnPropertyChanged(__.ImageUrl); } }
        }

        private String _Summary;
        /// <summary>摘要</summary>
        [DisplayName("摘要")]
        [Description("摘要")]
        [DataObjectField(false, false, true, 2147483647)]
        [BindColumn(6, "summary", "摘要", null, "nvarchar(2147483647)", 0, 0, true)]
        public virtual String Summary
        {
            get { return _Summary; }
            set { if (OnPropertyChanging(__.Summary, value)) { _Summary = value; OnPropertyChanged(__.Summary); } }
        }

        private String _Content;
        /// <summary>内容</summary>
        [DisplayName("内容")]
        [Description("内容")]
        [DataObjectField(false, false, true, 2147483647)]
        [BindColumn(7, "content", "内容", null, "nvarchar(2147483647)", 0, 0, true)]
        public virtual String Content
        {
            get { return _Content; }
            set { if (OnPropertyChanging(__.Content, value)) { _Content = value; OnPropertyChanged(__.Content); } }
        }

        private Int32 _UserId;
        /// <summary>用户ID</summary>
        [DisplayName("用户ID")]
        [Description("用户ID")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(10, "userid", "用户ID", null, "int", 10, 0, false)]
        public virtual Int32 UserId
        {
            get { return _UserId; }
            set { if (OnPropertyChanging(__.UserId, value)) { _UserId = value; OnPropertyChanged(__.UserId); } }
        }

        private Int32 _CommentStatus;
        /// <summary>评论状态</summary>
        [DisplayName("评论状态")]
        [Description("评论状态")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(9, "commentstatus", "评论状态", null, "int", 10, 0, false)]
        public virtual Int32 CommentStatus
        {
            get { return _CommentStatus; }
            set { if (OnPropertyChanging(__.CommentStatus, value)) { _CommentStatus = value; OnPropertyChanged(__.CommentStatus); } }
        }

        private Int32 _CommentCount;
        /// <summary>评论总数</summary>
        [DisplayName("评论总数")]
        [Description("评论总数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(10, "commentcount", "评论总数", null, "int", 10, 0, false)]
        public virtual Int32 CommentCount
        {
            get { return _CommentCount; }
            set { if (OnPropertyChanging(__.CommentCount, value)) { _CommentCount = value; OnPropertyChanged(__.CommentCount); } }
        }

        private Int32 _ViewCount;
        /// <summary>浏览总数</summary>
        [DisplayName("浏览总数")]
        [Description("浏览总数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(11, "viewcount", "浏览总数", null, "int", 10, 0, false)]
        public virtual Int32 ViewCount
        {
            get { return _ViewCount; }
            set { if (OnPropertyChanging(__.ViewCount, value)) { _ViewCount = value; OnPropertyChanged(__.ViewCount); } }
        }

        private String _Tag;
        /// <summary>标签</summary>
        [DisplayName("标签")]
        [Description("标签")]
        [DataObjectField(false, false, true, 255)]
        [BindColumn(12, "tag", "标签", null, "nvarchar(255)", 0, 0, true)]
        public virtual String Tag
        {
            get { return _Tag; }
            set { if (OnPropertyChanging(__.Tag, value)) { _Tag = value; OnPropertyChanged(__.Tag); } }
        }

        private Int32 _UrlFormat;
        /// <summary>Url格式</summary>
        [DisplayName("Url格式")]
        [Description("Url格式")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(13, "urlformat", "Url格式", null, "int", 10, 0, false)]
        public virtual Int32 UrlFormat
        {
            get { return _UrlFormat; }
            set { if (OnPropertyChanging(__.UrlFormat, value)) { _UrlFormat = value; OnPropertyChanged(__.UrlFormat); } }
        }

        private String _Template;
        /// <summary>模板</summary>
        [DisplayName("模板")]
        [Description("模板")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(14, "template", "模板", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Template
        {
            get { return _Template; }
            set { if (OnPropertyChanging(__.Template, value)) { _Template = value; OnPropertyChanged(__.Template); } }
        }

        private Int32 _Recommend;
        /// <summary>推荐</summary>
        [DisplayName("推荐")]
        [Description("推荐")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(15, "recommend", "推荐", null, "int", 10, 0, false)]
        public virtual Int32 Recommend
        {
            get { return _Recommend; }
            set { if (OnPropertyChanging(__.Recommend, value)) { _Recommend = value; OnPropertyChanged(__.Recommend); } }
        }

        private Int32 _Status;
        /// <summary>状态</summary>
        [DisplayName("状态")]
        [Description("状态")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(16, "status", "状态", null, "int", 10, 0, false)]
        public virtual Int32 Status
        {
            get { return _Status; }
            set { if (OnPropertyChanging(__.Status, value)) { _Status = value; OnPropertyChanged(__.Status); } }
        }

        private Int32 _TopStatus;
        /// <summary>是否置顶</summary>
        [DisplayName("是否置顶")]
        [Description("是否置顶")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(17, "topstatus", "是否置顶", null, "int", 10, 0, false)]
        public virtual Int32 TopStatus
        {
            get { return _TopStatus; }
            set { if (OnPropertyChanging(__.TopStatus, value)) { _TopStatus = value; OnPropertyChanged(__.TopStatus); } }
        }

        private Int32 _HideStatus;
        /// <summary>是否隐藏</summary>
        [DisplayName("是否隐藏")]
        [Description("是否隐藏")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(18, "hidestatus", "是否隐藏", null, "int", 10, 0, false)]
        public virtual Int32 HideStatus
        {
            get { return _HideStatus; }
            set { if (OnPropertyChanging(__.HideStatus, value)) { _HideStatus = value; OnPropertyChanged(__.HideStatus); } }
        }

        private DateTime _CreateDate;
        /// <summary>创建日期</summary>
        [DisplayName("创建日期")]
        [Description("创建日期")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(10, "createdate", "创建日期", null, "datetime", 0, 0, false)]
        public virtual DateTime CreateDate
        {
            get { return _CreateDate; }
            set { if (OnPropertyChanging(__.CreateDate, value)) { _CreateDate = value; OnPropertyChanged(__.CreateDate); } }
        }

        private DateTime _UpdateDate;
        /// <summary>更新日期</summary>
        [DisplayName("更新日期")]
        [Description("更新日期")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(20, "updatedate", "更新日期", null, "datetime", 0, 0, false)]
        public virtual DateTime UpdateDate
        {
            get { return _UpdateDate; }
            set { if (OnPropertyChanging(__.UpdateDate, value)) { _UpdateDate = value; OnPropertyChanged(__.UpdateDate); } }
        }
        #endregion

        #region 获取/设置 字段值
        /// <summary>
        /// 获取/设置 字段值。
        /// 一个索引，基类使用反射实现。
        /// 派生实体类可重写该索引，以避免反射带来的性能损耗
        /// </summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        public override Object this[String name]
        {
            get
            {
                switch (name)
                {
                    case __.Id : return _Id;
                    case __.CategoryId : return _CategoryId;
                    case __.Title : return _Title;
                    case __.Slug : return _Slug;
                    case __.ImageUrl : return _ImageUrl;
                    case __.Summary : return _Summary;
                    case __.Content : return _Content;
                    case __.UserId : return _UserId;
                    case __.CommentStatus : return _CommentStatus;
                    case __.CommentCount : return _CommentCount;
                    case __.ViewCount : return _ViewCount;
                    case __.Tag : return _Tag;
                    case __.UrlFormat : return _UrlFormat;
                    case __.Template : return _Template;
                    case __.Recommend : return _Recommend;
                    case __.Status : return _Status;
                    case __.TopStatus : return _TopStatus;
                    case __.HideStatus : return _HideStatus;
                    case __.CreateDate : return _CreateDate;
                    case __.UpdateDate : return _UpdateDate;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.Id : _Id = Convert.ToInt32(value); break;
                    case __.CategoryId : _CategoryId = Convert.ToInt32(value); break;
                    case __.Title : _Title = Convert.ToString(value); break;
                    case __.Slug : _Slug = Convert.ToString(value); break;
                    case __.ImageUrl : _ImageUrl = Convert.ToString(value); break;
                    case __.Summary : _Summary = Convert.ToString(value); break;
                    case __.Content : _Content = Convert.ToString(value); break;
                    case __.UserId : _UserId = Convert.ToInt32(value); break;
                    case __.CommentStatus : _CommentStatus = Convert.ToInt32(value); break;
                    case __.CommentCount : _CommentCount = Convert.ToInt32(value); break;
                    case __.ViewCount : _ViewCount = Convert.ToInt32(value); break;
                    case __.Tag : _Tag = Convert.ToString(value); break;
                    case __.UrlFormat : _UrlFormat = Convert.ToInt32(value); break;
                    case __.Template : _Template = Convert.ToString(value); break;
                    case __.Recommend : _Recommend = Convert.ToInt32(value); break;
                    case __.Status : _Status = Convert.ToInt32(value); break;
                    case __.TopStatus : _TopStatus = Convert.ToInt32(value); break;
                    case __.HideStatus : _HideStatus = Convert.ToInt32(value); break;
                    case __.CreateDate : _CreateDate = Convert.ToDateTime(value); break;
                    case __.UpdateDate : _UpdateDate = Convert.ToDateTime(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得帖子字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>ID</summary>
            public static readonly Field Id = FindByName(__.Id);

            ///<summary>分类ID</summary>
            public static readonly Field CategoryId = FindByName(__.CategoryId);

            ///<summary>标题</summary>
            public static readonly Field Title = FindByName(__.Title);

            ///<summary>重复</summary>
            public static readonly Field Slug = FindByName(__.Slug);

            ///<summary>图片地址</summary>
            public static readonly Field ImageUrl = FindByName(__.ImageUrl);

            ///<summary>摘要</summary>
            public static readonly Field Summary = FindByName(__.Summary);

            ///<summary>内容</summary>
            public static readonly Field Content = FindByName(__.Content);

            ///<summary>用户ID</summary>
            public static readonly Field UserId = FindByName(__.UserId);

            ///<summary>评论状态</summary>
            public static readonly Field CommentStatus = FindByName(__.CommentStatus);

            ///<summary>评论总数</summary>
            public static readonly Field CommentCount = FindByName(__.CommentCount);

            ///<summary>浏览总数</summary>
            public static readonly Field ViewCount = FindByName(__.ViewCount);

            ///<summary>标签</summary>
            public static readonly Field Tag = FindByName(__.Tag);

            ///<summary>Url格式</summary>
            public static readonly Field UrlFormat = FindByName(__.UrlFormat);

            ///<summary>模板</summary>
            public static readonly Field Template = FindByName(__.Template);

            ///<summary>推荐</summary>
            public static readonly Field Recommend = FindByName(__.Recommend);

            ///<summary>状态</summary>
            public static readonly Field Status = FindByName(__.Status);

            ///<summary>是否置顶</summary>
            public static readonly Field TopStatus = FindByName(__.TopStatus);

            ///<summary>是否隐藏</summary>
            public static readonly Field HideStatus = FindByName(__.HideStatus);

            ///<summary>创建日期</summary>
            public static readonly Field CreateDate = FindByName(__.CreateDate);

            ///<summary>更新日期</summary>
            public static readonly Field UpdateDate = FindByName(__.UpdateDate);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得帖子字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>ID</summary>
            public const String Id = "Id";

            ///<summary>分类ID</summary>
            public const String CategoryId = "CategoryId";

            ///<summary>标题</summary>
            public const String Title = "Title";

            ///<summary>重复</summary>
            public const String Slug = "Slug";

            ///<summary>图片地址</summary>
            public const String ImageUrl = "ImageUrl";

            ///<summary>摘要</summary>
            public const String Summary = "Summary";

            ///<summary>内容</summary>
            public const String Content = "Content";

            ///<summary>用户ID</summary>
            public const String UserId = "UserId";

            ///<summary>评论状态</summary>
            public const String CommentStatus = "CommentStatus";

            ///<summary>评论总数</summary>
            public const String CommentCount = "CommentCount";

            ///<summary>浏览总数</summary>
            public const String ViewCount = "ViewCount";

            ///<summary>标签</summary>
            public const String Tag = "Tag";

            ///<summary>Url格式</summary>
            public const String UrlFormat = "UrlFormat";

            ///<summary>模板</summary>
            public const String Template = "Template";

            ///<summary>推荐</summary>
            public const String Recommend = "Recommend";

            ///<summary>状态</summary>
            public const String Status = "Status";

            ///<summary>是否置顶</summary>
            public const String TopStatus = "TopStatus";

            ///<summary>是否隐藏</summary>
            public const String HideStatus = "HideStatus";

            ///<summary>创建日期</summary>
            public const String CreateDate = "CreateDate";

            ///<summary>更新日期</summary>
            public const String UpdateDate = "UpdateDate";

        }
        #endregion
    }

    /// <summary>帖子接口</summary>
    public partial interface IPosts
    {
        #region 属性
        /// <summary>ID</summary>
        Int32 Id { get; set; }

        /// <summary>分类ID</summary>
        Int32 CategoryId { get; set; }

        /// <summary>标题</summary>
        String Title { get; set; }

        /// <summary>重复</summary>
        String Slug { get; set; }

        /// <summary>图片地址</summary>
        String ImageUrl { get; set; }

        /// <summary>摘要</summary>
        String Summary { get; set; }

        /// <summary>内容</summary>
        String Content { get; set; }

        /// <summary>用户ID</summary>
        Int32 UserId { get; set; }

        /// <summary>评论状态</summary>
        Int32 CommentStatus { get; set; }

        /// <summary>评论总数</summary>
        Int32 CommentCount { get; set; }

        /// <summary>浏览总数</summary>
        Int32 ViewCount { get; set; }

        /// <summary>标签</summary>
        String Tag { get; set; }

        /// <summary>Url格式</summary>
        Int32 UrlFormat { get; set; }

        /// <summary>模板</summary>
        String Template { get; set; }

        /// <summary>推荐</summary>
        Int32 Recommend { get; set; }

        /// <summary>状态</summary>
        Int32 Status { get; set; }

        /// <summary>是否置顶</summary>
        Int32 TopStatus { get; set; }

        /// <summary>是否隐藏</summary>
        Int32 HideStatus { get; set; }

        /// <summary>创建日期</summary>
        DateTime CreateDate { get; set; }

        /// <summary>更新日期</summary>
        DateTime UpdateDate { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}