using System;
using System.IO;
using System.Text;
using Loachs.Common;

namespace Loachs.Web
{
    public partial class admin_themeedit : AdminPage
    {
        /// <summary>
        ///     文件路径
        /// </summary>
        protected string FilePath = RequestHelper.QueryString("filepath");

        /// <summary>
        ///     主题名
        /// </summary>
        protected string ThemeName = RequestHelper.QueryString("themename");

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("主题编辑");

            if (!IsPostBack)
            {
                BindFile();
            }
        }

        /// <summary>
        ///     绑定文件
        /// </summary>
        protected void BindFile()
        {
            if (!string.IsNullOrEmpty(FilePath))
            {
                using (
                    StreamReader objReader =
                        new StreamReader(
                            Server.MapPath(ConfigHelper.SitePath + "themes/" + ThemeName + "/" + FilePath),
                            Encoding.UTF8))
                {
                    txtContent.Text = objReader.ReadToEnd();
                    objReader.Close();
                }
            }
            else
            {
                Response.Redirect("themeedit.aspx?themename=" + ThemeName + "&filepath=template/default.html");
            }
        }

        /// <summary>
        ///     修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string filepath = Server.MapPath(ConfigHelper.SitePath + "themes/" + ThemeName + "/" + FilePath);
            using (FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                byte[] info = Encoding.UTF8.GetBytes(txtContent.Text);
                fs.Write(info, 0, info.Length);
                fs.Close();
            }
            ShowMessage("保存成功!");
        }
    }
}