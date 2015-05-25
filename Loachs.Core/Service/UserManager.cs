using System.Collections.Generic;
using System.Linq;
using Loachs.Data;
using Loachs.Entity;

namespace Loachs.Business
{
    /// <summary>
    ///     用户管理
    /// </summary>
    public class UserManager
    {
        private static readonly IUser Dao = DataAccess.CreateUser();
        //  private static readonly string CacheKey = "users";

        /// <summary>
        ///     列表
        /// </summary>
        private static List<UserInfo> _users;

        /// <summary>
        ///     lock
        /// </summary>
        private static readonly object LockHelper = new object();

        static UserManager()
        {
            LoadUser();
        }

        /// <summary>
        ///     初始化
        /// </summary>
        public static void LoadUser()
        {
            if (_users == null)
            {
                lock (LockHelper)
                {
                    if (_users == null)
                    {
                        _users = Dao.GetUserList();

                        //   BuildUser();
                    }
                }
            }
        }

        /// <summary>
        ///     添加用户
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        public static int InsertUser(UserInfo userinfo)
        {
            userinfo.UserId = Dao.InsertUser(userinfo);
            _users.Add(userinfo);
            _users.Sort();

            return userinfo.UserId;
        }

        /// <summary>
        ///     修改用户
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        public static int UpdateUser(UserInfo userinfo)
        {
            _users.Sort();
            return Dao.UpdateUser(userinfo);
        }

        /// <summary>
        ///     更新用户文章数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="addCount"></param>
        /// <returns></returns>
        public static int UpdateUserPostCount(int userId, int addCount)
        {
            UserInfo user = GetUser(userId);
            if (user != null)
            {
                user.PostCount += addCount;
                return UpdateUser(user);
            }
            return 0;
        }

        /// <summary>
        ///     更新用户评论数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="addCount"></param>
        /// <returns></returns>
        public static int UpdateUserCommentCount(int userId, int addCount)
        {
            UserInfo user = GetUser(userId);
            if (user != null)
            {
                user.CommentCount += addCount;

                return UpdateUser(user);
            }
            return 0;
        }

        /// <summary>
        ///     删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int DeleteUser(int userId)
        {
            UserInfo user = GetUser(userId);
            if (user != null)
            {
                _users.Remove(user);
            }

            return Dao.DeleteUser(userId);
        }

        /// <summary>
        ///     获取全部用户
        /// </summary>
        /// <returns></returns>
        public static List<UserInfo> GetUserList()
        {
            return _users;
        }

        /// <summary>
        ///     是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool ExistsUserName(string userName)
        {
            return Dao.ExistsUserName(userName);
        }

        /// <summary>
        ///     获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static UserInfo GetUser(int userId)
        {
            return _users.FirstOrDefault(user => user.UserId == userId);
        }

        /// <summary>
        ///     根据用户名获取用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static UserInfo GetUser(string userName)
        {
            return _users.FirstOrDefault(user => user.UserName.ToLower() == userName.ToLower());
        }

        /// <summary>
        ///     根据用户名和密码获取用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static UserInfo GetUser(string userName, string password)
        {
            return _users.FirstOrDefault(user => user.UserName.ToLower() == userName.ToLower() && user.Password.ToLower() == password.ToLower());
        }
    }
}