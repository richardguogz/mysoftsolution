<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MySoft.PlatformService.WebForm._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>服务统计信息</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        &nbsp;<ul>
            <% if (status != null)
               { %>
                <li>服务端状态信息如下：(<% =DateTime.Now %>)<asp:Button ID="btnClear" runat="server" 
                        onclick="btnClear_Click" Text="清除所有状态" />
            </li>
                <li><hr /></li>
                <li>汇总状态信息</li>
                <li>请求数:        <%=status.Summary.RequestCount%>times</li>
                <li>成功数:        <%=status.Summary.SuccessCount%>times</li>
                <li>错误数:        <%=status.Summary.ErrorCount%>times</li>
                <li>总耗时:        <%=status.Summary.ElapsedTime%>ms</li>
                <li>总流量:        <%= Math.Round(status.Summary.DataFlow * 1.0 / 1024, 4)%>kb</li>
                <li>服务运行总时间： <%=status.Summary.RunningSeconds%>seconds</li>
                <li>每秒请求数:    <%=status.Summary.AverageRequestCount%>times</li>
                <li>平均成功数:    <%=status.Summary.AverageSuccessCount%>times</li>
                <li>平均错误数:    <%=status.Summary.AverageErrorCount%>times</li>
                <li>平均耗时数:    <%=status.Summary.AverageElapsedTime%>ms</li>
                <li>每秒流量数:    <%=Math.Round(status.Summary.AverageDataFlow * 1.0 / 1024, 4)%>kb</li>
                <li><hr /></li>
                <li>最高状态信息</li>
                <li>最大请求数:    <%=status.Highest.RequestCount%>times (<%= status.Highest.RequestCountCounterTime%>)</li>
                <li>最大成功数:    <%=status.Highest.SuccessCount%>times (<%= status.Highest.SuccessCountCounterTime%>)</li>
                <li>最大错误数:    <%=status.Highest.ErrorCount%>times (<%= status.Highest.ErrorCountCounterTime%>)</li>
                <li>最大耗时:      <%=status.Highest.ElapsedTime%>ms (<%= status.Highest.ElapsedTimeCounterTime%>)</li>
                <li>最大流量:      <%=Math.Round(status.Highest.DataFlow * 1.0 / 1024, 4)%>kb (<%= status.Highest.DataFlowCounterTime%>)</li>
                <li><hr /></li>
                <li>当前状态信息</li>
                <% if (status.Latest != null)
                   {  %>
                <li>时间:      <%=status.Latest.CounterTime%></li>
                <li>请求数:    <%=status.Latest.RequestCount%>times</li>
                <li>成功数:    <%=status.Latest.SuccessCount%>times</li>
                <li>错误数:    <%=status.Latest.ErrorCount%>times</li>
                <li>耗时:      <%=status.Latest.ElapsedTime%>ms</li>
                <li>流量:      <%=Math.Round(status.Latest.DataFlow * 1.0 / 1024, 4)%>kb</li>
                    <% }
               } %>
        </ul>
        <hr />
        <ul>
            <li>客户端连接信息</li>
                <% if (clients != null)
               {
                   int index = 1;
                   foreach (MySoft.IoC.ConnectInfo client in clients)
                   {%>
            <li><% = index %>==> <%= client.IP %>(<font color="red"><%= client.Count %></font>)</li>
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
        }, 5000);
    </script>
</body>
</html>