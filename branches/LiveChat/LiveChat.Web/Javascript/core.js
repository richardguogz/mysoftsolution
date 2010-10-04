//打开会话窗
function openChat(el) {
    
    var url = el.getAttribute("lim:prefix");
    if (!url || url.length == 0) {
        url = httpWebsite + "/Transfer.aspx";
    } 
    else {
        url += "/Transfer.aspx";
    }
    
    var companyID = el.getAttribute("lim:company");
    url += "?companyID=" + companyID;

    var seatCode = el.getAttribute("lim:code");
    if ((null != seatCode && typeof seatCode != "undefined") && seatCode.length > 0) {
        url += "&seatCode=" + seatCode;
    }
    
    var sid = el.getAttribute("lim:skin");
    if ((null != sid && typeof sid != "undefined") && sid.length > 0) {
        url += "&skinID=" + sid;
    }
    
    url += "&tm=" + (new Date).getTime();
    var winAttr = "toolbar=0,scrollbars=0,location=0,menubar=0,resizable=1,width=680,height=450";
    window.open(url, 'chat_' + companyID, winAttr);
    
    return false;
};