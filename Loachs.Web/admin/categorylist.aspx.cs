using System;
using Loachs.Common;
using Loachs.Entity;
using StringHelper = Loachs.Common.StringHelper;

namespace Loachs.Web
{
    public partial class admin_categorylist : AdminPage
    {
        /// <summary>
        ///     分类ID
        /// </summary>
        protected int CategoryId = RequestHelper.QueryInt("categoryid");

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("分类管理");

            if (!IsPostBack)
            {
                BindCategoryList();
                
                if (Operate == OperateType.Update)
                {
                    BindCategory();
                    btnEdit.Text = "编辑";
                }
                if (Operate == OperateType.Delete)
                {
                    Delete();
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
                default:
                    break;
            }
        }

        protected void Delete()
        {
            Categorys.DeleteById(CategoryId);
            Response.Redirect("categorylist.aspx?result=3");
        }

        /// <summary>
        ///     绑定分类
        /// </summary>
        protected void BindCategory()
        {
            Categorys category = Categorys.FindById(CategoryId);
            if (category != null)
            {
                txtName.Text = StringHelper.HtmlDecode(category.Name);
                txtSlug.Text = StringHelper.HtmlDecode(category.Slug);
                txtDescription.Text = StringHelper.HtmlDecode(category.Description);
                txtDisplayOrder.Text = category.DisplayOrder.ToString();
            }
        }

        /// <summary>
        ///     绑定列表
        /// </summary>
        protected void BindCategoryList()
        {
            rptCategory.DataSource = Categorys.FindAllWithCache();
            rptCategory.DataBind();
        }

        /// <summary>
        ///     编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Categorys category = new Categorys();
            if (Operate == OperateType.Update)
            {
                category = Categorys.FindById(CategoryId);
            }
            else
            {
                category.CreateDate = DateTime.Now;
                category.Count = 0;
            }
            category.Name = StringHelper.HtmlEncode(txtName.Text);
            category.Slug = txtSlug.Text.Trim();
            if (string.IsNullOrEmpty(category.Slug))
            {
                category.Slug = category.Name;
            }

            category.Slug = StringHelper.HtmlEncode(PageUtils.FilterSlug(category.Slug, "cate"));

            category.Description = StringHelper.HtmlEncode(txtDescription.Text);
            category.DisplayOrder = StringHelper.StrToInt(txtDisplayOrder.Text, 1000);

            if (category.Name == "")
            {
                ShowError("请输入名称!");
                return;
            }

            if (Operate == OperateType.Update)
            {
                category.Update();
                Response.Redirect("categorylist.aspx?result=2");
            }
            else
            {
                category.Insert();
                Response.Redirect("categorylist.aspx?result=1");
            }
        }
    }
}