using System;
using System.Collections.Generic;
using System.Data;
using Loachs.Entity;
using Mono.Data.Sqlite;

namespace Loachs.Data.Access
{
    public class User : IUser
    {
        /// <summary>
        ///     添加
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        public int InsertUser(UserInfo userinfo)
        {
            string cmdText = @" insert into [loachs_users](
                                [Type],[UserName],[Name],[Password],[Email],[SiteUrl],[AvatarUrl],[Description],[displayorder],[Status],[PostCount],[CommentCount],[CreateDate])
                                values (
                                @Type,@UserName,@Name,@Password,@Email,@SiteUrl,@AvatarUrl,@Description,@Displayorder,@Status, @PostCount,@CommentCount,@CreateDate )";
            SqliteParameter[] prams =
            {
                SqliteDbHelper.MakeInParam("@Type", DbType.Int32, 4, userinfo.Type),
                SqliteDbHelper.MakeInParam("@UserName", DbType.String, 50, userinfo.UserName),
                SqliteDbHelper.MakeInParam("@Name", DbType.String, 50, userinfo.Name),
                SqliteDbHelper.MakeInParam("@Password", DbType.String, 50, userinfo.Password),
                SqliteDbHelper.MakeInParam("@Email", DbType.String, 50, userinfo.Email),
                SqliteDbHelper.MakeInParam("@SiteUrl", DbType.String, 255, userinfo.SiteUrl),
                SqliteDbHelper.MakeInParam("@AvatarUrl", DbType.String, 255, userinfo.AvatarUrl),
                SqliteDbHelper.MakeInParam("@Displayorder", DbType.String, 255, userinfo.Description),
                SqliteDbHelper.MakeInParam("@Status", DbType.Int32, 4, userinfo.Displayorder),
                SqliteDbHelper.MakeInParam("@Status", DbType.Int32, 4, userinfo.Status),
                SqliteDbHelper.MakeInParam("@PostCount", DbType.Int32, 4, userinfo.PostCount),
                SqliteDbHelper.MakeInParam("@CommentCount", DbType.Int32, 4, userinfo.CommentCount),
                SqliteDbHelper.MakeInParam("@CreateDate", DbType.Date, 8, userinfo.CreateDate)
            };
            int r = SqliteDbHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams);
            if (r > 0)
            {
                return
                    Convert.ToInt32(
                        SqliteDbHelper.ExecuteScalar(
                            "select   [UserId] from [loachs_users]  order by [UserId] desc limit 1"));
            }
            return 0;
        }

        /// <summary>
        ///     更新
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        public int UpdateUser(UserInfo userinfo)
        {
            string cmdText = @"update [loachs_users] set
                                [Type]=@Type,
                                [UserName]=@UserName,
                                [Name]=@Name,
                                [Password]=@Password,
                                [Email]=@Email,
                                [SiteUrl]=@SiteUrl,
                                [AvatarUrl]=@AvatarUrl,
                                [Description]=@Description,
                                [Displayorder]=@Displayorder,
                                [Status]=@Status,
                                [PostCount]=@PostCount,
                                [CommentCount]=@CommentCount,
                                [CreateDate]=@CreateDate
                                where UserId=@UserId";
            SqliteParameter[] prams =
            {
                SqliteDbHelper.MakeInParam("@Type", DbType.Int32, 4, userinfo.Type),
                SqliteDbHelper.MakeInParam("@UserName", DbType.String, 50, userinfo.UserName),
                SqliteDbHelper.MakeInParam("@Name", DbType.String, 50, userinfo.Name),
                SqliteDbHelper.MakeInParam("@Password", DbType.String, 50, userinfo.Password),
                SqliteDbHelper.MakeInParam("@Email", DbType.String, 50, userinfo.Email),
                SqliteDbHelper.MakeInParam("@SiteUrl", DbType.String, 255, userinfo.SiteUrl),
                SqliteDbHelper.MakeInParam("@AvatarUrl", DbType.String, 255, userinfo.AvatarUrl),
                SqliteDbHelper.MakeInParam("@Description", DbType.String, 255, userinfo.Description),
                SqliteDbHelper.MakeInParam("@Displayorder", DbType.String, 255, userinfo.Displayorder),
                SqliteDbHelper.MakeInParam("@Status", DbType.Int32, 4, userinfo.Status),
                SqliteDbHelper.MakeInParam("@PostCount", DbType.Int32, 4, userinfo.PostCount),
                SqliteDbHelper.MakeInParam("@CommentCount", DbType.Int32, 4, userinfo.CommentCount),
                SqliteDbHelper.MakeInParam("@CreateDate", DbType.Date, 8, userinfo.CreateDate),
                SqliteDbHelper.MakeInParam("@UserId", DbType.Int32, 4, userinfo.UserId)
            };
            return SqliteDbHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams);
        }

        /// <summary>
        ///     删除
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int DeleteUser(int userid)
        {
            string cmdText = "delete from [loachs_users] where [userid] = @userid";
            SqliteParameter[] prams =
            {
                SqliteDbHelper.MakeInParam("@userid", DbType.Int32, 4, userid)
            };
            return SqliteDbHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams);
        }

        ///// <summary>
        ///// 获取实体
        ///// </summary>
        ///// <param name="userName"></param>
        ///// <param name="password"></param>
        ///// <returns></returns>
        //public UserInfo GetUser(string userName, string password)
        //{
        //    string cmdText = "select * from [loachs_users] where [userName] = @userName and [Password]=@password";
        //    SqliteParameter[] prams = { 
        //                        SqliteDbHelper.MakeInParam("@userName",DbType.String,50,userName),
        //                        SqliteDbHelper.MakeInParam("@password",DbType.String,50,password),
        //                    };
        //    List<UserInfo> list = DataReaderToUserList(SqliteDbHelper.ExecuteReader(CommandType.Text, cmdText, prams));
        //    if (list.Count > 0)
        //    {
        //        return list[0];
        //    }
        //    return null;

        //}


        /// <summary>
        ///     获取全部
        /// </summary>
        /// <returns></returns>
        public List<UserInfo> GetUserList()
        {
            string cmdText = "select * from [loachs_users]  order by [displayorder] asc,[userid] asc";
            return DataReaderToUserList(SqliteDbHelper.ExecuteReader(cmdText));
        }

        /// <summary>
        ///     是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool ExistsUserName(string userName)
        {
            string cmdText = "select count(1) from [loachs_users] where [userName] = @userName ";
            SqliteParameter[] prams =
            {
                SqliteDbHelper.MakeInParam("@userName", DbType.String, 50, userName)
            };
            return Convert.ToInt32(SqliteDbHelper.ExecuteScalar(CommandType.Text, cmdText, prams)) > 0;
        }

        /// <summary>
        ///     数据转换
        /// </summary>
        /// <param name="read"></param>
        /// <returns></returns>
        private List<UserInfo> DataReaderToUserList(SqliteDataReader read)
        {
            List<UserInfo> list = new List<UserInfo>();
            while (read.Read())
            {
                UserInfo userinfo = new UserInfo
                {
                    UserId = Convert.ToInt32(read["UserId"]),
                    Type = Convert.ToInt32(read["Type"]),
                    UserName = Convert.ToString(read["UserName"]),
                    Name = Convert.ToString(read["Name"]),
                    Password = Convert.ToString(read["Password"]),
                    Email = Convert.ToString(read["Email"]),
                    SiteUrl = Convert.ToString(read["SiteUrl"]),
                    AvatarUrl = Convert.ToString(read["AvatarUrl"]),
                    Description = Convert.ToString(read["Description"]),
                    Displayorder = Convert.ToInt32(read["Displayorder"]),
                    Status = Convert.ToInt32(read["Status"]),
                    PostCount = Convert.ToInt32(read["PostCount"]),
                    CommentCount = Convert.ToInt32(read["CommentCount"]),
                    CreateDate = Convert.ToDateTime(read["CreateDate"])
                };

                list.Add(userinfo);
            }
            read.Close();
            return list;
        }
    }
}