<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MySoft.PlatformService.WebForm._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>服务统计信息</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        <ul>
            <% if (status != null)
               { %>
                <li>服务端状态信息如下：(<% =DateTime.Now %>)</li>
                <li><hr /></li>
                <li>请求数:    <%=status.RequestCount %> times</li>
                <li>成功数:    <%=status.SuccessCount %> times</li>
                <li>错误数:    <%=status.ErrorCount %> times</li>
                <li>总耗时:    <%=status.ElapsedTime %> ms</li>
                <li>总流量:    <%= Math.Round(status.DataFlow * 1.0 /1024,4) %> kb</li>
                <li><hr /></li>
                <li>服务计时区间：<%=status.TotalSeconds %> seconds</li>
                <li>每秒请求数:<%=status.AverageRequestCount%> times</li>
                <li>平均成功数:<%=status.AverageSuccessCount%> times</li>
                <li>平均错误数:<%=status.AverageErrorCount%> times</li>
                <li>平均耗时数:<%=status.AverageElapsedTime %> ms</li>
                <li>每秒流量数:<%=Math.Round(status.AverageDataFlow *1.0 /1024,4) %> kb</li>
            <% } %>
            <li><hr /></li>
            <li>最高状态信息</li>
            <%if (highestStatus != null)
              { %>
                <li>最大请求数:    <%=highestStatus.RequestCount%> times (<%= highestStatus.RequestCountOccurTime %>)</li>
                <li>最大成功数:    <%=highestStatus.SuccessCount%> times (<%= highestStatus.SuccessCountOccurTime %>)</li>
                <li>最大错误数:    <%=highestStatus.ErrorCount%> times (<%= highestStatus.ErrorCountOccurTime %>)</li>
                <li>最大耗时:      <%=highestStatus.ElapsedTime%> ms (<%= highestStatus.ElapsedTimeOccurTime %>)</li>
                <li>最大流量:      <%=Math.Round(highestStatus.DataFlow * 1.0 / 1024, 4)%> kb (<%= highestStatus.DataFlowOccurTime %>)</li>
            <% } %>
            <li><hr /></li>
            <li>当前状态信息</li>
            <%if (timeStatus != null)
              { %>
                <li>时间:      <%=timeStatus.CounterTime%></li>
                <li>请求数:    <%=timeStatus.RequestCount%> times</li>
                <li>成功数:    <%=timeStatus.SuccessCount%> times</li>
                <li>错误数:    <%=timeStatus.ErrorCount%> times</li>
                <li>耗时:      <%=timeStatus.ElapsedTime%> ms</li>
                <li>流量:      <%=Math.Round(timeStatus.DataFlow *1.0 /1024,4) %> kb</li>
            <% } %>
        </ul>
        <hr />
        <ul>
            <li>客户端连接信息</li>
            <% if (clients != null)
               {
                   int index = 1;
                   foreach (MySoft.IoC.ConnectInfo client in clients)
                   {%>
            <li><% = index %> ==> <%= client.IP %>(<%= client.Count %>)</li>
            <%  index++;
                   }
               } %>
        </ul>
    </div>
    </form>

    <script type="text/javascript">

        var timer = function () {
            document.location.reload();
        };

        setInterval(function () {
            timer();
        }, 10000);
    </script>
</body>
</html>