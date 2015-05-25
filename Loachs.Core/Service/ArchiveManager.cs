using System.Collections.Generic;
using Loachs.Common;
using Loachs.Data;
using Loachs.Entity;

namespace Loachs.Business
{
    /// <summary>
    ///     归档管理
    /// </summary>
    public class ArchiveManager
    {
        private static readonly IPost Dao = DataAccess.CreatePost();

        /// <summary>
        ///     获取归档
        /// </summary>
        /// <returns></returns>
        public static List<ArchiveInfo> GetArchive()
        {
            string archiveCacheKey = "archive";
            List<ArchiveInfo> list = (List<ArchiveInfo>) CacheHelper.Get(archiveCacheKey);

            if (list == null)
            {
                list = Dao.GetArchive();


                CacheHelper.Insert(archiveCacheKey, list, CacheHelper.MinuteFactor*5);
            }

            return list;
        }
    }
}