using System;
using System.Web.Security;
using System.Web.UI;

namespace Loachs.Web
{
    public partial class admin_logout : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PageUtils.RemoveUserCookie();
            FormsAuthentication.SignOut();
            Response.Redirect("../");
        }
    }
}