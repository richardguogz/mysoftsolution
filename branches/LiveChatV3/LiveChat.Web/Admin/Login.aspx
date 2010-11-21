<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="LiveChat.Web.Admin.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <title>����QQ</title>
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
                alert('��˾ID����Ϊ�գ�');
                $get('companyid').focus();
                return false;
            }
            
            if (userid.length == 0) {
                alert('�û�������Ϊ�գ�');
                $get('userid').focus();
                return false;
            }

            if (password.length == 0) {
                alert('���벻��Ϊ�գ�');
                $get('password').focus();
                return false;
            }

            if (code.length < 4) {
                alert('�������֤�볤�Ȳ�����');
                $get('code').focus();
                return false;
            }

            var ret = AjaxMethods.ValidateCode(code);
            if (ret == false) {
                alert('�������֤�벻��ȷ��');
                $get('code').focus();
            }
            else {
                ret = AjaxMethods.ValidateUser(companyid, userid, password);
                if (ret == false) {
                    alert('����Ĺ�˾ID���û������������');
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
            ��̨��¼</div>
        <div style="padding-left: 100px; padding-top: 30px; line-height: 30px;">
            ��˾ID��<input name="" id="companyid" name="companyid" type="text" style="width: 110px;" maxlength="10" /><br />
            �û�����<input name="" id="userid" type="text" style="width: 110px;" maxlength="10" /><br />
            �ܡ��룺<input name="" id="password" type="password" style="width: 110px;" maxlength="20" /><br />
            ��֤�룺<input name="" id="code" type="text" style="width: 50px;" maxlength="4" />
            <img src="/validate.aspx" title="���ͼƬ������������֤�롣" align="absmiddle" style="cursor: pointer;" id="imgValidate"
                onclick="updateValidate();" /></div>
        <div style="text-align: center; padding-top: 10px;">
            <input name="" type="submit" value="�� ¼" class="sou" />
        </div>
    </div>
    </form>
</body>
</html>
