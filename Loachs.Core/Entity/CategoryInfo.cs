using System;
using Loachs.Business;
using Loachs.Common;

namespace Loachs.Entity
{
    /// <summary>
    ///     分类实体
    ///     RSSURL
    /// </summary>
    public class CategoryInfo : IComparable<CategoryInfo>
    {
        public CategoryInfo()
        {
            Name = string.Empty;
            Slug = string.Empty;
            Description = string.Empty;
        }

        /// <summary>
        ///     ID
        /// </summary>
        public int CategoryId { set; get; }
        
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

        public int CompareTo(CategoryInfo other)
        {
            if (Displayorder != other.Displayorder)
            {
                return Displayorder.CompareTo(other.Displayorder);
            }
            return CategoryId.CompareTo(other.CategoryId);
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

                string slug = CategoryId.ToString();
                if (!string.IsNullOrEmpty(Slug))
                {
                    slug = StringHelper.UrlEncode(Slug);
                }


                if (Utils.IsSupportUrlRewriter == false)
                {
                    url = string.Format("{0}default.aspx?type=category&slug={1}", ConfigHelper.SiteUrl, slug);
                }
                else
                {
                    url = string.Format("{0}category/{1}{2}", ConfigHelper.SiteUrl, slug,
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
            get { return string.Format("<a href=\"{0}\" title=\"分类:{1}\">{2}</a>", Url, Name, Name); }
        }


        /// <summary>
        ///     订阅URL
        /// </summary>
        public string FeedUrl
        {
            get
            {
                return string.Format("{0}feed/post/categoryid/{1}{2}", ConfigHelper.SiteUrl, CategoryId,
                    SettingManager.GetSetting().RewriteExtension);
            }
        }

        /// <summary>
        ///     订阅连接
        /// </summary>
        public string FeedLink
        {
            get { return string.Format("<a href=\"{0}\" title=\"订阅:{1}\">订阅</a>", FeedUrl, Name); }
        }

        #endregion
    }
}