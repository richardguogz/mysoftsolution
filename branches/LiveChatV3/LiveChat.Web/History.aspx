﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="History.aspx.cs" Inherits="LiveChat.Web.History" %>

<%@ Import Namespace="LiveChat.Web" %>
<%@ Import Namespace="LiveChat.Entity" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>聊天记录</title>
    <style type="text/css">
        /* CSS Document talk mml_2009-4-14 */body
        {
            /* line-height: 18px; */
            font-size: 12px;
            font-family: "宋体" , "MS Gothic" , "Times New Roman" , "PMingLiU";
            margin: 0px;
            padding: 0px;
        }
        .talk
        {
            min-width: 640px;
            min-height: 450px;
        }
        .top
        {
            background-image: url(talk05.jpg);
            background-repeat: repeat-x;
            line-height: 43px;
            height: 43px;
        }
        .top .d_l
        {
            background-image: url(talk01.jpg);
            background-repeat: no-repeat;
            font-size: 12px;
            color: #fff;
            padding-left: 40px;
            width: 365px;
            float: left;
        }
        #headerBox
        {
            font-size: 12px;
            font-weight: bold;
            color: #fff;
        }
        .top .d_r
        {
            /*background-image: url(talk02.jpg);*/
            background-repeat: no-repeat;
            height: 43px;
            background-position: right;
            width: 165px;
            float: right;
            cursor: pointer;
        }
        .clear
        {
            clear: both;
        }
        .con
        {
            background-image: url(talk08.jpg);
            background-repeat: repeat-x;
            background-position: bottom;
            border-bottom: 1px solid #014e86;
            border-left: 1px solid #014e86;
            border-right: 1px solid #014e86;
        }
        .con2
        {
            border-bottom: 1px solid #add7ef;
            border-left: 1px solid #add7ef;
            border-right: 1px solid #add7ef;
        }
        .con3
        {
            border-bottom: 1px solid #79adcd;
            border-left: 1px solid #add7ef;
            border-right: 1px solid #add7ef;
            background-color: #e4f1f9;
            padding: 7px 4px 4px;
            position: relative;
        }
        .border
        {
            border: 1px solid #5a9bc9;
            background-color: #FFF;
        }
        .con .left_01
        {
            background-color: #FFF;
            padding: 8px;
        }
        .pic
        {
            border: 1px solid #e0e0e0;
        }
        .button
        {
            background-image: url(button_sele.gif);
            background-repeat: no-repeat;
            height: 25px;
            width: 76px;
            border: 0;
        }
        .button2
        {
            background-image: url(button_sele2.gif);
            background-repeat: no-repeat;
            height: 25px;
            width: 76px;
            border: 0;
            cursor: pointer;
        }
        table
        {
            margin: 0px;
            padding: 0px;
        }
        td
        {
            margin: 0px;
            padding: 0px;
        }
        .right
        {
            margin-right: 4px;
            margin-bottom: 7px;
            margin-left: 4px;
            padding: 8px;
        }
        .kj
        {
            height: 33px;
            width: 100%;
        }
        .kj ul
        {
            list-style-type: none;
            font-size: 12px;
            height: 33px;
            margin: 0;
            padding: 0;
        }
        .kj li
        {
            float: left;
            line-height: 25px;
            height: 25px;
            text-indent: 20px;
            padding-right: 1px;
            padding-left: 1px;
            margin-right: 5px;
            margin-top: 5px;
            margin-bottom: 5px;
        }
        .kj .send_mode
        {
            width: 82px;
            float: right;
            text-indent: 5px;
            padding-right: 0px;
            margin-right: 0px;
        }
        .name
        {
            color: #3f81c8;
        }
        .neirong
        {
            margin-bottom: 15px;
        }
        .tan
        {
            border: 1px solid #67a2cd;
            width: 165px;
            position: absolute;
            background-color: #FFF;
            line-height: 22px;
            right: 189px;
            bottom: 90px;
            display: none;
        }
        .bg_sele
        {
            background-color: #dfeef7;
            display: block;
            cursor: pointer;
        }
        .textarea, .textarea1, .textarea11
        {
            border: 1px solid #609ecb;
            width: 99%;
            overflow-y: auto;
            height: 90px;
            font-size: 12px;
        }
        .con3 .scoll
        {
            scrollbar-face-color: #C0E4FE;
            scrollbar-shadow-color: #FFFFFF;
            scrollbar-highlight-color: #FFFFFF;
            scrollbar-3dlight-color: #C0E4FE;
            scrollbar-darkshadow-color: #C0E4FE;
            scrollbar-track-color: #F7FCFF;
            scrollbar-arrow-color: #5A9BD5;
            overflow: auto;
            word-wrap: break-word;
            word-break: break-all;
            width: 100%;
            border: 1px solid #5a9bc9;
            background-color: #FFF;
        }
        p.info
        {
            font-size: 12px;
            color: #666666;
        }
        p.operator
        {
            font-size: 9pt;
            color: #2c82b5;
        }
        p.visitor
        {
            color: #e28c02;
            font-size: 9pt;
        }
        p.operator span
        {
            margin-left: 12px;
        }
        p.visitor span
        {
            margin-left: 12px;
        }
        span
        {
            mso-ascii-theme-font: minor-fareast;
            font-weight: normal;
            color: #000;
        }
        p
        {
            margin: 0px;
            padding: 4px 0; /* line-height: 1.5em; */
        }
        .ts_text
        {
            color: #26639F;
            font-size: 12px;
            line-height: 30px;
        }
        .ico
        {
            background-image: url(ico01.gif);
            background-repeat: no-repeat;
            background-position: left center;
        }
        .icof
        {
            background-image: url(file.png);
            background-repeat: no-repeat;
            background-position: left center;
        }
        .ico0
        {
            background-image: url(face/face1.gif);
            background-repeat: no-repeat;
            background-position: left center;
        }
        .ico2
        {
            background-image: url(ico02.gif);
            background-repeat: no-repeat;
            background-position: left center;
        }
        .ico3
        {
            background-image: url(ico002.gif);
            background-repeat: no-repeat;
            background-position: left center;
        }
        .ico4
        {
            background-image: url(ico03.gif);
            background-repeat: no-repeat;
            background-position: left center;
        }
        .hover
        {
            /*border: 1px solid #5A9BC9;*/
            background-color: #C0E4FE;
            cursor: pointer;
        }
        #history
        {
            height: 180px;
        }
        #history span
        {
            text-indent: 1em;
        }
        #historyBox
        {
            border: 1px solid #609ecb;
            padding: 5px;
            overflow-x: auto;
        }
        .historyTitle
        {
            line-height: 30px;
            text-align: center;
            background-color: #e4f1f9;
            border: solid 1px #609ecb;
        }
        #chatWindow
        {
            margin: 5px;
        }
        .list
        {
            margin: 0px;
            padding: 0px;
            padding-top: 10px;
        }
        .list li
        {
            margin: 3px 0px 0px 3px;
            font-size: 12px;
            list-style-type: none;
            line-height: 20px;
            text-align: left;
        }
        .list .itemimg
        {
            border: 0px;
            padding-right: 5px;
        }
        .emo
        {
            width: 19px;
            height: 19px;
            background-image: url(face.gif);
            background-repeat: no-repeat;
            cursor: pointer;
        }
        #tableface td
        {
            text-align: center;
        }
        #uploadFileBox
        {
            position: absolute;
            left: 6px;
            top: 184px;
            height: 50px;
            padding-left: 5px;
            background-color: #FFFFFF;
            width: 380px;
            border: 1px solid #5A9BC9;
            display: none;
        }
        #uploadFileFrame
        {
            height: 24px;
            overflow: hidden;
            width: 380px;
            border-top-style: none;
            border-right-style: none;
            border-bottom-style: none;
            border-left-style: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <!--ChatWindow -->
    <div id="chatWindow">
        <div class="historyTitle">
            <% =title %>
        </div>
        <div id="content-wrap">
            <div id="chatbody">
                <div id="historyBox">
                    <% foreach (ClientMessage msg in msgs)
                       {
                           if (msg.SenderID == userID)
                           { %>
                    <p class="visitor">
                        <% if (msg.Type == MessageType.Picture)
                           { %>
                        您向客服发送了一个截屏 <font style="font-weight: normal;">
                            <% = msg.SendTime.ToLongTimeString()%></font>:
                        <% }
                           else if (msg.Type == MessageType.File)
                           {
                        %>
                        您向客服传送了一个文件 <font style="font-weight: normal;">
                            <% = msg.SendTime.ToLongTimeString()%></font>:
                        <%
                            }
                           else
                           { %>
                        <% =msg.SenderName %>&nbsp;说 <font style="font-weight: normal;">
                            <% = msg.SendTime.ToLongTimeString() %></font>:
                        <% } %>
                        <br />
                        <span>
                            <% =msg.Content %></span>
                    </p>
                    <% }
                           else
                           { %>
                    <p class="operator">
                        <% if (msg.Type == MessageType.Picture)
                           { %>
                        <% = msg.SenderName %>向您发送了一个截屏 <font style="font-weight: normal;">
                            <% = msg.SendTime.ToLongTimeString()%></font>:
                        <% }
                           else if (msg.Type == MessageType.File)
                           {
                        %>
                        <% = msg.SenderName %>向您传送了一个文件 <font style="font-weight: normal;">
                            <% = msg.SendTime.ToLongTimeString()%></font>:
                        <%
                            }
                           else
                           { %>
                        <% =msg.SenderName %>&nbsp;说 <font style="font-weight: normal;">
                            <% = msg.SendTime.ToLongTimeString()%></font>:
                        <% } %>
                        <br />
                        <span>
                            <% =msg.Content %></span>
                    </p>
                    <% }
                       } %>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
