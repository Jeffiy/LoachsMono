﻿using System;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Loachs.Common;
using Loachs.Entity;
using StringHelper = Loachs.Common.StringHelper;

namespace Loachs.Web
{
    public partial class admin_commentlist : AdminPage
    {
        /// <summary>
        ///     审核
        /// </summary>
        private readonly int _approved = RequestHelper.QueryInt("approved", -1);

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("评论管理");

            OperateComment();

            ShowResult();

            if (!IsPostBack)
            {
                BindCommentList();
            }
        }

        /// <summary>
        ///     审核,删除单条记录
        /// </summary>
        private void OperateComment()
        {
            int commentId = RequestHelper.QueryInt("commentid", 0);
            if (Operate == OperateType.Delete)
            {
                Comments.DeleteById(commentId);

                Response.Redirect("commentlist.aspx?result=3&page=" + Pager1.PageIndex);
            }
            else if (Operate == OperateType.Update)
            {
                Comments comment = Comments.FindById(commentId);
                if (comment != null)
                {
                    comment.Approved = (int) ApprovedStatus.Success;
                    comment.Save();

                    Response.Redirect("commentlist.aspx?result=4&page=" + Pager1.PageIndex);
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
                case 3:
                    ShowMessage("删除成功!");
                    break;
                case 4:
                    ShowMessage("审核成功!");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        ///     绑定
        /// </summary>
        protected void BindCommentList()
        {
            int totalRecord = 0;

            var list = Comments.GetCommentList(Pager1.PageSize, Pager1.PageIndex, out totalRecord, 1,
                -1, -1, -1, _approved, -1, string.Empty);
            rptComment.DataSource = list;
            rptComment.DataBind();
            Pager1.RecordCount = totalRecord;
        }

        /// <summary>
        ///     文章连接
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        protected string GetPostLink(int postId)
        {
            Posts post = Posts.FindById(postId);
            if (post != null)
            {
                return string.Format(" 评: {0}", StringHelper.CutString(post.Title, 20, "..."));
            }
            return string.Empty;
        }

        /// <summary>
        ///     删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (RepeaterItem item in rptComment.Items)
            {
                HtmlInputCheckBox box = ((HtmlInputCheckBox) item.FindControl("chkRow"));
                if (box.Checked)
                {
                    int commentId = Convert.ToInt32(box.Value);
                    Comments.DeleteById(commentId);
                    i++;
                }
            }
            
            Response.Redirect("commentlist.aspx?result=3&page=" + Pager1.PageIndex + "&approved=" + _approved);
        }

        /// <summary>
        ///     审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnApproved_Click(object sender, EventArgs e)
        {
            foreach (Comments c in from RepeaterItem item in rptComment.Items select ((HtmlInputCheckBox)item.FindControl("chkRow")) into box where box.Checked select Convert.ToInt32(box.Value) into commentID select Comments.FindById(commentID) into c where c != null select c)
            {
                c.Approved = (int) ApprovedStatus.Success;
                c.Save();
            }
            
            Response.Redirect("commentlist.aspx?result=4&page=" + Pager1.PageIndex + "&approved=" + _approved);
        }
    }
}