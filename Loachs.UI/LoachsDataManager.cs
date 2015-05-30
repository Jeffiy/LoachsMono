using System;
using System.Collections.Generic;
using Loachs.Core.Config;
using Loachs.Entity;
using XCode;
using StringHelper = Loachs.Common.StringHelper;

namespace Loachs.Web
{
    /// <summary>
    ///     数据调用管理
    /// </summary>
    public class LoachsDataManager
    {
        /// <summary>
        ///     调用文章列表
        /// </summary>
        /// <remarks>
        ///     参数说明：
        ///     categoryid=7&userid=2&pagesize=10&page=2
        ///     categoryid=8&userid=2&count=10&order=desc&keyword=
        /// </remarks>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<Posts> GetPosts(string condition)
        {
            return Posts.FindAllWithCache();
        }

        ///// <summary>
        ///// 调用文章
        ///// </summary>
        ///// <remarks></remarks>
        ///// <param name="id">postid or slug</param>
        ///// <returns></returns>
        //public PostInfo GetPost(string key)
        //{
        //    if (StringHelper.IsInt(id))
        //    {
        //        return PostManager.GetPost(Convert.ToInt32(key));
        //    }
        //    return PostManager.GetPost(key);
        //}

        //public List<UserInfo> GetUsers(string filter)
        //{
        //    return UserManager.GetUserList();
        //}

        /// <summary>
        ///     调用用户
        /// </summary>
        /// <param name="key">userid or username</param>
        /// <returns></returns>
        public Users GetUser(string key)
        {
            if (StringHelper.IsInt(key))
            {
                return Users.FindById(Convert.ToInt32(key));
            }
            return Users.FindByName(key);
        }

        /// <summary>
        ///     调用评论
        /// </summary>
        /// <remarks>
        ///     postid=7&userid=3&pagesize=20&page=2&order=desc
        /// </remarks>
        /// <param name="condition"></param>
        /// <returns></returns>
        public EntityList<Comments> GetComments(string condition)
        {
            int pageSize = 10;
            int pageIndex = 1;
            int recordCount = 0;
            int order = SiteConfig.Current.CommentOrder;
            int userid = -1;
            int postid = -1;
            int parentid = -1;
            int approved = (int) ApprovedStatus.Success;
            int emailNotify = -1;
            string keyword = string.Empty;

            // int pageSize, int pageIndex, out int totalRecord, int order, int userId, int postId, int parentId, int approved, int emailNotify, string keyword)

            return Comments.GetCommentList(pageSize, pageIndex, out recordCount, order, userid, postid, parentid,
                approved, emailNotify, keyword);
        }

        public EntityList<Tags> GetTags(string filter)
        {
            return Tags.FindAllWithCache();
        }

        public string GetPager(string filter)
        {
            return string.Empty;
        }
    }
}