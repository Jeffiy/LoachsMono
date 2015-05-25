using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Xml;

namespace Loachs.MetaWeblog
{
    /// <summary>
    ///     Object is the outgoing XML-RPC response.  This objects properties are set
    ///     and the Response method is called sending the response via the HttpContext Response.
    /// </summary>
    internal class XMLRPCResponse
    {
        #region Local Vars

        private readonly string _methodName;

        #endregion

        #region Contructors

        /// <summary>
        ///     Constructor sets default value
        /// </summary>
        /// <param name="methodName">MethodName of called XML-RPC method</param>
        public XMLRPCResponse(string methodName)
        {
            _methodName = methodName;
            Blogs = new List<MWABlogInfo>();
            Categories = new List<MWACategory>();
            Keywords = new List<string>();
            Posts = new List<MWAPost>();
            Pages = new List<MWAPage>();
            Authors = new List<MWAAuthor>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Response generates the XML-RPC response and returns it to the caller.
        /// </summary>
        /// <param name="context">httpContext.Response.OutputStream is used from the context</param>
        public void Response(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            using (XmlTextWriter data = new XmlTextWriter(context.Response.OutputStream, Encoding.UTF8))
            {
                data.Formatting = Formatting.Indented;
                data.WriteStartDocument();
                data.WriteStartElement("methodResponse");
                if (_methodName == "fault")
                    data.WriteStartElement("fault");
                else
                    data.WriteStartElement("params");

                switch (_methodName)
                {
                    case "metaWeblog.newPost":
                        WriteNewPost(data);
                        break;
                    case "metaWeblog.getPost":
                        WritePost(data);
                        break;
                    case "metaWeblog.newMediaObject":
                        WriteMediaInfo(data);
                        break;
                    case "metaWeblog.getCategories":
                        WriteGetCategories(data);
                        break;
                    case "metaWeblog.getRecentPosts":
                        WritePosts(data);
                        break;
                    case "blogger.getUsersBlogs":
                    case "metaWeblog.getUsersBlogs":
                        WriteGetUsersBlogs(data);
                        break;
                    case "metaWeblog.editPost":
                    case "blogger.deletePost":
                    case "wp.editPage":
                    case "wp.deletePage":
                        WriteBool(data);
                        break;
                    case "wp.newPage":
                        WriteNewPage(data);
                        break;
                    case "wp.getPage":
                        WritePage(data);
                        break;
                    case "wp.getPageList":
                        WriteShortPages(data);
                        break;
                    case "wp.getPages":
                        WritePages(data);
                        break;
                    case "wp.getAuthors":
                        WriteAuthors(data);
                        break;
                    case "wp.getTags":
                        WriteKeywords(data);
                        break;
                    case "fault":
                        WriteFault(data);
                        break;
                }

                data.WriteEndElement();
                data.WriteEndElement();
                data.WriteEndDocument();
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     List if author structs.  Used by wp.getAuthors.
        /// </summary>
        public List<MWAAuthor> Authors { get; set; }

        /// <summary>
        ///     List of blog structs.  Used by blogger.getUsersBlogs.
        /// </summary>
        public List<MWABlogInfo> Blogs { get; set; }

        /// <summary>
        ///     List of category structs. Used by metaWeblog.getCategories.
        /// </summary>
        public List<MWACategory> Categories { get; set; }

        /// <summary>
        ///     List of Tags.  Used by wp.getTags.
        /// </summary>
        public List<string> Keywords { get; set; }

        /// <summary>
        ///     Marks whether function call was completed and successful.
        ///     Used by metaWeblog.editPost and blogger.deletePost.
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        ///     Fault Struct. Used by API to return error information
        /// </summary>
        public MWAFault Fault { get; set; }

        /// <summary>
        ///     MediaInfo Struct
        /// </summary>
        public MWAMediaInfo MediaInfo { get; set; }

        /// <summary>
        ///     Metaweblog Post Struct. Used by metaWeblog.getPost
        /// </summary>
        public MWAPost Post { get; set; }

        /// <summary>
        ///     Id of post that was just added.  Used by metaWeblog.newPost
        /// </summary>
        public string PostID { get; set; }

        /// <summary>
        ///     Id of page that was just added.
        /// </summary>
        public string PageID { get; set; }

        /// <summary>
        ///     List of Metaweblog Post Structs.  Used by metaWeblog.getRecentPosts
        /// </summary>
        public List<MWAPost> Posts { get; set; }

        /// <summary>
        ///     List of Page Structs
        /// </summary>
        public List<MWAPage> Pages { get; set; }

        /// <summary>
        ///     MWAPage struct
        /// </summary>
        public MWAPage Page { get; set; }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Writes Fault Parameters of Response.
        /// </summary>
        /// <param name="data">xml response</param>
        private void WriteFault(XmlTextWriter data)
        {
            data.WriteStartElement("value");
            data.WriteStartElement("struct");

            // faultCode
            data.WriteStartElement("member");
            data.WriteElementString("name", "faultCode");
            data.WriteElementString("value", Fault.FaultCode);
            data.WriteEndElement();

            // faultString
            data.WriteStartElement("member");
            data.WriteElementString("name", "faultString");
            data.WriteElementString("value", Fault.FaultString);
            data.WriteEndElement();

            data.WriteEndElement();
            data.WriteEndElement();
        }

        /// <summary>
        ///     Writes Boolean parameter of Response
        /// </summary>
        /// <param name="data">xml response</param>
        private void WriteBool(XmlTextWriter data)
        {
            string postValue = "0";
            if (Completed)
                postValue = "1";
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteElementString("boolean", postValue);
            data.WriteEndElement();
            data.WriteEndElement();
        }

        /// <summary>
        ///     Writes the Array of Category structs parameters of Response
        /// </summary>
        /// <param name="data">xml response</param>
        private void WriteGetCategories(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("array");
            data.WriteStartElement("data");

            foreach (MWACategory category in Categories)
            {
                data.WriteStartElement("value");
                data.WriteStartElement("struct");

                // description
                data.WriteStartElement("member");
                data.WriteElementString("name", "description");
                data.WriteElementString("value", category.Description);
                data.WriteEndElement();

                // categoryid
                data.WriteStartElement("member");
                data.WriteElementString("name", "categoryid");
                data.WriteElementString("value", category.Id);
                data.WriteEndElement();

                // title
                data.WriteStartElement("member");
                data.WriteElementString("name", "title");
                data.WriteElementString("value", category.Title);
                data.WriteEndElement();

                // htmlUrl 
                data.WriteStartElement("member");
                data.WriteElementString("name", "htmlUrl");
                data.WriteElementString("value", category.HtmlUrl);
                data.WriteEndElement();

                // rssUrl
                data.WriteStartElement("member");
                data.WriteElementString("name", "rssUrl");
                data.WriteElementString("value", category.RssUrl);
                data.WriteEndElement();

                data.WriteEndElement();
                data.WriteEndElement();
            }

            // Close tags
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
        }

        /// <summary>
        ///     Writes the Array of Keyword structs parameters of Response
        /// </summary>
        /// <param name="data">xml response</param>
        private void WriteKeywords(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("array");
            data.WriteStartElement("data");

            foreach (string keyword in Keywords)
            {
                data.WriteStartElement("value");
                data.WriteStartElement("struct");

                // keywordName
                data.WriteStartElement("member");
                data.WriteElementString("name", "name");
                data.WriteElementString("value", keyword);
                data.WriteEndElement();

                data.WriteEndElement();
                data.WriteEndElement();
            }

            // Close tags
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
        }


        /// <summary>
        ///     Writes the MediaInfo Struct of Response
        /// </summary>
        /// <param name="data">xml response</param>
        private void WriteMediaInfo(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("struct");

            // url
            data.WriteStartElement("member");
            data.WriteElementString("name", "url");
            data.WriteStartElement("value");
            data.WriteElementString("string", MediaInfo.Url);
            data.WriteEndElement();
            data.WriteEndElement();

            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
        }

        /// <summary>
        ///     Writes the PostID string of Response.
        /// </summary>
        /// <param name="data">xml response</param>
        private void WriteNewPost(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteElementString("string", PostID);
            data.WriteEndElement();
            data.WriteEndElement();
        }

        /// <summary>
        ///     Writes the PageID string of Response.
        /// </summary>
        /// <param name="data">xml response</param>
        private void WriteNewPage(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteElementString("string", PageID);
            data.WriteEndElement();
            data.WriteEndElement();
        }

        /// <summary>
        ///     Writes the Metaweblog Post Struct of Response.
        /// </summary>
        /// <param name="data">xml response</param>
        private void WritePost(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("struct");

            // postid
            data.WriteStartElement("member");
            data.WriteElementString("name", "postid");
            data.WriteStartElement("value");
            data.WriteElementString("string", Post.PostId);
            data.WriteEndElement();
            data.WriteEndElement();

            // title
            data.WriteStartElement("member");
            data.WriteElementString("name", "title");
            data.WriteStartElement("value");
            data.WriteElementString("string", Post.Title);
            data.WriteEndElement();
            data.WriteEndElement();

            // description
            data.WriteStartElement("member");
            data.WriteElementString("name", "description");
            data.WriteStartElement("value");
            data.WriteElementString("string", Post.Description);
            data.WriteEndElement();
            data.WriteEndElement();

            // link
            data.WriteStartElement("member");
            data.WriteElementString("name", "link");
            data.WriteStartElement("value");
            data.WriteElementString("string", Post.Link);
            data.WriteEndElement();
            data.WriteEndElement();

            // slug
            data.WriteStartElement("member");
            data.WriteElementString("name", "wp_slug");
            data.WriteStartElement("value");
            data.WriteElementString("string", Post.Slug);
            data.WriteEndElement();
            data.WriteEndElement();

            // excerpt
            data.WriteStartElement("member");
            data.WriteElementString("name", "mt_excerpt");
            data.WriteStartElement("value");
            data.WriteElementString("string", Post.Excerpt);
            data.WriteEndElement();
            data.WriteEndElement();

            // comment policy
            data.WriteStartElement("member");
            data.WriteElementString("name", "mt_allow_comments");
            data.WriteStartElement("value");
            data.WriteElementString("int", Post.CommentPolicy);
            data.WriteEndElement();
            data.WriteEndElement();

            // dateCreated
            data.WriteStartElement("member");
            data.WriteElementString("name", "dateCreated");
            data.WriteStartElement("value");
            data.WriteElementString("dateTime.iso8601", ConvertDatetoISO8601(Post.PostDate));
            data.WriteEndElement();
            data.WriteEndElement();

            // publish
            data.WriteStartElement("member");
            data.WriteElementString("name", "publish");
            data.WriteStartElement("value");
            if (Post.Publish)
                data.WriteElementString("boolean", "1");
            else
                data.WriteElementString("boolean", "0");
            data.WriteEndElement();
            data.WriteEndElement();

            // tags (mt_keywords)
            data.WriteStartElement("member");
            data.WriteElementString("name", "mt_keywords");
            data.WriteStartElement("value");
            string[] tags = new string[Post.Tags.Count];
            for (int i = 0; i < Post.Tags.Count; i++)
            {
                tags[i] = Post.Tags[i];
            }
            string tagList = string.Join(",", tags);
            data.WriteElementString("string", tagList);
            data.WriteEndElement();
            data.WriteEndElement();

            // categories
            if (Post.Categories.Count > 0)
            {
                data.WriteStartElement("member");
                data.WriteElementString("name", "categories");
                data.WriteStartElement("value");
                data.WriteStartElement("array");
                data.WriteStartElement("data");
                foreach (string cat in Post.Categories)
                {
                    data.WriteStartElement("value");
                    data.WriteElementString("string", cat);
                    data.WriteEndElement();
                }
                data.WriteEndElement();
                data.WriteEndElement();
                data.WriteEndElement();
                data.WriteEndElement();
            }

            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
        }

        /// <summary>
        ///     Writes the Metaweblog Post Struct of Response.
        /// </summary>
        /// <param name="data">xml response</param>
        private void WritePage(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("struct");

            // pageid
            data.WriteStartElement("member");
            data.WriteElementString("name", "page_id");
            data.WriteStartElement("value");
            data.WriteElementString("string", Page.PageId);
            data.WriteEndElement();
            data.WriteEndElement();

            // title
            data.WriteStartElement("member");
            data.WriteElementString("name", "title");
            data.WriteStartElement("value");
            data.WriteElementString("string", Page.Title);
            data.WriteEndElement();
            data.WriteEndElement();

            // description
            data.WriteStartElement("member");
            data.WriteElementString("name", "description");
            data.WriteStartElement("value");
            data.WriteElementString("string", Page.Description);
            data.WriteEndElement();
            data.WriteEndElement();

            // link
            data.WriteStartElement("member");
            data.WriteElementString("name", "link");
            data.WriteStartElement("value");
            data.WriteElementString("string", Page.Link);
            data.WriteEndElement();
            data.WriteEndElement();

            // mt_convert_breaks
            data.WriteStartElement("member");
            data.WriteElementString("name", "mt_convert_breaks");
            data.WriteStartElement("value");
            data.WriteElementString("string", "__default__");
            data.WriteEndElement();
            data.WriteEndElement();

            // dateCreated
            data.WriteStartElement("member");
            data.WriteElementString("name", "dateCreated");
            data.WriteStartElement("value");
            data.WriteElementString("dateTime.iso8601", ConvertDatetoISO8601(Page.PageDate));
            data.WriteEndElement();
            data.WriteEndElement();

            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
        }

        /// <summary>
        ///     Writes the array of Metaweblog Post Structs of Response.
        /// </summary>
        /// <param name="data">xml response</param>
        private void WritePosts(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("array");
            data.WriteStartElement("data");

            foreach (MWAPost post in Posts)
            {
                data.WriteStartElement("value");
                data.WriteStartElement("struct");

                // postid
                data.WriteStartElement("member");
                data.WriteElementString("name", "postid");
                data.WriteStartElement("value");
                data.WriteElementString("string", post.PostId);
                data.WriteEndElement();
                data.WriteEndElement();

                // dateCreated
                data.WriteStartElement("member");
                data.WriteElementString("name", "dateCreated");
                data.WriteStartElement("value");
                data.WriteElementString("dateTime.iso8601", ConvertDatetoISO8601(post.PostDate));
                data.WriteEndElement();
                data.WriteEndElement();

                // title
                data.WriteStartElement("member");
                data.WriteElementString("name", "title");
                data.WriteStartElement("value");
                data.WriteElementString("string", post.Title);
                data.WriteEndElement();
                data.WriteEndElement();

                // description
                data.WriteStartElement("member");
                data.WriteElementString("name", "description");
                data.WriteElementString("value", post.Description);
                data.WriteEndElement();

                // link
                data.WriteStartElement("member");
                data.WriteElementString("name", "link");
                data.WriteStartElement("value");
                data.WriteElementString("string", post.Link);
                data.WriteEndElement();
                data.WriteEndElement();

                // slug
                data.WriteStartElement("member");
                data.WriteElementString("name", "wp_slug");
                data.WriteStartElement("value");
                data.WriteElementString("string", post.Slug);
                data.WriteEndElement();
                data.WriteEndElement();

                // excerpt
                data.WriteStartElement("member");
                data.WriteElementString("name", "mt_excerpt");
                data.WriteStartElement("value");
                data.WriteElementString("string", post.Excerpt);
                data.WriteEndElement();
                data.WriteEndElement();

                // comment policy
                data.WriteStartElement("member");
                data.WriteElementString("name", "mt_allow_comments");
                data.WriteStartElement("value");
                data.WriteElementString("string", post.CommentPolicy);
                data.WriteEndElement();
                data.WriteEndElement();

                // tags (mt_keywords)
                data.WriteStartElement("member");
                data.WriteElementString("name", "mt_keywords");
                data.WriteStartElement("value");
                string[] tags = new string[post.Tags.Count];
                for (int i = 0; i < post.Tags.Count; i++)
                {
                    tags[i] = post.Tags[i];
                }
                string tagList = string.Join(",", tags);
                data.WriteElementString("string", tagList);
                data.WriteEndElement();
                data.WriteEndElement();

                // publish
                data.WriteStartElement("member");
                data.WriteElementString("name", "publish");
                data.WriteStartElement("value");
                if (post.Publish)
                    data.WriteElementString("boolean", "1");
                else
                    data.WriteElementString("boolean", "0");
                data.WriteEndElement();
                data.WriteEndElement();

                // categories
                if (post.Categories.Count > 0)
                {
                    data.WriteStartElement("member");
                    data.WriteElementString("name", "categories");
                    data.WriteStartElement("value");
                    data.WriteStartElement("array");
                    data.WriteStartElement("data");
                    foreach (string cat in post.Categories)
                    {
                        data.WriteStartElement("value");
                        data.WriteElementString("string", cat);
                        data.WriteEndElement();
                    }
                    data.WriteEndElement();
                    data.WriteEndElement();
                    data.WriteEndElement();
                    data.WriteEndElement();
                }

                data.WriteEndElement();
                data.WriteEndElement();
            }

            // Close tags
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
        }

        private void WritePages(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("array");
            data.WriteStartElement("data");

            foreach (MWAPage page in Pages)
            {
                data.WriteStartElement("value");
                data.WriteStartElement("struct");

                // pageid
                data.WriteStartElement("member");
                data.WriteElementString("name", "page_id");
                data.WriteStartElement("value");
                data.WriteElementString("string", page.PageId);
                data.WriteEndElement();
                data.WriteEndElement();

                // title
                data.WriteStartElement("member");
                data.WriteElementString("name", "title");
                data.WriteStartElement("value");
                data.WriteElementString("string", page.Title);
                data.WriteEndElement();
                data.WriteEndElement();

                // description
                data.WriteStartElement("member");
                data.WriteElementString("name", "description");
                data.WriteStartElement("value");
                data.WriteElementString("string", page.Description);
                data.WriteEndElement();
                data.WriteEndElement();

                // link
                data.WriteStartElement("member");
                data.WriteElementString("name", "link");
                data.WriteStartElement("value");
                data.WriteElementString("string", page.Link);
                data.WriteEndElement();
                data.WriteEndElement();

                // mt_convert_breaks
                data.WriteStartElement("member");
                data.WriteElementString("name", "mt_convert_breaks");
                data.WriteStartElement("value");
                data.WriteElementString("string", "__default__");
                data.WriteEndElement();
                data.WriteEndElement();

                // dateCreated
                data.WriteStartElement("member");
                data.WriteElementString("name", "dateCreated");
                data.WriteStartElement("value");
                data.WriteElementString("dateTime.iso8601", ConvertDatetoISO8601(page.PageDate));
                data.WriteEndElement();
                data.WriteEndElement();

                // page_parent_id
                if (!string.IsNullOrEmpty(page.PageParentId))
                {
                    data.WriteStartElement("member");
                    data.WriteElementString("name", "page_parent_id");
                    data.WriteStartElement("value");
                    data.WriteElementString("string", page.PageParentId);
                    data.WriteEndElement();
                    data.WriteEndElement();
                }

                data.WriteEndElement();
                data.WriteEndElement();
            }

            // Close tags
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
        }

        private void WriteShortPages(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("array");
            data.WriteStartElement("data");

            foreach (MWAPage page in Pages)
            {
                data.WriteStartElement("value");
                data.WriteStartElement("struct");

                // pageid
                data.WriteStartElement("member");
                data.WriteElementString("name", "page_id");
                data.WriteStartElement("value");
                data.WriteElementString("string", page.PageId);
                data.WriteEndElement();
                data.WriteEndElement();

                // title
                data.WriteStartElement("member");
                data.WriteElementString("name", "page_title");
                data.WriteStartElement("value");
                data.WriteElementString("string", page.Title);
                data.WriteEndElement();
                data.WriteEndElement();

                // page_parent_id
                data.WriteStartElement("member");
                data.WriteElementString("name", "page_parent_id");
                data.WriteStartElement("value");
                data.WriteElementString("string", page.PageParentId);
                data.WriteEndElement();
                data.WriteEndElement();

                // dateCreated
                data.WriteStartElement("member");
                data.WriteElementString("name", "dateCreated");
                data.WriteStartElement("value");
                data.WriteElementString("dateTime.iso8601", ConvertDatetoISO8601(page.PageDate));
                data.WriteEndElement();
                data.WriteEndElement();

                // dateCreated gmt
                data.WriteStartElement("member");
                data.WriteElementString("name", "date_created_gmt");
                data.WriteStartElement("value");
                data.WriteElementString("dateTime.iso8601", ConvertDatetoISO8601(page.PageDate));
                data.WriteEndElement();
                data.WriteEndElement();

                data.WriteEndElement();
                data.WriteEndElement();
            }

            // Close tags
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
        }

        /// <summary>
        ///     Writes array of BlogInfo structs of Response
        /// </summary>
        /// <param name="data"></param>
        private void WriteGetUsersBlogs(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("array");
            data.WriteStartElement("data");

            foreach (MWABlogInfo blog in Blogs)
            {
                data.WriteStartElement("value");
                data.WriteStartElement("struct");

                // url
                data.WriteStartElement("member");
                data.WriteElementString("name", "url");
                data.WriteElementString("value", blog.Url);
                data.WriteEndElement();

                // blogid
                data.WriteStartElement("member");
                data.WriteElementString("name", "blogid");
                data.WriteElementString("value", blog.BlogId);
                data.WriteEndElement();

                // blogName
                data.WriteStartElement("member");
                data.WriteElementString("name", "blogName");
                data.WriteElementString("value", blog.BlogName);
                data.WriteEndElement();

                data.WriteEndElement();
                data.WriteEndElement();
            }

            // Close tags
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
        }

        /// <summary>
        ///     Convert Date to format expected by MetaWeblog Response.
        /// </summary>
        /// <param name="date">DateTime to convert</param>
        /// <returns>ISO8601 date string</returns>
        private string ConvertDatetoISO8601(DateTime date)
        {
            string temp = date.Year + date.Month.ToString().PadLeft(2, '0') + date.Day.ToString().PadLeft(2, '0') +
                          "T" + date.Hour.ToString().PadLeft(2, '0') + ":" + date.Minute.ToString().PadLeft(2, '0') +
                          ":" + date.Second.ToString().PadLeft(2, '0');
            return temp;
        }

        /// <summary>
        ///     Writes the Array of Category structs parameters of Response
        /// </summary>
        /// <param name="data">xml response</param>
        private void WriteAuthors(XmlTextWriter data)
        {
            data.WriteStartElement("param");
            data.WriteStartElement("value");
            data.WriteStartElement("array");
            data.WriteStartElement("data");

            foreach (MWAAuthor author in Authors)
            {
                data.WriteStartElement("value");
                data.WriteStartElement("struct");

                // user id
                data.WriteStartElement("member");
                data.WriteElementString("name", "user_id");
                data.WriteElementString("value", author.UserId);
                data.WriteEndElement();

                // login
                data.WriteStartElement("member");
                data.WriteElementString("name", "user_login");
                data.WriteElementString("value", author.UserLogin);
                data.WriteEndElement();

                // display name 
                data.WriteStartElement("member");
                data.WriteElementString("name", "display_name");
                data.WriteElementString("value", author.DisplayName);
                data.WriteEndElement();

                // user email
                data.WriteStartElement("member");
                data.WriteElementString("name", "user_email");
                data.WriteElementString("value", author.UserEmail);
                data.WriteEndElement();

                // meta value
                data.WriteStartElement("member");
                data.WriteElementString("name", "meta_value");
                data.WriteElementString("value", author.MetaValue);
                data.WriteEndElement();

                data.WriteEndElement();
                data.WriteEndElement();
            }

            // Close tags
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
            data.WriteEndElement();
        }

        #endregion
    }
}