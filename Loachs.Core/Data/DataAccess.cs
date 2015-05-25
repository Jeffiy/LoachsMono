using System.Reflection;

namespace Loachs.Data
{
    public static class DataAccess
    {
        private static readonly string path = "Loachs.Data.Access";
        private static readonly object LockHelper = new object();
        public static ITag Itag = null;
        public static ICategory Icategory = null;
        public static IPost Ipost = null;
        public static IUser Iuser = null;
        public static ILink Ilink = null;
        public static IComment Icomment = null;
        public static IStatistics Istatistics = null;
        public static ISetting Isetting = null;

        public static ITag CreateTag()
        {
            string className = path + ".Tag";
            return CreateInstance(Itag, className);
        }

        public static ICategory CreateCategory()
        {
            string className = path + ".Category";
            return CreateInstance(Icategory, className);
        }

        public static IPost CreatePost()
        {
            string className = path + ".Post";

            return CreateInstance(Ipost, className);
        }

        public static IUser CreateUser()
        {
            string className = path + ".User";
            return CreateInstance(Iuser, className);
        }

        public static ILink CreateLink()
        {
            string className = path + ".Link";
            return CreateInstance(Ilink, className);
        }

        public static IComment CreateComment()
        {
            string className = path + ".Comment";
            return CreateInstance(Icomment, className);
        }

        public static IStatistics CreateStatistics()
        {
            string className = path + ".Statistics";
            return CreateInstance(Istatistics, className);
        }

        public static ISetting CreateSetting()
        {
            string className = path + ".Setting";
            return CreateInstance(Isetting, className);
        }

        /// <summary>
        ///     实例化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static T CreateInstance<T>(T instance, string className)
        {
            if (instance == null)
            {
                lock (LockHelper)
                {
                    if (instance == null)
                    {
                        instance = (T) Assembly.Load(path).CreateInstance(className);
                    }
                }
            }
            return instance;
        }
    }

    //public class DatabaseProvider<T> //where T : new()
    //{
    //    private DatabaseProvider()
    //    { }

    //    private static T _instance = default(T);
    //    private static object lockHelper = new object();

    //    static DatabaseProvider()
    //    {

    //    }


    //    public static T Instance
    //    {
    //        get
    //        {
    //            if (_instance == null)
    //            {
    //                lock (lockHelper)
    //                {
    //                    if (_instance == null)
    //                    {
    //                        //  _instance = new T();
    //                        //  _instance =  (T)Assembly.Load(path).CreateInstance(className);
    //                    }
    //                }
    //            }
    //            return _instance;
    //        }
    //    }

    //}
}