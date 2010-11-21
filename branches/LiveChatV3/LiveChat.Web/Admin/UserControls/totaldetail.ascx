<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="totaldetail.ascx.cs" Inherits="LiveChat.Web.Admin.UserControls.totaldetail" %>
<%@ Import Namespace="LiveChat.Web.Admin" %>

<div id="<% =totalDate %>top1" class="onhand" onclick="changeTab('<% =totalDate %>',1);">��ʱ��ͼ��鿴</div>
<div id="<% =totalDate %>top2" class="tab" onclick="changeTab('<% =totalDate %>',2);">������ͼ��鿴</div>
<div id="<% =totalDate %>top3" class="tab" onclick="changeTab('<% =totalDate %>',3);">��ʱ���б�鿴</div>
<div id="<% =totalDate %>top4" class="tab" onclick="changeTab('<% =totalDate %>',4);">�������б�鿴</div>

<div id="content" class="content">
<!--ͼ�η�ʽ��ʾ -->
<div id="<% =totalDate %>topcontent1" style="float: left; text-align: center; margin-left: -2px; ">
<div class="listTitle"><% = totalType %>��<% =totalDate %>
    &nbsp;&nbsp;�Ự�ܴ�����<span style="color:Red;"><% =totalCount %></span>
</div>
<table>
<%
    if (totalDictInfo1 == null)
    { %><tr><td colspan="10">ͳ�ƻỰ���ݳ��������ԣ�</td></tr><% }
    else if (totalDictInfo1.Count == 0)
    { %><tr><td colspan="10">û�лỰ���ݣ�</td></tr><% }
    else
    {
 %>
	<tr style="height:300px;">
		<td class="chartcaption" style="vertical-align:middle;border-right:1px solid #004477;border-bottom:none;">
			<p>��</p><p>��</p><p>��</p><p>��</p><p>��</p><p>(��)</p>
		</td>
		<% foreach (TotalInfo info in totalDictInfo1.Values)
  { %>
		<td style="height:100%;line-height:normal;"><span><% = info.TotalCount %></span>
		<div title="<% =info.Rate * 100%>%" class="tdbgpic" style="height:<% =info.Percent * 290 %>px;">
		</div></td>
        <% } %>
		<td></td>
	</tr>
	<tr>
		<td style="border:none;"></td>
		
		<% foreach (string key in totalDictInfo1.Keys)
  { %>
		<td style="border:none;"><% =ConvertTo<int>(key) %> ��</td>
        <% } %>
		<td class="chartcaption" style="border:none;">ʱ��</td>
	</tr>
<% } %>
</table>
</div>
<div id="<% =totalDate %>topcontent2" style="float: left; text-align: center; margin-left: -2px; display:none;">
<div class="listTitle"><% = totalType %>��<% =totalDate %>
    &nbsp;&nbsp;�Ự�ܴ�����<span style="color:Red;"><% =totalCount %></span>
</div>
<table>
<%
    if (totalDictInfo2 == null)
    { %><tr><td colspan="10">ͳ�ƻỰ���ݳ��������ԣ�</td></tr><% }
    else if (totalDictInfo2.Count == 0)
    { %><tr><td colspan="10">û�лỰ���ݣ�</td></tr><% }
    else
    {
 %>
	<tr style="height:300px;">
		<td class="chartcaption" style="vertical-align:middle;border-right:1px solid #004477;border-bottom:none;">
			<p>��</p><p>��</p><p>��</p><p>��</p><p>��</p><p>(��)</p>
		</td>
		<% foreach (TotalInfo info in totalDictInfo2.Values)
  { %>
		<td style="height:100%;line-height:normal;"><span><% =info.TotalCount %></span>
		<div title="<% =info.Rate * 100%>%" class="tdbgpic" style="height:<% =info.Percent * 290 %>px;">
		</div></td>
        <% } %>
		<td></td>
	</tr>
	<tr>
		<td style="border:none;"></td>
		<% foreach (string key in totalDictInfo2.Keys)
  { %>
		<td style="border:none;"><% =key%></td>
        <% } %>
		<td class="chartcaption" style="border:none;">ʡ��</td>
	</tr>
<% } %>
</table>
</div>
<!--�б�ʽ��ʾ-->
<div id="<% =totalDate %>topcontent3" style="float: left; text-align: center; margin-left: -2px;display:none;">
<div class="listTitle"><% = totalType %>��<% =totalDate %>
    &nbsp;&nbsp;�Ự�ܴ�����<span style="color:Red;"><% =totalCount %></span>
</div>
<table>
<%
    int index = 0;
    if (totalDictInfo1 == null)
    { %><tr><td colspan="10">ͳ�ƻỰ���ݳ��������ԣ�</td></tr><% }
    else if (totalDictInfo1.Count == 0)
    { %><tr><td colspan="10">û�лỰ���ݣ�</td></tr><% }
    else
    {
 %>
    <tr class="title">
        <td style="width:120px;">ʱ��</td><td style="width:150px;">�Ự��(��)</td><td style="width:250px;">ռ�ܻỰ����</td>
        <td style="width:120px;">ʱ��</td><td style="width:150px;">�Ự��(��)</td><td style="width:250px;">ռ�ܻỰ����</td>
    </tr>
    <% 
        foreach (TotalInfo info in totalDictInfo1.Values)
        {
            if (index % 2 == 0)
            { %><tr><% }
         %>
            <td><% = ConvertTo<int>(info.TotalData)%> ��</td>
            <td><% =info.TotalCount %></td>
            <td style="vertical-align:middle; text-align:left;">
                <div style="float:left;width:<% =info.Percent * 250 %>px;height:16px;
                background:url(/admin/images/picbg.jpg);" title="<% =info.Rate * 100 %>%">&nbsp;</div>
            </td>
          <%
        if (index % 2 == 1)
        { %></tr><% }
        index++;
        }
    } %>
</table>
</div>
<div id="<% =totalDate %>topcontent4" style="float: left; text-align: center; margin-left: -2px; display:none;">
<div class="listTitle"><% = totalType %>��<% =totalDate %>
    &nbsp;&nbsp;�Ự�ܴ�����<span style="color:Red;"><% =totalCount %></span>
</div>
<table>
<%
     if (totalDictInfo2 == null)
    { %><tr><td colspan="10">ͳ�ƻỰ���ݳ��������ԣ�</td></tr><% }
    else if (totalDictInfo2.Count == 0)
    { %><tr><td colspan="10">û�лỰ���ݣ�</td></tr><% }
    else
    {
 %>
    <tr class="title">
        <td style="width:120px;">����(ʡ)</td><td style="width:150px;">�Ự��(��)</td><td style="width:250px;">ռ�ܻỰ����</td>
        <td style="width:120px;">����(ʡ)</td><td style="width:150px;">�Ự��(��)</td><td style="width:250px;">ռ�ܻỰ����</td>
    </tr>
    <% 
    index = 0;
    foreach (TotalInfo info in totalDictInfo2.Values)
    {
        if (index % 2 == 0)
        { %><tr><% }
        %>
        <td><% =info.TotalData %></td>
        <td><% =info.TotalCount %></td>
        <td style="vertical-align:middle; text-align:left;">
            <div style="float:left;width:<% =info.Percent * 250 %>px;height:16px;
                background:url(/admin/images/picbg.jpg);" title="<% =info.Rate * 100 %>%">&nbsp;</div>
        </td>
    <%  if (index % 2 == 1)
        { %></tr><% }
        index++;
        }
    }%>
</table>
</div>
</div>

<script type="text/javascript">

    function changeTab(id, index) {
        //alert(id);
        for (var i = 1; i <= 4; i++) {
            if (i == index) {
                $(id + 'topcontent' + i).style.display = '';
                $(id + 'top' + i).className = 'onhand';
            } else {
                $(id + 'topcontent' + i).style.display = 'none';
                $(id + 'top' + i).className = 'tab';
            }
        }
    }
</script>