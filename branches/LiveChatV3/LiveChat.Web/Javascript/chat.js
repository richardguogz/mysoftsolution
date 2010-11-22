var sid = setInterval('getSessionInfo()', 5000);
var tid = setInterval('getMessage()', 5000);
var isValidate = true;
var isChatClosing = false;

//Ajax.showErrorMessage = false;
Ajax.onException = function(ex) {
    if (ex.Success == false) {

        //清除定时器
        clearInterval(sid);

        sessionID = null;

        doCloseChat();
    };
};

function setRefresh() {

    if (!window.ActiveXObject) {
        //$('switch').style.display = 'none';
        $('active').style.display = 'none';
    }

    $attach(window, 'beforeunload', closeWindow);
    $attach(window, 'unload', exitWindow);

    if (sessionID) {
        getSessionInfo();
    }
    else {
        var isOnline = AjaxMethods.GetSeatOnline();
        if (!isOnline) {

            addTipToChat("您好，暂时没有客服人员为您服务，请留言！");

        } else {

            addTipToChat("当前客服人员在线，请发送消息建立会话！");
        }
    }

    $('inputbox').onkeydown = function(e) {
        if (e) event = e;
        if (event.keyCode == 13 && !event.ctrlKey) {
            return sendMessage();
        }
    }

    $('inputbox').focus();
}

function closeWindow(e) {
    try {
        if (sessionID) {
            e.returnValue = '您确定要关闭本次对话吗？';
        }
    }
    catch (ex) { }
}

function exitWindow() {
    try {
        if (isValidate) {
            logout();
        }
    }
    catch (ex) { }
}

function getSessionInfo() {

    if (sessionID) {

        //清除定时器
        clearInterval(sid);

        return;
    }

    AjaxMethods.GetSessionInfo(function(session) {

        if (session && session.length > 5) {

            sessionID = session[0];
            var companyName = session[1];
            var seatName = session[2];

            $('seatName').innerHTML = seatName;
            $('telePhone').innerHTML = session[4];
            $('mobileNumber').innerHTML = session[5];
            $('email').innerHTML = session[6];

            document.title = "您正在与" + companyName + "客服【" + seatName + "】会话... ";
            $('headerBox').innerHTML = document.title;

            //var inputbox = $('inputbox');
            //inputbox.focus();

            getMessage();
        }
    });
}

function sendMessage() {

    if (!sessionID && isChatClosing) {
        alert('会话已经结束，请重新发送请求建立会话！');
        return false;
    }

    var msg = $('inputbox');

    var text = msg.value;
    text = text.replace(/(^\s*)|(\s*$)/ig, '');
    text = text.replace(/\r/ig, '');
    text = text.replace(/\n/ig, '<br/>');
    text = text.replace(/{FACE#([\d]+)#}/ig, '<img border="0" src="' + chatWebSite + '/images/face/$1.gif" />');

    if (text.replace('<br/>', '').length == 0) {
        alert('发送内容不允许为空！');
        msg.value = '';
        msg.focus();
        return false;
    }

    if (text.replace('<br/>', '').length > 1000) {
        alert('发送内容不能大于1000个字符！');
        msg.focus();
        return false;
    }

    addMsgToChat('您&nbsp;说:<br/><span>' + text + '</span>', true);

    var content = msg.value;
    content = content.replace(/{FACE#([\d]+)#}/ig, '<img border="0" src="' + chatWebSite + '/images/face/$1.gif" />');

    //发送到服务器端
    AjaxMethods.SendMessage(MessageType.Text, sessionID, seatCode, content, function(ssid) {

        if (ssid == null) {

            addMsgToChat('您&nbsp;说:<br/><span>消息"' + text + '"发送失败！</span>', true);

            return;
        }
    });

    msg.value = '';
    msg.focus();

    return false;
}

function getMessage() {

    //如果ID未产生，则直接跳过
    if (!sessionID) {

        return;
    }

    AjaxMethods.GetMessages(sessionID, function(msgs) {

        if (msgs == null) {

            addTipToChat("<font color='red'>本次会话已经结束，欢迎下次光顾！</font>");

            document.title = '本次会话已经结束(' + new Date().toLocaleTimeString() + ')';
            $('headerBox').innerHTML = document.title;

            //会话建立,则终止会话定时器
            clearInterval(tid);

            sessionID = null;

            isChatClosing = true;

            return;
        }
        else if (msgs.length > 0) {

            //播放声音
            //if(window.ActiveXObject) {
            //    if($('closesound').style.display !='none') {
            //        $('framePlayer').src = '/player.htm';
            //    }
            //}

            //开始显示消息
            dynamicMsg.start(10000);

            for (var index = 0; index < msgs.length; index++) {

                var msg = msgs[index];
                if (msg == null) continue;

                if (msg.Type == MessageType.Url) {

                    //推送url,暂不自动打开
                    addMsgToChat(msg.SenderName + '向您推送了一个链接:<br/><span>' + msg.Content + '</span>');

                } else if (msg.Type == MessageType.Picture) {

                    //接收发送过来的图片
                    addMsgToChat(msg.SenderName + '向您发送了一个截屏:<br/><span>' + msg.Content + '</span>');

                } else {

                    //接收发送的消息
                    addMsgToChat(msg.SenderName + '&nbsp;说:<br/><span>' + msg.Content + '</span>');

                }

                $('footerBox').innerHTML = '最后消息接收于:' + msg.SendTime.toLocaleTimeString();
            }
        }
    });
}

function addMsgToChat(msg, visit) {

    var body = $('history');
    var p = $create('p');
    p.innerHTML = msg;
    if (visit) p.className = 'visitor';
    else p.className = 'operator';
    body.appendChild(p);

    try {
        p.scrollIntoView(true);
    }
    catch (ex) {
        body.scrollTop = body.scrollHeight;
    }
}

function addTipToChat(tip) {

    var body = $('history');
    var p = $create('p');
    p.innerHTML = tip; ;
    p.className = 'info';
    body.appendChild(p);

    try {
        p.scrollIntoView(true);
    }
    catch (ex) {
        body.scrollTop = body.scrollHeight;
    }

}

function showShortSel() {

    if (!window.XMLHttpRequest) {
        $('shortKeyMenu').style.bottom = '98px';
    }

    if ($('shortKeyMenu').style.display == 'none') {
        $('shortKeyMenu').style.display = 'block';
    } else {
        $('shortKeyMenu').style.display = 'none';
    }
}

function changeShortcut(style) {
    $('shortKeyMenu').style.display = 'none';
    $('shortKeyTip').innerHTML = '[发送快捷键:' + style + ']';

    if (style == 'Enter') {
        $('shortKeyMenu1').innerHTML = '&nbsp;*按Enter键发送消息';
        $('shortKeyMenu2').innerHTML = '&nbsp;&nbsp;按Ctrl+Enter键发送消息';
        $('inputbox').onkeydown = function(e) {
            if (e) event = e;
            if (event.keyCode == 13 && !event.ctrlKey) {
                return sendMessage();
            }
        }
    }
    else {
        $('shortKeyMenu1').innerHTML = '&nbsp;&nbsp;按Enter键发送消息';
        $('shortKeyMenu2').innerHTML = '&nbsp;*按Ctrl+Enter键发送消息';
        $('inputbox').onkeydown = function(e) {
            if (e) event = e;
            if (event.keyCode == 13 && event.ctrlKey) {
                return sendMessage();
            }
        }
    }
}

function saveHistory() {
    //保存历史记录
    if (!sessionID) {
        alert('当前不存在与客服的聊天记录！');
        return;
    }

    //打开聊天记录窗口
    var url = AjaxMethods.CreateFile(sessionID, userID);
    if (url) {
        url = "/download.aspx?filePath=" + url;
        openWindow(url, 'fund123ChatHistory', '');
    }
}

function doCloseChat() {

    //关闭对话
    window.close();
}

//退出登录
function logout() {

    return AjaxMethods.EndRequest(sessionID);
}

function openBannerLink(url) {
    //打开链接
    openWindow(url, '', '');
}

function doclose() {
    //关闭声音
    $('closesound').style.display = 'none';
    $('opensound').style.display = 'block';
}

function doopen() {
    //打开声音
    $('closesound').style.display = 'block';
    $('opensound').style.display = 'none';
}

function showUploadFile(fileName) {

    if (!sessionID) {
        alert('需要与客服建立会话后才能传送文件！');
        return;
    }

    //上传文件
    $('uploadFileFrame').src = fileName;

    if ($('uploadFileBox').style.display == 'none') {
        $('uploadFileBox').style.display = 'block';
    } else {
        $('uploadFileBox').style.display = 'none';
    }
}

function hideUploadFileForm() {
    $('uploadFileBox').style.display = 'none';
}

function uploadFileOnSubmit(fileName) {

    hideUploadFileForm();
    addTipToChat('正在传送文件"' + fileName + '"，请稍候......', true);
    return '/Upload.aspx';
}

//写出文件信息
function writeFile(url1, url2, isfile) {

    var value = null;
    var content = null;
    var msgType = MessageType.Text;
    if (isfile) {
        msgType = MessageType.File;
        value = '文件"' + url1 + '"传送成功！';
        content = "<a href='" + url2 + "' target='_blank'>" + url1 + "</a>";
    }
    else {
        msgType = MessageType.Picture;
        value = "<a href='" + url2 + "' target='_blank'><img border='0px' alt='查看大图' src='" + url1 + "' /></a>";
        content = value;
    }

    if (msgType == MessageType.Picture) {

        addMsgToChat('您向客服发送了一个图片:<br/><span>' + value + '</span>', true);
    }
    else if (msgType == MessageType.File) {

        addMsgToChat('<span>' + value + '</span>', true);
    }

    //发送到服务器端
    AjaxMethods.SendMessage(msgType, sessionID, seatCode, content, function(ssid) {
        if (ssid == null) {

            addMsgToChat('您&nbsp;说:<br/><span>消息发送失败！</span>', true);

            return;
        }
    });

    var msg = $('inputbox');
    msg.focus();

    return false;
}

//打印出错误消息
function writeError(msg) {
    addMsgToChat('<span>' + msg + '</span>', true);
}

//设置表情
function setface() {
    if ($('emotionpane').style.display == 'none')
        $('emotionpane').style.display = '';
    else
        $('emotionpane').style.display = 'none';
}

//设置当前表情
function setcurrface(index) {

    var msg = $('inputbox');
    msg.value += '{FACE#' + index + '#}';

    //给消息框设置焦点
    msg.focus();

    setface();
}

//截屏
function download() {

    if (!sessionID) {
        alert('需要与客服建立会话后才能截屏！');
        return;
    }

    if (window.ActiveXObject) {

        //截 屏
        try {
            var obj = new ActiveXObject("SL.ImageUtil2");

            //进行截屏操作
            if (obj.CaptureScreenPreview()) {
                addTipToChat('正在向客服发送截屏，请稍候......', true);

                var base64String = obj.GetImageInBase64(2)

                AjaxMethods.SendImage(base64String, function(urls) {
                    if (urls) {
                        writeFile(urls[0], urls[1]);
                    }
                    else {
                        addTipToChat('截屏发送失败，请检查网络是否正常！', true);
                    }
                }
                );
            }
        }
        catch (e) {

            alert('您没有安装截屏插件！');

            openWindow(chatWebSite + '/activex.htm', '', '');
        }
    }
    else {
        alert('firefox不支持截屏操作！');
    }
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

/*  
* 处理新消息提示的操作  
*/
function DynamicMessage(msg, hiddenMsg) {
    var defaultMsg;
    var intervalMsg;
    var bRunning = false;
    var bMessage = false;
    this.start = function(interval) {
        if (bRunning) return;

        defaultMsg = document.title;
        intervalMsg = setInterval(function() {
            if (!bMessage) {
                document.title = msg;
                bMessage = true;
            } else {
                document.title = hiddenMsg;
                bMessage = false;
            }
        }, 500);

        //清除定时器
        setTimeout(function() {
            if (intervalMsg != null) {
                clearInterval(intervalMsg);
                document.title = defaultMsg;
                bRunning = false;
                bMessage = false;
            }
        }, interval);

        //正在运行
        bRunning = true;
    };
};

//实例化一个消息显示类
var dynamicMsg = new DynamicMessage("【您有新的消息】", "【】");