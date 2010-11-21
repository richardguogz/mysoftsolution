<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="analysis.aspx.cs" Inherits="LiveChat.Web.Admin.analysis" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <title>面料QQ</title>
    <link rel="stylesheet" type="text/css" href="/admin/css/main.css" />
    <link rel="stylesheet" type="text/css" href="/admin/css/mail.css" />

    <script type="text/javascript" src="/admin/datepicker/WdatePicker.js"></script>

    <style type="text/css">
        #history
        {
            background-color: #F5FFFA;
            border: 1px solid #609ecb;
            padding: 5px;
            overflow-x: auto;
            text-align: left;
            line-height: normal;
            margin: 2px;
        }
        p.info
        {
            font-size: 12px;
            color: #666666;
        }
        p.operator
        {
            font-size: 9pt;
            color: #2c82b5;
        }
        p.visitor
        {
            color: #e28c02;
            font-size: 9pt;
        }
        p.operator span
        {
            margin-left: 12px;
            margin-top: 3px;
            display: block;
        }
        p.visitor span
        {
            margin-left: 12px;
            margin-top: 3px;
            display: block;
        }
    </style>

    <script type="text/javascript">
        function queryTotal() {

            gotoPage(1);
        }

        function changeSelect(target) {
            var select = $('dpSeat');
            select.options.length = 0;
            if (target.value == "-1") {
                select.options[0] = new Option('全部', '-1');
                return;
            }
            var list = AjaxMethods.GetSeats(target.value);
            select.options[0] = new Option('全部', '-1');
            for (var i = 0; i < list.length; i++) {
                select.options[i+1] = new Option(list[i].ShowName, list[i].SeatCode);
            }
        }

        function gotoPage(page) {

            var startDate = $('startDate');
            var endDate = $('endDate');
            if (startDate.value.length == 0) {
                startDate.focus();
                alert('请选择开始日期！');
                return;
            }

            if (endDate.value.length == 0) {
                endDate.focus();
                alert('请选择结束日期！');
                return;
            }

            var param = { startDate: startDate.value, endDate: endDate.value, pageIndex: page };

            var company = $('dpCompany');
            var seat = $('dpSeat');
            if (company.value != '-1') param.companyID = company.value;
            if (seat.value != '-1') param.seatCode = seat.value;

            $('queryInfo').innerHTML = '正在查询数据，请稍候......';
            Ajax.updatePanel('queryInfo', '/Admin/UserControls/analysis.ascx', param);
        }
    </script>

</head>
<body class="listBg" style="background: #fff;" onload="queryTotal();">
    <form id="form1" runat="server">
    <center>
        <div class="rounde">
            <div class="listInfo">
                <div style="width: 100%">
                    <span>公司名称：
                        <asp:DropDownList ID="dpCompany" runat="server" onchange="changeSelect(this);">
                        </asp:DropDownList>
                    </span>客服名称：<span>
                        <asp:DropDownList ID="dpSeat" runat="server">
                        </asp:DropDownList>
                    </span>
                    <br />
                    </span><span>开始日期：<input type="text" id="startDate" size="15" class="Wdate" value="<% =DateTime.Parse(DateTime.Today.ToString("yyyy-MM-01")).ToString("yyyy-MM-dd") %>"
                        onclick="WdatePicker({startDate:'%y-%M-%d',dateFmt:'yyyy-MM-dd'});" />
                        &nbsp;结束日期：<input type="text" id="endDate" size="15" class="Wdate" value="<% =DateTime.Today.ToString("yyyy-MM-dd") %>"
                            onclick="WdatePicker({startDate:'%y-%M-%d',dateFmt:'yyyy-MM-dd'});" />
                        &nbsp;<input type="button" value="开始查询" class="listBtn" onclick="queryTotal();" />&nbsp;
                    </span><span style="float: right;"><a href="total.aspx?param=<%= GetRequestParam<string>("param", null) %>&<%= DateTime.Now.ToString("yyyyMMddHHmmss") %>">数据统计</a> </span>
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
