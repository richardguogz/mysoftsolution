<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="LiveChat.Web.Admin.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <title>面料QQ</title>
    <style type="text/css">
        .sou
        {
            background: url(images/lsd.gif) 0px -56px;
            width: 80px;
            height: 24px;
            border: none;
            cursor: pointer;
            color: #fff;
            font-weight: bold;
        }
        .ts
        {
            background: url(images/ts.gif);
            width: 404px;
            height: 250px;
            margin: 0 auto;
            font-size: 13px;
            margin-top: 100px;
        }
    </style>

    <script type="text/javascript">

        function onload() {
            updateValidate();
            $get('companyid').focus();
        }
        
        function updateValidate() {
            document.getElementById('imgValidate').src = '/validate.aspx?' + Math.random();
        }

        function checkSubmit() {

            var companyid = $get('companyid').value;
            var userid = $get('userid').value;
            var password = $get('password').value;
            var code = $get('code').value;

            if (companyid.length == 0) {
                alert('公司ID不能为空！');
                $get('companyid').focus();
                return false;
            }
            
            if (userid.length == 0) {
                alert('用户名不能为空！');
                $get('userid').focus();
                return false;
            }

            if (password.length == 0) {
                alert('密码不能为空！');
                $get('password').focus();
                return false;
            }

            if (code.length < 4) {
                alert('输入的验证码长度不够！');
                $get('code').focus();
                return false;
            }

            var ret = AjaxMethods.ValidateCode(code);
            if (ret == false) {
                alert('输入的验证码不正确！');
                $get('code').focus();
            }
            else {
                ret = AjaxMethods.ValidateUser(companyid, userid, password);
                if (ret == false) {
                    alert('输入的公司ID、用户名或密码错误！');
                    $get('companyid').focus();
                }
            }

            return ret;
        }
        
    </script>

</head>
<body onload="onload();">
    <form id="form1" runat="server" onsubmit="return checkSubmit();">
    <div class="ts">
        <div style="font-weight: bold; padding-left: 15px; padding-top: 10px;">
            后台登录</div>
        <div style="padding-left: 100px; padding-top: 30px; line-height: 30px;">
            公司ID：<input name="" id="companyid" name="companyid" type="text" style="width: 110px;" maxlength="10" /><br />
            用户名：<input name="" id="userid" type="text" style="width: 110px;" maxlength="10" /><br />
            密　码：<input name="" id="password" type="password" style="width: 110px;" maxlength="20" /><br />
            验证码：<input name="" id="code" type="text" style="width: 50px;" maxlength="4" />
            <img src="/validate.aspx" title="点击图片可重新生成验证码。" align="absmiddle" style="cursor: pointer;" id="imgValidate"
                onclick="updateValidate();" /></div>
        <div style="text-align: center; padding-top: 10px;">
            <input name="" type="submit" value="登 录" class="sou" />
        </div>
    </div>
    </form>
</body>
</html>
