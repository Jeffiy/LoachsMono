using System.Net;
using Loachs.Business;

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
                if (_rewriteExtension == "unknow" || _rewriteExtension != SettingManager.GetSetting().RewriteExtension)
                {
                    _rewriteExtension = SettingManager.GetSetting().RewriteExtension;

                    var url = ConfigHelper.SiteUrl + "checkurlrewriter" + SettingManager.GetSetting().RewriteExtension;

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