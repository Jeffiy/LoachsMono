using System;
using System.Diagnostics;
using System.IO;
using System.Web;
using Loachs.Common;
using Loachs.Entity;
using NewLife;
using XCode;

namespace Loachs.Web
{
    public partial class admin_default : AdminPage
    {
        /// <summary>
        ///     文件夹大小
        /// </summary>
        public long DirSize;

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("首页");

            if (Request["Act"] == "Restart")
            {
                HttpRuntime.UnloadAppDomain();
                Response.Redirect(Path.GetFileName(Request.Path));
                return;
            }

            if (!IsPostBack)
            {
                CheckStatistics();

                commentlist = Comments.GetCommentList(15, 1, -1, -1, -1, (int) ApprovedStatus.Wait, -1,
                    string.Empty);
                //rptComment.DataSource = list;
                //rptComment.DataBind();
                //if (list.Count == 0)
                //{
                //    CommentMessage = "暂时无待审核评论";
                //}


                DbPath = ConfigHelper.SitePath + ConfigHelper.DbConnection;
                FileInfo file = new FileInfo(Server.MapPath(ConfigHelper.SitePath + ConfigHelper.DbConnection));
                DbSize = GetFileSize(file.Length);

                UpfilePath = ConfigHelper.SitePath + "upfiles";

                GetDirectorySize(Server.MapPath(UpfilePath));

                UpfileSize = GetFileSize(DirSize);

                GetDirectoryCount(Server.MapPath(UpfilePath));

            }

            ShowResult();

            if (string.IsNullOrEmpty(ResponseMessage))
            {
                if (PageUtils.IsIE6 || PageUtils.IsIE7)
                {
                    ShowError(
                        "小提示: 系统发现您正在使用老旧的浏览器(IE内核版本过低或启用了兼容模式) , 建议您升级或更换更标准更好体验的 <a href=\"http://www.microsoft.com/windows/internet-explorer/\" title=\"Microsoft Internet Explorer\" target=\"_blank\">IE8 +</a> , <a href=\"http://www.google.com/chrome?hl=zh-CN\" title=\"Google Chrome\" target=\"_blank\">Chrome</a> , <a href=\"http://www.mozillaonline.com/\" title=\"Mozilla Firefox\" target=\"_blank\">Firefox</a> , <a href=\"http://www.apple.com.cn/safari/\" title=\"Apple Safari\" target=\"_blank\">Safari</a>");
                }
            }
        }

        /// <summary>
        ///     显示结果
        /// </summary>
        protected void ShowResult()
        {
            int result = RequestHelper.QueryInt("result");
            switch (result)
            {
                case 11:
                    ShowMessage("统计分类文章数完成!");
                    break;
                case 12:
                    ShowMessage("统计标签文章数完成!");
                    break;
                case 13:
                    ShowMessage("统计作者文章和评论数完成!");
                    break;
                default:
                    break;
            }
        }

        protected void CheckStatistics()
        {
            Sites s = Sites.SiteInfo;
            bool update = false;

            int totalPosts = Posts.GetPostCount(-1, -1, -1, (int) PostStatus.Published, 0);
            if (totalPosts != s.PostCount)
            {
                s.PostCount = totalPosts;
                update = true;
            }

            int totalComments = Comments.GetCommentCount(true);
            if (totalComments != s.CommentCount)
            {
                s.CommentCount = totalComments;
                update = true;
            }
            int totalTags = Tags.FindCount();
            if (totalTags != s.TagCount)
            {
                s.TagCount = totalTags;
                update = true;
            }
            if (update)
            {
                s.Save();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="size">byte</param>
        /// <returns></returns>
        protected string GetFileSize(long size)
        {
            string fileSize = string.Empty;
            if (size > (1024*1024*1024))
                fileSize = ((double) size/(1024*1024*1024)).ToString(".##") + " GB";
            else if (size > (1024*1024))
                fileSize = ((double) size/(1024*1024)).ToString(".##") + " MB";
            else if (size > 1024)
                fileSize = ((double) size/1024).ToString(".##") + " KB";
            else if (size == 0)
                fileSize = "0 Byte";
            else
                fileSize = ((double) size/1).ToString(".##") + " Byte";

            return fileSize;
        }

        /// <summary>
        ///     递归文件夹大小
        /// </summary>
        /// <param name="dirp"></param>
        /// <returns></returns>
        private long GetDirectorySize(string dirp)
        {
            DirectoryInfo mydir = new DirectoryInfo(dirp);
            foreach (FileSystemInfo fsi in mydir.GetFileSystemInfos())
            {
                if (fsi is FileInfo)
                {
                    FileInfo fi = (FileInfo) fsi;
                    DirSize += fi.Length;
                }
                else
                {
                    DirectoryInfo di = (DirectoryInfo) fsi;
                    string newDir = di.FullName;
                    GetDirectorySize(newDir);
                }
            }
            return DirSize;
        }

        /// <summary>
        ///     递归文件数量
        /// </summary>
        /// <param name="dirp"></param>
        /// <returns></returns>
        private int GetDirectoryCount(string dirp)
        {
            DirectoryInfo mydir = new DirectoryInfo(dirp);
            foreach (FileSystemInfo fsi in mydir.GetFileSystemInfos())
            {
                if (fsi is FileInfo)
                {
                    UpfileCount += 1;
                }
                else
                {
                    DirectoryInfo di = (DirectoryInfo) fsi;
                    string newDir = di.FullName;
                    GetDirectoryCount(newDir);
                }
            }
            return UpfileCount;
        }

        protected void btnCategory_Click(object sender, EventArgs e)
        {
            EntityList<Categorys> list = Categorys.FindAllWithCache();
            foreach (Categorys category in list)
            {
                category.Count = Posts.GetPostCount(category.Id, -1, -1, (int)PostStatus.Published, 0);
                category.Save();
            }
            Response.Redirect("default.aspx?result=11");
        }

        protected void btnTag_Click(object sender, EventArgs e)
        {
            var list = Tags.FindAllWithCache();
            foreach (Tags tag in list)
            {
                tag.Count = Posts.GetPostCount(-1, tag.Id, -1, (int)PostStatus.Published, 0);
                tag.Save();
            }
            Response.Redirect("default.aspx?result=12");
        }

        protected void btnUser_Click(object sender, EventArgs e)
        {
            var list = Users.FindAllWithCache();
            foreach (Users user in list)
            {
                user.PostCount = Posts.GetPostCount(-1, -1, user.Id, (int)PostStatus.Published, 0);
                user.CommentCount = Comments.GetCommentCount(user.Id, -1, false);
                user.Save();
            }
            Response.Redirect("default.aspx?result=13");
        }
        
        protected String GetWebServerName()
        {
            String name = Request.ServerVariables["Server_SoftWare"];
            if (String.IsNullOrEmpty(name)) name = Process.GetCurrentProcess().ProcessName;

            // 检测集成管道，低版本.Net不支持，请使用者根据情况自行注释
            try
            {
                if (UsingIntegratedPipeline()) name += " [集成管道]";
            }
            catch { }

            return name;
        }

        Boolean UsingIntegratedPipeline()
        {
            return HttpRuntime.UsingIntegratedPipeline;
        }
        
        #region 变量

        /// <summary>
        ///     数据库大小
        /// </summary>
        protected string DbSize;

        /// <summary>
        ///     数据库路径
        /// </summary>
        protected string DbPath;

        /// <summary>
        ///     附件大小
        /// </summary>
        protected string UpfileSize;

        /// <summary>
        ///     附件路径
        /// </summary>
        protected string UpfilePath;

        /// <summary>
        ///     附件个数
        /// </summary>
        protected int UpfileCount;

        protected EntityList<Comments> commentlist;

        #endregion
    }
}