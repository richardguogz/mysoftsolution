/**
 * Window for Youth - (1.2 base jQuery), also need jquery.myext.js
 *
 * please reserving all of the comment, otherwise you should not use those code
 *
 * @Author    muqiao
 * @Version   1.2
 * @Copyright 2008 (c) muqiao (http://hi.baidu.com/emkiao)
 * @Revision  $Id$
 */
(function($){
var namespace = arguments[1];
var IE6 = $.browser.msie && parseInt($.browser.version) < 7;
var Dialog = window[namespace] = function(){
	var optionIndex = 0;
	if(arguments.length > 0){
		if(typeof arguments[0] == "string" ){
			this.id = arguments[0];
			optionIndex = 1;
		}else{
			this.id = arguments[0] ? arguments[0].id : null;
		}
	}
	if(!this.id){
		this.id = "window_" + new Date().getTime();
	}
	if(Dialogs.dialogs[this.id]){
		return Dialogs.dialogs[this.id].show();
	}
	this.options = $.extend($.clone(Dialogs.settings),arguments[optionIndex] || {});
	if(document.getElementById(this.id)){
		this.options.content = document.getElementById(this.id).innerHTML;
	}
	this.relchilds = [];
	this.parnetwin = null;
	Dialogs.relation(this.options.parentId,this);
	this.parent = $(document.body);
	this.below  = null;
	this.above  = null;
	this.dialog = null;
	Dialogs.dialogs[this.id] = this;
	return this;
};
Dialog.prototype = {
	toggle:function(){
		return this[this.visible ? 'hide' : 'show'];
	},
	show:function(show){
		if(this.visible) return this.toFront();
		this.visible = true;
		this.dialog == null && _createWin(this);
		var o = this.options,
			after = $.fbind(function(){
				this.lightbox && this.lightbox.show();
				this.toFront();
			},this),
			gon = true;
		typeof o.onShow == 'function' && (gon = o.onShow.call(this)!==false);
		if(!gon) return this;
		typeof show == 'function' && (show.call(this.dialog,this,after)||true) ||
		typeof o.show == 'function' && (o.show.call(this.dialog,this,after)||true) ||
		this.dialog.show(1,after);
		return this;
	},
	hide:function(hide){
		if(!this.visible) return this;
		this.visible = false;
		var o = this.options,
			after = $.fbind(function(){
				this.lightbox && this.lightbox.hide();
			},this),
			gon = true;
		typeof o.onHide == 'function' && (gon = o.onHide.call(this)!==false);
		typeof hide == 'function' && (hide.call(this.dialog,this,after)||true) ||
		typeof o.hide == 'function' && (o.hide.call(this.dialog,this,after)||true) ||
		this.dialog.hide(1,after);
		return this;
	},
	close:function(){
		var clean = true;
		if(!this.options) return;
		if(typeof this.options.onClose == 'function'){
			clean = this.options.onClose.call(this);
		}
		if(clean === false) return;
		this.dowhat && this.endDrag && this.endDrag();
		this.dialog.remove();
		if(this.lightbox){
			this.lightbox.remove();
		}
		if(typeof this.options.afterClose=='function'){
			this.options.afterClose();
		}
		if(this.autoposition){
			$(window).unbind('scroll',this.autoposition).unbind('resize',this.autoposition);
		}
		delete Dialogs.dialogs[this.id];
		Dialogs.dialogs[this.id] != undefined && (Dialogs.dialogs[this.id] = null);
		if(Dialogs.calls[this.id]){
			var l = Dialogs.calls[this.id].length;
			while(l){
				delete Dialogs.calls[this.id][--l];
				Dialogs.calls[this.id][l] != undefined && (Dialogs.calls[this.id][l] = null);
			}
			delete Dialogs.calls[this.id];
			Dialogs.calls[this.id] != undefined && (Dialogs.calls[this.id] = null);
		}
		if(Dialogs.focusedWindow == this){
			delete Dialogs.focusedWindow;
			Dialogs.focusedWindow = this.below;
		}
		Dialogs.shift(this);
		for(var k in this){
			this[k] = null;
		}
	},
	html:function(content){
		var o = this.options;
		if(content==undefined){
			return o.content||'';
		}
		content==''&& (content = "&nbsp;");
		if(this.dialog != null){
			if(o.url){
				this.content.src = null;
				o.url = null;
				this.content = $('<div class="dialog_content"> </div>');
				$('#'+this.id+'_table_content',this.dialog).empty().append(this.content);
			}
			this.content.empty().append(content);
			o.autosize && this.autoAdaptSize();
			o.autocenter && this.center();
			try{$('input:visible, textarea',this.content)[0].focus()}catch(e){}
		}
		o.content = content;
		return this;
	},
	center:function(){
		var wsize = $.getWindowScroll();
		var top  = (wsize.height - (this.height + this.heightN + this.heightS))/2;
		var left = (wsize.width - (this.width + this.widthW + this.widthE))/2;
		if(!this.posfixed){
			top += wsize.top;
			left += wsize.left;
		}
		
		//确保显示在可见范围内
		if(top < 0) top = 0;
		if(left < 0) left = 0;
		
		this.dialog.css({top:top,left:left});
	},
	load:function(url){
		var dialog = this;
		$.ajax({// Request the remote document
			url: url,
			type: 'GET',
			dataType: "html",
			success: function(data){
				dialog.html(data);
			},
			error:function(){
				dialog.html('<font color="red">request occured some error!</font>');
			}
		});
		return this;
	},
	status:function(status){
		if(status==undefined){
			return this.options.status||'';
		}
		if(status==''){
			status = "&nbsp;";
		}
		if(this.dialog!=null){
			$('#'+this.id+'_bottom',this.dialog).empty().append(status);
		}
		this.options.status = status;
		return this;
	},
	title:function(title){
		if(title==undefined){
			return this.options.title||'';
		}
		if(title==''){
			title = "&nbsp;";
		}
		if(this.dialog!=null){
			$('#'+this.id+'_top',this.dialog).empty().append(title);
		}
		this.options.title = title;
		return this;
	},
	href:function(url){
		if(url == undefined){
			return this.options.url || '';
		}
		if(url==''){
			url = "";
		}
		if (this.dialog != null) {
			if(this.options.url){
				this.content.attr('src',url);
			}
			// Not an url content, change div to iframe
			else{
				this.content = $("<iframe frameborder='0' name='" + this.id + "_content'  src='" + url + "' width='" + this.width + "' height='" + this.height + "'> </iframe>");
				$('#' + this.id + "_table_content").empty().append(this.content);
			}
		}
	    this.options.url = url;
		return this;
	},
	size:function(width,height){
		// Check min size
		(!width || (!this.minimized && width < this.options.minWidth)) && (width = this.options.minWidth);
		(!height || (!this.minimized && height < this.options.minHeight)) && (height = this.options.minHeight);
		this.width  = width;
		this.height = height;
		this.dialog.css({width:width+ this.widthW + this.widthE,height:this.height+ this.heightN + this.heightS});
		this.content.css({height:height,width:width});
		this.masker && this.masker.css('display') !='none' && this.masker.css({width:width ,height:height});
		return this;
	},
	autoAdaptSize:function(){
		var w = this.content[0].scrollWidth;
		var h = this.content[0].scrollHeight;
		if(w==this.width && h==this.hright) return;
		this.size(w,h);
		//{{{ hack get real scrollWidth and scrollHeight
		var i = 1;
		do{
			this.content[0].scrollLeft += 20;
			if(this.content[0].scrollLeft < 20*i){
				break;
			}
		}while(i++);
		i = 1;
		do{
			this.content[0].scrollTop += 20
			if(this.content[0].scrollTop < 20*i){
				break;
			}
		}while(i++);
		w += this.content[0].scrollLeft;
		h += this.content[0].scrollTop;
		this.content[0].scrollLeft = 0;
		this.content[0].scrollTop  = 0;
		//}}} hack end
		var wsize = $.getWindowScroll();
		var maxH = wsize.height - this.heightN - this.heightS - 10;
		var maxW = wsize.width - this.widthW - this.widthE - 10;
		h > maxH && (h = maxH);
		w > maxW && (w = maxW);
		this.size(w,h);
	},
	location:function(top,left){
		top  = $.toFloat(top,this.top);
		left = $.toFloat(left,this.left);
		this.dialog.css({top:top,left:left});
		return this;
	},
	minimize:function(){
		var r2 = $('#'+this.id + "_row2");
		if(!this.minimized){
			this.minimized = true;
			var dh = r2.height();
			this.r2Height = dh;
			var h  = this.dialog.height() - dh;
			this.height -= dh;
			r2.hide();
			this.dialog.css('height',h);
		}else{
			this.minimized = false;
			var dh = this.r2Height;
			this.r2Height = null;
			var h  = this.dialog.height() + dh;
			this.height += dh;
			this.dialog.css('height',h);
			r2.show();
			this.toFront();
		}
		return this;
	},
	maximize:function(){
		if(this.minimized) return this;
		if(this.storedLocation != null){
			_restoreLocation(this);
			this.maximized = false;
		}else{
			_storeLocation(this);
			this.maximized = true;
			var win = $(window),doc =  $(document);
			var width = win.width() - this.widthW - this.widthE;
			var height= win.height() - this.heightN - this.heightS;
			this.size(width, height);
			this.posfixed ?
				this.dialog.css({top:0,left:0}) :
				this.dialog.css({top:(doc.scrollTop() || 0), left:(doc.scrollLeft() || 0)});
		}
		return this.toFront();
	},
	toFront:function(){
		Dialogs.toFront(this);
		return this;
	},
	ZIndex: function(zindex){
		if(zindex == undefined)
			return $.toInt(this.dialog.css('zIndex'));
		this.dialog.css('zIndex',zindex);
		return this;
	},
	blur:function(){
		var can = true;
		typeof this.options.onBlur == 'function' && (can = this.options.onBlur.call(this) !== false);
		return can;
	},
	focus:function(){
		typeof this.options.onFocus == 'function' && this.options.onFocus.call(this);
		return this;
	}
};
var Dialogs = window[namespace+'s'] = {
	maxZIndex:99,
	focusedWindow:null,
	dialogs:{},
	calls:{},
	settings:{
		minWidth:    120,
		minHeight:   70,
		closable:    true,
		resizable:   false,
		minimizable: false,
		maximizable: false,
		draggable:   false,
		autosize:    true,
		lightbox:    false,
		autocenter:  true,
		autopos:     null, // 'center' / 'fixed'
		left:        'auto',
		top:         'auto',
		title:       '&nbsp;',
		status:      '&nbsp;',
		width:       0,
		height:      0
	},
	prevsets:function(settings){
		$.extend(Dialogs.settings,settings||{});
	},
	toFront:function(win){
		if(!Dialogs.focusedWindow){
			Dialogs.focusedWindow = win;
			win.ZIndex(++Dialogs.maxZIndex);
			win.focus();
			return;
		}
		if(win == Dialogs.focusedWindow){
			return;
		}
		if($.inArray(Dialogs.focusedWindow,win.relchilds)!=-1){
			return;
		}
		if(win.parentwin){
			Dialogs.toFront(win.parentwin);
		}
		if(Dialogs.focusedWindow.blur() != false){
			Dialogs.shift(win);
			win.below = Dialogs.focusedWindow;
			win.above = null;
			Dialogs.focusedWindow.above = win;
			Dialogs.focusedWindow = win;
			var origI = win.ZIndex();
			win.ZIndex(++Dialogs.maxZIndex);
			var dI = Dialogs.maxZIndex - origI;
			var l = win.relchilds.length;
			while(l){
				var w = win.relchilds[--l];
				if(!w.options) continue;
				var I = w.ZIndex()+dI;
				w.ZIndex(I);
				Dialogs.maxZIndex < I && (Dialogs.maxZIndex = I);
			}
			win.focus();
		}
		try{$('input:visible, textarea',Dialogs.focusedWindow.content)[0].focus()}catch(e){}
	},
	relation:function(Id,win){
		if(Id == undefined) return;
		var pwin = Id == 'auto' ? Dialogs.focusedWindow : Dialogs.dialogs[Id];
		if(pwin instanceof Dialog){
			pwin.relchilds.push(win);
			win.parentwin = pwin;
		}
	},
	shift:function(win){
		(win.below instanceof Dialog) && (win.below.above = win.above);
		(win.above instanceof Dialog) && (win.above.below = win.below);
	},
	close: function(id){
		var win = id==undefined ? Dialogs.focusedWindow : Dialogs.dialogs[id];
		if(win instanceof Dialog){
			win.close();
		}
	},
	show: function(id){
		if(Dialogs.dialogs[id] instanceof Dialog){
			Dialogs.dialogs[id].show();
		}
	},
	alert:function(message,options){
		options || (options = {});
		options.button = options.button || {};
		options.button.ok =  options.button.ok || ['OK',function(){this.close();}];
		return Dialogs.opendialog(message,options);
	},
	confirm:function(message,options){
		options || (options = {});
		options.button = options.button || {};
		options.button.ok =  options.button.ok || ['OK',function(){this.close();}];
		options.button.cancel = options.button.cancel||['CANCEL',function(){this.close();}];
		return Dialogs.opendialog(message,options);
	},
	info:function(message,timeout,options){
		options || (options = {});
		if(timeout){
			options.onShow = function(){
				setTimeout($.fbind(this.close,this),$.toInt(timeout,5000));
			};
		}
		return Dialogs.opendialog(message,options);
	},
	redirect:function(message,timeout,url,options){
		options || (options = {});
		options.afterClose = function(){
			window.location = url||location.href;
		};
		if(timeout){
			timeout = $.toInt(timeout,5000);
		}else{
			timeout = 5000;
		}
		options.onShow = function(){
			this.timeout = timeout;
			this.timer = setInterval($.fbind(function(){
				if((this.timeout -= 1000) < 0 ){
					clearInterval(this.timer);
					this.close();
					return;
				}
				this.status('<b>leave : '+(this.timeout/1000)+ ' second</b>');
			},this),1000);
		};
		return Dialogs.opendialog(message,options);
	},
	load:function(url,options){
		options || (options = {});
		var dialog = (new Dialog(options)).html('<div class="dialog_progress"> </div>').show();
		$.ajax({// Request the remote document
			url: url,
			type: 'GET',
			dataType: "html",
			success: function(data,status){
				dialog.html(data);// data.replace(/<script(.|\s)*?\/script>/g,'')
				if(typeof options.loadsuccess =='function'){
					options.loadsuccess.call(dialog);
				}
			},
			error:function(xhr,status){
				dialog.html('<font color="red">request occured some error!</font>');
				if(typeof options.loaderror =='function'){
					options.loaderror.call(dialog);
				}
			}
		});
		return dialog;
	},
	opendialog:function(message,options){
		options || (options = {});
		options.lightbox = typeof options.lightbox =='undefined' ? true : options.lightbox;
		options.draggable = typeof options.draggable  =='undefined' ? true : options.draggable;
		return (new Dialog(options)).html(message).show();
	},
	callBack:function(id,i){
		Dialogs.calls[id] && typeof Dialogs.calls[id][i] == 'function' && Dialogs.calls[id][i]();
	}
};
//{{{ private method
var _createWin = function(obj){
	if(obj.dialog != null){
		return obj;
	}
	var o = obj.options;
	obj.dialog = $('<div id="'+obj.id+'" class="dialog"></div>');
	if(o.url){
		obj.content = $('<iframe frameborder="0" name="' + obj.id + '_content"  src="' + o.url + '"> </iframe>');
	}else{
		obj.content = $('<div class="dialog_content">'+(o.content||'&nbsp;')+'</div>');
	}
	var closeDiv = o.closable ? "<div class='dialog_close' id='"+ obj.id +"_close'> </div>" : "";
	var minDiv = o.minimizable ? "<div class='dialog_minimize' id='"+ obj.id +"_minimize'> </div>" : "";
	var maxDiv = o.maximizable ? "<div class='dialog_maximize' id='"+ obj.id +"_maximize'> </div>" : "";
	var seAttributes = o.resizable ? "class='dialog_sizer' id='" + obj.id + "_sizer'" : "class='dialog_se'";
	obj.dialog.append("<table id='"+ obj.id
		+"_row1' class=\"table_window\"><tr><td class='dialog_nw'></td><td class='dialog_n'><div id='"
		+ obj.id+"_top' class='dialog_title title_window'>"+ o.title
		+"</div>"+closeDiv + maxDiv+ minDiv +"</td><td class='dialog_ne'></td></tr></table><table id='"+obj.id
		+"_row2' class=\"table_window\"><tr><td class='dialog_w'></td><td id='"
		+ obj.id +"_table_content' class='dialog_content' valign='top'></td><td class='dialog_e'></td></tr>"
		+"</table><table id='"+ obj.id
		+"_row3' class=\"table_window\"><tr><td class='dialog_sw'></td><td class='dialog_s'><div id='"
		+ obj.id+"_bottom' class='status_bar'>"+o.status+"</div></td><td "
		+ seAttributes+ "></td></tr></table>");
	function mouseout(){
		this.style.backgroundPosition = "top";
	}
	function mouseover(){
		this.style.backgroundPosition = "center";
	}
	function mousedown(){
		this.style.backgroundPosition = "bottom";
	}
	o.closable &&
		$('#'+obj.id+'_close',obj.dialog).bind("mouseover",mouseover).bind("mouseout",mouseout).bind("mousedown",mousedown).bind("mouseup",mouseover).bind("click",$.fbind(obj.close,obj));
	o.minimizable &&
		$('#'+obj.id+'_minimize',obj.dialog).bind("mouseover",mouseover).bind("mouseout",mouseout).bind("mousedown",mousedown).bind("mouseup",mouseover).bind("click",$.fbind(obj.minimize,obj));
	o.maximizable &&
		$('#'+obj.id+'_maximize',obj.dialog).bind("mouseover",mouseover).bind("mouseout",mouseout).bind("mousedown",mousedown).bind("mouseup",mouseover).bind("click",$.fbind(obj.maximize,obj));
	var buttons = '';
	if(o.button){
		Dialogs.calls[obj.id] = [];
		buttons = '<div id="'+obj.id+'_button" class="dialog_buttons">';
		for(var key in o.button){
			var l = Dialogs.calls[obj.id].length;
			buttons += '<button type="button" onclick="Dialogs.callBack(\''+obj.id+'\','+l+')">'+o.button[key][0]+'</button>';
			Dialogs.calls[obj.id][l] = $.fbind(o.button[key][1],obj);
		}
		buttons += '</div>';
	}
	$('#' + obj.id + "_table_content",obj.dialog).append(obj.content).append(buttons);
	typeof o.onload == 'function' &&
		obj.content.bind('load',$.fbind(o.onload,obj));
	if(o.draggable || o.resizable){
		obj.initDrag = $.fbindE(_initDrag,obj);
		obj.endDrag  = $.fbindE(_endDrag,obj);
		obj.updateDrag = $.fbindE(_updateDrag,obj);
		obj.masker = $('<div class="dialog_masker" style="display:none"></div>').insertBefore(obj.content);
	}
	if(o.draggable){
		var topbar = $('#'+obj.id+'_top',obj.dialog).bind("mousedown",obj.initDrag).parent().addClass("top_draggable").bind("mousedown",obj.initDrag);
		topbar.prev().addClass("top_draggable").bind("mousedown",obj.initDrag);
		topbar.next().addClass("top_draggable").bind("mousedown",obj.initDrag);
	}
	o.maximizable && $('#'+obj.id+'_top',obj.dialog).parent().bind("dblclick",$.fbind(obj.maximize,obj));
	o.resizable && (obj.sizer = $('#'+obj.id+'_sizer',obj.dialog).bind("mousedown",obj.initDrag));
	obj.content.bind("mousedown",$.fbind(obj.toFront,obj));
	o.lightbox && _initLightbox(obj);
	obj.parent.prepend(obj.dialog.css('visibility','hidden'));
	_getWindowBorderSize(obj);
	obj.width = o.width || 0;
	obj.height = o.height || 0;
	obj.size(o.width, o.height);
	o.autosize && obj.autoAdaptSize();
	var wsize = $.getWindowScroll();
	if(o.autopos == 'fixed'){
		if(IE6){
			obj.autoposition = function(){
				if(obj.dowhat) return obj;
				var newscroll = $.getWindowScroll();
				var top  =  parseFloat(obj.dialog.css('top'))  + newscroll.top - obj.scrollcoords.top;
				var left =  parseFloat(obj.dialog.css('left')) + newscroll.left - obj.scrollcoords.left;
				obj.dialog.css({top:top,left:left});
				obj.scrollcoords = newscroll;
			}
			obj.scrollcoords = wsize;
			$(window).bind('scroll',obj.autoposition);
		}else{
			obj.dialog.css('position','fixed');
			obj.posfixed = true;
		}
	}else if(o.autopos == 'center'){
		IE6 || (obj.dialog.css('position','fixed') && (obj.posfixed = true));
		obj.autoposition = function(){
			var newscroll = $.getWindowScroll();
			var top  = (newscroll.height - (obj.height + obj.heightN + obj.heightS))/2;
			var left = (newscroll.width - (obj.width + obj.widthW + obj.widthE))/2;
			if(IE6){
				top += newscroll.top;
				left += newscroll.left;
			}
			obj.dialog.css({top:top,left:left});
		};
		$(window).bind('resize scroll',obj.autoposition);
	}
	if(o.top=='auto'){
		o.top = (wsize.height - (obj.height + obj.heightN + obj.heightS))/2;
		obj.posfixed || (o.top += wsize.top);
	}
	if(o.left=='auto'){
		o.left = (wsize.width - (obj.width + obj.widthW + obj.widthE))/2;
		obj.posfixed || (o.left += wsize.left);
	}
	obj.dialog.css({top:$.toFloat(o.top),left:$.toFloat(o.left),visibility:'visible'});
	_checkIEOverlapping(obj);
};
var _initDrag = function(evt){
	var target = evt.target;
	if(((this.sizer && target == this.sizer[0]) && this.minimized)
		|| ((this.sizer && target != this.sizer[0]) && this.maximized))
		return;
	if($.inArray(target.getAttribute('id'),[this.id+'_close',this.id+'_maximize',this.id+'_minimize']) != -1){
		return;
	}
	this.dowhat = null;
	this.pointer = {X: evt.pageX,Y: evt.pageY};
	if(this.sizer && target == this.sizer[0]){
		this.dowhat = 'resizing';
		this.widthOrg = this.width;
		this.heightOrg = this.height;
	}
	else{
		this.dowhat = 'dragging';
		if(target.getAttribute('id')==this.id+'_close'){
			return;
		}
	}
	this.toFront();
	if(!this.minimized) {
		this.masker.css({
			zIndex: 1,
			width:  this.width,
			height: this.height
		}).show();
		IE6 && $('select',this.content).each(function(){
			// hide select
			this.oldVisibility = this.style.visibility ? this.style.visibility : "visible";
			this.style.visibility = "hidden";
		});
	}
	$(document).bind('mouseup',this.endDrag).bind('mousemove',this.updateDrag);
	document.body.ondrag = function () { return false; };
	document.body.onselectstart = function () { return false; };
};
var _updateDrag = function(evt){
	if(this.dowhat == null){
		return;
	}
	var pointer  = {X: evt.pageX,Y: evt.pageY};
	var dx = pointer.X - this.pointer.X;
	var dy = pointer.Y - this.pointer.Y;
	var w = this.widthOrg  + dx;
	var h = this.heightOrg + dy;
	var left =  parseFloat(this.dialog.css('left')) + dx;
	var top  =  parseFloat(this.dialog.css('top'))  + dy;
	// Resize case, update width/height
	if(this.dowhat == 'resizing'){
		this.size(w ,h);
	}
	// Move case, update top/left
	else if(this.dowhat == 'dragging'){
		this.pointer = pointer;
		this.dialog.css({top:top,left:left});
	}
};
var _endDrag = function(evt){
	this.dowhat = null;
	if(!this.minimized){
		this.masker.hide();
		IE6 && $('select',this.content).each(function(){
			if(typeof this.oldVisibility != 'undefined'){
				try{
					this.style.visibility = this.oldVisibility;
				}catch(e){
					this.style.visibility = "visible";
				}
				this.oldVisibility = null;
			}else{
				this.style.visibility && (this.style.visibility = "visible");
			}
		});
	}
	$(document).unbind('mouseup',this.endDrag).unbind('mousemove',this.updateDrag);
	document.body.ondrag = null;
	document.body.onselectstart = null;
};
var _storeLocation = function(obj){
	if(obj.storedLocation == null){
		obj.storedLocation = {
			top: obj.dialog.css('top'),
			left: obj.dialog.css('left'),
			width: obj.width, height: obj.height
		};
	}
};
var _restoreLocation = function(obj){
	obj.dialog.css({top: obj.storedLocation.top,left: obj.storedLocation.left});
	obj.size(obj.storedLocation.width, obj.storedLocation.height);
	obj.storedLocation = null;
};
var _getWindowBorderSize = function(obj){
	var _createHiddenDiv = function(className){
		return $('<div class="'+className+' style="display:none"></div>').prependTo(document.body);
	}
	// Hack to get real window border size!!
	var div = _createHiddenDiv("dialog_n");
	obj.heightN = div.getDimensions().height;
	div.remove();

	div = _createHiddenDiv("dialog_s");
	obj.heightS = div.getDimensions().height;
	div.remove();

	obj.options.button && (obj.heightS += $('#'+obj.id+'_button',obj.dialog).getDimensions().height);

	div = _createHiddenDiv("dialog_e");
	obj.widthE = div.getDimensions().width;
	div.remove();
	div = _createHiddenDiv("dialog_w");
	obj.widthW = div.getDimensions().width;
	div.remove();
	div = null;
};
var _checkIEOverlapping = function(obj){
	if(obj.iefix || !IE6){
		return;
	}
	obj.iefix = $('<iframe style="display:none;position:absolute;" src="javascript:false;document.write(\'\')" frameborder="0" scrolling="no"></iframe>');
	if(obj.lightbox){
		obj.lightbox.html('<p style="width:100%;height:100%;">').prepend(obj.iefix);
	}else{
		obj.dialog.prepend(obj.iefix);
	}
	obj.iefix.css({opacity:0,top: 0,left: 0,zIndex: -1,width: '100%',height: '100%',display:'block'});
	try{$('input:visible, textarea',obj.content)[0].focus()}catch(e){}
};
var _initLightbox = function(obj){
	if(obj.lightbox){
		obj.lightbox.css('zIndex',(++Dialogs.maxZIndex));
	}else{
		obj.lightbox = $('<div id="'+obj.id+'_overlay" class="overlay_dialog"></div>').prependTo(obj.parent);
		obj.lightbox.css({display:'none',top:0,left:0,zIndex:(++Dialogs.maxZIndex),width:'100%',position:'fixed',height:'100%'});
		if(IE6){// use setExpression instead
			var o = obj.lightbox.css('position','absolute')[0];
			o.style.setExpression('width',$.getPageSize().pageWidth);
			o.style.setExpression('height',$.getPageSize().pageHeight);
		}
	}
};
//}}} end private method
})(jQuery,'Dialog');