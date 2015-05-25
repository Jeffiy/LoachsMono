﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace Loachs.MetaWeblog
{
    /// <summary>
    ///     Obejct is the incoming XML-RPC Request.  Handles parsing the XML-RPC and
    ///     fills its properties with the values sent in the request.
    /// </summary>
    internal class XMLRPCRequest
    {
        #region Local Vars

        private List<XmlNode> _inputParams;

        #endregion

        #region Contructors

        /// <summary>
        ///     Loads XMLRPCRequest object from HttpContext
        /// </summary>
        /// <param name="input">incoming HttpContext</param>
        public XMLRPCRequest(HttpContext input)
        {
            string inputXML = ParseRequest(input);
            //LogMetaWeblogCall(inputXML);
            LoadXMLRequest(inputXML); // Loads Method Call and Associated Variables
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Name of Called Metaweblog Function
        /// </summary>
        public string MethodName { get; private set; }

        /// <summary>
        ///     AppKey is a key generated by the calling application.  It is sent with blogger API calls.
        /// </summary>
        /// <remarks>
        ///     BlogEngine.NET doesn't require specific AppKeys for API calls.  It is no longer standard practive.
        /// </remarks>
        public string AppKey { get; private set; }

        /// <summary>
        ///     ID of the Blog to call the function on.  Since BlogEngine supports only a single blog instance,
        ///     this incoming parameter is not used.
        /// </summary>
        public string BlogID { get; private set; }

        /// <summary>
        ///     MediaObject is a struct sent by the metaWeblog.newMediaObject function.
        ///     It contains information about the media and the object in a bit array.
        /// </summary>
        public MWAMediaObject MediaObject { get; private set; }

        /// <summary>
        ///     Number of post request by the metaWeblog.getRecentPosts function
        /// </summary>
        public int NumberOfPosts { get; private set; }

        /// <summary>
        ///     Password for user validation
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        ///     Metaweblog Post struct containing information post including title, content, and categories.
        /// </summary>
        public MWAPost Post { get; private set; }

        /// <summary>
        ///     Metaweblog Page Struct
        /// </summary>
        public MWAPage Page { get; private set; }

        /// <summary>
        ///     The PostID Guid in string format
        /// </summary>
        public string PostID { get; private set; }

        /// <summary>
        ///     PageID Guid in string format
        /// </summary>
        public string PageID { get; private set; }

        /// <summary>
        ///     Publish determines wheter or not a post will be marked as published by BlogEngine.
        /// </summary>
        public bool Publish { get; private set; }

        /// <summary>
        ///     Login for user validation
        /// </summary>
        public string UserName { get; private set; }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Retrieves the content of the input stream
        ///     and return it as plain text.
        /// </summary>
        private string ParseRequest(HttpContext context)
        {
            byte[] buffer = new byte[context.Request.InputStream.Length];
            context.Request.InputStream.Read(buffer, 0, buffer.Length);

            return Encoding.UTF8.GetString(buffer);
        }

        /// <summary>
        ///     Loads object properties with contents of passed xml
        /// </summary>
        /// <param name="xml">xml doc with methodname and parameters</param>
        private void LoadXMLRequest(string xml)
        {
            XmlDocument request = new XmlDocument();
            try
            {
                if (!(xml.StartsWith("<?xml") || xml.StartsWith("<method")))
                {
                    xml = xml.Substring(xml.IndexOf("<?xml"));
                }
                request.LoadXml(xml);
            }
            catch (Exception ex)
            {
                throw new MetaWeblogException("01", "Invalid XMLRPC Request. (" + ex.Message + ")");
            }

            // Method name is always first
            MethodName = request.DocumentElement.ChildNodes[0].InnerText;

            // Parameters are next (and last)
            _inputParams = new List<XmlNode>();
            foreach (XmlNode node in request.SelectNodes("/methodCall/params/param"))
            {
                _inputParams.Add(node);
            }

            // Determine what params are what by method name
            switch (MethodName)
            {
                case "metaWeblog.newPost":
                    BlogID = _inputParams[0].InnerText;
                    UserName = _inputParams[1].InnerText;
                    Password = _inputParams[2].InnerText;
                    Post = GetPost(_inputParams[3]);
                    if (_inputParams[4].InnerText == "0" || _inputParams[4].InnerText == "false")
                        Publish = false;
                    else
                        Publish = true;
                    break;
                case "metaWeblog.editPost":
                    PostID = _inputParams[0].InnerText;
                    UserName = _inputParams[1].InnerText;
                    Password = _inputParams[2].InnerText;
                    Post = GetPost(_inputParams[3]);
                    if (_inputParams[4].InnerText == "0" || _inputParams[4].InnerText == "false")
                        Publish = false;
                    else
                        Publish = true;
                    break;
                case "metaWeblog.getPost":
                    PostID = _inputParams[0].InnerText;
                    UserName = _inputParams[1].InnerText;
                    Password = _inputParams[2].InnerText;
                    break;
                case "metaWeblog.newMediaObject":
                    BlogID = _inputParams[0].InnerText;
                    UserName = _inputParams[1].InnerText;
                    Password = _inputParams[2].InnerText;
                    MediaObject = GetMediaObject(_inputParams[3]);
                    break;
                case "metaWeblog.getCategories":
                case "wp.getAuthors":
                case "wp.getPageList":
                case "wp.getPages":
                case "wp.getTags":
                    BlogID = _inputParams[0].InnerText;
                    UserName = _inputParams[1].InnerText;
                    Password = _inputParams[2].InnerText;
                    break;
                case "metaWeblog.getRecentPosts":
                    BlogID = _inputParams[0].InnerText;
                    UserName = _inputParams[1].InnerText;
                    Password = _inputParams[2].InnerText;
                    NumberOfPosts = int.Parse(_inputParams[3].InnerText, CultureInfo.InvariantCulture);
                    break;
                case "blogger.getUsersBlogs":
                case "metaWeblog.getUsersBlogs":
                    AppKey = _inputParams[0].InnerText;
                    UserName = _inputParams[1].InnerText;
                    Password = _inputParams[2].InnerText;
                    break;
                case "blogger.deletePost":
                    AppKey = _inputParams[0].InnerText;
                    PostID = _inputParams[1].InnerText;
                    UserName = _inputParams[2].InnerText;
                    Password = _inputParams[3].InnerText;
                    if (_inputParams[4].InnerText == "0" || _inputParams[4].InnerText == "false")
                        Publish = false;
                    else
                        Publish = true;
                    break;
                case "blogger.getUserInfo":
                    AppKey = _inputParams[0].InnerText;
                    UserName = _inputParams[1].InnerText;
                    Password = _inputParams[2].InnerText;
                    break;
                case "wp.newPage":
                    BlogID = _inputParams[0].InnerText;
                    UserName = _inputParams[1].InnerText;
                    Password = _inputParams[2].InnerText;
                    Page = GetPage(_inputParams[3]);
                    if (_inputParams[4].InnerText == "0" || _inputParams[4].InnerText == "false")
                        Publish = false;
                    else
                        Publish = true;
                    break;
                case "wp.getPage":
                    BlogID = _inputParams[0].InnerText;
                    PageID = _inputParams[1].InnerText;
                    UserName = _inputParams[2].InnerText;
                    Password = _inputParams[3].InnerText;
                    break;
                case "wp.editPage":
                    BlogID = _inputParams[0].InnerText;
                    PageID = _inputParams[1].InnerText;
                    UserName = _inputParams[2].InnerText;
                    Password = _inputParams[3].InnerText;
                    Page = GetPage(_inputParams[4]);
                    if (_inputParams[5].InnerText == "0" || _inputParams[5].InnerText == "false")
                        Publish = false;
                    else
                        Publish = true;
                    break;
                case "wp.deletePage":
                    BlogID = _inputParams[0].InnerText;
                    UserName = _inputParams[1].InnerText;
                    Password = _inputParams[2].InnerText;
                    PageID = _inputParams[3].InnerText;
                    break;
                default:
                    throw new MetaWeblogException("02", "未知方法. (" + MethodName + ")");
            }
        }

        /// <summary>
        ///     Creates a Metaweblog Post object from the XML struct
        /// </summary>
        /// <param name="node">XML contains a Metaweblog Post Struct</param>
        /// <returns>Metaweblog Post Struct Obejct</returns>
        private MWAPost GetPost(XmlNode node)
        {
            MWAPost temp = new MWAPost();
            List<string> cats = new List<string>();
            List<string> tags = new List<string>();

            // Require Title and Description
            try
            {
                temp.Title = node.SelectSingleNode("value/struct/member[name='title']").LastChild.InnerText;
                temp.Description = node.SelectSingleNode("value/struct/member[name='description']").LastChild.InnerText;
            }
            catch (Exception ex)
            {
                throw new MetaWeblogException("05",
                    "Post Struct Element, Title or Description,  not Sent. (" + ex.Message + ")");
            }
            if (node.SelectSingleNode("value/struct/member[name='link']") == null)
                temp.Link = "";
            else
                temp.Link = node.SelectSingleNode("value/struct/member[name='link']").LastChild.InnerText;

            if (node.SelectSingleNode("value/struct/member[name='mt_allow_comments']") == null)
                temp.CommentPolicy = "";
            else
                temp.CommentPolicy =
                    node.SelectSingleNode("value/struct/member[name='mt_allow_comments']").LastChild.InnerText;

            if (node.SelectSingleNode("value/struct/member[name='mt_excerpt']") == null)
                temp.Excerpt = "";
            else
                temp.Excerpt = node.SelectSingleNode("value/struct/member[name='mt_excerpt']").LastChild.InnerText;

            if (node.SelectSingleNode("value/struct/member[name='wp_slug']") == null)
                temp.Slug = "";
            else
                temp.Slug = node.SelectSingleNode("value/struct/member[name='wp_slug']").LastChild.InnerText;

            if (node.SelectSingleNode("value/struct/member[name='wp_author_id']") == null)
                temp.Author = "";
            else
                temp.Author = node.SelectSingleNode("value/struct/member[name='wp_author_id']").LastChild.InnerText;

            if (node.SelectSingleNode("value/struct/member[name='categories']") != null)
            {
                XmlNode categoryArray = node.SelectSingleNode("value/struct/member[name='categories']").LastChild;
                cats.AddRange(from XmlNode catnode in categoryArray.SelectNodes("array/data/value/string")
                    select catnode.InnerText);
            }
            temp.Categories = cats;

            // postDate has a few different names to worry about
            if (node.SelectSingleNode("value/struct/member[name='dateCreated']") != null)
            {
                try
                {
                    string tempDate =
                        node.SelectSingleNode("value/struct/member[name='dateCreated']").LastChild.InnerText;
                    temp.PostDate = DateTime.ParseExact(tempDate, "yyyyMMdd'T'HH':'mm':'ss",
                        CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                }
                catch
                {
                    // Ignore PubDate Error
                }
            }
            else if (node.SelectSingleNode("value/struct/member[name='pubDate']") != null)
            {
                try
                {
                    string tempPubDate =
                        node.SelectSingleNode("value/struct/member[name='pubDate']").LastChild.InnerText;
                    temp.PostDate = DateTime.ParseExact(tempPubDate, "yyyyMMdd'T'HH':'mm':'ss",
                        CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                }
                catch
                {
                    // Ignore PubDate Error
                }
            }

            // WLW tags implementation using mt_keywords
            if (node.SelectSingleNode("value/struct/member[name='mt_keywords']") != null)
            {
                string tagsList = node.SelectSingleNode("value/struct/member[name='mt_keywords']").LastChild.InnerText;
                foreach (string item in tagsList.Split(','))
                {
                    if (
                        string.IsNullOrEmpty(
                            tags.Find(
                                t => t.Equals(item.Trim(), StringComparison.OrdinalIgnoreCase))))
                    {
                        tags.Add(item.Trim());
                    }
                }
            }
            temp.Tags = tags;

            return temp;
        }

        /// <summary>
        ///     Creates a Metaweblog Page object from the XML struct
        /// </summary>
        /// <param name="node">XML contains a Metaweblog Page Struct</param>
        /// <returns>Metaweblog Page Struct Obejct</returns>
        private MWAPage GetPage(XmlNode node)
        {
            MWAPage temp = new MWAPage();

            // Require Title and Description
            try
            {
                temp.Title = node.SelectSingleNode("value/struct/member[name='title']").LastChild.InnerText;
                temp.Description = node.SelectSingleNode("value/struct/member[name='description']").LastChild.InnerText;
            }
            catch (Exception ex)
            {
                throw new MetaWeblogException("06",
                    "Page Struct Element, Title or Description,  not Sent. (" + ex.Message + ")");
            }
            if (node.SelectSingleNode("value/struct/member[name='link']") == null)
                temp.Link = "";
            else
                temp.Link = node.SelectSingleNode("value/struct/member[name='link']").LastChild.InnerText;

            if (node.SelectSingleNode("value/struct/member[name='dateCreated']") != null)
            {
                try
                {
                    string tempDate =
                        node.SelectSingleNode("value/struct/member[name='dateCreated']").LastChild.InnerText;
                    temp.PageDate = DateTime.ParseExact(tempDate, "yyyyMMdd'T'HH':'mm':'ss",
                        CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                }
                catch
                {
                    // Ignore PubDate Error
                }
            }

            //Keywords
            if (node.SelectSingleNode("value/struct/member[name='mt_keywords']") == null)
                temp.MtKeywords = "";
            else
                temp.MtKeywords = node.SelectSingleNode("value/struct/member[name='mt_keywords']").LastChild.InnerText;

            if (node.SelectSingleNode("value/struct/member[name='wp_page_parent_id']") != null)
                temp.PageParentId =
                    node.SelectSingleNode("value/struct/member[name='wp_page_parent_id']").LastChild.InnerText;

            return temp;
        }

        /// <summary>
        ///     Creates a Metaweblog Media object from the XML struct
        /// </summary>
        /// <param name="node">XML contains a Metaweblog MediaObject Struct</param>
        /// <returns>Metaweblog MediaObject Struct Obejct</returns>
        private MWAMediaObject GetMediaObject(XmlNode node)
        {
            MWAMediaObject temp = new MWAMediaObject();
            temp.Name = node.SelectSingleNode("value/struct/member[name='name']").LastChild.InnerText;
            if (node.SelectSingleNode("value/struct/member[name='type']") == null)
                temp.Type = "notsent";
            else
                temp.Type = node.SelectSingleNode("value/struct/member[name='type']").LastChild.InnerText;
            temp.Bits =
                Convert.FromBase64String(node.SelectSingleNode("value/struct/member[name='bits']").LastChild.InnerText);

            return temp;
        }

        private void LogMetaWeblogCall(string message)
        {
            //   string saveFolder = System.Web.HttpContext.Current.Server.MapPath(BlogSettings.Instance.StorageLocation);
            string saveFolder = HttpContext.Current.Server.MapPath("/");
            string saveFile = Path.Combine(saveFolder, "lastmetaweblogcall.txt");

            try
            {
                // Save message to file
                using (FileStream fileWrtr = new FileStream(saveFile, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter streamWrtr = new StreamWriter(fileWrtr))
                    {
                        streamWrtr.WriteLine(message);
                        streamWrtr.Close();
                    }
                    fileWrtr.Close();
                }
            }
            catch
            {
                // Ignore all errors
            }
        }

        #endregion
    }
}