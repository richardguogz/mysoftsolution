﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MySoft.PlatformService.WebForm._Default" %>

<%@ Register Src="StatusControl.ascx" TagName="StatusControl" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>服务统计信息</title>
    <style type="text/css">
        li
        {
            list-style-type: none;
        }
        
        ul
        {
            margin: 0px;
            padding: 0px;
        }
    </style>
    <script type="text/javascript">
        function ShowDiv(id) {
            var inputs = document.getElementsByName('inputNode');
            for (var i = 0; i < inputs.length; i++) {
                var div = document.getElementById(inputs[i].value);
                if (div.id == id) {
                    div.style.display = '';
                    document.getElementById('currentIndex').value = i;

                    timer();
                }
                else {
                    div.style.display = 'none';
                }
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" id="currentIndex" value="0" />
    <div style="cursor: pointer; text-align: center; width: 150px; background: #f90;
        border: 3px solid #ccc; margin: 5px; padding: 5px; float: left;" onclick="if(confirm('确定重置服务端的所有状态吗?')) { AjaxMethods.ClearServerStatus(); }">
        重置状态信息
    </div>
    <% foreach (var node in nodelist)
       {%>
    <input type="hidden" name="inputNode" value="div<% = node.Key %>" />
    <div onclick="ShowDiv('div<% = node.Key %>');" style="cursor: pointer; text-align: center;
        width: 150px; background: #ddd; border: 3px solid #ccc; margin: 5px; padding: 5px;
        float: left;">
        查看【<% = node.Key %>】
    </div>
    <% } %>
    <div style="clear: both;">
    </div>
    <div id="divContainer">
        <uc1:StatusControl ID="StatusControl1" runat="server" />
    </div>
    </form>
    <script type="text/javascript">

        var timer = function () {
            var value = document.getElementById('currentIndex').value;
            Ajax.updatePanel('divContainer', '~/StatusControl.ascx', { CurrentIndex: value });
        };

        setInterval(function () {
            timer();
        }, 5000);
    </script>
</body>
</html>