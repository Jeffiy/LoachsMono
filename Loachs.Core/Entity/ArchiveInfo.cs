using System;
using Loachs.Common;
using Loachs.Core.Config;

namespace Loachs.Entity
{
    /// <summary>
    ///     归档实体
    /// </summary>
    public class ArchiveInfo
    {
        /// <summary>
        ///     日期,用于拼Url
        /// </summary>
        public DateTime Date { set; get; }

        /// <summary>
        ///     url地址
        /// </summary>
        public string Url
        {
            get
            {
                string url = string.Empty;

                if (Utils.IsSupportUrlRewriter == false)
                {
                    url = string.Format("{0}default.aspx?type=archive&date={1}", ConfigHelper.SiteUrl,
                        Date.ToString("yyyyMM"));
                }
                else
                {
                    return ConfigHelper.SiteUrl + "archive/" + Date.ToString("yyyyMM") +
                           SiteConfig.Current.RewriteExtension;
                }
                return Utils.CheckPreviewThemeUrl(url);
            }
        }

        /// <summary>
        ///     连接
        /// </summary>
        public string Link
        {
            get { return string.Format("<a href=\"{0}\" >{1}</a>", Url, Date.ToString("yyyyMM")); }
        }

        /// <summary>
        ///     数量
        /// </summary>
        public int Count { set; get; }
    }
}