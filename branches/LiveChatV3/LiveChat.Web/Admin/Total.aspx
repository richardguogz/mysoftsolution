<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="total.aspx.cs" Inherits="LiveChat.Web.Admin.total" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <title>����QQ</title>
    <link rel="stylesheet" type="text/css" href="/admin/css/main.css" />
    <link rel="stylesheet" type="text/css" href="/admin/css/mail.css" />

    <script type="text/javascript" src="/admin/datepicker/WdatePicker.js"></script>

    <script type="text/javascript">
        function queryTotal(type) {

            var param = {};
            if (type == 'day') {

                var startDate = $('startDate');
                var endDate = $('endDate');
                if (startDate.value.length == 0) {
                    startDate.focus();
                    alert('��ѡ��ʼ���ڣ�');
                    return;
                }

                if (endDate.value.length == 0) {
                    endDate.focus();
                    alert('��ѡ��������ڣ�');
                    return;
                }
                param = { startDate: startDate.value, endDate: endDate.value, type: type };
            } else {

                var startMonth = $('startMonth');
                var endMonth = $('endMonth');
                if (startMonth.value.length == 0) {
                    startMonth.focus();
                    alert('��ѡ��ʼ�·ݣ�');
                    return;
                }

                if (endMonth.value.length == 0) {
                    endMonth.focus();
                    alert('��ѡ������·ݣ�');
                    return;
                }
                param = { startMonth: startMonth.value, endMonth: endMonth.value, type: type };
            }

            var company = $('dpCompany');
            var seat = $('dpSeat');
            if (company.value != '-1') param.companyID = company.value;
            if (seat.value != '-1') param.seatCode = seat.value;

            $('queryInfo').innerHTML = '���ڲ�ѯ���ݣ����Ժ�......';
            Ajax.updatePanel('queryInfo', '/Admin/UserControls/total.ascx', param);
        }

        function changeType(value) {
            if (value == 'day') {
                $('top1').style.display = '';
                $('top2').style.display = 'none';
            } else {
                $('top1').style.display = 'none';
                $('top2').style.display = '';
            }
        }

        function changeSelect(target) {
            var select = $('dpSeat');
            select.options.length = 0;
            if (target.value == "-1") {
                select.options[0] = new Option('ȫ��', '-1');
                return;
            }
            var list = AjaxMethods.GetSeats(target.value);
            select.options[0] = new Option('ȫ��', '-1');
            for (var i = 0; i < list.length; i++) {
                select.options[i+1] = new Option(list[i].ShowName, list[i].SeatCode);
            }
        }
    </script>

</head>
<body class="listBg" style="background: #fff;" onload="queryTotal('day');">
    <form id="form1" runat="server">
    <center>
        <div class="rounde">
            <div class="listInfo">
                <div style="width: 100%">
                    <span>��˾���ƣ�
                        <asp:DropDownList ID="dpCompany" runat="server" onchange="changeSelect(this);">
                        </asp:DropDownList>
                    </span>�ͷ����ƣ�<span>
                        <asp:DropDownList ID="dpSeat" runat="server">
                        </asp:DropDownList>
                    </span>
                    <br />
                    <span>ͳ�����ͣ�<select id="selectType" onchange="changeType(this.value);">
                        <option value="day">����ͳ��</option>
                        <option value="month">����ͳ��</option>
                    </select>
                    </span><span id="top1">��ʼ���ڣ�<input type="text" id="startDate" size="15" class="Wdate"
                        value="<% =DateTime.Parse(DateTime.Today.ToString("yyyy-MM-01")).ToString("yyyy-MM-dd") %>"
                        onclick="WdatePicker({startDate:'%y-%M-%d',dateFmt:'yyyy-MM-dd'});" />
                        &nbsp;�������ڣ�<input type="text" id="endDate" size="15" class="Wdate" value="<% =DateTime.Today.ToString("yyyy-MM-dd") %>"
                            onclick="WdatePicker({startDate:'%y-%M-%d',dateFmt:'yyyy-MM-dd'});" />
                        &nbsp;<input type="button" value="��ʼ��ѯ" class="listBtn" onclick="queryTotal('day');" />&nbsp;
                    </span><span id="top2" style="display: none;">��ʼ�·ݣ�<input type="text" id="startMonth"
                        size="15" class="Wdate" value="<% =DateTime.Today.AddMonths(-1).ToString("yyyy-MM") %>"
                        onclick="WdatePicker({startDate:'%y-%M',dateFmt:'yyyy-MM'});" />
                        &nbsp;�����·ݣ�<input type="text" id="endMonth" size="15" class="Wdate" value="<% =DateTime.Today.ToString("yyyy-MM") %>"
                            onclick="WdatePicker({startDate:'%y-%M',dateFmt:'yyyy-MM'});" />
                        &nbsp;<input type="button" value="��ʼ��ѯ" class="listBtn" onclick="queryTotal('month');" />&nbsp;
                    </span><span style="float: right;"><a href="analysis.aspx?param=<%= GetRequestParam<string>("param", null) %>&<%= DateTime.Now.ToString("yyyyMMddHHmmss") %>">������ϸ</a> </span>
                </div>
                <div class="listLine">
                </div>
                <div class="hack2">
                </div>
                <div id="queryInfo">
                </div>
            </div>
        </div>
    </center>
    </form>
</body>
</html>
