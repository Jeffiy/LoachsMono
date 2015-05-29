using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Loachs.Common;
using Loachs.Entity;
using StringHelper = Loachs.Common.StringHelper;

namespace Loachs.Web
{
    public partial class admin_postedit : AdminPage
    {
        /// <summary>
        ///     默认分类ID
        /// </summary>
        protected int CategoryId = RequestHelper.QueryInt("categoryid", 0);

        protected string HeaderTitle = "添加文章";

        /// <summary>
        ///     提示
        /// </summary>
        protected int Message = RequestHelper.QueryInt("message", 0);

        /// <summary>
        ///     ID
        /// </summary>
        protected int PostId = RequestHelper.QueryInt("postid", 0);

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("添加文章");


            if (!IsPostBack)
            {
                LoadDefault();

                if (Operate == OperateType.Update)
                {
                    BindPost();
                    HeaderTitle = "修改文章";
                    btnEdit.Text = "修改";
                    SetPageTitle("修改文章");
                    switch (Message)
                    {
                        case 1:
                            ShowMessage(string.Format("添加成功! <a href=\"{0}\">{0}</a>", Posts.FindById(PostId).Url));
                            break;
                        case 2:
                            ShowMessage(string.Format("修改成功! <a href=\"{0}\">{0}</a>", Posts.FindById(PostId).Url));
                            break;
                    }
                }
                //else if (Action == "delete")
                //{
                //    DeleteArticle();
                //}
                //else
                //{
                //    ddlCategory.SelectedValue = CategoryID.ToString();
                //}
            }
        }

        /// <summary>
        ///     加载默认数据
        /// </summary>
        protected void LoadDefault()
        {
            List<Categorys> list = Categorys.FindAllWithCache();
            ddlCategory.Items.Clear();
            ddlCategory.Items.Add(new ListItem("无分类", "0"));
            foreach (Categorys c in list)
            {
                ddlCategory.Items.Add(new ListItem(c.Name + " (" + c.Count + ") ", c.Id.ToString()));
            }

            ddlUrlType.Items.Clear();
            //  ddlUrlFormat.Items.Add(new ListItem("默认(由程序决定)", "0"));
            ddlUrlType.Items.Add(
                new ListItem(
                    ConfigHelper.SiteUrl + "post/" + DateTime.Now.ToString(@"yyyy\/MM\/dd") + "/slug" +
                    Setting.RewriteExtension, "1"));
            ddlUrlType.Items.Add(new ListItem(ConfigHelper.SiteUrl + "post/slug" + Setting.RewriteExtension, "2"));

            //   ddlUrlType.Items.Add(new ListItem(ConfigHelper.AppUrl + "post/分类别名/别名或ID" + setting.RewriteExtension, "3"));

            ddlTemplate.Items.Clear();
            DirectoryInfo dir =
                new DirectoryInfo(Server.MapPath(ConfigHelper.SitePath + "themes/" + Setting.Theme + "/template"));
            foreach (FileInfo file in dir.GetFiles("post*", SearchOption.TopDirectoryOnly))
            {
                ddlTemplate.Items.Add(new ListItem(file.Name));
            }
        }

        /// <summary>
        ///     绑定实体
        /// </summary>
        protected void BindPost()
        {
            Posts p = Posts.FindById(PostId);
            if (p != null)
            {
                txtTitle.Text = StringHelper.HtmlDecode(p.Title);
                txtSummary.Text = p.Summary;
                txtContents.Text = p.Content;
                chkStatus.Checked = p.Status == 1;
                ddlCategory.SelectedValue = p.CategoryId.ToString();
                chkCommentStatus.Checked = p.CommentStatus == 1;

                txtCustomUrl.Text = StringHelper.HtmlDecode(p.Slug);

                chkTopStatus.Checked = p.TopStatus == 1;
                chkRecommend.Checked = p.Recommend == 1;
                chkHideStatus.Checked = p.HideStatus == 1;

                ddlUrlType.SelectedValue = p.UrlFormat.ToString();
                ddlTemplate.SelectedValue = p.Template;

                //绑定标签,需改进
                string tag = p.Tag;
                if (!string.IsNullOrWhiteSpace(tag))
                {
                    tag = tag.Replace("{", "");
                    string[] taglist = tag.Split('}');
                    foreach (
                        Tags tagInfo in
                            taglist.Select(tagId => Tags.FindById(StringHelper.StrToInt(tagId)))
                                .Where(taginfo => taginfo != null))
                    {
                        txtTags.Text += tagInfo.Name + ",";
                    }
                    txtTags.Text = txtTags.Text.TrimEnd(',');
                }
                if (p.UserId != PageUtils.CurrentUser.Id &&
                    PageUtils.CurrentUser.Type != (int) UserType.Administrator)
                {
                    Response.Redirect("postlist.aspx?result=444");
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Posts p = new Posts();

            if (Operate == OperateType.Update)
            {
                p = Posts.FindById(PostId);
            }
            else
            {
                p.CommentCount = 0;
                p.ViewCount = 0;
                p.CreateDate = DateTime.Now;
                p.UserId = PageUtils.CurrentUserId;
            }

            p.CategoryId = StringHelper.StrToInt(ddlCategory.SelectedValue, 0);
            p.CommentStatus = chkCommentStatus.Checked ? 1 : 0;
            p.Content = txtContents.Text;
            p.Slug = PageUtils.FilterSlug(txtCustomUrl.Text, "post", true);
            p.Status = chkStatus.Checked ? 1 : 0;
            p.TopStatus = chkTopStatus.Checked ? 1 : 0;
            p.HideStatus = chkHideStatus.Checked ? 1 : 0;
            p.Summary = txtSummary.Text;
            p.Tag = GetTagIdList(txtTags.Text.Trim());
            p.Title = StringHelper.HtmlEncode(txtTitle.Text);
            //  p.TopStatus = chkTopStatus.Checked ? 1 : 0;

            p.UrlFormat = StringHelper.StrToInt(ddlUrlType.SelectedValue, 1);
            p.Template = ddlTemplate.SelectedValue;
            p.Recommend = chkRecommend.Checked ? 1 : 0;


            //  p.Type = 0;// (int)PostType.Article;
            p.UpdateDate = DateTime.Now;

            if (chkSaveImage.Checked)
            {
                p.Content = SaveRemoteImage(p.Content);
            }

            if (Operate == OperateType.Update)
            {
                p.Update();
                //  TagManager.ResetTag(oldTag + p.Tag);
                Response.Redirect("postedit.aspx?operate=update&postid=" + PostId + "&message=2");
            }
            else
            {
                p.Insert();

                SendEmail(p);

                // TagManager.ResetTag(p.Tag);

                Response.Redirect("postedit.aspx?operate=update&postid=" + p.Id + "&message=1");
            }
        }

        /// <summary>
        ///     发邮件
        /// </summary>
        /// <param name="post"></param>
        private void SendEmail(Posts post)
        {
            var site = Sites.GetSetting();
            if (site.SendMailAuthorByPost == 1)
            {
                var list = Users.FindAllWithCache().ToList();
                List<string> emailList = new List<string>();

                foreach (Users user in from user in list where StringHelper.IsEmail(user.Email) where PageUtils.CurrentUser.Email != user.Email where !emailList.Contains(user.Email) select user)
                {
                    emailList.Add(user.Email);

                    var subject = string.Format("[新文章通知]{0}", post.Title);
                    string body = string.Format("{0}有新文章了:<br/>", site.SiteName);
                    body += "<hr/>";
                    body += "<br />标题: " + post.Link;
                    body += post.Detail;
                    body += "<hr/>";
                    body += "<br />作者: " + PageUtils.CurrentUser.Link;
                    body += "<br />时间: " + post.CreateDate;
                    body += "<br />文章连接: " + post.Link;
                    body += "<br />注:系统自动通知邮件,不要回复。";

                    EmailHelper.SendAsync(user.Email, subject, body);
                }
            }
        }

        /// <summary>
        ///     由标签名称列表返回标签ID列表,带{},新标签自动添加
        /// </summary>
        /// <param name="tagNames"></param>
        /// <returns></returns>
        protected string GetTagIdList(string tagNames)
        {
            if (string.IsNullOrEmpty(tagNames))
            {
                return string.Empty;
            }
            string tagIds = string.Empty;
            tagNames = tagNames.Replace("，", ",");

            string[] names = tagNames.Split(',');

            foreach (string n in names)
            {
                if (!string.IsNullOrEmpty(n))
                {
                    Tags t = Tags.FindByName(n);

                    //if (t == null)
                    //{
                    //    t = TagManager.GetTagBySlug(n);
                    //}

                    //  int tagId = TagManager.GetTagId(n);

                    if (t == null)
                    {
                        t = new Tags
                        {
                            Count = 0,
                            CreateDate = DateTime.Now,
                            Description = n,
                            DisplayOrder = 1000,
                            Name = n,
                            Slug = StringHelper.HtmlEncode(PageUtils.FilterSlug(n, "tag"))
                        };

                        t.Save();
                    }
                    tagIds += "{" + t.Id + "}";
                }
            }
            return tagIds;
        }

        /// <summary>
        ///     保存远程图片
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private string SaveRemoteImage(string html)
        {
            string Reg = @"<img.*src=.?(http|https).+>";
            string currentHost = Request.Url.Host;
            // <img.*?src="(?<url>.*?)".*?>
            List<Uri> urlList = (from Match m in Regex.Matches(html, Reg, RegexOptions.IgnoreCase | RegexOptions.Compiled) let reg = new Regex(@"src=('|"")?(http|https).+?('|""|>| )+?", RegexOptions.IgnoreCase) select reg.Match(m.Value).Value.Replace("src=", "").Replace("'", "").Replace("\"", "").Replace(@">", "") into imgUrl select new Uri(imgUrl) into u where u.Host != currentHost select u).ToList();

            //获取图片URL地址

            //去掉重复
            List<Uri> urlList2 = new List<Uri>();
            foreach (Uri u2 in urlList.Where(u2 => !urlList2.Contains(u2)))
            {
                urlList2.Add(u2);
            }

            //保存
            WebClient wc = new WebClient();
            int i = 0;
            foreach (Uri u2 in urlList2)
            {
                i++;
                string extName = ".jpg";
                if (Path.HasExtension(u2.AbsoluteUri))
                {
                    extName = Path.GetExtension(u2.AbsoluteUri);
                    if (extName.IndexOf('?') >= 0)
                    {
                        extName = extName.Substring(0, extName.IndexOf('?'));
                    }
                }

                string path = ConfigHelper.SitePath + "upfiles/" + DateTime.Now.ToString("yyyyMM") + "/";


                if (!Directory.Exists(Server.MapPath(path)))
                {
                    Directory.CreateDirectory(Server.MapPath(path));
                }
                //  Response.Write(newDir);

                string newFileName = path + "auto_" + DateTime.Now.ToString("ddHHmmss") + i + extName;

                wc.DownloadFile(u2, Server.MapPath(newFileName)); //非图片后缀要改成图片后缀
                //  Response.Write(u2.AbsoluteUri + "||<br>");

                //是否合法
                if (IsAllowedImage(Server.MapPath(newFileName)))
                {
                    html = html.Replace(u2.AbsoluteUri, newFileName);
                }
                else
                {
                    File.Delete(Server.MapPath(newFileName));
                }
            }
            return html;
        }

        /// <summary>
        ///     检查是否为允许的图片格式
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private bool IsAllowedImage(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            string fileclass = "";
            try
            {
                byte buffer = r.ReadByte();
                fileclass = buffer.ToString();
                buffer = r.ReadByte();
                fileclass += buffer.ToString();
            }
            catch
            {
                return false;
            }
            r.Close();
            fs.Close();
            /*文件扩展名说明
             *7173        gif 
             *255216      jpg
             *13780       png
             *6677        bmp
             *239187      txt,aspx,asp,sql
             *208207      xls.doc.ppt
             *6063        xml
             *6033        htm,html
             *4742        js
             *8075        xlsx,zip,pptx,mmap,zip
             *8297        rar   
             *01          accdb,mdb
             *7790        exe,dll           
             *5666        psd 
             *255254      rdp 
             *10056       bt种子 
             *64101       bat 
             */


            // String[] fileType = { "255216", "7173", "6677", "13780", "8297", "5549", "870", "87111", "8075" };
            string[] fileType = {"255216", "7173", "6677", "13780"};

            return fileType.Any(t => fileclass == t);
        }
    }
}