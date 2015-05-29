﻿using System;
﻿using System.ComponentModel;
﻿using XCode;
﻿using XCode.Configuration;
﻿using XCode.DataAccessLayer;

namespace Loachs.Entity
{
    /// <summary>链接</summary>
    [Serializable]
    [DataObject]
    [Description("链接")]
    [BindIndex("PK_linksid", true, "id")]
    [BindTable("Links", Description = "链接", ConnName = "loachsConn", DbType = DatabaseType.SQLite)]
    public partial class Links : ILinks
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

        private String _Name;
        /// <summary>名称</summary>
        [DisplayName("名称")]
        [Description("名称")]
        [DataObjectField(false, false, true, 255)]
        [BindColumn(3, "name", "名称", null, "nvarchar(255)", 0, 0, true, Master=true)]
        public virtual String Name
        {
            get { return _Name; }
            set { if (OnPropertyChanging(__.Name, value)) { _Name = value; OnPropertyChanged(__.Name); } }
        }

        private String _Href;
        /// <summary>链接</summary>
        [DisplayName("链接")]
        [Description("链接")]
        [DataObjectField(false, false, true, 255)]
        [BindColumn(4, "href", "链接", null, "nvarchar(255)", 0, 0, true)]
        public virtual String Href
        {
            get { return _Href; }
            set { if (OnPropertyChanging(__.Href, value)) { _Href = value; OnPropertyChanged(__.Href); } }
        }

        private Int32 _Position;
        /// <summary>位置</summary>
        [DisplayName("位置")]
        [Description("位置")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(5, "position", "位置", null, "int", 10, 0, false)]
        public virtual Int32 Position
        {
            get { return _Position; }
            set { if (OnPropertyChanging(__.Position, value)) { _Position = value; OnPropertyChanged(__.Position); } }
        }

        private String _Target;
        /// <summary>目标</summary>
        [DisplayName("目标")]
        [Description("目标")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(6, "target", "目标", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Target
        {
            get { return _Target; }
            set { if (OnPropertyChanging(__.Target, value)) { _Target = value; OnPropertyChanged(__.Target); } }
        }

        private String _Description;
        /// <summary>描述</summary>
        [DisplayName("描述")]
        [Description("描述")]
        [DataObjectField(false, false, true, 255)]
        [BindColumn(7, "description", "描述", null, "nvarchar(255)", 0, 0, true)]
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
        [BindColumn(9, "status", "状态", null, "int", 10, 0, false)]
        public virtual Int32 Status
        {
            get { return _Status; }
            set { if (OnPropertyChanging(__.Status, value)) { _Status = value; OnPropertyChanged(__.Status); } }
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
                    case __.Name : return _Name;
                    case __.Href : return _Href;
                    case __.Position : return _Position;
                    case __.Target : return _Target;
                    case __.Description : return _Description;
                    case __.DisplayOrder : return _DisplayOrder;
                    case __.Status : return _Status;
                    case __.CreateDate : return _CreateDate;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.Id : _Id = Convert.ToInt32(value); break;
                    case __.Type: _Type = Convert.ToInt32(value); break;
                    case __.Name : _Name = Convert.ToString(value); break;
                    case __.Href : _Href = Convert.ToString(value); break;
                    case __.Position: _Position = Convert.ToInt32(value); break;
                    case __.Target : _Target = Convert.ToString(value); break;
                    case __.Description : _Description = Convert.ToString(value); break;
                    case __.DisplayOrder: _DisplayOrder = Convert.ToInt32(value); break;
                    case __.Status: _Status = Convert.ToInt32(value); break;
                    case __.CreateDate : _CreateDate = Convert.ToDateTime(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得链接字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>ID</summary>
            public static readonly Field Id = FindByName(__.Id);

            ///<summary>类型</summary>
            public static readonly Field Type = FindByName(__.Type);

            ///<summary>名称</summary>
            public static readonly Field Name = FindByName(__.Name);

            ///<summary>链接</summary>
            public static readonly Field Href = FindByName(__.Href);

            ///<summary>位置</summary>
            public static readonly Field Position = FindByName(__.Position);

            ///<summary>目标</summary>
            public static readonly Field Target = FindByName(__.Target);

            ///<summary>描述</summary>
            public static readonly Field Description = FindByName(__.Description);

            ///<summary>排序</summary>
            public static readonly Field DisplayOrder = FindByName(__.DisplayOrder);

            ///<summary>状态</summary>
            public static readonly Field Status = FindByName(__.Status);

            ///<summary>创建日期</summary>
            public static readonly Field CreateDate = FindByName(__.CreateDate);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得链接字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>ID</summary>
            public const String Id = "Id";

            ///<summary>类型</summary>
            public const String Type = "Type";

            ///<summary>名称</summary>
            public const String Name = "Name";

            ///<summary>链接</summary>
            public const String Href = "Href";

            ///<summary>位置</summary>
            public const String Position = "Position";

            ///<summary>目标</summary>
            public const String Target = "Target";

            ///<summary>描述</summary>
            public const String Description = "Description";

            ///<summary>排序</summary>
            public const String DisplayOrder = "DisplayOrder";

            ///<summary>状态</summary>
            public const String Status = "Status";

            ///<summary>创建日期</summary>
            public const String CreateDate = "CreateDate";

        }
        #endregion
    }

    /// <summary>链接接口</summary>
    public partial interface ILinks
    {
        #region 属性
        /// <summary>ID</summary>
        Int32 Id { get; set; }

        /// <summary>类型</summary>
        Int32 Type { get; set; }

        /// <summary>名称</summary>
        String Name { get; set; }

        /// <summary>链接</summary>
        String Href { get; set; }

        /// <summary>位置</summary>
        Int32 Position { get; set; }

        /// <summary>目标</summary>
        String Target { get; set; }

        /// <summary>描述</summary>
        String Description { get; set; }

        /// <summary>排序</summary>
        Int32 DisplayOrder { get; set; }

        /// <summary>状态</summary>
        Int32 Status { get; set; }

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