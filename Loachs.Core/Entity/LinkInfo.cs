using System;
using Loachs.Common;

namespace Loachs.Entity
{
    /// <summary>
    ///     连接实体(包括导航和友情连接)
    /// </summary>
    public class LinkInfo : IComparable<LinkInfo>
    {
        /// <summary>
        ///     ID
        /// </summary>
        public int LinkId { set; get; }

        /// <summary>
        ///     类型(待用,现默认为0)
        /// </summary>
        public int Type { set; get; }

        /// <summary>
        ///     名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        ///     连接地址
        /// </summary>
        public string Href { set; get; }

        /// <summary>
        ///     位置
        /// </summary>
        public int Position { set; get; }

        /// <summary>
        ///     打开方式
        /// </summary>
        public string Target { set; get; }

        /// <summary>
        ///     描述
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        ///     排序
        /// </summary>
        public int Displayorder { set; get; }

        /// <summary>
        ///     状态(1:显示,0:隐藏)
        /// </summary>
        public int Status { set; get; }

        /// <summary>
        ///     创建日期
        /// </summary>
        public DateTime CreateDate { set; get; }

        public int CompareTo(LinkInfo other)
        {
            if (Displayorder != other.Displayorder)
            {
                return Displayorder.CompareTo(other.Displayorder);
            }
            return LinkId.CompareTo(other.LinkId);
        }

        #region 非字段

        /// <summary>
        ///     连接Url
        /// </summary>
        public string Url
        {
            get { return Href.Replace("${siteurl}", ConfigHelper.SiteUrl); }
        }

        /// <summary>
        ///     连接地址
        /// </summary>
        public string Link
        {
            get
            {
                return string.Format("<a href=\"{0}\" title=\"{1}\" target=\"{2}\">{3}</a>", Url, Description, Target,
                    Name);
            }
        }

        #endregion
    }
}