<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="LiveChat.Web.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>网站在线客服系统 v2.0</title>

    <script type="text/javascript" src="/javascript/monitor.js"></script>

</head>
<body>
    <form id="form2" runat="server">
    <div>
        <asp:Repeater ID="Repeater1" runat="server">
            <HeaderTemplate>
                <table cellpadding="0" cellspacing="0" style="width: 400px; height: 100%;" id="table"
                    align="center">
                    <tr>
                        <td align="center" style="font-size: 12px; border: solid 1px #eee; padding: 10px;">
                            文字方式
                        </td>
                        <td align="center" style="font-size: 12px; border: solid 1px #eee; padding: 10px;">
                            图形方式
                        </td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td align="center" valign="middle" style="font-size: 12px; border: solid 1px #eee;
                        padding: 10px;">
                        <a id="A<%# Eval("CompanyID") %>" lim:prefix="<%# Eval("ChatWebSite") %>" lim:company="<%# Eval("CompanyID") %>"
                            lim:skin="skin001" onclick="openChat(this);" href="javascript:void(0);">
                            <%# Eval("CompanyName") %></a>（<script type="text/javascript" src="http://chat.zgsxw.com/online.aspx?type=text&companyID=<%# Eval("CompanyID") %>"></script>）
                    </td>
                    <td align="center" valign="middle" style="font-size: 12px; border: solid 1px #eee;
                        padding: 10px;">
                        <img id="IMG<%# Eval("CompanyID") %>" lim:prefix="<%# Eval("ChatWebSite") %>" lim:company="<%# Eval("CompanyID") %>"
                            lim:skin="skin001" onclick="openChat(this);" src="http://chat.zgsxw.com/online.aspx?type=jpg&companyID=<%# Eval("CompanyID") %>&online=http://chat.zgsxw.com/images/kefulink5.gif&offline=http://chat.zgsxw.com/images/kefulink5off.gif"
                            alt="<%# Eval("CompanyName") %>" style="cursor: pointer;" />
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                <tr>
                    <td align="center" valign="middle" style="font-size: 12px; border: solid 1px #eee;
                        padding: 10px;">
                        <a id="A1001" lim:prefix="http://chat.zgsxw.com" lim:company="1001" lim:code="super"
                            lim:skin="skin001" onclick="openChat(this);" href="javascript:void(0);">管理员</a>（<script
                                type="text/javascript" src="http://chat.zgsxw.com/online.aspx?type=text&companyID=1001&seatCode=super"></script>）
                    </td>
                    <td align="center" valign="middle" style="font-size: 12px; border: solid 1px #eee;
                        padding: 10px;">
                        <img id="IMG1001" lim:prefix="http://chat.zgsxw.com" lim:company="1001" lim:code="super"
                            lim:skin="skin001" onclick="openChat(this);" src="http://chat.zgsxw.com/online.aspx?type=jpg&companyID=1001&seatCode=super&online=http://chat.zgsxw.com/images/kefulink5.gif&offline=http://chat.zgsxw.com/images/kefulink5off.gif"
                            alt="管理员" style="cursor: pointer;" />
                    </td>
                </tr>
                <tr>
                    <td align="center" valign="middle" style="font-size: 12px; border: solid 1px #eee;
                        padding: 10px;">
                        <a id="A1" lim:prefix="http://chat.zgsxw.com" lim:company="1001" lim:code="0002"
                            lim:skin="skin001" onclick="openChat(this);" href="javascript:void(0);">客服02</a>（<script
                                type="text/javascript" src="http://chat.zgsxw.com/online.aspx?type=text&companyID=1001&seatCode=0002"></script>）
                    </td>
                    <td align="center" valign="middle" style="font-size: 12px; border: solid 1px #eee;
                        padding: 10px;">
                        <img id="IMG1" lim:prefix="http://chat.zgsxw.com" lim:company="1001" lim:code="0002"
                            lim:skin="skin001" onclick="openChat(this);" src="http://chat.zgsxw.com/online.aspx?type=jpg&companyID=1001&seatCode=0002&online=http://chat.zgsxw.com/images/kefulink5.gif&offline=http://chat.zgsxw.com/images/kefulink5off.gif"
                            alt="客服02" style="cursor: pointer;" />
                    </td>
                </tr>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    </form>
</body>
</html>
