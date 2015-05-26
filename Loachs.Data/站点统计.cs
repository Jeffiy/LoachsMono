﻿using System;
﻿using System.ComponentModel;
﻿using XCode;
﻿using XCode.Configuration;
﻿using XCode.DataAccessLayer;

namespace Loachs.Data
{
    /// <summary>站点统计</summary>
    [Serializable]
    [DataObject]
    [Description("站点统计")]
    [BindIndex("PK_sitesId", true, "id")]
    [BindTable("Sites", Description = "站点统计", ConnName = "loachsConn", DbType = DatabaseType.SQLite)]
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

        private Int32 _Postcount;
        /// <summary>帖子总数</summary>
        [DisplayName("帖子总数")]
        [Description("帖子总数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(2, "postcount", "帖子总数", null, "int", 10, 0, false)]
        public virtual Int32 Postcount
        {
            get { return _Postcount; }
            set { if (OnPropertyChanging(__.Postcount, value)) { _Postcount = value; OnPropertyChanged(__.Postcount); } }
        }

        private Int32 _Commentcount;
        /// <summary>评论总数</summary>
        [DisplayName("评论总数")]
        [Description("评论总数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "commentcount", "评论总数", null, "int", 10, 0, false)]
        public virtual Int32 Commentcount
        {
            get { return _Commentcount; }
            set { if (OnPropertyChanging(__.Commentcount, value)) { _Commentcount = value; OnPropertyChanged(__.Commentcount); } }
        }

        private Int32 _Visitcount;
        /// <summary>浏览总数</summary>
        [DisplayName("浏览总数")]
        [Description("浏览总数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(4, "visitcount", "浏览总数", null, "int", 10, 0, false)]
        public virtual Int32 Visitcount
        {
            get { return _Visitcount; }
            set { if (OnPropertyChanging(__.Visitcount, value)) { _Visitcount = value; OnPropertyChanged(__.Visitcount); } }
        }

        private Int32 _Tagcount;
        /// <summary>标签总数</summary>
        [DisplayName("标签总数")]
        [Description("标签总数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(5, "tagcount", "标签总数", null, "int", 10, 0, false)]
        public virtual Int32 Tagcount
        {
            get { return _Tagcount; }
            set { if (OnPropertyChanging(__.Tagcount, value)) { _Tagcount = value; OnPropertyChanged(__.Tagcount); } }
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
                    case __.Postcount : return _Postcount;
                    case __.Commentcount : return _Commentcount;
                    case __.Visitcount : return _Visitcount;
                    case __.Tagcount : return _Tagcount;
                    case __.Setting : return _Setting;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.Id : _Id = Convert.ToInt32(value); break;
                    case __.Postcount : _Postcount = Convert.ToInt32(value); break;
                    case __.Commentcount : _Commentcount = Convert.ToInt32(value); break;
                    case __.Visitcount : _Visitcount = Convert.ToInt32(value); break;
                    case __.Tagcount : _Tagcount = Convert.ToInt32(value); break;
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
            public static readonly Field Postcount = FindByName(__.Postcount);

            ///<summary>评论总数</summary>
            public static readonly Field Commentcount = FindByName(__.Commentcount);

            ///<summary>浏览总数</summary>
            public static readonly Field Visitcount = FindByName(__.Visitcount);

            ///<summary>标签总数</summary>
            public static readonly Field Tagcount = FindByName(__.Tagcount);

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
            public const String Postcount = "Postcount";

            ///<summary>评论总数</summary>
            public const String Commentcount = "Commentcount";

            ///<summary>浏览总数</summary>
            public const String Visitcount = "Visitcount";

            ///<summary>标签总数</summary>
            public const String Tagcount = "Tagcount";

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
        Int32 Postcount { get; set; }

        /// <summary>评论总数</summary>
        Int32 Commentcount { get; set; }

        /// <summary>浏览总数</summary>
        Int32 Visitcount { get; set; }

        /// <summary>标签总数</summary>
        Int32 Tagcount { get; set; }

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