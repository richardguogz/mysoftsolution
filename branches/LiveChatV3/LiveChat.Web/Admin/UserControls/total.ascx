<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="total.ascx.cs" Inherits="LiveChat.Web.Admin.UserControls.total" %>
<div id="wrapper">
    <div id="tongji" style="width:100%;">
        <div class="listTitle">
            统计分析 (<% =startDate.ToString("yyyy/MM/dd")%>～<% =endDate.ToString("yyyy/MM/dd") %>)</div>
        <asp:Repeater ID="Repeater1" runat="server">
            <HeaderTemplate>
                <table>
                    <tr class="title">
                        <td>
                            公司名称
                        </td>
                        <td>
                            客服名称
                        </td>
                        <td>
                            <%= isMonth ? "月份" : "日期" %>
                        </td>
                        <td>
                            数量
                        </td>
                        <td>
                            操作
                        </td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%# Eval("CompanyName") %>
                    </td>
                    <td>
                        <%# Eval("SeatName") %>
                    </td>
                    <td>
                        <%# Eval("TotalDate")%>
                    </td>
                    <td>
                        <%# Eval("SessionCount")%>
                    </td>
                    <td>
                        <a id="query_<%# Eval("SeatID") %>_<%# Eval("TotalDate")%>" href="javascript:queryDetail('<%# Eval("SeatID") %>','<%# Eval("TotalDate")%>',<%# Eval("SessionCount")%>);">
                            阅读明细</a> <a id="close_<%# Eval("SeatID") %>_<%# Eval("TotalDate")%>" href="javascript:closeDetail('<%# Eval("SeatID") %>','<%# Eval("TotalDate")%>');"
                                style="display: none;">关闭阅读</a>
                    </td>
                </tr>
                <tr style="display: none;" id="tr_<%# Eval("SeatID") %>_<%# Eval("TotalDate")%>">
                    <td colspan="10">
                        <div id="div_<%# Eval("SeatID") %>_<%# Eval("TotalDate")%>"></div>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</div>

<script type="text/javascript">
    function queryDetail(id, date,count) {
        var sid = id + '_' + date;

        $get('query_' + sid).style.display = 'none';
        $get('close_' + sid).style.display = '';

        $get('tr_' + sid).style.display = '';

        if ($get('div_' + sid).innerHTML == '') {
            $get('div_' + sid).innerHTML = '正在读取明细，请稍候......';
            var param = { seatID: id, totalDate : date, totalCount : count };
            Ajax.updatePanel('div_' + sid, '/Admin/UserControls/totaldetail.ascx', param);
        }
    }

    function closeDetail(id, date) {
        var sid = id + '_' + date;

        $get('query_' + sid).style.display = '';
        $get('close_' + sid).style.display = 'none';

        $get('tr_' + sid).style.display = 'none';
    }
</script>

