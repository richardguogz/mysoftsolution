
//提交Form
function submitForm() {

    var form1 = document.getElementById('form1');
    //var error = document.getElementById('error');

    var ret = Validator.Validate(form1, 2);
    if (ret == false) return false;

    var url = form1.action;
    if (url.indexOf('?') >= 0) url += '&action=add';
    else url += '?action=add';
    Ajax.postForm(form1, function(success) {

        $get('error').style.display = 'none';
        $get('message').style.display = 'block';
        if (success) {
            $get('message').innerHTML = '留言提交成功！';
            $get('dialback').click();
        }
        else
            $get('message').innerHTML = '留言提交失败！';

    }, url);

    return false;
}


function openBannerLink(url) {
    //打开链接
    openWindow(url, '', '');
}


function openWindow(url, name, param) {
    try {
        this.newWindow = window.open(url, name, param);
        if (this.newWindow) {
            this.newWindow.focus();
            this.newWindow.opener = window;
        }
    } catch (e) { }
}