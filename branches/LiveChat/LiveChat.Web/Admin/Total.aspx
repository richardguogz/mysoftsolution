<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Total.aspx.cs" Inherits="LiveChat.Web.Admin.Total" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head><meta http-equiv="Content-Type" content="text/html; charset=gb2312" /><title>
	�ޱ����ĵ�
</title><meta name="description" content="�������ʼ���̨����ϵͳ" /><meta name="keywords" content="���������ʼ���̨���ʼ�����ϵͳ���������ʼ�������������ϵͳ����̨����ϵͳ" /><link rel="stylesheet" type="text/css" href="/css/main.css" /><link rel="stylesheet" type="text/css" href="/css/mail.css" />
    <script type="text/javascript" src="/datepicker/WdatePicker.js"></script>
    <script type ="text/javascript">
        function queryTotal(type) {

            var param = {};
            if(type == 'day') {
                                            
                var startDate = $('startDate');
                var endDate = $('endDate');
                if(startDate.value.length ==0) {
                    startDate.focus();
                    alert('��ѡ��ʼ���ڣ�');
                    return;
                }
                
                if(endDate.value.length ==0) {
                    endDate.focus();
                    alert('��ѡ��������ڣ�');
                    return;
                }
                param = { startDate:startDate.value,endDate:endDate.value,type:type };
            } else {
                                            
                var startMonth = $('startMonth');
                var endMonth = $('endMonth');
                if(startMonth.value.length ==0) {
                    startMonth.focus();
                    alert('��ѡ��ʼ�·ݣ�');
                    return;
                }
                
                if(endMonth.value.length ==0) {
                    endMonth.focus();
                    alert('��ѡ������·ݣ�');
                    return;
                }
                param = { startMonth:startMonth.value,endMonth:endMonth.value,type:type };
            }

            $('queryInfo').innerHTML = '���ڲ�ѯ���ݣ����Ժ�......';
            Ajax.updatePanel('queryInfo','/UserControls/total.ascx',param);
        }
        
        function totalMail(){
       
            if(!confirm('ȷ���Խ�����������½��л�����'))return;
            
            $('spanUpdate').innerHTML = '���ڻ������ݣ����Ժ�......';
            AjaxMethods.TotalMail(function(success) {;
                if(success) {
                     $('spanUpdate').innerHTML = '����������ɣ�';
                } else {
                     $('spanUpdate').innerHTML = '��������ʧ�ܣ�';
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
            if (!confirm("������ݽ϶࣬������Excel�ļ���Ҫ�ϳ�ʱ�䣬ȷ��������")) return;

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
                $('filePath').innerHTML = "<a target='_blank' href='/download.aspx?filePath=" + filePath + "'>����</a>";
            }
            else {
                $('filePath').innerHTML = "�����ļ�ʧ��!";
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
                    ѡ���ѯ���ͣ�<select id="selectType" onchange="changeType(this.value);">
                    <option value="day">����ͳ��</option>
                    <option value="month">����ͳ��</option>
                </select>
                </span>
                <span id="top1">
                    ��ʼ���ڣ�<input type="text" id="startDate" size="15" class="Wdate" value="2010-08-29" onclick="WdatePicker({startDate:'%y-%M-%d',dateFmt:'yyyy-MM-dd'});"/>
                    &nbsp;�������ڣ�<input type="text" id="endDate" size="15" class="Wdate" value="2010-09-29" onclick="WdatePicker({startDate:'%y-%M-%d',dateFmt:'yyyy-MM-dd'});"/>
                    &nbsp;<input type="button" value="��ʼ��ѯ" class="listBtn" onclick="queryTotal('day');" />&nbsp;
                </span>
                <span id="top2" style="display:none;">
                    ��ʼ�·ݣ�<input type="text" id="startMonth" size="15" class="Wdate" value="2010-08" onclick="WdatePicker({startDate:'%y-%M',dateFmt:'yyyy-MM'});"/>
                    &nbsp;�����·ݣ�<input type="text" id="endMonth" size="15" class="Wdate" value="2010-09" onclick="WdatePicker({startDate:'%y-%M',dateFmt:'yyyy-MM'});"/>
                    &nbsp;<input type="button" value="��ʼ��ѯ" class="listBtn" onclick="queryTotal('month');" />&nbsp;
                </span>
                <!--���� -->
                <span>
                    <input type="button" onclick="exportExcel();" value="����Excel�ļ�" />
                </span>
                <span style="padding-left:50;" id="filePath"></span>
            </div>
	          <div class="listLine"></div>
	          <div class="hack2"></div>
            <div id="queryInfo"></div>
            <div class="title" style="padding:5px;">
                <input type="button" value="��������" class="listBtn" onclick="totalMail();" />
                &nbsp;<span style="font-weight:normal; color:Red;" id="spanUpdate"></span>
            </div>
            </div>
            </div>
         </center>
    </form>
</body>
</html>

