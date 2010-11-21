<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="analysis.ascx.cs" Inherits="LiveChat.Web.Admin.UserControls.analysis" %>
<div id="wrapper">
    <div id="tongji">
        <div class="listTitle">
            ͳ�Ʒ��� (<% =startDate.ToString("yyyy/MM/dd")%>��<% =endDate.ToString("yyyy/MM/dd") %>)</div>
        <asp:Repeater ID="rptData" runat="server">
            <HeaderTemplate>
                <table>
                    <tr class="title">
                        <td>
                            ��˾����
                        </td>
                        <td>
                            �ͷ�����
                        </td>
                        <td>
                            �û�ID
                        </td>
                        <td>
                            ���Ե���
                        </td>
                        <td>
                            ������Ϣ
                        </td>
                        <td>
                            ��ʼʱ��
                        </td>
                        <td>
                            ����ʱ��
                        </td>
                        <td>
                            ����
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
                        <%# Eval("UserID") %>
                    </td>
                    <td>
                        <%# Eval("FromAddress") %>
                    </td>
                    <td width="160px">
                        <%# CutLen(Eval("RequestMessage"),20,"...") %>
                    </td>
                    <td>
                        <%# Eval("StartTime", "{0:yyyy-MM-dd HH:mm:ss}")%>
                    </td>
                    <td>
                        <%# Eval("EndTime", "{0:yyyy-MM-dd HH:mm:ss}")%>
                    </td>
                    <td>
                        <a id="query_<%# Eval("SID") %>" href="javascript:queryDetail('<%# Eval("SID") %>');">
                            ������ϸ</a> <a id="close_<%# Eval("SID") %>" href="javascript:closeDetail('<%# Eval("SID") %>');"
                                style="display: none;">�ر���ϸ</a>
                    </td>
                </tr>
                <tr style="display: none;" id="tr_<%# Eval("SID") %>">
                    <td colspan="10" align="left">
                        <div id="div_<%# Eval("SID") %>"></div>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <asp:Label ID="txtHtml" runat="server" Text="Label"></asp:Label>
    </div>
</div>

<script type="text/javascript">
    function queryDetail(sid) {

        $get('query_' + sid).style.display = 'none';
        $get('close_' + sid).style.display = '';

        $get('tr_' + sid).style.display = '';

        if ($get('div_' + sid).innerHTML == '') {
            $get('div_' + sid).innerHTML = '���ڶ�ȡ��ϸ�����Ժ�......';
            var param = { sid: sid };
            Ajax.updatePanel('div_' + sid, '/Admin/UserControls/analysisdetail.ascx', param);
        }
    }

    function closeDetail(sid) {
        $get('query_' + sid).style.display = '';
        $get('close_' + sid).style.display = 'none';

        $get('tr_' + sid).style.display = 'none';
    }
</script>

