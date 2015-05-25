using System;
using Loachs.Business;
using Loachs.Common;

namespace Loachs.Entity
{
    /// <summary>
    ///     归档实体
    /// </summary>
    public class ArchiveInfo
    {
        //  private string _name;
        //  private string _url;
        //  private string _link;

        //[Obsolete]
        ///// <summary>
        ///// 名称
        ///// </summary>
        //public string Name
        //{
        //    set { _name = value; }
        //    get { return _name; }
        //}
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
                           SettingManager.GetSetting().RewriteExtension;
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