using Loachs.Data;
using Loachs.Entity;

namespace Loachs.Business
{
    /// <summary>
    ///     统计管理
    /// </summary>
    public static class StatisticsManager
    {
        private static readonly IStatistics Dao = DataAccess.CreateStatistics();

        /// <summary>
        ///     缓存统计
        /// </summary>
        private static StatisticsInfo _statistics;

        /// <summary>
        ///     lock
        /// </summary>
        private static readonly object LockHelper = new object();

        static StatisticsManager()
        {
            LoadStatistics();
        }

        /// <summary>
        ///     初始化
        /// </summary>
        private static void LoadStatistics()
        {
            if (_statistics == null)
            {
                lock (LockHelper)
                {
                    if (_statistics == null)
                    {
                        _statistics = Dao.GetStatistics();
                    }
                }
            }
        }

        /// <summary>
        ///     获取
        /// </summary>
        /// <returns></returns>
        public static StatisticsInfo GetStatistics()
        {
            return _statistics;
        }

        /// <summary>
        ///     修改
        /// </summary>
        /// <returns></returns>
        public static bool UpdateStatistics()
        {
            return Dao.UpdateStatistics(_statistics);
        }

        /// <summary>
        ///     更新文章数
        /// </summary>
        /// <param name="addCount">增加数，可为负数</param>
        /// <returns></returns>
        public static bool UpdateStatisticsPostCount(int addCount)
        {
            _statistics.PostCount += addCount;
            return UpdateStatistics();
        }

        /// <summary>
        ///     更新评论数
        /// </summary>
        /// <param name="addCount">增加数，可为负数</param>
        /// <returns></returns>
        public static bool UpdateStatisticsCommentCount(int addCount)
        {
            _statistics.CommentCount += addCount;
            return UpdateStatistics();
        }
    }
}