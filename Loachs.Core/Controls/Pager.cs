//http://topic.csdn.net/t/20050825/17/4231529.html

using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace Loachs.Controls
{
    /// <summary>
    ///     分页控件
    /// </summary>
    [DefaultProperty("Text"),
     ToolboxData("<{0}:Pager runat=server></{0}:Pager>")]
    public class Pager : Control
    {
        //以下脚本用于从文本框输入页码
        private const string StrScript =
            "<script language='javascript'>\n" +
            " function ChoosePage(ctrl,max)" +
            " { " +
            "		if(ctrl.value >= 1 && ctrl.value <= max) " +
            "		{ " +
            @"			var url=location.href.replace(/&Page=\d*|Page=\d*/ig,''); " +
            "			if(url.lastIndexOf('?')==-1) " +
            "			{ location.href = url + '?Page=' + ctrl.value; } " +
            "			else " +
            "			{ location.href = url + '&Page=' + ctrl.value; } " +
            "		} " +
            "		else\n" +
            "		{ " +
            "			alert('可输入数字范围: 1-' + max); " +
            "			return false; " +
            "		} " +
            " }" +
            "</script>";

        private string _cssclass = "pager";
        //public int intStartRecord;	//起始记录
//        private string _currenturl = string.Empty; //当前页面路径
        public int Pageindex = 1;
        private string _rewriteurl = string.Empty; //重写地址
//        private int _totalrecord = 0;
        public int IntPageIndex = 1; //当时页
        public int IntPageSize = 10; //每页显示的记录数
        //public int intPageCount;	//计算总共有多少页
        public int IntRecordCount = 0; //一共有多少记录
        public int IntShowNum = 5; //控件UI显示数字的个数

        public Pager()
        {
        }

        /// <summary>
        ///     样式类名
        /// </summary>
        public string CssClass
        {
            get { return _cssclass; }
            set { _cssclass = value; }
        }

        /// <summary>
        ///     每页显示的记录数
        /// </summary>
        [DefaultValue(10), Category("Customer")]
        public int PageSize
        {
            set
            {
                if (value <= 0)
                    IntPageSize = 1;
                else
                    IntPageSize = value;
            }
            get { return IntPageSize; }
        }

        /// <summary>
        ///     当前页
        /// </summary>
        public int PageIndex
        {
            get { return Pageindex; }
            set { Pageindex = value > 0 ? value : 1; }
        }

        /// <summary>
        ///     记录总数
        /// </summary>
        public int RecordCount
        {
            get { return Convert.ToInt32(ViewState["_totalrecord"]); }
            set
            {
                if (value < 0)
                {
                    throw new Exception("记录总条数不能为负数");
                }
                ViewState["_totalrecord"] = value;
            }
        }

        /// <summary>
        ///     总页数
        /// </summary>
        [DefaultValue(1), Category("Customer")]
        public int PageSum
        {
            get
            {
                if (RecordCount%IntPageSize > 0)
                {
                    return (RecordCount/IntPageSize) + 1;
                }
                return (RecordCount/IntPageSize);
            }
        }

        /// <summary>
        ///     显示数字的个数
        ///     ShowNum>=4
        /// </summary>
        public int ShowNum
        {
            set { IntShowNum = value; }
            get
            {
                if (IntShowNum < 5)
                {
                    IntShowNum = 5;
                }
                return IntShowNum;
            }
        }

        /// <summary>
        ///     重写地址
        /// </summary>
        public string RewriteUrl
        {
            get { return _rewriteurl; }
            set { _rewriteurl = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //获取当前页数

            if (Context.Request.QueryString["Page"] == null || Context.Request.QueryString["Page"] == "")
            {
                PageIndex = 1;
            }
            else
            {
                PageIndex = int.Parse(Context.Request.QueryString["Page"]);
            }
        }

        //protected override void OnPreRender(EventArgs e)
        //{
        //    //			base.OnPreRender (e);
        //    //			if(!Page.IsClientScriptBlockRegistered("WEREW-332DFAF-FDAFDSFDSAFD"))
        //    //			{
        //    //				Page.RegisterClientScriptBlock("WEREW-332DFAF-FDAFDSFDSAFD",strScript);
        //    //			}
        //}

        /// <summary>
        /// </summary>
        /// <param name="output"></param>
        protected override void Render(HtmlTextWriter output)
        {
            base.Render(output);
            string strTotal;
            string strLeft = null;
            string strRight = null;
            string strCenterNum = null; //循环生成数字序列部分

            //string first = "<font face=webdings title=首页>9</font>";
            //string prev = "<font face=webdings title=上页>7</font>";
            //string next = "<font face=webdings title=下页>8</font>";
            //string last = "<font face=webdings title=尾页>:</font>";

            string first = "首页";
            string prev = "上一页";
            string next = "下一页";
            string last = "尾页";

            if (string.IsNullOrEmpty(RewriteUrl))
            {
                Regex rx = new Regex(@"&Page=\d*|Page=\d*", RegexOptions.IgnoreCase);

                string strFileUrl = Context.Request.Url.LocalPath; //文件相对路径,不包括参数和域名

                //string strUrlQuery=System.Web.HttpUtility.UrlDecode(Context.Request.Url.Query.Replace("?",string.Empty),System.Text.Encoding.GetEncoding("UTF-8"));		//去年问号的参数 同时解码中文参数
                string strUrlQuery = Context.Request.Url.Query.Replace("?", string.Empty); //去年问号的参数

                string strRxUrl = rx.Replace(strUrlQuery, string.Empty); //去年问号,page=n,&page=n的参数
                string strNewUrl = strFileUrl + "?" + strRxUrl; //新路径(不含&page=n)

                //显示统计信息
                //strTotal = "页:" + this.PageIndex.ToString() + "/" + this.PageSum.ToString() + "&nbsp;&nbsp;" + "每页" + this.PageSize.ToString() + "条" + "&nbsp;&nbsp;共" + this.RecordCount.ToString() + "条&nbsp;&nbsp;";     
                strTotal = "共" + RecordCount + "条&nbsp;&nbsp;";

                //显示首页 上一页
                if (PageIndex != 1)
                {
                    if (strRxUrl.Length > 0) //判断是否有其它参数
                    {
                        strLeft = "<a href=" + strNewUrl + "&Page=1>" + first + "</a>";
                    }
                    else
                    {
                        strLeft = "<a href=" + strNewUrl + "Page=1>" + first + "</a>";
                    }

                    if (strRxUrl.Length > 0) //判断是否有其它参数
                    {
                        strLeft += "<a href=" + strNewUrl + "&Page=" + (PageIndex - 1) + ">" + prev + "</a>";
                    }
                    else
                    {
                        strLeft += "<a href=" + strNewUrl + "Page=" + (PageIndex - 1) + ">" + prev + "</a>";
                    }
                }

                //显示尾页 下一页
                if (PageIndex < PageSum && PageSum > 1)
                {
                    if (strRxUrl.Length > 0)
                    {
                        strRight = "<a href=" + strNewUrl + "&Page=" + (PageIndex + 1) + ">" + next + "</a>";
                    }
                    else
                    {
                        strRight = "<a href=" + strNewUrl + "Page=" + (PageIndex + 1) + ">" + next + "</a>";
                    }

                    if (strRxUrl.Length > 0)
                    {
                        strRight += "<a href=" + strNewUrl + "&Page=" + PageSum + ">" + last + "</a>";
                    }
                    else
                    {
                        strRight += "<a href=" + strNewUrl + "Page=" + PageSum + ">" + last + "</a>";
                    }
                }

                int min = 1; //要显示的页面数最小值
                int max = ShowNum; //要显示的页面数最大值

                if (PageIndex > PageSum)
                {
                    PageIndex = PageSum;
                }

                //获取循环数字的最小值和最大值
                if (PageIndex <= 3 && PageIndex > 0) //0<当前页<=3, 
                {
                    if (PageSum < ShowNum)
                    {
                        min = 1;
                        max = PageSum;
                    }
                    else
                    {
                        min = 1;
                        max = ShowNum;
                    }
                }
                else if (PageIndex > 3 && PageIndex < PageSum - (ShowNum - 4)) //当前页>3,当前页比最大页<6
                {
                    min = PageIndex - 2;
                    max = PageIndex + (ShowNum - 3);
                }
                else if (PageIndex >= PageSum - (ShowNum - 4))
                    // && this.PageSum>(this.ShowNum-4))	//当前页比最大页>=6,当前页>6   this.PageSum-(this.ShowNum-4):可能为负
                {
                    min = (PageSum - ShowNum + 1) < 1 ? 1 : (PageSum - ShowNum + 1);
                    max = PageSum;
                }

                //循环显示数字
                for (int i = min; i <= max; i++)
                {
                    if (PageIndex == i) //如果是当前页，用粗体和红色显示
                    {
                        //strCenterNum += "<B style='color:red'>" + i + "</B>&nbsp;" + "\n";
                        strCenterNum += "<span class=\"current\">" + i + "</span>";
                    }
                    else
                    {
                        if (strRxUrl.Length > 0) //判断是否有其它参数
                        {
                            strCenterNum += "<a href=" + strNewUrl + "&Page=" + i + ">" + i + "</a>"; // + "\n";
                        }
                        else
                        {
                            strCenterNum += "<a href=" + strNewUrl + "Page=" + i + ">" + i + "</a>";
                        }
                    }
                }
            }
            else
            {
                //显示首页 上一页
                if (PageIndex != 1)
                {
                    strLeft += "<a href='" + string.Format(RewriteUrl, 1) + "'>" + first + "</a>";
                    strLeft += "<a href='" + string.Format(RewriteUrl, PageIndex - 1) + "'>" + prev + "</a>";
                }

                //显示尾页 下一页
                if (PageIndex < PageSum && PageSum > 1)
                {
                    strRight += "<a href='" + string.Format(RewriteUrl, PageIndex + 1) + "'>" + next + "</a>";
                    strRight += "<a href='" + string.Format(RewriteUrl, PageSum) + "'>" + last + "</a>";
                }

                int min = 1; //要显示的页面数最小值
                int max = ShowNum; //要显示的页面数最大值

                if (PageIndex > PageSum)
                {
                    PageIndex = PageSum;
                }

                //获取循环数字的最小值和最大值
                if (PageIndex <= 3 && PageIndex > 0) //0<当前页<=3, 
                {
                    if (PageSum < ShowNum)
                    {
                        min = 1;
                        max = PageSum;
                    }
                    else
                    {
                        min = 1;
                        max = ShowNum;
                    }
                }
                else if (PageIndex > 3 && PageIndex < PageSum - (ShowNum - 4)) //当前页>3,当前页比最大页<6
                {
                    min = PageIndex - 2;
                    max = PageIndex + (ShowNum - 3);
                }
                else if (PageIndex >= PageSum - (ShowNum - 4))
                    // && this.PageSum>(this.ShowNum-4))	//当前页比最大页>=6,当前页>6   this.PageSum-(this.ShowNum-4):可能为负
                {
                    min = (PageSum - ShowNum + 1) < 1 ? 1 : (PageSum - ShowNum + 1);
                    max = PageSum;
                }

                //循环显示数字
                for (int i = min; i <= max; i++)
                {
                    if (PageIndex == i) //如果是当前页，用粗体和红色显示
                    {
                        strCenterNum += "<span class=\"current\">" + i + "</span>";
                    }
                    else
                    {
                        strCenterNum += "<a href=\"" + string.Format(RewriteUrl, i) + "\">" + i + "</a>";
                    }
                }
            }
            //	strTotal = "共<span class=\"totalnum\">" + this.RecordCount.ToString() + "</span>条&nbsp;&nbsp;";     
            strTotal = string.Format("<span class=\"total\">共有<strong>{0}</strong>条</span>", RecordCount);
            output.Write("<div id=\"" + UniqueID + "\" class= \"" + CssClass + "\">");
            output.Write("<div>");
            output.Write(strTotal + strLeft + strCenterNum + strRight);
            output.Write("</div>");
            output.Write("</div>");
        }
    }
}