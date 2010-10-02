<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Total.aspx.cs" Inherits="LiveChat.Web.Admin.Total" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head><meta http-equiv="Content-Type" content="text/html; charset=gb2312" /><title>
	无标题文档
</title><meta name="description" content="数米网邮件后台管理系统" /><meta name="keywords" content="数米网，邮件后台，邮件管理系统，数米网邮件，数米网管理系统，后台管理系统" /><link rel="stylesheet" type="text/css" href="/css/main.css" /><link rel="stylesheet" type="text/css" href="/css/mail.css" />
    <script type="text/javascript" src="/datepicker/WdatePicker.js"></script>
    <script type ="text/javascript">
        function queryTotal(type) {

            var param = {};
            if(type == 'day') {
                                            
                var startDate = $('startDate');
                var endDate = $('endDate');
                if(startDate.value.length ==0) {
                    startDate.focus();
                    alert('请选择开始日期！');
                    return;
                }
                
                if(endDate.value.length ==0) {
                    endDate.focus();
                    alert('请选择结束日期！');
                    return;
                }
                param = { startDate:startDate.value,endDate:endDate.value,type:type };
            } else {
                                            
                var startMonth = $('startMonth');
                var endMonth = $('endMonth');
                if(startMonth.value.length ==0) {
                    startMonth.focus();
                    alert('请选择开始月份！');
                    return;
                }
                
                if(endMonth.value.length ==0) {
                    endMonth.focus();
                    alert('请选择结束月份！');
                    return;
                }
                param = { startMonth:startMonth.value,endMonth:endMonth.value,type:type };
            }

            $('queryInfo').innerHTML = '正在查询数据，请稍候......';
            Ajax.updatePanel('queryInfo','/UserControls/total.ascx',param);
        }
        
        function totalMail(){
       
            if(!confirm('确定对今天的数据重新进行汇总吗？'))return;
            
            $('spanUpdate').innerHTML = '正在汇总数据，请稍候......';
            AjaxMethods.TotalMail(function(success) {;
                if(success) {
                     $('spanUpdate').innerHTML = '汇总数据完成！';
                } else {
                     $('spanUpdate').innerHTML = '汇总数据失败！';
                }
            });
        }
        
        function changeType(value) {
            if(value == 'day') {
                $('top1').style.display = '';
                $('top2').style.display = 'none';
            } else {
                $('top1').style.display = 'none';
                $('top2').style.display = '';
            }
        }

        function changeTab(id,index) {
            for(var i=1;i<=4;i++){
                if(i==index) {
                    $(id + 'topcontent' + i).style.display = '';
                    $(id + 'top' + i).className = 'onhand';
                } else {
                    $(id + 'topcontent' + i).style.display = 'none';
                    $(id + 'top' + i).className = 'tab';
                }
            }
        }
    </script>
    
    <script type="text/javascript">
        function exportExcel() {
            if (!confirm("如果数据较多，则生成Excel文件需要较长时间，确定生成吗？")) return;

            var type = $('selectType').value;
            var startTime, endTime;
            if (type == 'day') {
                startTime = $('startDate').value;
                endTime = $('endDate').value;
            } else {
                startTime = $('startMonth').value;
                endTime = $('endMonth').value;
            }

            var filePath = AjaxMethods.ExportExcel(type, startTime, endTime);

            if (filePath) {
                $('filePath').innerHTML = "<a target='_blank' href='/download.aspx?filePath=" + filePath + "'>下载</a>";
            }
            else {
                $('filePath').innerHTML = "导出文件失败!";
            }
        }
    </script>
    </head>
    <body class="listBg" style="background:#fff;" onload="queryTotal('day');">
        <form name="form1" method="post" action="/total.aspx?companyID=gtfund" id="form1">
<div>
<input type="hidden" name="__VIEWSTATE" id="__VIEWSTATE" value="/wEPDwUKMTg4MzExMDkwMGRkUbW9HgFgc96rKm8jvobvRTJwpmg=" />
</div>

<link rel="stylesheet" type="text/css" href="/WebResource.axd?d=2a2an1Z8rh39oEIPi6U342GMNG4N4DRBqouZae8we_6oUXWmutAymFDI5LVT-fy80&t=634175481824341022" />
<script src="/WebResource.axd?d=2a2an1Z8rh39oEIPi6U342GMNG4N4DRBqouZae8we_4lLKVxjOi8lauTtJ0DwuTD0&t=634175481824341022" type="text/javascript"></script>
<script src="/WebResource.axd?d=2a2an1Z8rh39oEIPi6U342GMNG4N4DRBqouZae8we_6YeD3YayuiXD_8KSjeQGc40&t=634175481824341022" type="text/javascript"></script>
<script src="/Ajax/ASP.total_aspx.ashx?L2bJo6BMKZtmXL68+IoN3O2jo5L+P0rb7IW+TtZn4kY=" type="text/javascript"></script>

    <center>
          <div class="rounde">
	        <div class="listInfo">
	        <div style="width:100%"> 
                <span>
                    选择查询类型：<select id="selectType" onchange="changeType(this.value);">
                    <option value="day">按天统计</option>
                    <option value="month">按月统计</option>
                </select>
                </span>
                <span id="top1">
                    开始日期：<input type="text" id="startDate" size="15" class="Wdate" value="2010-08-29" onclick="WdatePicker({startDate:'%y-%M-%d',dateFmt:'yyyy-MM-dd'});"/>
                    &nbsp;结束日期：<input type="text" id="endDate" size="15" class="Wdate" value="2010-09-29" onclick="WdatePicker({startDate:'%y-%M-%d',dateFmt:'yyyy-MM-dd'});"/>
                    &nbsp;<input type="button" value="开始查询" class="listBtn" onclick="queryTotal('day');" />&nbsp;
                </span>
                <span id="top2" style="display:none;">
                    开始月份：<input type="text" id="startMonth" size="15" class="Wdate" value="2010-08" onclick="WdatePicker({startDate:'%y-%M',dateFmt:'yyyy-MM'});"/>
                    &nbsp;结束月份：<input type="text" id="endMonth" size="15" class="Wdate" value="2010-09" onclick="WdatePicker({startDate:'%y-%M',dateFmt:'yyyy-MM'});"/>
                    &nbsp;<input type="button" value="开始查询" class="listBtn" onclick="queryTotal('month');" />&nbsp;
                </span>
                <!--汇总 -->
                <span>
                    <input type="button" onclick="exportExcel();" value="生成Excel文件" />
                </span>
                <span style="padding-left:50;" id="filePath"></span>
            </div>
	          <div class="listLine"></div>
	          <div class="hack2"></div>
            <div id="queryInfo"></div>
            <div class="title" style="padding:5px;">
                <input type="button" value="汇总数据" class="listBtn" onclick="totalMail();" />
                &nbsp;<span style="font-weight:normal; color:Red;" id="spanUpdate"></span>
            </div>
            </div>
            </div>
         </center>
    </form>
</body>
</html>

