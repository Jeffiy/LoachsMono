using System;
using System.Collections.Generic;
using Loachs.Business;
using Loachs.Common;

namespace Loachs.Entity
{
    /// <summary>
    ///     文章实体
    ///     是否要加个排序字段
    ///     是否要加个图片URL字段
    /// </summary>
    public class PostInfo : IComparable<PostInfo>
    {
        private int _commentstatus;
        private string _template = "post.html";
        private int _type;

        /// <summary>
        ///     ID
        /// </summary>
        public int PostId { set; get; }

        ///// <summary>
        ///// 类型
        ///// </summary>
        //public int Type
        //{
        //    set { _type = value; }
        //    get { return _type; }
        //}
        /// <summary>
        ///     分类ID
        /// </summary>
        public int CategoryId { set; get; }

        /// <summary>
        ///     标题
        /// </summary>
        public string Title { set; get; }

        /// <summary>
        ///     别名
        /// </summary>
        public string Slug { set; get; }

        /// <summary>
        ///     摘要
        /// </summary>
        public string Summary { set; get; }

        /// <summary>
        ///     正文
        /// </summary>
        public string Content { set; get; }

        /// <summary>
        ///     用户ID
        /// </summary>
        public int UserId { set; get; }

        /// <summary>
        ///     是否允许评论
        /// </summary>
        public int CommentStatus
        {
            set { _commentstatus = value; }
            get
            {
                if (_commentstatus == 1 && SettingManager.GetSetting().CommentStatus == 1)
                {
                    return 1;
                }
                return 0;
            }
        }

        /// <summary>
        ///     评论数
        /// </summary>
        public int CommentCount { set; get; }

        /// <summary>
        ///     点击数
        /// </summary>
        public int ViewCount { set; get; }

        /// <summary>
        ///     标签
        /// </summary>
        public string Tag { set; get; }

        /// <summary>
        ///     URL类型,见枚举PostUrlFormat
        /// </summary>
        public int UrlFormat { set; get; }

        /// <summary>
        ///     模板
        /// </summary>
        public string Template
        {
            set { _template = value; }
            get { return _template; }
        }

        /// <summary>
        ///     推荐
        /// </summary>
        public int Recommend { set; get; }

        /// <summary>
        ///     状态
        /// </summary>
        public int Status { set; get; }

        /// <summary>
        ///     置顶
        /// </summary>
        public int TopStatus { set; get; }

        /// <summary>
        ///     隐藏于列表
        /// </summary>
        public int HideStatus { set; get; }

        /// <summary>
        ///     添加时间
        /// </summary>
        public DateTime CreateDate { set; get; }

        /// <summary>
        ///     最后编辑时间
        /// </summary>
        public DateTime UpdateDate { set; get; }

        public int CompareTo(PostInfo other)
        {
            return other.PostId.CompareTo(PostId);
        }

        #region 非字段

        //private string _url;
        //private string _link;
        //private string _detail;
        //private UserInfo _user;
        //private CategoryInfo _category;
        //private List<TagInfo> _tags;
        //private List<PostInfo> _relatedposts;

        /// <summary>
        ///     内容地址
        /// </summary>
        /// <remarks>
        ///     desc:自动判断是否支持URL重写
        ///     date:2012.7.5
        /// </remarks>
        public string Url
        {
            get
            {
                string url = string.Empty;

                if (Utils.IsSupportUrlRewriter == false)
                {
                    url = string.Format("{0}default.aspx?type=post&name={1}", ConfigHelper.SiteUrl,
                        !string.IsNullOrEmpty(Slug) ? StringHelper.UrlEncode(Slug) : PostId.ToString());
                }
                else
                {
                    switch ((PostUrlFormat) UrlFormat)
                    {
                        //case PostUrlFormat.Category:
                        //    url = string.Format("{0}post/{1}/{2}{3}", ConfigHelper.AppUrl, StringHelper.UrlEncode(this.Category.Slug), !string.IsNullOrEmpty(this.Slug) ? StringHelper.UrlEncode(this.Slug) : this.PostId.ToString(), SettingManager.GetSetting().RewriteExtension);
                        //    break;

                        case PostUrlFormat.Slug:
                            url = string.Format("{0}post/{1}{2}", ConfigHelper.SiteUrl,
                                !string.IsNullOrEmpty(Slug) ? StringHelper.UrlEncode(Slug) : PostId.ToString(),
                                SettingManager.GetSetting().RewriteExtension);
                            break;
                        case PostUrlFormat.Date:
                        default:
                            url = string.Format("{0}post/{1}/{2}{3}", ConfigHelper.SiteUrl,
                                CreateDate.ToString(@"yyyy\/MM\/dd"),
                                !string.IsNullOrEmpty(Slug) ? StringHelper.UrlEncode(Slug) : PostId.ToString(),
                                SettingManager.GetSetting().RewriteExtension);
                            break;
                    }
                }
                return Utils.CheckPreviewThemeUrl(url);
            }
        }

        /// <summary>
        ///     分页URL
        /// </summary>
        public string PageUrl
        {
            get
            {
                string url = string.Empty;

                if (Utils.IsSupportUrlRewriter == false)
                {
                    url = string.Format("{0}default.aspx?type=post&name={1}&page={2}", ConfigHelper.SiteUrl,
                        !string.IsNullOrEmpty(Slug) ? StringHelper.UrlEncode(Slug) : PostId.ToString(), "{0}");
                }
                else
                {
                    switch ((PostUrlFormat) UrlFormat)
                    {
                        case PostUrlFormat.Slug:

                            url = string.Format("{0}post/{1}/page/{2}{3}", ConfigHelper.SiteUrl,
                                !string.IsNullOrEmpty(Slug) ? StringHelper.UrlEncode(Slug) : PostId.ToString(), "{0}",
                                SettingManager.GetSetting().RewriteExtension);
                            break;

                        case PostUrlFormat.Date:
                        default:
                            url = string.Format("{0}post/{1}/{2}/page/{3}{4}", ConfigHelper.SiteUrl,
                                CreateDate.ToString(@"yyyy\/MM\/dd"),
                                !string.IsNullOrEmpty(Slug) ? StringHelper.UrlEncode(Slug) : PostId.ToString(), "{0}",
                                SettingManager.GetSetting().RewriteExtension);
                            break;
                    }
                }
                return Utils.CheckPreviewThemeUrl(url);
            }
        }

        /// <summary>
        ///     连接
        /// </summary>
        public string Link
        {
            get { return string.Format("<a href=\"{0}\">{1}</a>", Url, Title); }
        }

        /// <summary>
        ///     订阅URL
        /// </summary>
        public string FeedUrl
        {
            get
            {
                //return string.Format("{0}feed/comment/postid/{1}{2}", ConfigHelper.SiteUrl, this.PostId, SettingManager.GetSetting().RewriteExtension);
                return string.Format("{0}feed/comment/postid/{1}.aspx", ConfigHelper.SiteUrl, PostId);
            }
        }

        /// <summary>
        ///     订阅连接
        /// </summary>
        public string FeedLink
        {
            get { return string.Format("<a href=\"{0}\" title=\"订阅:{1} 的评论\">订阅</a>", FeedUrl, Title); }
        }

        /// <summary>
        ///     文章 详情
        ///     根据设置而定,可为摘要,正文
        /// </summary>
        public string Detail
        {
            get
            {
                switch (SettingManager.GetSetting().PostShowType)
                {
                    case 1:
                        return string.Empty;
                    case 2:
                    default:
                        return Summary;

                    case 3:
                        return StringHelper.CutString(StringHelper.RemoveHtml(Content), 200, "...");
                    case 4:
                        return Content;
                }
            }
        }

        /// <summary>
        ///     Rss 详情
        ///     根据设置而定,可为摘要,正文,前200字,空等
        /// </summary>
        public string FeedDetail
        {
            get
            {
                switch (SettingManager.GetSetting().RssShowType)
                {
                    case 1:
                        return string.Empty;
                    case 2:
                    default:
                        return Summary;

                    case 3:
                        return StringHelper.CutString(StringHelper.RemoveHtml(Content), 200, "...");
                    case 4:
                        return Content;
                }
            }
        }

        /// <summary>
        ///     作者
        /// </summary>
        public UserInfo Author
        {
            get
            {
                UserInfo user = UserManager.GetUser(UserId);
                if (user != null)
                {
                    return user;
                }
                return new UserInfo();
            }
        }

        /// <summary>
        ///     所属分类
        /// </summary>
        public CategoryInfo Category
        {
            get
            {
                CategoryInfo category = CategoryManager.GetCategory(CategoryId);
                if (category != null)
                {
                    return category;
                }
                return new CategoryInfo();
            }
        }

        /// <summary>
        ///     对应标签
        /// </summary>
        public List<TagInfo> Tags
        {
            get
            {
                string temptags = Tag.Replace("{", "").Replace("}", ",");

                if (temptags.Length > 0)
                {
                    temptags = temptags.TrimEnd(',');
                }

                return TagManager.GetTagList(temptags);
            }
        }

        /// <summary>
        ///     下一篇文章(新)
        /// </summary>
        public PostInfo Next
        {
            get
            {
                List<PostInfo> list = PostManager.GetPostList();
                PostInfo post =
                    list.Find(
                        delegate(PostInfo p) { return p.HideStatus == 0 && p.Status == 1 && p.PostId > this.PostId; });
                return post ?? new PostInfo();
            }
        }

        /// <summary>
        ///     上一篇文章(旧)
        /// </summary>
        public PostInfo Previous
        {
            get
            {
                List<PostInfo> list = PostManager.GetPostList();
                PostInfo post =
                    list.Find(
                        delegate(PostInfo p) { return p.HideStatus == 0 && p.Status == 1 && p.PostId < this.PostId; });

                return post ?? new PostInfo();
            }
        }

        /// <summary>
        ///     相关文章
        /// </summary>
        public List<PostInfo> RelatedPosts
        {
            get
            {
                if (string.IsNullOrEmpty(Tag))
                {
                    return new List<PostInfo>();
                }

                List<PostInfo> list =
                    PostManager.GetPostList()
                        .FindAll(delegate(PostInfo p) { return p.HideStatus == 0 && p.Status == 1; });

                string tags = Tag.Replace("}", "},");
                tags = tags.TrimEnd(',');

                string[] temparray = tags.Split(',');

                int num = 0;
                List<PostInfo> list2 = list.FindAll(delegate(PostInfo p)
                {
                    if (num >= SettingManager.GetSetting().PostRelatedCount)
                    {
                        return false;
                    }

                    foreach (string tag in temparray)
                    {
                        if (p.Tag.IndexOf(tag) != -1 && p.PostId != this.PostId)
                        {
                            num++;
                            return true;
                        }
                    }
                    return false;
                });


                return list2;
            }
        }

        #endregion
    }
}