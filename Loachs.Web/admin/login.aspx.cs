using System;
using System.Web.Security;
using Loachs.Entity;
using StringHelper = Loachs.Common.StringHelper;

namespace Loachs.Web
{
    public partial class admin_login : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (PageUtils.IsLogin)
            {
                Response.Redirect("default.aspx");
            }
            Title = "登录 - Powered by Loachs";
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = StringHelper.HtmlEncode(txtUserName.Text.Trim());
            string password = StringHelper.GetMD5(txtPassword.Text.Trim());
            int expires = chkRemember.Checked ? 43200 : 0;
            string verifyCode = txtVerifyCode.Text;

            if (string.IsNullOrEmpty(verifyCode) || verifyCode != PageUtils.VerifyCode)
            {
                lblMessage.Text = "验证码错误!";
                return;
            }

            Users user = Users.Login(userName, password);

            if (user != null)
            {
                if (user.Status == 0)
                {
                    lblMessage.Text = "此用户已停用!";
                    return;
                }
                FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
                PageUtils.WriteUserCookie(user.Id, user.UserName, user.Password, expires);
                Response.Redirect("default.aspx");
            }
            else
            {
                lblMessage.Text = "用户名或密码错误!";
            }
        }
    }
}