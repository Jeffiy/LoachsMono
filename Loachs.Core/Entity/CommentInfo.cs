using System;
using System.Web.Security;
using Loachs.Business;
using Loachs.Common;

namespace Loachs.Entity
{
    /// <summary>
    ///     评论实体
    /// </summary>
    public class CommentInfo
    {
        /// <summary>
        ///     评论ID
        /// </summary>
        public int CommentId { set; get; }

        /// <summary>
        ///     父ID
        /// </summary>
        public int ParentId { set; get; }

        /// <summary>
        ///     文章ID
        /// </summary>
        public int PostId { set; get; }

        /// <summary>
        ///     用户ID
        /// </summary>
        public int UserId { set; get; }

        /// <summary>
        ///     姓名
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        ///     Email
        /// </summary>
        public string Email { set; get; }

        /// <summary>
        ///     网址
        /// </summary>
        public string SiteUrl { set; get; }

        /// <summary>
        ///     正文
        /// </summary>
        public string Content { set; get; }

        /// <summary>
        ///     邮件通知
        ///     1:通知,0:不通知
        /// </summary>
        public int EmailNotify { set; get; }

        /// <summary>
        ///     IP地址
        /// </summary>
        public string IpAddress { set; get; }

        /// <summary>
        ///     审核
        /// </summary>
        public int Approved { set; get; }

        /// <summary>
        ///     创建时间
        /// </summary>
        public DateTime CreateDate { set; get; }

        #region 非字段
        
        /// <summary>
        ///     标题(实为过滤html的正文某长度文字)
        /// </summary>
        public string Title
        {
            get { return StringHelper.CutString(StringHelper.RemoveHtml(Content), 20, ""); }
        }


        /// <summary>
        ///     URl地址
        /// </summary>
        /// <remarks>
        ///     desc:考虑分页，总是跳到最后一页，需改进
        ///     date:2012.7
        /// </remarks>
        public string Url
        {
            get
            {
                PostInfo post = PostManager.GetPost(PostId);
                if (post != null)
                {
                    return string.Format("{0}#comment-{1}", post.Url, CommentId);
                }
                return "###";
            }
        }

        /// <summary>
        ///     评论连接
        /// </summary>
        public string Link
        {
            get { return string.Format("<a href=\"{0}\" title=\"{1} 发表于 {2}\">{3}</a>", Url, Name, CreateDate, Title); }
        }


        /// <summary>
        ///     作者连接
        /// </summary>
        public string AuthorLink
        {
            get
            {
                if (UserId > 0)
                {
                    UserInfo user = UserManager.GetUser(UserId);
                    if (user != null)
                    {
                        return user.Link;
                    }
                }
                else if (StringHelper.IsHttpUrl(SiteUrl))
                {
                    return string.Format("<a href=\"{0}\" target=\"_blank\" title=\"{1}\">{1}</a>", SiteUrl, Name);
                }
                return Name;
            }
        }

        /// <summary>
        ///     层次
        /// </summary>
        public int Floor { set; get; }

        /// <summary>
        ///     Gravatar 加密后的字符串
        /// </summary>
        public string GravatarCode
        {
            get { return FormsAuthentication.HashPasswordForStoringInConfigFile(Email, "MD5").ToLower(); }
        }

        /// <summary>
        ///     评论对应文章
        /// </summary>
        public PostInfo Post
        {
            get
            {
                PostInfo post = PostManager.GetPost(PostId);
                if (post != null)
                {
                    return post;
                }
                return new PostInfo();
            }
        }

        #endregion
    }
}