/**
 * 此为Youth项目的一部分
 *
 * @Version   0.9
 * @Copyright Copyright (c) muqiao (http://hi.baidu.com/emkiao)
 * @Revision  $Id$
 */
Array.prototype.concat || (Array.prototype.concat = function(){
	// specially for opera
	var arr = [];
	for(var i = 0, l = this.length; i < l; i++) arr.push(this[i]);
	for(var i = 0, l = arguments.length; i < l; i++){
		if(typeof arguments[i] == 'undefined') continue;
		if(arguments[i].constructor == Array){
			for(var j = 0, ll = arguments[i].length; j < ll; j++){
				arr.push(arguments[i][j]);
			}
		}else{
			arr.push(arguments[i]);
		}
	}
	return arr;
});
(function($){
$.fn.extend({
	getDimensions: function(){
		var el = this[0];
		var display = this.css('display');
		if (display != 'none' && display != null) // Safari bug
		return {width: el.offsetWidth, height: el.offsetHeight};
		var els = el.style, oV = els.visibility, oP = els.position, oD = els.display;
		els.visibility = 'hidden';
		els.position = 'absolute';
		els.display = 'block';
		var oW = el.clientWidth, oH = el.clientHeight;
		els.display = oD;
		els.position = oP;
		els.visibility = oV;
		return {width: oW, height: oH};
	}
});
$.extend({
	toFloat:function(obj){
		obj = parseFloat(obj);
		isNaN(obj) && (obj = arguments[1]||0);
		return obj;
	},
	/**
	 * parseInt失败时返回 0
	 */
	toInt:function(obj){
		obj = parseInt(obj);
		isNaN(obj) && (obj = arguments[1]||0);
		return obj;
	},
	/**
	 * 克隆一个对象
	 * @param {Object} object
	 */
	clone: function(obj){
		var newobj = {};
		for(var key in obj){
			typeof obj[key] != 'undefined' && (newobj[key] = obj[key]);
		}
		return newobj;
	},
	/**
	 * convert any object to array
	 * @param {Object} iterable
	 */
	array:function(iterable){
		if(!iterable){
			return typeof iterable == 'undefined' ? [] : [iterable];
		}
		if(iterable.constructor == Array){
			return iterable;
		}
		var i = iterable.length, s = [], t;
		if(typeof i != 'number' || (t = typeof iterable) && t == 'string' || t == 'function' || iterable.setInterval){
			s[0] = iterable;
		}else{
			while(i){
				s[--i] = iterable[i];
			}
		}
		return s;
	},
	getWindowScroll:function(){
		var T, L, W, H,win = window, dom = document.documentElement, doc = document.body;
		T = dom && dom.scrollTop || doc && doc.scrollTop || 0;
		L = dom && dom.scrollLeft || doc && doc.scrollLeft || 0;
		if(win.innerWidth){
			W = win.innerWidth;
			H = win.innerHeight;
		}else{
			W = dom && dom.clientWidth || doc && doc.clientWidth;
			H = dom && dom.clientHeight || doc && doc.clientHeight;
		}
		return { top: T, left: L, width: W, height: H };
	},
	getPageSize:function(){
		var windowWidth, windowHeight,
			xScroll, yScroll,
			win = window, dom = document.documentElement, doc = document.body;
		if (win.innerHeight && win.scrollMaxY) {
			xScroll = doc.scrollWidth;
			yScroll = win.innerHeight + win.scrollMaxY;
		}else{
			xScroll = Math.max(dom ? dom.scrollWidth : 0,doc.scrollWidth,doc.offsetWidth);
			yScroll = Math.max(dom ? dom.scrollHeight : 0,doc.scrollHeight,doc.offsetHeight);
		}
		if(win.innerHeight){
			windowWidth = win.innerWidth;
			windowHeight = win.innerHeight;
		}else{
			windowWidth = dom && dom.clientWidth || doc && doc.clientWidth;
			windowHeight = dom && dom.clientHeight || doc && doc.clientHeight;
		}
		yScroll < windowHeight && (yScroll = windowHeight);
		xScroll < windowWidth && (xScroll = windowWidth);
		return {pageWidth: xScroll ,pageHeight: yScroll , windowWidth: windowWidth, windowHeight: windowHeight};
	},
	/**
	 * 闭包一个函数
	 * @param {Object} bind 作用域
	 * @param {Array}  args 附加参数
	 * @example
	 * 	function test(param1,param1){
	 * 		alert(this.tagName);// this == document.body
	 *      alert('I have arguments:'+param1+', '+param1);
	 * 	}
	 *  test.bind(document.body,['param1','param2']);
	 *  test();// result 'BODY'
	 */
	fbind:function(fn,bind,args){
		return function(){
			return fn.apply(bind||null,$.array(args).concat(arguments));
		}
	},
	/**
	 * 闭包一个函数作为事件监听程式
	 * @param {Object} bind 作用域
	 * @param {Array}  args 附加参数
	 * @example 于上一个函数差不多 只是参数列表里面多一个event对象
	 * 		function test(evt,param){
	 * 			alert(evt.pageX);
	 * 			alert(this);// this == document
	 * 		}
	 * 		test.bindE(document,['param']);
	 * 		$(document).click(test);
	 * 		// 单击则 弹出提示
	 */
	fbindE:function(fn,bind,args){
		return function(e){
			e = $.event.fix(window.event || e || {});
			var ret = fn.apply(bind||null,[e].concat(args));
			if(typeof ret == 'undefined'){
				e.preventDefault();
				e.stopPropagation();
			}
			return ret;
		};
	}
});
})(jQuery);