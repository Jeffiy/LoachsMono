namespace Loachs.Entity
{
    /// <summary>
    ///     统计实体
    /// </summary>
    public class StatisticsInfo
    {
        /// <summary>
        ///     文章数
        /// </summary>
        public int PostCount { set; get; }

        /// <summary>
        ///     评论数
        /// </summary>
        public int CommentCount { set; get; }

        /// <summary>
        ///     访问数
        /// </summary>
        public int VisitCount { set; get; }

        /// <summary>
        ///     标签数
        /// </summary>
        public int TagCount { set; get; }
    }
}