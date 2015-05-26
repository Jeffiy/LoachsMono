﻿using System;
﻿using System.ComponentModel;
﻿using XCode;
﻿using XCode.Configuration;
﻿using XCode.DataAccessLayer;

namespace Loachs.Data
{
    /// <summary>用户信息</summary>
    [Serializable]
    [DataObject]
    [Description("用户信息")]
    [BindIndex("PK_usersid", true, "id")]
    [BindTable("Users", Description = "用户信息", ConnName = "loachsConn", DbType = DatabaseType.SQLite)]
    public partial class Users : IUsers
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

        private Int32 _Type;
        /// <summary>类型</summary>
        [DisplayName("类型")]
        [Description("类型")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(2, "type", "类型", null, "int", 10, 0, false)]
        public virtual Int32 Type
        {
            get { return _Type; }
            set { if (OnPropertyChanging(__.Type, value)) { _Type = value; OnPropertyChanged(__.Type); } }
        }

        private String _UserName;
        /// <summary>用户名</summary>
        [DisplayName("用户名")]
        [Description("用户名")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(3, "username", "用户名", null, "nvarchar(50)", 0, 0, true)]
        public virtual String UserName
        {
            get { return _UserName; }
            set { if (OnPropertyChanging(__.UserName, value)) { _UserName = value; OnPropertyChanged(__.UserName); } }
        }

        private String _Name;
        /// <summary>昵称</summary>
        [DisplayName("昵称")]
        [Description("昵称")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(4, "name", "昵称", null, "nvarchar(50)", 0, 0, true, Master=true)]
        public virtual String Name
        {
            get { return _Name; }
            set { if (OnPropertyChanging(__.Name, value)) { _Name = value; OnPropertyChanged(__.Name); } }
        }

        private String _Password;
        /// <summary>密码</summary>
        [DisplayName("密码")]
        [Description("密码")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(5, "password", "密码", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Password
        {
            get { return _Password; }
            set { if (OnPropertyChanging(__.Password, value)) { _Password = value; OnPropertyChanged(__.Password); } }
        }

        private String _Email;
        /// <summary>Email</summary>
        [DisplayName("Email")]
        [Description("Email")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(6, "email", "Email", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Email
        {
            get { return _Email; }
            set { if (OnPropertyChanging(__.Email, value)) { _Email = value; OnPropertyChanged(__.Email); } }
        }

        private String _SiteUrl;
        /// <summary>网址</summary>
        [DisplayName("网址")]
        [Description("网址")]
        [DataObjectField(false, false, true, 255)]
        [BindColumn(7, "siteurl", "网址", null, "nvarchar(255)", 0, 0, true)]
        public virtual String SiteUrl
        {
            get { return _SiteUrl; }
            set { if (OnPropertyChanging(__.SiteUrl, value)) { _SiteUrl = value; OnPropertyChanged(__.SiteUrl); } }
        }

        private String _AvatarUrl;
        /// <summary>头像链接</summary>
        [DisplayName("头像链接")]
        [Description("头像链接")]
        [DataObjectField(false, false, true, 255)]
        [BindColumn(10, "avatarurl", "头像链接", null, "nvarchar(255)", 0, 0, true)]
        public virtual String AvatarUrl
        {
            get { return _AvatarUrl; }
            set { if (OnPropertyChanging(__.AvatarUrl, value)) { _AvatarUrl = value; OnPropertyChanged(__.AvatarUrl); } }
        }

        private String _Description;
        /// <summary>描述</summary>
        [DisplayName("描述")]
        [Description("描述")]
        [DataObjectField(false, false, true, 255)]
        [BindColumn(9, "description", "描述", null, "nvarchar(255)", 0, 0, true)]
        public virtual String Description
        {
            get { return _Description; }
            set { if (OnPropertyChanging(__.Description, value)) { _Description = value; OnPropertyChanged(__.Description); } }
        }

        private Int32 _DisplayOrder;
        /// <summary>排序</summary>
        [DisplayName("排序")]
        [Description("排序")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(10, "displayorder", "排序", null, "int", 10, 0, false)]
        public virtual Int32 DisplayOrder
        {
            get { return _DisplayOrder; }
            set { if (OnPropertyChanging(__.DisplayOrder, value)) { _DisplayOrder = value; OnPropertyChanged(__.DisplayOrder); } }
        }

        private Int32 _Status;
        /// <summary>状态</summary>
        [DisplayName("状态")]
        [Description("状态")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(11, "status", "状态", null, "int", 10, 0, false)]
        public virtual Int32 Status
        {
            get { return _Status; }
            set { if (OnPropertyChanging(__.Status, value)) { _Status = value; OnPropertyChanged(__.Status); } }
        }

        private Int32 _PostCount;
        /// <summary>帖子总数</summary>
        [DisplayName("帖子总数")]
        [Description("帖子总数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(12, "postcount", "帖子总数", null, "int", 10, 0, false)]
        public virtual Int32 PostCount
        {
            get { return _PostCount; }
            set { if (OnPropertyChanging(__.PostCount, value)) { _PostCount = value; OnPropertyChanged(__.PostCount); } }
        }

        private Int32 _CommentCount;
        /// <summary>评论总数</summary>
        [DisplayName("评论总数")]
        [Description("评论总数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(13, "commentcount", "评论总数", null, "int", 10, 0, false)]
        public virtual Int32 CommentCount
        {
            get { return _CommentCount; }
            set { if (OnPropertyChanging(__.CommentCount, value)) { _CommentCount = value; OnPropertyChanged(__.CommentCount); } }
        }

        private DateTime _CreateDate;
        /// <summary>创建日期</summary>
        [DisplayName("创建日期")]
        [Description("创建日期")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(14, "createdate", "创建日期", null, "datetime", 0, 0, false)]
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
                    case __.Type : return _Type;
                    case __.UserName : return _UserName;
                    case __.Name : return _Name;
                    case __.Password : return _Password;
                    case __.Email : return _Email;
                    case __.SiteUrl : return _SiteUrl;
                    case __.AvatarUrl : return _AvatarUrl;
                    case __.Description : return _Description;
                    case __.DisplayOrder : return _DisplayOrder;
                    case __.Status : return _Status;
                    case __.PostCount : return _PostCount;
                    case __.CommentCount : return _CommentCount;
                    case __.CreateDate : return _CreateDate;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.Id : _Id = Convert.ToInt32(value); break;
                    case __.Type : _Type = Convert.ToInt32(value); break;
                    case __.UserName : _UserName = Convert.ToString(value); break;
                    case __.Name : _Name = Convert.ToString(value); break;
                    case __.Password : _Password = Convert.ToString(value); break;
                    case __.Email : _Email = Convert.ToString(value); break;
                    case __.SiteUrl : _SiteUrl = Convert.ToString(value); break;
                    case __.AvatarUrl : _AvatarUrl = Convert.ToString(value); break;
                    case __.Description : _Description = Convert.ToString(value); break;
                    case __.DisplayOrder : _DisplayOrder = Convert.ToInt32(value); break;
                    case __.Status : _Status = Convert.ToInt32(value); break;
                    case __.PostCount : _PostCount = Convert.ToInt32(value); break;
                    case __.CommentCount : _CommentCount = Convert.ToInt32(value); break;
                    case __.CreateDate : _CreateDate = Convert.ToDateTime(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得用户信息字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>ID</summary>
            public static readonly Field Id = FindByName(__.Id);

            ///<summary>类型</summary>
            public static readonly Field Type = FindByName(__.Type);

            ///<summary>用户名</summary>
            public static readonly Field UserName = FindByName(__.UserName);

            ///<summary>昵称</summary>
            public static readonly Field Name = FindByName(__.Name);

            ///<summary>密码</summary>
            public static readonly Field Password = FindByName(__.Password);

            ///<summary>Email</summary>
            public static readonly Field Email = FindByName(__.Email);

            ///<summary>网址</summary>
            public static readonly Field SiteUrl = FindByName(__.SiteUrl);

            ///<summary>头像链接</summary>
            public static readonly Field AvatarUrl = FindByName(__.AvatarUrl);

            ///<summary>描述</summary>
            public static readonly Field Description = FindByName(__.Description);

            ///<summary>排序</summary>
            public static readonly Field DisplayOrder = FindByName(__.DisplayOrder);

            ///<summary>状态</summary>
            public static readonly Field Status = FindByName(__.Status);

            ///<summary>帖子总数</summary>
            public static readonly Field PostCount = FindByName(__.PostCount);

            ///<summary>评论总数</summary>
            public static readonly Field CommentCount = FindByName(__.CommentCount);

            ///<summary>创建日期</summary>
            public static readonly Field CreateDate = FindByName(__.CreateDate);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得用户信息字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>ID</summary>
            public const String Id = "Id";

            ///<summary>类型</summary>
            public const String Type = "Type";

            ///<summary>用户名</summary>
            public const String UserName = "UserName";

            ///<summary>昵称</summary>
            public const String Name = "Name";

            ///<summary>密码</summary>
            public const String Password = "Password";

            ///<summary>Email</summary>
            public const String Email = "Email";

            ///<summary>网址</summary>
            public const String SiteUrl = "SiteUrl";

            ///<summary>头像链接</summary>
            public const String AvatarUrl = "AvatarUrl";

            ///<summary>描述</summary>
            public const String Description = "Description";

            ///<summary>排序</summary>
            public const String DisplayOrder = "DisplayOrder";

            ///<summary>状态</summary>
            public const String Status = "Status";

            ///<summary>帖子总数</summary>
            public const String PostCount = "PostCount";

            ///<summary>评论总数</summary>
            public const String CommentCount = "CommentCount";

            ///<summary>创建日期</summary>
            public const String CreateDate = "CreateDate";

        }
        #endregion
    }

    /// <summary>用户信息接口</summary>
    public partial interface IUsers
    {
        #region 属性
        /// <summary>ID</summary>
        Int32 Id { get; set; }

        /// <summary>类型</summary>
        Int32 Type { get; set; }

        /// <summary>用户名</summary>
        String UserName { get; set; }

        /// <summary>昵称</summary>
        String Name { get; set; }

        /// <summary>密码</summary>
        String Password { get; set; }

        /// <summary>Email</summary>
        String Email { get; set; }

        /// <summary>网址</summary>
        String SiteUrl { get; set; }

        /// <summary>头像链接</summary>
        String AvatarUrl { get; set; }

        /// <summary>描述</summary>
        String Description { get; set; }

        /// <summary>排序</summary>
        Int32 DisplayOrder { get; set; }

        /// <summary>状态</summary>
        Int32 Status { get; set; }

        /// <summary>帖子总数</summary>
        Int32 PostCount { get; set; }

        /// <summary>评论总数</summary>
        Int32 CommentCount { get; set; }

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