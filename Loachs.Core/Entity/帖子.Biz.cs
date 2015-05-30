﻿using System;
﻿using System.Collections.Generic;
﻿using System.ComponentModel;
﻿using System.Linq;
﻿using System.Xml.Serialization;
﻿using Loachs.Common;
﻿using Loachs.Core.Config;
﻿using NewLife.Data;
﻿using XCode;
﻿using XCode.Membership;
﻿using StringHelper = Loachs.Common.StringHelper;

namespace Loachs.Entity
{
    /// <summary>帖子</summary>
    public partial class Posts : LogEntity<Posts>
    {
        #region 对象操作﻿

        /// <summary>验证数据，通过抛出异常的方式提示验证失败。</summary>
        /// <param name="isNew"></param>
        public override void Valid(Boolean isNew)
        {
			// 如果没有脏数据，则不需要进行任何处理
			if (!HasDirty) return;

            // 这里验证参数范围，建议抛出参数异常，指定参数名，前端用户界面可以捕获参数异常并聚焦到对应的参数输入框
            //if (String.IsNullOrEmpty(Name)) throw new ArgumentNullException(_.Name, _.Name.DisplayName + "无效！");
            //if (!isNew && ID < 1) throw new ArgumentOutOfRangeException(_.ID, _.ID.DisplayName + "必须大于0！");

            // 建议先调用基类方法，基类方法会对唯一索引的数据进行验证
            base.Valid(isNew);

            // 在新插入数据或者修改了指定字段时进行唯一性验证，CheckExist内部抛出参数异常
            //if (isNew || Dirtys[__.Name]) CheckExist(__.Name);
            
            if (isNew && !Dirtys[__.CreateDate]) CreateDate = DateTime.Now;
            if (!Dirtys[__.UpdateDate]) UpdateDate = DateTime.Now;
        }

        ///// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //protected override void InitData()
        //{
        //    base.InitData();

        //    // InitData一般用于当数据表没有数据时添加一些默认数据，该实体类的任何第一次数据库操作都会触发该方法，默认异步调用
        //    // Meta.Count是快速取得表记录数
        //    if (Meta.Count > 0) return;

        //    // 需要注意的是，如果该方法调用了其它实体类的首次数据库操作，目标实体类的数据初始化将会在同一个线程完成
        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}[{1}]数据……", typeof(Posts).Name, Meta.Table.DataTable.DisplayName);

        //    var entity = new Posts();
        //    entity.CategoryId = 0;
        //    entity.Title = "abc";
        //    entity.Slug = "abc";
        //    entity.ImageUrl = "abc";
        //    entity.Summary = "abc";
        //    entity.Content = "abc";
        //    entity.UserId = 0;
        //    entity.CommentStatus = 0;
        //    entity.CommentCount = 0;
        //    entity.ViewCount = 0;
        //    entity.Tag = "abc";
        //    entity.UrlFormat = 0;
        //    entity.Template = "abc";
        //    entity.Recommend = 0;
        //    entity.Status = 0;
        //    entity.TopStatus = 0;
        //    entity.HideStatus = 0;
        //    entity.CreateDate = DateTime.Now;
        //    entity.UpdateDate = DateTime.Now;
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}[{1}]数据！", typeof(Posts).Name, Meta.Table.DataTable.DisplayName);
        //}
        
        /// <summary>已重载。基类先调用Valid(true)验证数据，然后在事务保护内调用OnInsert</summary>
        /// <returns></returns>
        public override Int32 Insert()
        {
            //统计
            Sites.UpdateStatisticsPostCount(1);
            //用户
            Users.UpdateUserPostCount(UserId, 1);
            //分类
            UpdateCategoryCount(CategoryId, 1);
            //标签
            Entity.Tags.UpdateTagUseCount(Tag, 1);

            return base.Insert();
        }

        ///// <summary>已重载。在事务保护范围内处理业务，位于Valid之后</summary>
        ///// <returns></returns>
        //protected override Int32 OnInsert()
        //{
        //    return base.OnInsert();
        //}

        /// <summary>已重载。基类先调用Valid(true)验证数据，然后在事务保护内调用OnUpdate</summary>
        /// <returns></returns>
        public override Int32 Update()
        {
            Posts oldPost = FindById(Id);
            if (oldPost != null)
            {
                if (oldPost.CategoryId != CategoryId)
                {
                    //分类
                    UpdateCategoryCount(oldPost.CategoryId, -1);
                    UpdateCategoryCount(CategoryId, 1);
                }

                //标签
                Entity.Tags.UpdateTagUseCount(oldPost.Tag, -1);
                Entity.Tags.UpdateTagUseCount(Tag, 1);
            }
            return base.Update();
        }

        /// <summary>已重载。基类先调用Valid(true)验证数据，然后在事务保护内调用OnDelete</summary>
        /// <returns></returns>
        public override Int32 Delete()
        {
            //统计
            Sites.UpdateStatisticsPostCount(-1);
            //用户
            Users.UpdateUserPostCount(UserId, -1);
            //分类
            UpdateCategoryCount(CategoryId, -1);
            //标签
            Entity.Tags.UpdateTagUseCount(Tag, -1);

            //删除所有评论
            Comments.DeleteByPostId(Id);

            return base.Delete();
        }
        #endregion

        #region 扩展属性﻿
        [XmlIgnore]
        public string Date { get; set; }

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
                        !string.IsNullOrEmpty(Slug) ? StringHelper.UrlEncode(Slug) : Id.ToString());
                }
                else
                {
                    var site = SiteConfig.Current;
                    switch ((PostUrlFormat)UrlFormat)
                    {
                        //case PostUrlFormat.Category:
                        //    url = string.Format("{0}post/{1}/{2}{3}", ConfigHelper.AppUrl, StringHelper.UrlEncode(this.Category.Slug), !string.IsNullOrEmpty(this.Slug) ? StringHelper.UrlEncode(this.Slug) : this.PostId.ToString(), SettingManager.GetSetting().RewriteExtension);
                        //    break;

                        case PostUrlFormat.Slug:
                            url = string.Format("{0}post/{1}{2}", ConfigHelper.SiteUrl,
                                !string.IsNullOrEmpty(Slug) ? StringHelper.UrlEncode(Slug) : Id.ToString(),
                                site.RewriteExtension);
                            break;
                        case PostUrlFormat.Date:
                        default:
                            url = string.Format("{0}post/{1}/{2}{3}", ConfigHelper.SiteUrl,
                                CreateDate.ToString(@"yyyy\/MM\/dd"),
                                !string.IsNullOrEmpty(Slug) ? StringHelper.UrlEncode(Slug) : Id.ToString(),
                                site.RewriteExtension);
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
                        !string.IsNullOrEmpty(Slug) ? StringHelper.UrlEncode(Slug) : Id.ToString(), "{0}");
                }
                else
                {
                    var site = SiteConfig.Current;
                    switch ((PostUrlFormat)UrlFormat)
                    {
                        case PostUrlFormat.Slug:

                            url = string.Format("{0}post/{1}/page/{2}{3}", ConfigHelper.SiteUrl,
                                !string.IsNullOrEmpty(Slug) ? StringHelper.UrlEncode(Slug) : Id.ToString(), "{0}",
                                site.RewriteExtension);
                            break;

                        case PostUrlFormat.Date:
                        default:
                            url = string.Format("{0}post/{1}/{2}/page/{3}{4}", ConfigHelper.SiteUrl,
                                CreateDate.ToString(@"yyyy\/MM\/dd"),
                                !string.IsNullOrEmpty(Slug) ? StringHelper.UrlEncode(Slug) : Id.ToString(), "{0}",
                                site.RewriteExtension);
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
                //return string.Format("{0}feed/comment/postid/{1}{2}", ConfigHelper.SiteUrl, this.PostId, SiteConfig.Current.RewriteExtension);
                return string.Format("{0}feed/comment/postid/{1}.aspx", ConfigHelper.SiteUrl, Id);
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
                switch (SiteConfig.Current.PostShowType)
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
                switch (SiteConfig.Current.RssShowType)
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
        public Users Author
        {
            get
            {
                return Users.FindById(UserId);
            }
        }

        /// <summary>
        ///     所属分类
        /// </summary>
        public Categorys Category
        {
            get
            {
                return Categorys.FindById(CategoryId);
            }
        }

        /// <summary>
        ///     对应标签
        /// </summary>
        public EntityList<Tags> Tags
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Tag))
                {
                    string temptags = Tag.Replace("{", "").Replace("}", ",");

                    if (temptags.Length > 0)
                    {
                        temptags = temptags.TrimEnd(',');
                    }

                    return Entity.Tags.GetTagList(temptags);
                }
                return new EntityList<Tags>();
            }
        }

        /// <summary>
        ///     下一篇文章(新)
        /// </summary>
        public Posts Next
        {
            get
            {
                return Find(_.HideStatus == 0 & _.Status == 1 & _.Id > Id);
            }
        }

        /// <summary>
        ///     上一篇文章(旧)
        /// </summary>
        public Posts Previous
        {
            get
            {
                return Find(_.HideStatus == 0 & _.Status == 1 & _.Id < Id);
            }
        }

        /// <summary>
        ///     相关文章
        /// </summary>
        public List<Posts> RelatedPosts
        {
            get
            {
                if (string.IsNullOrEmpty(Tag))
                {
                    return new List<Posts>();
                }

                string tags = Tag.Replace("}", "},");
                tags = tags.TrimEnd(',');

                string[] temparray = tags.Split(',');

                int num = 0;
                List<Posts> list = FindAll(_.HideStatus == 0 & _.Status == 1, null)
                    .ToList()
                    .FindAll(p =>
                    {
                        if (num >= SiteConfig.Current.PostRelatedCount)
                        {
                            return false;
                        }

                        if (temparray.Any(tag => p.Tag.IndexOf(tag) != -1 && p.Id != Id))
                        {
                            num++;
                            return true;
                        }
                        return false;
                    });

                return list;
            }
        }
        #endregion

        #region 扩展查询﻿
        /// <summary>根据id查找</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Posts FindById(Int32 id)
        {
            if (Meta.Count >= 1000)
                return Find(_.Id, id);
            else // 实体缓存
                return Meta.Cache.Entities.Find(__.Id, id);
            // 单对象缓存
            //return Meta.SingleCache[id];
        }

        /// <summary>根据标题查找</summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Posts FindByName(string name)
        {
            if (Meta.Count >= 1000)
                return Find(_.Title, name);
            else // 实体缓存
                return Meta.Cache.Entities.Find(__.Title, name);
        }

        /// <summary>根据别名查找</summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Posts FindBySlug(string slug)
        {
            if (Meta.Count >= 1000)
                return Find(_.Slug, slug);
            else // 实体缓存
                return Meta.Cache.Entities.Find(__.Slug, slug);
        }
        #endregion

        #region 高级查询
        // 以下为自定义高级查询的例子

        /// <summary>查询满足条件的记录集，分页、排序</summary>
        /// <param name="userid">用户编号</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="key">关键字</param>
        /// <param name="param">分页排序参数，同时返回满足条件的总记录数</param>
        /// <returns>实体集</returns>
        public static EntityList<Posts> Search(Int32 userid, DateTime start, DateTime end, String key, PageParameter param)
        {
            // WhereExpression重载&和|运算符，作为And和Or的替代
            // SearchWhereByKeys系列方法用于构建针对字符串字段的模糊搜索，第二个参数可指定要搜索的字段
            var exp = SearchWhereByKeys(key, null, null);

            // 以下仅为演示，Field（继承自FieldItem）重载了==、!=、>、<、>=、<=等运算符
            //if (userid > 0) exp &= _.OperatorID == userid;
            //if (isSign != null) exp &= _.IsSign == isSign.Value;
            //exp &= _.OccurTime.Between(start, end); // 大于等于start，小于end，当start/end大于MinValue时有效

            return FindAll(exp, param);
        }
        #endregion

        #region 扩展操作
        #endregion

        #region 业务
        //更新文章数
        public static int UpdateCategoryCount(int categoryId, int addCount)
        {
            if (categoryId == 0)
            {
                return 0;
            }

            Categorys category = Categorys.FindById(categoryId);
            if (category == null) return 0;

            category.Count += addCount;
            return category.Save();
        }

        /// <summary>根据id删除</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Int32 DeleteById(Int32 id)
        {
            return Delete(new []{__.Id}, new object[]{id});
        }

        /// <summary>
        ///     更新点击数
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="addCount"></param>
        /// <returns></returns>
        public static int UpdatePostViewCount(int postId, int addCount)
        {
            Posts post = FindById(postId);

            if (post != null)
            {
                post.ViewCount += addCount;
                return post.Save();
            }

            return 0;
        }
        
        /// <summary>
        ///     更新评论数
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="addCount"></param>
        /// <returns></returns>
        public static int UpdatePostCommentCount(int postId, int addCount)
        {
            Posts post = FindById(postId);

            if (post != null)
            {
                post.CommentCount += addCount;
                return post.Save();
            }

            return 0;
        }
        
        /// <summary>
        ///     获取文章数
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="tagId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int GetPostCount(int categoryId, int tagId, int userId)
        {
            int recordCount = 0;
            GetPostList(1, 1, out recordCount, categoryId, tagId, userId, -1, -1, -1, -1, string.Empty, string.Empty,
                string.Empty);

            return recordCount;
        }

        /// <summary>
        ///     获取文章数
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="tagId"></param>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <param name="hidestatus"></param>
        /// <returns></returns>
        public static int GetPostCount(int categoryId, int tagId, int userId, int status, int hidestatus)
        {
            int recordCount = 0;
            GetPostList(1, 1, out recordCount, categoryId, tagId, userId, -1, status, -1, hidestatus, string.Empty,
                string.Empty, string.Empty);

            return recordCount;
        }

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="recordCount">The record count.</param>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="tagId">The tag identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="recommend">The recommend.</param>
        /// <param name="status">The status.</param>
        /// <param name="topstatus">The topstatus.</param>
        /// <param name="hidestatus">The hidestatus.</param>
        /// <param name="begindate">The begindate.</param>
        /// <param name="enddate">The enddate.</param>
        /// <param name="keyword">The keyword.</param>
        /// <returns></returns>
        public static EntityList<Posts> GetPostList(int pageSize, int pageIndex, out int recordCount, int categoryId,
            int tagId, int userId, int recommend, int status, int topstatus, int hidestatus, string begindate,
            string enddate, string keyword)
        {
            var exp = new WhereExpression();

            if (categoryId != -1) exp &= _.CategoryId == categoryId;
            if (tagId != -1) exp &= _.Tag.Contains("{" + tagId + "}");
            if (userId != -1) exp &= _.UserId == userId;
            if (recommend != -1) exp &= _.Recommend == recommend;
            if (status != -1) exp &= _.Status == status;
            if (topstatus != -1) exp &= _.TopStatus == topstatus;
            if (hidestatus != -1) exp &= _.HideStatus == hidestatus;
            if (!string.IsNullOrEmpty(begindate) || !string.IsNullOrEmpty(enddate)) exp &= _.CreateDate.Between(begindate.ToDateTime(), enddate.ToDateTime());
            if (!string.IsNullOrEmpty(keyword)) exp &= (_.Summary.Contains(keyword) | _.Title.Contains(keyword));
            
            PageParameter pageParameter = new PageParameter
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            var list = FindAll(exp, pageParameter);
            recordCount = pageParameter.TotalCount;
            return list;
        }

        public static EntityList<Posts> GetPostList(int rowCount, int categoryId, int userId, int recommend, int status,
            int topstatus, int hidestatus)
        {
            var exp = new WhereExpression();
            
            if (categoryId != -1) exp &= _.CategoryId == categoryId;
            if (userId != -1) exp &= _.UserId == userId;
            if (recommend != -1) exp &= _.Recommend == recommend;
            if (status != -1) exp &= _.Status == status;
            if (topstatus != -1) exp &= _.TopStatus == topstatus;
            if (hidestatus != -1) exp &= _.HideStatus == hidestatus;

            return FindAll(exp, null, null, 0, rowCount);
        }

        /// <summary>
        ///     获取归档
        /// </summary>
        public static List<ArchiveInfo> GetArchive()
        {
            var list = FindAll("[status]=1 and [hidestatus]=0 group by strftime('%Y%m',createdate)",
                "strftime('%Y%m',createdate) desc", "strftime('%Y%m',createdate) as [date] , count(*) as [ViewCount]", 0, 0);
            List<ArchiveInfo> listArchiveInfo = new List<ArchiveInfo>();
            foreach (var l in list)
            {
                ArchiveInfo archive = new ArchiveInfo
                {
                    Date = new DateTime(int.Parse(l.Date.Substring(0, 4)), int.Parse(l.Date.Substring(4,2)), 1),
                    Count = l.ViewCount
                };
                listArchiveInfo.Add(archive);
            }
            return listArchiveInfo;
        }
        #endregion
    }
}