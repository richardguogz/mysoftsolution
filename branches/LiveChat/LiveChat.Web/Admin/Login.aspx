<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="LiveChat.Web.Admin.Login" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>全球面料网后台管理系统</title>
    <link href="css/main.css" rel="stylesheet" type="text/css" />

    <script src="/javascript/common.js" type="text/javascript"></script>

    <script src="/javascript/jquery/jquery.min.js" type="text/javascript""></script>

    <script src="/javascript/jquery/jquery.validate.min.js" type="text/javascript"></script>

    <script type="text/javascript">

        function loginSuccess(id) {
            if (parent != null) {
                parent.location = "/index.aspx";
            }
            else {
                location = "/index.aspx";
            }
        }

        function OnSubmit() {
            var uid = $get('Admin_UserName').value;
            var pwd = $get('Admin_Password').value;
            var vcode = $get("Admin_Validate").value;

            if (!uid) {
                alert('用户名不能为空！');
                return false;
            }

            if (!pwd) {
                alert('密码不能为空！');
                return false;
            }

            if (!vcode) {
                alert('请输入验证码!');
                return false;
            }

            return true;
        }

        //初始化页面处理
        $(function() {
            var name = $get('Admin_UserName')
            if (name) name.focus();
        });
    </script>

</head>
<body id="login">
    <form id="form1" runat="server" onsubmit="return OnSubmit();">
    <div id="container">
        <div class="loginfo">
            <ul>
                <li>帐&nbsp;&nbsp;&nbsp;&nbsp;号：&nbsp;<input name="Admin_UserName" id="Admin_UserName"
                    type="text" style="width: 150px" /></li>
                <li>密&nbsp;&nbsp;&nbsp;&nbsp;码：&nbsp;<input name="Admin_Password" id="Admin_Password"
                    type="password" style="width: 150px" /></li>
                <li>验证码：&nbsp;<input name="Admin_Validate" id="Admin_Validate" type="text" style="width: 54px" />&nbsp;&nbsp;<img
                    style="vertical-align: top;" src="validate.aspx" /></li>
                <li>
                    <input type="checkbox" id="ChkSaveLogin" name="ChkSaveLogin" /><label for="ChkSaveLogin">记住我的登录状态&nbsp;
			            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label></li>
                <li class="btn">
                    <input type="submit" value="登 录" /></li>
            </ul>
            <div class="spacelogin">
            </div>
            <div class="btminfo">
                技术支持：全球面料网</div>
        </div>
    </div>
    </form>
</body>
</html>
