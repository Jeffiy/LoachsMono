﻿using System;
﻿using System.ComponentModel;
﻿using XCode;
﻿using XCode.Configuration;
﻿using XCode.DataAccessLayer;

namespace Loachs.Entity
{
    /// <summary>评论</summary>
    [Serializable]
    [DataObject]
    [Description("评论")]
    [BindIndex("PK_commentsid", true, "id")]
    [BindIndex("FK_postid", false, "postid")]
    [BindIndex("FK_userid", false, "userid")]
    [BindRelation("postid", false, "Post", "ID")]
    [BindRelation("userid", false, "User", "ID")]
    [BindTable("Comments", Description = "评论", ConnName = "loachsConn", DbType = DatabaseType.SQLite)]
    public partial class Comments : IComments
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

        private Int32 _PostId;
        /// <summary>日志ID</summary>
        [DisplayName("日志ID")]
        [Description("日志ID")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(2, "postid", "日志ID", null, "int", 10, 0, false)]
        public virtual Int32 PostId
        {
            get { return _PostId; }
            set { if (OnPropertyChanging(__.PostId, value)) { _PostId = value; OnPropertyChanged(__.PostId); } }
        }

        private Int32 _ParentId;
        /// <summary>ParentId</summary>
        [DisplayName("ParentId")]
        [Description("ParentId")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "parentid", "ParentId", null, "int", 10, 0, false)]
        public virtual Int32 ParentId
        {
            get { return _ParentId; }
            set { if (OnPropertyChanging(__.ParentId, value)) { _ParentId = value; OnPropertyChanged(__.ParentId); } }
        }

        private Int32 _UserId;
        /// <summary>用户ID</summary>
        [DisplayName("用户ID")]
        [Description("用户ID")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(4, "userid", "用户ID", null, "int", 10, 0, false)]
        public virtual Int32 UserId
        {
            get { return _UserId; }
            set { if (OnPropertyChanging(__.UserId, value)) { _UserId = value; OnPropertyChanged(__.UserId); } }
        }

        private String _Name;
        /// <summary>名称</summary>
        [DisplayName("名称")]
        [Description("名称")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(5, "name", "名称", null, "nvarchar(50)", 0, 0, true, Master=true)]
        public virtual String Name
        {
            get { return _Name; }
            set { if (OnPropertyChanging(__.Name, value)) { _Name = value; OnPropertyChanged(__.Name); } }
        }

        private String _Email;
        /// <summary>邮箱</summary>
        [DisplayName("邮箱")]
        [Description("邮箱")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(6, "email", "邮箱", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Email
        {
            get { return _Email; }
            set { if (OnPropertyChanging(__.Email, value)) { _Email = value; OnPropertyChanged(__.Email); } }
        }

        private String _SiteUrl;
        /// <summary>地址</summary>
        [DisplayName("地址")]
        [Description("地址")]
        [DataObjectField(false, false, true, 200)]
        [BindColumn(7, "siteurl", "地址", null, "nvarchar(200)", 0, 0, true)]
        public virtual String SiteUrl
        {
            get { return _SiteUrl; }
            set { if (OnPropertyChanging(__.SiteUrl, value)) { _SiteUrl = value; OnPropertyChanged(__.SiteUrl); } }
        }

        private String _Content;
        /// <summary>内容</summary>
        [DisplayName("内容")]
        [Description("内容")]
        [DataObjectField(false, false, true, 2147483647)]
        [BindColumn(10, "content", "内容", null, "nvarchar(2147483647)", 0, 0, true)]
        public virtual String Content
        {
            get { return _Content; }
            set { if (OnPropertyChanging(__.Content, value)) { _Content = value; OnPropertyChanged(__.Content); } }
        }

        private String _IpAddress;
        /// <summary>IP</summary>
        [DisplayName("IP")]
        [Description("IP")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(9, "ipaddress", "IP", null, "nvarchar(50)", 0, 0, true)]
        public virtual String IpAddress
        {
            get { return _IpAddress; }
            set { if (OnPropertyChanging(__.IpAddress, value)) { _IpAddress = value; OnPropertyChanged(__.IpAddress); } }
        }

        private Int32 _EmailNotify;
        /// <summary>邮件提醒</summary>
        [DisplayName("邮件提醒")]
        [Description("邮件提醒")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(10, "emailnotify", "邮件提醒", null, "int", 10, 0, false)]
        public virtual Int32 EmailNotify
        {
            get { return _EmailNotify; }
            set { if (OnPropertyChanging(__.EmailNotify, value)) { _EmailNotify = value; OnPropertyChanged(__.EmailNotify); } }
        }

        private Int32 _Approved;
        /// <summary>是否审核</summary>
        [DisplayName("是否审核")]
        [Description("是否审核")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(11, "approved", "是否审核", null, "int", 10, 0, false)]
        public virtual Int32 Approved
        {
            get { return _Approved; }
            set { if (OnPropertyChanging(__.Approved, value)) { _Approved = value; OnPropertyChanged(__.Approved); } }
        }

        private DateTime _CreateDate;
        /// <summary>创建日期</summary>
        [DisplayName("创建日期")]
        [Description("创建日期")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(12, "createdate", "创建日期", null, "datetime", 0, 0, false)]
        public virtual DateTime CreateDate
        {
            get { return _CreateDate; }
            set { if (OnPropertyChanging(__.CreateDate, value)) { _CreateDate = value; OnPropertyChanged(__.CreateDate); } }
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
                    case __.PostId : return _PostId;
                    case __.ParentId : return _ParentId;
                    case __.UserId : return _UserId;
                    case __.Name : return _Name;
                    case __.Email : return _Email;
                    case __.SiteUrl : return _SiteUrl;
                    case __.Content : return _Content;
                    case __.IpAddress : return _IpAddress;
                    case __.EmailNotify : return _EmailNotify;
                    case __.Approved : return _Approved;
                    case __.CreateDate : return _CreateDate;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.Id : _Id = Convert.ToInt32(value); break;
                    case __.PostId : _PostId = Convert.ToInt32(value); break;
                    case __.ParentId : _ParentId = Convert.ToInt32(value); break;
                    case __.UserId : _UserId = Convert.ToInt32(value); break;
                    case __.Name : _Name = Convert.ToString(value); break;
                    case __.Email : _Email = Convert.ToString(value); break;
                    case __.SiteUrl : _SiteUrl = Convert.ToString(value); break;
                    case __.Content : _Content = Convert.ToString(value); break;
                    case __.IpAddress : _IpAddress = Convert.ToString(value); break;
                    case __.EmailNotify : _EmailNotify = Convert.ToInt32(value); break;
                    case __.Approved : _Approved = Convert.ToInt32(value); break;
                    case __.CreateDate : _CreateDate = Convert.ToDateTime(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得评论字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>ID</summary>
            public static readonly Field Id = FindByName(__.Id);

            ///<summary>日志ID</summary>
            public static readonly Field PostId = FindByName(__.PostId);

            ///<summary>ParentId</summary>
            public static readonly Field ParentId = FindByName(__.ParentId);

            ///<summary>用户ID</summary>
            public static readonly Field UserId = FindByName(__.UserId);

            ///<summary>名称</summary>
            public static readonly Field Name = FindByName(__.Name);

            ///<summary>邮箱</summary>
            public static readonly Field Email = FindByName(__.Email);

            ///<summary>地址</summary>
            public static readonly Field SiteUrl = FindByName(__.SiteUrl);

            ///<summary>内容</summary>
            public static readonly Field Content = FindByName(__.Content);

            ///<summary>IP</summary>
            public static readonly Field IpAddress = FindByName(__.IpAddress);

            ///<summary>邮件提醒</summary>
            public static readonly Field EmailNotify = FindByName(__.EmailNotify);

            ///<summary>是否审核</summary>
            public static readonly Field Approved = FindByName(__.Approved);

            ///<summary>创建日期</summary>
            public static readonly Field CreateDate = FindByName(__.CreateDate);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得评论字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>ID</summary>
            public const String Id = "Id";

            ///<summary>日志ID</summary>
            public const String PostId = "PostId";

            ///<summary>ParentId</summary>
            public const String ParentId = "ParentId";

            ///<summary>用户ID</summary>
            public const String UserId = "UserId";

            ///<summary>名称</summary>
            public const String Name = "Name";

            ///<summary>邮箱</summary>
            public const String Email = "Email";

            ///<summary>地址</summary>
            public const String SiteUrl = "SiteUrl";

            ///<summary>内容</summary>
            public const String Content = "Content";

            ///<summary>IP</summary>
            public const String IpAddress = "IpAddress";

            ///<summary>邮件提醒</summary>
            public const String EmailNotify = "EmailNotify";

            ///<summary>是否审核</summary>
            public const String Approved = "Approved";

            ///<summary>创建日期</summary>
            public const String CreateDate = "CreateDate";

        }
        #endregion
    }

    /// <summary>评论接口</summary>
    public partial interface IComments
    {
        #region 属性
        /// <summary>ID</summary>
        Int32 Id { get; set; }

        /// <summary>日志ID</summary>
        Int32 PostId { get; set; }

        /// <summary>ParentId</summary>
        Int32 ParentId { get; set; }

        /// <summary>用户ID</summary>
        Int32 UserId { get; set; }

        /// <summary>名称</summary>
        String Name { get; set; }

        /// <summary>邮箱</summary>
        String Email { get; set; }

        /// <summary>地址</summary>
        String SiteUrl { get; set; }

        /// <summary>内容</summary>
        String Content { get; set; }

        /// <summary>IP</summary>
        String IpAddress { get; set; }

        /// <summary>邮件提醒</summary>
        Int32 EmailNotify { get; set; }

        /// <summary>是否审核</summary>
        Int32 Approved { get; set; }

        /// <summary>创建日期</summary>
        DateTime CreateDate { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}