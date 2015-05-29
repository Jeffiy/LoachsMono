using System.Net;
using Loachs.Entity;

namespace Loachs.Common
{
    public class Utils
    {
        private static string _rewriteExtension = "unknow";
        private static bool _isSupportUrlRewriter;

        /// <summary>
        ///     当前环境是否支持当前配置的URl重写模式
        /// </summary>
        /// <remarks>
        ///     date:2012.7.5
        /// </remarks>
        public static bool IsSupportUrlRewriter
        {
            get
            {
                var site = Sites.GetSetting();
                if (_rewriteExtension == "unknow" || _rewriteExtension != site.RewriteExtension)
                {
                    _rewriteExtension = site.RewriteExtension;

                    var url = ConfigHelper.SiteUrl + "checkurlrewriter" + site.RewriteExtension;

                    var code = NetHelper.GetHttpStatusCode(url);
                    _isSupportUrlRewriter = code == HttpStatusCode.OK;
                }
                return _isSupportUrlRewriter;
            }
        }

        /// <summary>
        ///     预览主题URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string CheckPreviewThemeUrl(string url)
        {
            var theme = RequestHelper.QueryString("theme");
            if (!string.IsNullOrEmpty(theme))
            {
                if (url.IndexOf('?') > 0)
                {
                    url += "&theme=" + theme;
                }
                else
                {
                    url += "?theme=" + theme;
                }
            }

            return url;
        }
    }
}