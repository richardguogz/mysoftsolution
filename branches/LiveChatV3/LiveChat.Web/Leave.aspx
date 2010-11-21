    <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Leave.aspx.cs" Inherits="LiveChat.Web.Leave" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>用户留言</title>
    <meta http-equiv="Content-Type" content="text/html;charset=UTF-8" />
    <link type="text/css" href="/images/leave.css" rel="stylesheet" rev="stylesheet" />

    <script type="text/javascript" src="/javascript/leave.js"></script>
    <script src="/javascript/validator.js" type="text/javascript"></script>
</head>
<body>
    <div class="wrap">
        <form id="form1" runat="server">
        <div class="head">
            <div id="head">
                <label>
                    您正在给我们留言：</label></div>
            <span class="left"></span><span class="right"></span>
        </div>
        <h1>
            请留言，我们会尽快给您回复！</h1>
        <div class="content">
            <input type="hidden" name="leavewordLostKey" value="8000E2C8B6AB91B8C7957BEE5C01E670256" />
            <fieldset>
                <p class="small">
                    <label>
                        <span style="color: red">*</span>姓 名:</label><input name="name" type="text" value="" msg="姓名不能为空！"
                            require="true" tabindex="1" /></p>
                <p class="fixfloat small">
                    <label>
                        联系电话:</label><input type="text" name="contact" value="" class="phone" tabindex="2" /></p>
                <p>
                    <label>
                        <span style="color: red">*</span>电子邮件:</label><input name="email" type="text" require="true" dataType="Email" msg="输入的邮件地址不正确！"
                            value="" tabindex="3" /></p>
                <p>
                    <label>
                        <span style="color: red">*</span>留言主题:</label><input name="subject" value="" tabindex="4" type="text" require="true" dataType="LimitB" min="10" max="100" msg="留言主题在10-100个字符之间！" /></p>
                <div class="ctn">
                    <label>
                        <span style="color: red">*</span>留言内容:</label><textarea name="feed" require="true" dataType="LimitB" min="10" max="1000" msg="留言内容在10-1000个字符之间！"
                            tabindex="5"></textarea></div>
                <div id="mobile">
                    <ul>
                        <li style='visibility: hidden;'>
                            <input id="mobilecheck" type="checkbox" value="true" class="checkbox" /><span id="sms"
                                style='display: none;'>同时将留言发送到手机</span>&nbsp;</li>
                        <li>验证码:<input require="true" msg="验证码不能为空！" type="text" name="textImage" id="textImage" /></li>
                        <li>
                            <img src='/validate.aspx' id="textImage1" /></li>
                        <li><a href="#" onclick="reloadTextImage();">重新获取</a></li>
                    </ul>
                </div>
            </fieldset>
        </div>
        <div class="footer">
        </div>
        <div class="button">
            <p>
                <input type="button" id="enter" tabindex="6" value="发送" onclick="submitForm();" /><input
                    type="reset" value="重置" id="dialback"></p>
        </div>
        <div id="error">
        </div>
        <div id="message">
        </div>
        </form>
        <div id="live800">
        </div>
    </div>

    <script type="text/javascript">
        Ajax.showErrorMessage = false;
        Ajax.onException = function(data) {
            if (data.Success == false) {

                $get('message').style.display = 'none';
                $get('error').style.display = 'block';
                $get('error').innerHTML = data.Message.replace(/\r\n/ig,'<br/>');
            }
        };

        function reloadTextImage() {

            var img = document.getElementById('textImage1');
            img.src = '/validate.aspx?' + Math.random();
        }
    </script>

</body>
</html>
