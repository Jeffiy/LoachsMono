<%@ Page Language="C#" MasterPageFile="admin.master" AutoEventWireup="true" Inherits="Loachs.Web.admin_default" Title="无标题页" Codebehind="default.aspx.cs" %>
<%@ Import Namespace="System.Diagnostics" %>
<%@ Import Namespace="Loachs.Common" %>
<%@ Import Namespace="Loachs.Entity" %>
<%@ Import Namespace="Loachs.Business" %>
<%@ Import Namespace="NewLife" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h2>管理中心</h2>
<%=ResponseMessage%>
<div  class="right">
    <h4>重新统计数据</h4>
    
    <p><asp:Button ID="btnCategory" runat="server"  onclick="btnCategory_Click"  Text="重新统计分类文章数" Width="180" Font-Bold="false" /></p>
    <p><asp:Button ID="btnTag" runat="server"  onclick="btnTag_Click"  Text="重新统计标签文章数"   Width="180" Font-Bold="false"  /></p>
    <p><asp:Button ID="btnUser" runat="server"  onclick="btnUser_Click"  Text="重新统计作者文章和评论数" Width="180"   Font-Bold="false" /></p>
    <p class="notice">小提示:这些操作比较耗时.</p>
</div>
<div class="left" >
    <table width="100%">
        <tr class="category">
            <td>最新待审核评论</td>
            <td ></td>
        </tr>
       <%foreach (Comments comment in commentlist)
         { %>
                <tr class="row">
                   
                    <td colspan="2" >
                     <span style="float:right;">
                        <a href="commentlist.aspx?operate=update&commentid=<%=comment.Id%>">审核</a>
                        <a href="commentlist.aspx?operate=delete&commentid=<%=comment.Id%>" onclick="return confirm('确定要删除吗?')">删除</a>   
                     </span>
                        [<%=comment.AuthorLink%>] <span style="cursor:help;" title="<%=comment.Content%>"><%=Loachs.Common.StringHelper.CutString( Loachs.Common.StringHelper.RemoveHtml(comment.Content ) ,50,"...")%></span>
                   
                    </td>
                </tr>
        <%} %>
        <%if (commentlist.Count == 0)
          { %>
        <tr class="rowend">
                   
                    <td colspan="2" >
                     暂无待审核评论
                    </td>
                </tr>
        <%} %>
        
        <tr class="category">
            <td>网站信息</td>
            <td style="width:70%;"></td>
        </tr>
        <tr class="row">
            <td>文章:</td>
            <td><%= Sites.SiteInfo.PostCount %> 篇</td>
        </tr>
        <tr class="row">
            <td>评论:</td>
            <td><%=Sites.SiteInfo.CommentCount %> 条</td>
        </tr>
        
        <tr class="row">
            <td>标签:</td>
            <td><%=Sites.SiteInfo.TagCount %> 个</td>
        </tr>
         <tr class="row">
            <td>访问量:</td>
            <td><%=Sites.SiteInfo.VisitCount %> 次</td>
        </tr>
        
        <tr class="row">
            <td>数据库:</td>
            <td><%=DbPath %> (<%=DbSize%>)</td>
        </tr>
        <tr class="row">
            <td>附件:</td>
            <td><%=UpfilePath %> (共<%=UpfileCount%> 个, <%=UpfileSize%>)</td>
        </tr>
         <tr class="row">
            <td>程序目录:</td>
            <td><%= HttpRuntime.AppDomainAppPath%></td>
        </tr>
        <tr class="rowend">
            <td>程序版本:</td>
            <td><%= Setting.Version %></td>
        </tr>
        <tr class="category">
            <td>服务器信息</td>
            <td ></td>
        </tr>
         <tr class="row">
            <td>应用系统:</td>
            <td><%= HttpRuntime.AppDomainAppVirtualPath%>&nbsp;<a href="?Act=Restart" onclick="return confirm('仅重启ASP.Net应用程序域，而不是操作系统！\n确认重启？')">重启应用系统</a></td>
        </tr>
         <tr class="row">
            <td>操作系统:</td>
            <td><%=Runtime.OSName%></td>
        </tr>
         <tr class="row">
            <td>计算机用户:</td>
            <td><%= Environment.UserName%>/<%= Environment.MachineName%></td>
        </tr>
         <tr class="row">
            <td>应用程序域:</td>
            <td><%= AppDomain.CurrentDomain.FriendlyName %></td>
        </tr>
         <tr class="row">
            <td>.NET 版本:</td>
            <td><%=Environment.Version%></td>
        </tr>
         <tr class="row">
            <td>Web服务:</td>
            <td><%=GetWebServerName()%></td>
        </tr>
         <tr class="row">
            <td>处理器:</td>
             <td>
                 <%= Environment.ProcessorCount %> 核心，
                 <%= Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER") %>
             </td>
        </tr>
         <tr class="row" title="这里使用了服务器默认的时间格式！后面是开机时间。">
            <td>时间:</td>
             <td>
                 <%= DateTime.Now%>，<%= new TimeSpan(Environment.TickCount)%>
             </td>
        </tr>
         <tr class="row">
            <td>内存:</td>
             <td>
                 <% var process = Process.GetCurrentProcess(); %>
                 工作集:<%= (process.WorkingSet64/1024).ToString("n0") + "KB" %>
                 提交:<%= (process.PrivateMemorySize64/1024).ToString("n0") + "KB" %>
                 GC:<%= (GC.GetTotalMemory(false)/1024).ToString("n0") + "KB" %>
             </td>
        </tr>
         <tr class="row">
            <td>进程时间:</td>
             <td>
                <%= process.TotalProcessorTime.TotalSeconds.ToString("N2")%>秒 启动于<%= process.StartTime.ToString("yyyy-MM-dd HH:mm:ss")%>
             </td>
        </tr>
         <tr class="row">
            <td>Session:</td>
             <td>
                <%= Session.Contents.Count%>个，<%= Session.Timeout%>分钟，SessionID：<%= Session.Contents.SessionID%>
             </td>
        </tr>
         <tr class="row">
            <td>Cache:</td>
             <td>
                <%= Cache.Count%>个，可用：<%= (Cache.EffectivePrivateBytesLimit / 1024).ToString("n0")%>KB
             </td>
        </tr>
        <tr class="rowend">
            <td>服务器IP:</td>
            <td><%=Request.ServerVariables["LOCAl_ADDR"]%></td>
        </tr>
    </table>
</div>
</asp:Content>

