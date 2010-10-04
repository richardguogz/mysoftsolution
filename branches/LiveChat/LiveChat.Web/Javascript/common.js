function checkAllChange() {
    $(".check").attr("checked", $("#checkAll").attr("checked"))
}

function checkItemChange(id) {
    if (!$("#" + id).attr("checked")) {
        $("#checkAll").attr("checked", false);
    }
    else {
        var arr = $(".check");
        for (var i = 0; i < arr.length; i++) {
            if (!arr[i].checked) {
                $("#checkAll").attr("checked", false);
                return;
            }
        }
        $("#checkAll").attr("checked", true);
    }
}

function removeTDemptyText() {
    var arr = document.getElementsByTagName("td");
    for(i=0;i <arr.length;i++)
    {
        arr[i].innerHTML = arr[i].innerHTML.trim();
        if(arr[i]=="") arr[i].innerHTML = "&nbsp;";
    } 
    
    arr = document.getElementsByTagName("th");
    for(i=0;i <arr.length;i++)
    {
        arr[i].innerHTML = arr[i].innerHTML.trim();
        if(arr[i]=="") arr[i].innerHTML = "&nbsp;";
    } 
}

function $get(id) {
    return document.getElementById(id);
}

String.prototype.trim = function() { 
    return this.replace(/(^\s*)|(\s*$)/g, ""); 
}