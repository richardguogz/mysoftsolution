<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MySoft.PlatformService.WebForm._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        <ul>
            <% if (status != null)
               { %>
                <li>服务端状态信息如下：</li>
                <li>请求数:    <%=status.RequestCount %> times</li>
                <li>错误数:    <%=status.ErrorCount %> times</li>
                <li>总耗时:    <%=status.ElapsedTime %> ms</li>
                <li>总流量:    <%=status.DataFlow %> bytes</li>
                <li><br /></li>
                <li>服务计时区间：<%=status.TotalSeconds %> seconds</li>
                <li>每秒请求数:<%=status.AverageRequestCount%> times</li>
                <li>平均错误数:<%=status.AverageErrorCount%> times</li>
                <li>平均耗时数:<%=status.AverageElapsedTime %> ms</li>
                <li>每秒流量数:<%=status.AverageDataFlow%> bytes</li>
            <% } %>
        </ul>
        <ul>
            <% if (clients != null)
               {
                   var index = 1;
                   foreach (var client in clients)
                   {%>
            <li>连接信息(<% = index %>)：<%= client.ToString() %></li>
            <%  index++;
                   }
               } %>
        </ul>
    </div>
    </form>
</body>
</html>
