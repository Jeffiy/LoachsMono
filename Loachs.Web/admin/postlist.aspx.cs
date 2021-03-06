﻿using System;
using System.Web.UI.WebControls;
using Loachs.Common;
using Loachs.Entity;
using StringHelper = Loachs.Common.StringHelper;

namespace Loachs.Web
{
    public partial class admin_postlist : AdminPage
    {
        /// <summary>
        ///     postid
        /// </summary>
        protected int PostId = RequestHelper.QueryInt("postid");

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("文章管理");
            
            if (Operate == OperateType.Delete)
            {
                Delete();
            }

            if (!IsPostBack)
            {
                LoadDefaultData();

                BindPostList();
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
                case 3:
                    ShowMessage("删除成功!");
                    break;
                case 444:
                    ShowMessage("权限不够!");
                    break;
                default:
                    break;
            }
        }

        protected void Delete()
        {
            Posts post = Posts.FindById(PostId);
            if (post == null)
            {
                return;
            }
            if (PageUtils.CurrentUser.Type != (int) UserType.Administrator &&
                PageUtils.CurrentUser.Id != post.UserId)
            {
                Response.Redirect("postlist.aspx?result=444");
            }

            Posts.DeleteById(PostId);

            Response.Redirect("postlist.aspx?result=3");
        }

        protected string GetEditLink(object postId, object userId)
        {
            string t = " <a href=\"postedit.aspx?operate=update&postid=" + postId + "\">编辑</a>";
            if (Convert.ToInt32(userId) == PageUtils.CurrentUser.Id ||
                PageUtils.CurrentUser.Type == (int) UserType.Administrator)
            {
                return t;
            }
            return string.Empty;
        }

        protected string GetDeleteLink(object postId, object userId)
        {
            string t = " <a href=\"postlist.aspx?operate=delete&postid=" + postId +
                       "\" onclick=\"return confirm('删除文章同时会删除该文章的相关评论,确定要删除吗?');\">删除</a>";
            if (Convert.ToInt32(userId) == PageUtils.CurrentUser.Id ||
                PageUtils.CurrentUser.Type == (int) UserType.Administrator)
            {
                return t;
            }
            return string.Empty;
        }

        /// <summary>
        ///     加载默认数据
        /// </summary>
        private void LoadDefaultData()
        {
            ddlCategory.Items.Clear();
            ddlCategory.Items.Add(new ListItem("不限", "-1"));
            foreach (Categorys category in Categorys.FindAllWithCache())
            {
                ddlCategory.Items.Add(new ListItem(category.Name, category.Id.ToString()));
            }

            ddlAuthor.Items.Clear();
            ddlAuthor.Items.Add(new ListItem("不限", "-1"));
            foreach (Users user in Users.FindAllWithCache())
            {
                ddlAuthor.Items.Add(new ListItem(user.Name, user.Id.ToString()));
            }
        }

        /// <summary>
        ///     绑定
        /// </summary>
        protected void BindPostList()
        {
            string keyword = StringHelper.CutString(StringHelper.SqlEncode(RequestHelper.QueryString("keyword")), 20);
            int categoryId = RequestHelper.QueryInt("categoryid", -1);
            int userId = RequestHelper.QueryInt("userid", -1);
            int recommend = RequestHelper.QueryInt("recommend", -1);
            int hide = RequestHelper.QueryInt("hide", -1);

            txtKeyword.Text = keyword;
            ddlCategory.SelectedValue = categoryId.ToString();
            ddlAuthor.SelectedValue = userId.ToString();
            chkRecommend.Checked = recommend == 1;
            chkHideStatus.Checked = hide == 1;

            int totalRecord = 0;

            var list = Posts.GetPostList(Pager1.PageSize, Pager1.PageIndex, out totalRecord, categoryId,
                -1, userId, recommend, -1, -1, hide, string.Empty, string.Empty, keyword);
            rptPost.DataSource = list;
            rptPost.DataBind();
            Pager1.RecordCount = totalRecord;
        }

        /// <summary>
        ///     搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = StringHelper.UrlEncode(txtKeyword.Text);
            int categoryId = StringHelper.StrToInt(ddlCategory.SelectedValue, -1);
            int userId = StringHelper.StrToInt(ddlAuthor.SelectedValue, -1);
            int recommend = chkRecommend.Checked ? 1 : -1;
            int hide = chkHideStatus.Checked ? 1 : -1;

            Response.Redirect(string.Format(
                "postlist.aspx?keyword={0}&categoryid={1}&userid={2}&recommend={3}&hide={4}", keyword, categoryId,
                userId, recommend, hide));
        }
    }
}