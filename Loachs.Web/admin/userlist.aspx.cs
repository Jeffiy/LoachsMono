using System;
using System.Text.RegularExpressions;
using Loachs.Common;
using Loachs.Entity;
using StringHelper = Loachs.Common.StringHelper;

namespace Loachs.Web
{
    public partial class admin_userlist : AdminPage
    {
        /// <summary>
        ///     修改时提示
        /// </summary>
        protected string PasswordMessage = string.Empty;

        /// <summary>
        ///     用户Id
        /// </summary>
        protected int UserId = RequestHelper.QueryInt("UserId");

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("作者管理");


            if (!IsPostBack)
            {
                BindUserList();

                if (Operate == OperateType.Update)
                {
                    BindUser();
                    btnEdit.Text = "修改";
                    txtUserName.Enabled = false;

                    PasswordMessage = "(不修改请留空)";
                }
                else if (Operate == OperateType.Delete)
                {
                    DeleteUser();
                }
            }

            ShowResult();
        }

        /// <summary>
        ///     显示结果
        /// </summary>
        protected void ShowResult()
        {
            int result = RequestHelper.QueryInt("result");
            switch (result)
            {
                case 1:
                    ShowMessage("添加成功!");
                    break;
                case 2:
                    ShowMessage("修改成功!");
                    break;
                case 3:
                    ShowMessage("删除成功!");
                    break;
                case 4:
                    ShowError("不能删除自己!");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        ///     绑定实体
        /// </summary>
        protected void BindUser()
        {
            Users u = Users.FindById(UserId);
            if (u != null)
            {
                txtUserName.Text = StringHelper.HtmlDecode(u.UserName);
                txtNickName.Text = StringHelper.HtmlDecode(u.Name);

                txtEmail.Text = StringHelper.HtmlDecode(u.Email);

                chkStatus.Checked = u.Status == 1 ? true : false;

                ddlUserType.SelectedValue = u.Type.ToString();

                txtDisplayOrder.Text = u.DisplayOrder.ToString();
            }
            if (u != null && u.Id == PageUtils.CurrentUserId)
            {
                ddlUserType.Enabled = false;
                chkStatus.Enabled = false;
            }
        }

        /// <summary>
        ///     绑定列表
        /// </summary>
        protected void BindUserList()
        {
            rptUser.DataSource = Users.FindAllWithCache();
            rptUser.DataBind();
        }

        /// <summary>
        ///     删除用户
        /// </summary>
        protected void DeleteUser()
        {
            if (UserId == PageUtils.CurrentUserId)
            {
                Response.Redirect("userlist.aspx?result=4");
            }
            Users.DeleteById(UserId);
            Response.Redirect("userlist.aspx?result=3");
        }

        protected string GetUserType(object userType)
        {
            int type = Convert.ToInt32(userType);
            switch (type)
            {
                case (int) UserType.Administrator:
                    return "管理员";
                case (int) UserType.Author:
                    return "写作者";
                default:
                    return "未知身份";
            }
        }

        /// <summary>
        ///     编辑用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Users u = new Users();
            if (Operate == OperateType.Update)
            {
                u = Users.FindById(UserId);
            }
            else
            {
                u.CommentCount = 0;
                u.CreateDate = DateTime.Now;
                u.PostCount = 0;
                u.UserName = StringHelper.HtmlEncode(txtUserName.Text.Trim());
            }

            u.Email = StringHelper.HtmlEncode(txtEmail.Text.Trim());
            u.SiteUrl = string.Empty; // StringHelper.HtmlEncode(txtSiteUrl.Text.Trim());
            u.Status = chkStatus.Checked ? 1 : 0;
            u.Description = string.Empty; // StringHelper.TextToHtml(txtDescription.Text);
            u.Type = StringHelper.StrToInt(ddlUserType.SelectedValue, 0);
            u.Name = StringHelper.HtmlEncode(txtNickName.Text.Trim());
            u.AvatarUrl = string.Empty;
            u.DisplayOrder = StringHelper.StrToInt(txtDisplayOrder.Text, 1000);

            if (!string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                u.Password = StringHelper.GetMD5(txtPassword.Text.Trim());
            }

            if (!string.IsNullOrEmpty(txtPassword.Text.Trim()) && txtPassword.Text != txtPassword2.Text)
            {
                ShowError("两次密码输入不相同!");
                return;
            }


            if (Operate == OperateType.Update)
            {
                u.Save();

                //  如果修改自己,更新COOKIE
                if (!string.IsNullOrEmpty(txtPassword.Text.Trim()) && u.Id == PageUtils.CurrentUserId)
                {
                    PageUtils.WriteUserCookie(u.Id, u.UserName, u.Password, 0);
                }
                Response.Redirect("userlist.aspx?result=2");
            }
            else
            {
                //System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"[A-Za-z0-9\u4e00-\u9fa5-]");
                //if (!reg.IsMatch(u.UserName))
                //{
                //    ShowError("用户名不合法！");
                //    return;
                //}

                if (string.IsNullOrEmpty(u.UserName))
                {
                    ShowError("请输入登陆用户名!");
                    return;
                }

                Regex reg = new Regex("[A-Za-z0-9\u4e00-\u9fa5-]");
                if (!reg.IsMatch(u.UserName))
                {
                    ShowError("用户名限字母,数字,中文,连字符!");
                    return;
                }
                if (StringHelper.IsInt(u.UserName))
                {
                    ShowError("用户名不能为全数字!");
                    return;
                }

                if (string.IsNullOrEmpty(u.Password))
                {
                    ShowError("请输入密码!");
                    return;
                }
                if (Users.FindByName(u.UserName) != null)
                {
                    ShowError("该登陆用户名已存在,请换之");
                    return;
                }

                u.Save();

                Response.Redirect("userlist.aspx?result=1");
            }
        }
    }
}