// JavaScript Document
function getPageScroll(){
  var yScroll;
  if (self.pageYOffset) {
    yScroll = self.pageYOffset;
  } else if (document.documentElement && document.documentElement.scrollTop){   // Explorer 6 Strict
    yScroll = document.documentElement.scrollTop;
  } else if (document.body) {// all other Explorers
    yScroll = document.body.scrollTop;
  }

  arrayPageScroll = new Array('',yScroll)
  return arrayPageScroll;
}

function getPageSize(){  
  var xScroll, yScroll;  
  if (window.innerHeight && window.scrollMaxY) {  
    xScroll = document.body.scrollWidth;
    yScroll = window.innerHeight + window.scrollMaxY;
  } else if (document.body.scrollHeight > document.body.offsetHeight){ // all but Explorer Mac
    xScroll = document.body.scrollWidth;
    yScroll = document.body.scrollHeight;
  } else { // Explorer Mac...would also work in Explorer 6 Strict, Mozilla and Safari
    xScroll = document.body.offsetWidth;
    yScroll = document.body.offsetHeight;
  }

  var windowWidth, windowHeight;
  if (self.innerHeight) {  // all except Explorer
    windowWidth = self.innerWidth;
    windowHeight = self.innerHeight;
  } else if (document.documentElement && document.documentElement.clientHeight) { // Explorer 6 Strict Mode
    windowWidth = document.documentElement.clientWidth;
    windowHeight = document.documentElement.clientHeight;
  } else if (document.body) { // other Explorers
    windowWidth = document.body.clientWidth;
    windowHeight = document.body.clientHeight;
  }  
  
  // for small pages with total height less then height of the viewport
  if(yScroll < windowHeight){
    pageHeight = windowHeight;
  } else {
    pageHeight = yScroll;
  }

  if(xScroll < windowWidth){  
    pageWidth = windowWidth;
  } else {
    pageWidth = xScroll;
  }

  arrayPageSize = new Array(pageWidth,pageHeight,windowWidth,windowHeight)
  return arrayPageSize;
}
  
function autoResize(){
   
    var number = 57;
    var left = 164;
    var right = 265;
    
    if (!window.XMLHttpRequest) {
        number = 66;
    }
    
    var div = document.getElementById("div");
    div.style.margin = "1px";
    
    var pinfo = getPageSize();
    var width = pinfo[2] - 2;
    var height = pinfo[3] - 2;
    
    div.style.width = width + "px";
    div.style.height = height + "px";

    if(width < 645) {
        width = 645;
        div.style.width = "645px";
    }

    if(height < 450) {
        height = 450;
        div.style.height = "450px";
    }

    var div1 = document.getElementById("div1");
    var div2 = document.getElementById("div2");
    var div3 = document.getElementById("div3");
    div1.style.height = (height - number) + "px";
    div2.style.height = (height - right - number)+ "px";
    div3.style.height = (height - left - number - 30) + "px";
    div3.style.width = (width - 200) + "px";

    var div4 = document.getElementById("emotionpane");
    div4.style.top = (height - 291 -30) + 'px';

    var div5 = document.getElementById("uploadFileBox");
    div5.style.top = (height - 266 - 30) + 'px';

    var divText = document.getElementById('divText');
    if (divText) {
        divText.innerHTML = divText.innerHTML;
    }
}

//设置自适应
window.onresize = autoResize;