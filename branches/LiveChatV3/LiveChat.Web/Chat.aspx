<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Chat.aspx.cs" Inherits="LiveChat.Web.Chat" %>

<%@ Import Namespace="LiveChat.Web" %>
<%@ Import Namespace="LiveChat.Entity" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <title>对话窗口</title>
    <link href="images/main.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        var chatWebSite = '<% = company.ChatWebSite %>';
        var sessionID = '<% =sessionID %>';
        var skinID = '<% =skinID %>';
        var userID = '<% =userID %>';
        var seatCode = '<% =seatCode %>';
    </script>

</head>
<body onload="autoResize();setRefresh();">
    <form id="formChat" runat="server" />
    <div class="talk" id="div">
        <table width="100%" border="0" cellpadding="0" cellspacing="0" id="chatbox">
            <tr>
                <td height="43">
                    <div>
                        <div class="top">
                            <div class="d_l">
                                <span id="headerBox" runat="server">&nbsp;
                                    <!-- 头部信息 -->
                                </span>
                            </div>
                        </div>
                        <div class="d_r" id="adBox" runat="server" style="position: absolute; top: 1px; right: 0px;">
                            <!-- 这里放广告信息 -->
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <div class="con">
            <div class="con2">
                <div class="con3" id="div1">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="top">
                                <div id="div3" class="scoll" style="width: 99.8%;">
                                    <div id="history" class="left_01">
                                        <!-- 聊天信息 -->
                                    </div>
                                </div>
                            </td>
                            <td width="5px" rowspan="2">
                            </td>
                            <td width="180px" rowspan="2" valign="top">
                                <div class="right border">
                                    <div style="padding-bottom: 10px; text-align: center;" id="adLogo" runat="server">
                                        <!--右边的广告 -->
                                    </div>
                                    <div style="text-align: center; background: url(images/talk04.jpg) no-repeat;" id="div2"
                                        class="scoll">
                                        <ul class="list">
                                            <li><b>公司：</b><a href='<%= company.WebSite.IndexOf("http://") < 0?"http://" + company.WebSite:company.WebSite %>'
                                                target="_blank"><%= company.CompanyName %></a></li>
                                            <li><b>客服：</b><span id='seatName'><%= seat==null?"":seat.ShowName %></span></li>
                                            <li><b>电话：</b><span id='telePhone'><%= seat==null?"":seat.Telephone %></span></li>
                                            <li><b>手机：</b><span id='mobileNumber'><%= seat==null?"":seat.MobileNumber %></span></li>
                                            <li><b>邮箱：</b><span id='email'><%= seat==null?"":seat.Email %></span></li>
                                        </ul>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td height="164" valign="top">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td colspan="2">
                                            <div class="kj">
                                                <div id="emotionpane" style="border-style: ridge; border-width: 1px; visibility: visible;
                                                    position: absolute; width: 405px; height: 275px; z-index: 105; left: 6px; top: -43px;
                                                    background-color: rgb(255, 255, 255); display: none;">
                                                    <table height="100%" cellpadding="2" border="0" width="100%" bordercolordark="#F3F3CD"
                                                        bordercolorlight="#FFFFFF" id="tableface">
                                                        <tbody>
                                                            <% for (int i = 0; i < 135; i++)
                                                               {
                                                                   if (i % 15 == 0)
                                                                   {%>
                                                            <tr>
                                                                <% }%>
                                                                <td onmouseout="this.bgColor='#ffffff'" onmouseover="this.bgColor='#FFC895'">
                                                                    <div onclick="setcurrface(<% = i %>);" style="background: url('/images/face/<% = i %>.gif');" class="emo">
                                                                    </div>
                                                                </td>
                                                                <%
                                                                    if (i % 15 == 14)
                                                                    {%>
                                                            </tr>
                                                            <% }
                                                               }%>
                                                        </tbody>
                                                    </table>
                                                </div>
                                                <div id="uploadFileBox" style="display: none;">
                                                    <div style="margin-left: 18px; padding: 3px;">
                                                        文件大小限制在4M以内！</div>
                                                    <iframe id="uploadFileFrame" height="30px" frameborder="0" scrolling="no"></iframe>
                                                </div>
                                                <ul>
                                                    <li onclick="setface();" id='chatface' onmouseover="this.className='ico0 hover';"
                                                        onmouseout="this.className='ico0';" class="ico0">表情</li>
                                                    <li onclick="showUploadFile('/uploadFile.htm');" id='file' onmouseover="this.className='icof hover';"
                                                        onmouseout="this.className='icof';" class="icof">&nbsp;传送文件</li>
                                                    <li onclick="download();" id='active' onmouseover="this.className='ico hover';" onmouseout="this.className='ico';"
                                                        class="ico">截屏</li>
                                                    <li onclick="doclose();" id="closesound" onmouseover="this.className='ico2 hover';"
                                                        onmouseout="this.className='ico2';" class="ico2">关闭提示音</li>
                                                    <li onclick="doopen();" id="opensound" onmouseover="this.className='ico3 hover';"
                                                        onmouseout="this.className='ico3';" class="ico3" style="display: none;">打开提示音</li>
                                                    <li onclick="saveHistory();" id='chathistory' onmouseover="this.className='ico4 hover';"
                                                        onmouseout="this.className='ico4';" class="ico4">保存记录</li>
                                                    <li onmouseover="this.className='send_mode hover';" onmouseout="this.className='send_mode';"
                                                        class="send_mode" onclick="showShortSel();">消息发送方式</li>
                                                    <li>
                                                        <div class="tan" id="shortKeyMenu" style="display: none;">
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td>
                                                                        <span onclick="changeShortcut('Enter');" id="shortKeyMenu1" onmouseover="this.className='bg_sele';"
                                                                            onmouseout="this.className='';">&nbsp;*按 Enter 键发送消息</span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span onclick="changeShortcut('Ctrl + Enter');" id="shortKeyMenu2" onmouseover="this.className='bg_sele';"
                                                                            onmouseout="this.className='';">&nbsp;&nbsp;按 Ctrl+Enter 键发送消息</span>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <textarea id='inputbox' name="textarea" class="textarea11" onclick="$('shortKeyMenu').style.display = 'none';"></textarea>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="40" class="ts_text"">
                                            <div>
                                                <span id="shortKeyTip">[发送快捷键:Enter]</span>&nbsp;&nbsp;<span id="footerBox"></span></div>
                                        </td>
                                        <td align="right">
                                            <input onclick="doCloseChat();" type="button" class="button" onmouseover="this.className='button2';"
                                                onmouseout="this.className='button';" value="关闭" />
                                            <input onclick="sendMessage();" type="button" class="button" onmouseover="this.className='button2';"
                                                onmouseout="this.className='button';" value="发送" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="width: 100%;">
                                <div id="divText" runat="server">
                                    <marquee direction="left" behavior="scroll" width="100%" scrolldelay="50" scrollamount="2"
                                        id="adText" runat="server">
                                       <!--文字广告 -->
                                    </marquee>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <iframe id="framePlayer" src="" frameborder="0" height="0px" width="1px;"></iframe>
    </div>

    <script language="javascript" type="text/javascript" src="javascript/main.js?20100921"></script>

    <script type="text/javascript" src="javascript/chat.js?20100921" charset="utf-8"></script>

</body>
</html>
