using System.IO;
using System.Web;
using System.Web.Security;
using Loachs.Common;
using Loachs.Entity;

/// <summary>
///     操作类型
/// </summary>
public enum OperateType
{
    /// <summary>
    ///     添加
    /// </summary>
    Insert = 0,

    /// <summary>
    ///     更新
    /// </summary>
    Update = 1,

    /// <summary>
    ///     删除
    /// </summary>
    Delete = 2
}

namespace Loachs.Web
{
    /// <summary>
    ///     后台基类
    /// </summary>
    public class AdminPage : PageBase
    {
        /// <summary>
        ///     操作
        /// </summary>
        private readonly string _operate = RequestHelper.QueryString("Operate");

        /// <summary>
        ///     输出信息
        /// </summary>
        protected string ResponseMessage = string.Empty;

        protected SettingInfo Setting;

        public AdminPage()
        {
            CheckLoginAndPermission();
            Setting = Sites.GetSetting();
        }

        /// <summary>
        ///     操作类型
        /// </summary>
        public OperateType Operate
        {
            get
            {
                switch (_operate)
                {
                    case "update":
                        return OperateType.Update;
                    case "delete":
                        return OperateType.Delete;
                    default:
                        return OperateType.Insert;
                }
            }
        }

        /// <summary>
        ///     操作字符串
        /// </summary>
        public string OperateString
        {
            get { return _operate; }
        }

        /// <summary>
        ///     检查登录和权限
        /// </summary>
        protected void CheckLoginAndPermission()
        {
            if (!PageUtils.IsLogin)
            {
                HttpContext.Current.Response.Redirect("login.aspx?returnurl=" +
                                                      HttpContext.Current.Server.UrlEncode(RequestHelper.CurrentUrl));
            }
            Users user = Users.FindById(PageUtils.CurrentUserId);

            if (user == null) //删除已登陆用户时有效
            {
                PageUtils.RemoveUserCookie();
                FormsAuthentication.SignOut();
                HttpContext.Current.Response.Redirect("login.aspx?returnurl=" +
                                                      HttpContext.Current.Server.UrlEncode(RequestHelper.CurrentUrl));
                return;
            }

            if (PageUtils.CurrentUser.Status == 0)
            {
                ResponseError("您的用户名已停用", "您的用户名已停用,请与管理员联系!");
            }

            string[] plist =
            {
                "themelist.aspx", "themeedit.aspx", "linklist.aspx", "userlist.aspx", "setting.aspx",
                "categorylist.aspx", "taglist.aspx", "commentlist.aspx"
            };
            if (PageUtils.CurrentUser.Type == (int) UserType.Author)
            {
                string pageName = Path.GetFileName(HttpContext.Current.Request.Url.ToString()).ToLower();

                foreach (string p in plist)
                {
                    if (pageName == p)
                    {
                        ResponseError("没有权限", "您没有权限使用此功能,请与管理员联系!");
                    }
                }
            }
        }

        protected void SetPageTitle(string title)
        {
            Page.Title = title + " - 管理中心 - Powered by Loachs";
        }

        /// <summary>
        ///     错误提示
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public string ShowError(string error)
        {
            ResponseMessage = string.Format("<div class=\"p_error\">{0}</div>", error);
            return ResponseMessage;
        }

        /// <summary>
        ///     正确提示
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string ShowMessage(string message)
        {
            ResponseMessage = string.Format("<div class=\"p_message\">{0}</div>", message);
            return ResponseMessage;
        }
    }
}