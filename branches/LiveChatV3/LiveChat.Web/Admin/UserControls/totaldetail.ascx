<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="totaldetail.ascx.cs" Inherits="LiveChat.Web.Admin.UserControls.totaldetail" %>
<%@ Import Namespace="LiveChat.Web.Admin" %>

<div id="<% =totalDate %>top1" class="onhand" onclick="changeTab('<% =totalDate %>',1);">按时段图表查看</div>
<div id="<% =totalDate %>top2" class="tab" onclick="changeTab('<% =totalDate %>',2);">按地区图表查看</div>
<div id="<% =totalDate %>top3" class="tab" onclick="changeTab('<% =totalDate %>',3);">按时段列表查看</div>
<div id="<% =totalDate %>top4" class="tab" onclick="changeTab('<% =totalDate %>',4);">按地区列表查看</div>

<div id="content" class="content">
<!--图形方式显示 -->
<div id="<% =totalDate %>topcontent1" style="float: left; text-align: center; margin-left: -2px; ">
<div class="listTitle"><% = totalType %>：<% =totalDate %>
    &nbsp;&nbsp;会话总次数：<span style="color:Red;"><% =totalCount %></span>
</div>
<table>
<%
    if (totalDictInfo1 == null)
    { %><tr><td colspan="10">统计会话数据出错，请重试！</td></tr><% }
    else if (totalDictInfo1.Count == 0)
    { %><tr><td colspan="10">没有会话数据！</td></tr><% }
    else
    {
 %>
	<tr style="height:300px;">
		<td class="chartcaption" style="vertical-align:middle;border-right:1px solid #004477;border-bottom:none;">
			<p>客</p><p>服</p><p>会</p><p>话</p><p>数</p><p>(次)</p>
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
		<td style="border:none;"><% =ConvertTo<int>(key) %> 点</td>
        <% } %>
		<td class="chartcaption" style="border:none;">时段</td>
	</tr>
<% } %>
</table>
</div>
<div id="<% =totalDate %>topcontent2" style="float: left; text-align: center; margin-left: -2px; display:none;">
<div class="listTitle"><% = totalType %>：<% =totalDate %>
    &nbsp;&nbsp;会话总次数：<span style="color:Red;"><% =totalCount %></span>
</div>
<table>
<%
    if (totalDictInfo2 == null)
    { %><tr><td colspan="10">统计会话数据出错，请重试！</td></tr><% }
    else if (totalDictInfo2.Count == 0)
    { %><tr><td colspan="10">没有会话数据！</td></tr><% }
    else
    {
 %>
	<tr style="height:300px;">
		<td class="chartcaption" style="vertical-align:middle;border-right:1px solid #004477;border-bottom:none;">
			<p>客</p><p>服</p><p>会</p><p>话</p><p>数</p><p>(次)</p>
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
		<td class="chartcaption" style="border:none;">省份</td>
	</tr>
<% } %>
</table>
</div>
<!--列表方式显示-->
<div id="<% =totalDate %>topcontent3" style="float: left; text-align: center; margin-left: -2px;display:none;">
<div class="listTitle"><% = totalType %>：<% =totalDate %>
    &nbsp;&nbsp;会话总次数：<span style="color:Red;"><% =totalCount %></span>
</div>
<table>
<%
    int index = 0;
    if (totalDictInfo1 == null)
    { %><tr><td colspan="10">统计会话数据出错，请重试！</td></tr><% }
    else if (totalDictInfo1.Count == 0)
    { %><tr><td colspan="10">没有会话数据！</td></tr><% }
    else
    {
 %>
    <tr class="title">
        <td style="width:120px;">时段</td><td style="width:150px;">会话数(次)</td><td style="width:250px;">占总会话比例</td>
        <td style="width:120px;">时段</td><td style="width:150px;">会话数(次)</td><td style="width:250px;">占总会话比例</td>
    </tr>
    <% 
        foreach (TotalInfo info in totalDictInfo1.Values)
        {
            if (index % 2 == 0)
            { %><tr><% }
         %>
            <td><% = ConvertTo<int>(info.TotalData)%> 点</td>
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
<div class="listTitle"><% = totalType %>：<% =totalDate %>
    &nbsp;&nbsp;会话总次数：<span style="color:Red;"><% =totalCount %></span>
</div>
<table>
<%
     if (totalDictInfo2 == null)
    { %><tr><td colspan="10">统计会话数据出错，请重试！</td></tr><% }
    else if (totalDictInfo2.Count == 0)
    { %><tr><td colspan="10">没有会话数据！</td></tr><% }
    else
    {
 %>
    <tr class="title">
        <td style="width:120px;">区域(省)</td><td style="width:150px;">会话数(次)</td><td style="width:250px;">占总会话比例</td>
        <td style="width:120px;">区域(省)</td><td style="width:150px;">会话数(次)</td><td style="width:250px;">占总会话比例</td>
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