using System;
using Loachs.Business;
using Loachs.Common;

namespace Loachs.Entity
{
    /// <summary>
    ///     标签实体
    /// </summary>
    public class TagInfo : IComparable<TagInfo>
    {
        public TagInfo()
        {
            Name = string.Empty;
            Slug = string.Empty;
            Description = string.Empty;
        }

        /// <summary>
        ///     ID
        /// </summary>
        public int TagId { set; get; }
        
        /// <summary>
        ///     名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        ///     别名
        /// </summary>
        public string Slug { set; get; }

        /// <summary>
        ///     描述
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        ///     排序
        /// </summary>
        public int Displayorder { set; get; }

        /// <summary>
        ///     次数
        /// </summary>
        public int Count { set; get; }

        /// <summary>
        ///     创建日期
        /// </summary>
        public DateTime CreateDate { set; get; }

        public int CompareTo(TagInfo other)
        {
            if (Displayorder != other.Displayorder)
            {
                return Displayorder.CompareTo(other.Displayorder);
            }
            return TagId.CompareTo(other.TagId);
        }
        
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
                        SettingManager.GetSetting().RewriteExtension);
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