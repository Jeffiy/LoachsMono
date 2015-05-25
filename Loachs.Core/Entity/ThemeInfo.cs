namespace Loachs.Entity
{
    /// <summary>
    ///     主题实体
    /// </summary>
    public class ThemeInfo
    {
        /// <summary>
        ///     主题名
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        ///     对应程序版本
        /// </summary>
        public string Version { set; get; }

        /// <summary>
        ///     作者
        /// </summary>
        public string Author { set; get; }

        /// <summary>
        ///     作者Email
        /// </summary>
        public string Email { set; get; }

        /// <summary>
        ///     作者网站
        /// </summary>
        public string SiteUrl { set; get; }

        /// <summary>
        ///     发布日期
        /// </summary>
        public string PubDate { set; get; }
    }
}