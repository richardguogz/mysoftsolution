<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="analysisdetail.ascx.cs"
    Inherits="LiveChat.Web.Admin.UserControls.analysisdetail" %>
<%@ Import Namespace="LiveChat.Entity" %>
<div class="left_01" id="history">
    <!-- ������Ϣ -->
    <% foreach (P2SMessage item in msglist)
       { %>
    <% if (item.SenderID == createID)
       { %>
    <p class="operator">
        <% =item.SenderName %>&nbsp;˵:<font color="gray" style="font-size: 9px;">(<% =item.SendTime.ToString("yyyy-MM-dd HH:mm:ss") %>)</font><br />
        <span>
            <% =item.Content %></span></p>
    <% }
       else
       { %>
    <p class="visitor">
        <% =item.SenderName %>&nbsp;˵:<font color="gray" style="font-size: 9px;">(<% =item.SendTime.ToString("yyyy-MM-dd HH:mm:ss") %>)</font><br />
        <span>
            <% =item.Content %></span></p>
    <% } %>
    <% } %>
</div>
