﻿using System;
﻿using System.ComponentModel;
﻿using XCode;
﻿using XCode.Configuration;
﻿using XCode.DataAccessLayer;

namespace Loachs.Entity
{
    /// <summary>站点设置信息</summary>
    [Serializable]
    [DataObject]
    [Description("站点设置信息")]
    [BindIndex("PK_sitesId", true, "id")]
    [BindTable("Sites", Description = "站点设置信息", ConnName = "loachsConn", DbType = DatabaseType.SQLite)]
    public partial class Sites : ISites
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

        private Int32 _postCount;
        /// <summary>帖子总数</summary>
        [DisplayName("帖子总数")]
        [Description("帖子总数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(2, "postcount", "帖子总数", null, "int", 10, 0, false)]
        public virtual Int32 PostCount
        {
            get { return _postCount; }
            set { if (OnPropertyChanging(__.PostCount, value)) { _postCount = value; OnPropertyChanged(__.PostCount); } }
        }

        private Int32 _commentCount;
        /// <summary>评论总数</summary>
        [DisplayName("评论总数")]
        [Description("评论总数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "commentcount", "评论总数", null, "int", 10, 0, false)]
        public virtual Int32 CommentCount
        {
            get { return _commentCount; }
            set { if (OnPropertyChanging(__.CommentCount, value)) { _commentCount = value; OnPropertyChanged(__.CommentCount); } }
        }

        private Int32 _visitCount;
        /// <summary>浏览总数</summary>
        [DisplayName("浏览总数")]
        [Description("浏览总数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(4, "visitcount", "浏览总数", null, "int", 10, 0, false)]
        public virtual Int32 VisitCount
        {
            get { return _visitCount; }
            set { if (OnPropertyChanging(__.VisitCount, value)) { _visitCount = value; OnPropertyChanged(__.VisitCount); } }
        }

        private Int32 _tagCount;
        /// <summary>标签总数</summary>
        [DisplayName("标签总数")]
        [Description("标签总数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(5, "tagcount", "标签总数", null, "int", 10, 0, false)]
        public virtual Int32 TagCount
        {
            get { return _tagCount; }
            set { if (OnPropertyChanging(__.TagCount, value)) { _tagCount = value; OnPropertyChanged(__.TagCount); } }
        }

        private String _Setting;
        /// <summary>设置</summary>
        [DisplayName("设置")]
        [Description("设置")]
        [DataObjectField(false, false, true, 2147483647)]
        [BindColumn(6, "setting", "设置", null, "nvarchar(2147483647)", 0, 0, true)]
        public virtual String Setting
        {
            get { return _Setting; }
            set { if (OnPropertyChanging(__.Setting, value)) { _Setting = value; OnPropertyChanged(__.Setting); } }
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
                    case __.PostCount : return _postCount;
                    case __.CommentCount : return _commentCount;
                    case __.VisitCount : return _visitCount;
                    case __.TagCount : return _tagCount;
                    case __.Setting : return _Setting;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.Id : _Id = Convert.ToInt32(value); break;
                    case __.PostCount : _postCount = Convert.ToInt32(value); break;
                    case __.CommentCount : _commentCount = Convert.ToInt32(value); break;
                    case __.VisitCount : _visitCount = Convert.ToInt32(value); break;
                    case __.TagCount : _tagCount = Convert.ToInt32(value); break;
                    case __.Setting : _Setting = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得站点统计字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>ID</summary>
            public static readonly Field Id = FindByName(__.Id);

            ///<summary>帖子总数</summary>
            public static readonly Field Postcount = FindByName(__.PostCount);

            ///<summary>评论总数</summary>
            public static readonly Field Commentcount = FindByName(__.CommentCount);

            ///<summary>浏览总数</summary>
            public static readonly Field Visitcount = FindByName(__.VisitCount);

            ///<summary>标签总数</summary>
            public static readonly Field Tagcount = FindByName(__.TagCount);

            ///<summary>设置</summary>
            public static readonly Field Setting = FindByName(__.Setting);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得站点统计字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>ID</summary>
            public const String Id = "Id";

            ///<summary>帖子总数</summary>
            public const String PostCount = "PostCount";

            ///<summary>评论总数</summary>
            public const String CommentCount = "CommentCount";

            ///<summary>浏览总数</summary>
            public const String VisitCount = "VisitCount";

            ///<summary>标签总数</summary>
            public const String TagCount = "TagCount";

            ///<summary>设置</summary>
            public const String Setting = "Setting";

        }
        #endregion
    }

    /// <summary>站点统计接口</summary>
    public partial interface ISites
    {
        #region 属性
        /// <summary>ID</summary>
        Int32 Id { get; set; }

        /// <summary>帖子总数</summary>
        Int32 PostCount { get; set; }

        /// <summary>评论总数</summary>
        Int32 CommentCount { get; set; }

        /// <summary>浏览总数</summary>
        Int32 VisitCount { get; set; }

        /// <summary>标签总数</summary>
        Int32 TagCount { get; set; }

        /// <summary>设置</summary>
        String Setting { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}