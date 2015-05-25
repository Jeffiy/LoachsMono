using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;

namespace Loachs.Common
{
    /// <summary>
    ///     缓存类
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        ///     CacheDependency 说明
        ///     如果您向 Cache 中添加某个具有依赖项的项，当依赖项更改时，
        ///     该项将自动从 Cache 中删除。例如，假设您向 Cache 中添加某项，
        ///     并使其依赖于文件名数组。当该数组中的某个文件更改时，
        ///     与该数组关联的项将从缓存中删除。
        ///     [C#]
        ///     Insert the cache item.
        ///     CacheDependency dep = new CacheDependency(fileName, dt);
        ///     cache.Insert("key", "value", dep);
        /// </summary>
        /// <summary>
        ///     天
        /// </summary>
        public static readonly int DayFactor = 17280;

        /// <summary>
        ///     小时
        /// </summary>
        public static readonly int HourFactor = 720;

        /// <summary>
        ///     分钟
        /// </summary>
        public static readonly int MinuteFactor = 12;

        /// <summary>
        ///     秒
        /// </summary>
        public static readonly double SecondFactor = 0.2;

        /// <summary>
        ///     调节
        /// </summary>
        //  private static int Factor = 0;
        private static int _factor = 5;

        private static readonly Cache Cache;
        private static readonly string CachePrefix = ConfigHelper.SitePrefix;

        /// <summary>
        ///     Static initializer should ensure we only have to look up the current cache
        ///     instance once.
        /// </summary>
        static CacheHelper()
        {
            HttpContext context = HttpContext.Current;
            Cache = context != null ? context.Cache : HttpRuntime.Cache;
        }

        private CacheHelper()
        {
        }

        public static void ReSetFactor(int cacheFactor)
        {
            _factor = cacheFactor;
        }

        /// <summary>
        ///     清除所有缓存
        /// </summary>
        public static void Clear()
        {
            IDictionaryEnumerator cacheEnum = Cache.GetEnumerator();
            ArrayList al = new ArrayList();
            while (cacheEnum.MoveNext())
            {
                al.Add(cacheEnum.Key);
            }

            foreach (string key in al)
            {
                Cache.Remove(key);
            }
        }

        public static void RemoveByPattern(string pattern)
        {
            IDictionaryEnumerator cacheEnum = Cache.GetEnumerator();
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
            // Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline  );
            while (cacheEnum.MoveNext())
            {
                if (regex.IsMatch(CachePrefix + cacheEnum.Key))
                    Cache.Remove(CachePrefix + cacheEnum.Key);
            }
        }

        /// <summary>
        ///     清除缓存
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            Cache.Remove(CachePrefix + key);
        }

        /// <summary>
        ///     缓存OBJECT.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public static void Insert(string key, object obj)
        {
            Insert(key, obj, null, 1);
        }

        /// <summary>
        ///     缓存obj 并建立依赖项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="dep"></param>
        public static void Insert(string key, object obj, CacheDependency dep)
        {
            Insert(key, obj, dep, MinuteFactor*3);
        }

        /// <summary>
        ///     按秒缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="seconds"></param>
        public static void Insert(string key, object obj, int seconds)
        {
            Insert(key, obj, null, seconds);
        }

        /// <summary>
        ///     按秒缓存对象 并存储优先级
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="seconds"></param>
        /// <param name="priority"></param>
        public static void Insert(string key, object obj, int seconds, CacheItemPriority priority)
        {
            Insert(key, obj, null, seconds, priority);
        }

        /// <summary>
        ///     按秒缓存对象 并建立依赖项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="dep"></param>
        /// <param name="seconds"></param>
        public static void Insert(string key, object obj, CacheDependency dep, int seconds)
        {
            Insert(key, obj, dep, seconds, CacheItemPriority.Normal);
        }

        /// <summary>
        ///     按秒缓存对象 并建立具有优先级的依赖项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="dep"></param>
        /// <param name="seconds"></param>
        /// <param name="priority"></param>
        public static void Insert(string key, object obj, CacheDependency dep, int seconds, CacheItemPriority priority)
        {
            if (obj != null)
            {
                Cache.Insert(CachePrefix + key, obj, dep, DateTime.Now.AddSeconds(_factor*seconds), TimeSpan.Zero,
                    priority, null);
            }
        }

        ///// <summary>
        ///// 插入生存周期短的缓存项目
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="obj"></param>
        ///// <param name="secondFactor"></param>
        //public static void MicroInsert(string key, object obj, int secondFactor)
        //{
        //    if (obj != null)
        //    {
        //        _cache.Insert(key, obj, null, DateTime.Now.AddSeconds(Factor * secondFactor), TimeSpan.Zero);
        //    }
        //}


        ///// <summary>
        ///// 插入生存周期很长的缓存项目，在系统运行期间永久缓存
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="obj"></param>
        //public static void MaxInsert(string key, object obj)
        //{
        //    MaxInsert(key, obj, null);
        //}

        ///// <summary>
        ///// 具有依赖项的最大时间缓存
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="obj"></param>
        ///// <param name="dep"></param>
        //public static void MaxInsert(string key, object obj, CacheDependency dep)
        //{
        //    if (obj != null)
        //    {
        //        _cache.Insert(key, obj, dep, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.AboveNormal, null);
        //    }
        //}

        ///// <summary>
        ///// Insert an item into the cache for the Maximum allowed time
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="obj"></param>
        //public static void Permanent(string key, object obj)
        //{
        //    Permanent(key, obj, null);
        //}

        //public static void Permanent(string key, object obj, CacheDependency dep)
        //{
        //    if (obj != null)
        //    {
        //        _cache.Insert(key, obj, dep, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.NotRemovable, null);
        //    }
        //}
        /// <summary>
        ///     获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            return Cache[CachePrefix + key];
        }
    }
}