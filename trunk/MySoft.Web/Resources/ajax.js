/**
* Ajax Release 1.0.0.0.
* Copyright (C) 2007 Metaha.
* Get obj is callback,id,element
* Post obj is callback,id,element
* PostForm form_obj is formid,formelement
**/

Object.extend = function(dest, source, replace) {
    for (var prop in source) {
        if (replace == false && dest[prop] != null) { continue; }
        dest[prop] = source[prop];
    }
    return dest;
};

Object.extend(Array.prototype, {
    add: function(o) {
        this[this.length] = o;
    },
    addRange: function(items) {
        if (items.length > 0) {
            for (var i = 0; i < items.length; i++) {
                if (typeof (items[i]) == 'function') continue;
                this.push(items[i]);
            }
        }
    },
    clear: function() {
        this.length = 0;
        return this;
    },
    remove: function() {
        if (this.length == 0) { return null; }
        var o = this[0];
        for (var i = 0; i < this.length - 1; i++) {
            this[i] = this[i + 1];
        }
        this.length--;
        return o;
    }
}, false);

var StringBuilder = function(s) {
    this.v = [];
    if (s) this.v.add(s);
};

Object.extend(StringBuilder.prototype, {
    Append: function(s) {
        this.v.add(s);
    },
    AppendLine: function(s) {
        this.v.add(s + "\r\n");
    },
    Clear: function() {
        this.v.clear();
    },
    ToString: function() {
        return this.v.join("");
    }
}, true);

Object.extend(window, {
    __Class: {
        create: function() {
            return function() {
                if (typeof this.initialize == "function") {
                    this.initialize.apply(this, arguments);
                }
            };
        }
    },
    $: function() {
        var elements = [];
        for (var i = 0; i < arguments.length; i++) {
            var e = arguments[i];
            if (typeof e == 'string') {
                e = document.getElementById(e);
            }
            if (arguments.length == 1) {
                return e;
            }
            elements.add(e);
        }
        return elements;
    },
    $get: function(id) {
        return document.getElementById(id);
    },
    $name: function(name) {
        return document.getElementsByName(name);
    },
    $tag: function(tagName) {
        return document.getElementsByTagName(tagName);
    },
    $create: function(tagName) {
        return document.createElement(tagName);
    },
    $attach: function(element, eventName, eventFunction) {
        if (typeof (element) == 'string') element = $get(element);

        if (document.attachEvent) {
            element.attachEvent("on" + eventName, eventFunction);
        }
        else {
            element.addEventListener(eventName, eventFunction, false);
        }
    }
}, false);

Object.extend(String.prototype, {
    trimLeft: function() {
        return this.replace(/^\s*/, "");
    },
    trimRight: function() {
        return this.replace(/\s*$/, "");
    },
    trim: function() {
        return this.trimRight().trimLeft();
    },
    endsWith: function(s) {
        if (this.length == 0 || this.length < s.length) { return false; }
        return (this.substr(this.length - s.length) == s);
    },
    startsWith: function(s) {
        if (this.length == 0 || this.length < s.length) { return false; }
        return (this.substr(0, s.length) == s);
    },
    split: function(c) {
        var a = [];
        if (this.length == 0) return a;
        var p = 0;
        for (var i = 0; i < this.length; i++) {
            if (this.charAt(i) == c) {
                a.add(this.substring(p, i));
                p = ++i;
            }
        }
        a.add(s.substr(p));
        return a;
    }
}, false);

Object.extend(String, {
    format: function(s) {
        for (var i = 1; i < arguments.length; i++) {
            s = s.replace("{" + (i - 1) + "}", arguments[i]);
        }
        return s;
    },
    isNullOrEmpty: function(s) {
        if (s == null || s.length == 0) {
            return true;
        }
        return false;
    }
}, false);

var Ajax = new function() {
    this.timeout = 3600000;
    this.cache = {};
    this.getURL = function(url) {
        if (url) return url;
        if (typeof (AjaxInfo) != "undefined") {
            return AjaxInfo.url;
        }
        url = location.href;
        if (url.indexOf("#") > 0) {
            url = url.substr(0, url.indexOf("#"));
        }
        return url;
    };
    this.getData = function(xmlHttp, type) {
        var ct = xmlHttp.getResponseHeader("Content-Type");
        var data = !type && ct && ct.indexOf("xml") >= 0;
        data = type == "xml" || data ? xmlHttp.responseXML : xmlHttp.responseText;
        if (type == 'json') {
            if (data) {
                data = eval('(' + data + ')');
            } else {
                data = null;
            }
        }
        return data;
    };
    this.setRequest = function(ajax) {
        ajax.setcallback(Ajax.OnRequestStart, Ajax.OnRequestEnd, Ajax.OnException, Ajax.OnTimeout, this.timeout);
    };
};

Object.extend(Ajax, {
    toJSON: function(o) {
        if (o == null)
            return "null";
        switch (o.constructor) {
            case String:
                var v = [];
                for (var i = 0; i < o.length; i++) {
                    var c = o.charAt(i);
                    if (c >= " ") {
                        if (c == "\\" || c == '"') v.add("\\");
                        v.add(c);
                    } else {
                        switch (c) {
                            case "\n": v.add("\\n"); break;
                            case "\r": v.add("\\r"); break;
                            case "\b": v.add("\\b"); break;
                            case "\f": v.add("\\f"); break;
                            case "\t": v.add("\\t"); break;
                            default:
                                v.add("\\u00");
                                v.add(c.charCodeAt().toString(16));
                        }
                    }
                }
                return '"' + v.join('') + '"';
            case Array:
                var v = [];
                for (var i = 0; i < o.length; i++)
                    v.add(Ajax.toJSON(o[i]));
                return "[" + v.join(",") + "]";
            case Number:
                return isFinite(o) ? o.toString() : Ajax.toJSON(null);
            case Boolean:
                return o.toString();
            case Date:
                return '"' + o.getUTCFullYear() + '-' +
                           f(o.getUTCMonth() + 1) + '-' +
                           f(o.getUTCDate()) + 'T' +
                           f(o.getUTCHours()) + ':' +
                           f(o.getUTCMinutes()) + ':' +
                           f(o.getUTCSeconds()) + 'Z"';
            default:
                if (typeof o.toJSON == "function")
                    return o.toJSON();
        }
        if (typeof o == "object") {
            var v = [];
            for (attr in o) {
                if (typeof o[attr] != "function")
                    v.add('"' + attr + '":' + Ajax.toJSON(o[attr]));
            }
            if (v.length > 0)
                return "{" + v.join(",") + "}";
            return "{}";
        }
        return o.toString();

        function f(n) {
            return n < 10 ? '0' + n : n;
        }
    },
    toQueryParams: function(s) {
        var pairs = s.match(/^\??(.*)$/)[1].split('&');
        if (pairs == null) return null;
        var params = [];
        for (var i = 0; i < pairs.length; i++) {
            var pair = pairs[i].split('=');
            params[pair[0]] = pair[1];
        }
        return params;
    },
    toQueryString: function(a) {
        var s = [];

        if (a == null) return a;
        if (a.constructor == Array) {
            for (var i = 0; i < a.length; i++) {
                s.add(encodeURIComponent(a[i][0]) + "=" + encodeURIComponent(a[i][1]));
            }
        }
        else {
            for (var j in a) {
                s.add(encodeURIComponent(j) + "=" + encodeURIComponent(a[j]));
            }
        }

        return s.join("&");
    }
});

Object.extend(Ajax, {
    OnRequestStart: function() { },
    OnRequestEnd: function() { },
    OnException: function() { },
    OnTimeout: function() { },
    SetTimeout: function(value) {
        this.timeout = value;
    },
    Get: function(obj, url, header) {
        var ajax = new AJAXRequest();
        this.setRequest(ajax);
        ajax.setheader(header);
        ajax.get(url, obj);
    },
    GetData: function(url, dtype, header) {
        var data = null;
        if (!dtype) dtype = 'html';
        var ajax = new AJAXRequest();
        this.setRequest(ajax);
        ajax.setheader(header);
        ajax.get(url, function(xmlHttp) {
            data = Ajax.getData(xmlHttp, dtype);
        }, false);
        return data;
    },
    Post: function(obj, url, args, header) {
        var content = Ajax.toQueryString(args);
        var ajax = new AJAXRequest();
        this.setRequest(ajax);
        ajax.setheader(header);
        ajax.post(url, content, obj);
    },
    PostData: function(url, args, dtype, header) {
        var data = null;
        var content = Ajax.toQueryString(args);
        if (!dtype) dtype = 'html';
        var ajax = new AJAXRequest();
        this.setRequest(ajax);
        ajax.setheader(header);
        ajax.post(url, content, function(xmlHttp) {
            data = Ajax.getData(xmlHttp, dtype);
        }, false);
        return data;
    },
    PostForm: function(form_obj, callback, url, ptype, header) {
        if (typeof (form_obj) == 'string') form_obj = $get(form_obj);
        var callback1 = function(xmlHttp) {
            callback(xmlHttp.responseText);
        };
        var ajax = new AJAXRequest();
        this.setRequest(ajax);
        ajax.setheader(header);
        if (url && ptype) ajax.postf(form_obj, callback1, url, ptype);
        else if (url) ajax.postf(form_obj, callback1, url);
        else ajax.postf(form_obj, callback1);
    },
    Send: function(url, args, type, async, header, callback) {
        var content = Ajax.toQueryString(args);
        var ev = {
            url: url,
            content: content,
            method: type,
            async: async,
            oncomplete: callback
        };
        var ajax = new AJAXRequest();
        this.setRequest(ajax);
        ajax.setheader(header);
        ajax.call(ev);
    },
    Update: function(obj, url, interval, times, header) {
        var ajax = new AJAXRequest();
        this.setRequest(ajax);
        ajax.setheader(header);
        ajax.update(obj, url, interval, times);
    }
});

var AjaxClass = function(url) {
    this.url = url;
    this.header = [];
};

Object.extend(AjaxClass.prototype, {
    addHeader: function(type, value) {
        var header = [type, value];
        this.header.add(header);
    },
    clearHeader: function() {
        this.header.clear();
    },
    invoke: function(method, args, type, async, callback) {
        var json = null;
        this.addHeader('X-Ajax-Method', method);
        Ajax.Send(this.url, args, type, async, this.header, function(xmlHttp) {
            json = Ajax.getData(xmlHttp, 'json');
            if (json) {
                if (async && callback) {
                    if (json.Success)
                        callback(json.Message);
                    else
                        alert(json.Message);
                }
            }
        });
        if (!async) {
            if (json) {
                if (json.Success)
                    return json.Message;
                else
                    alert(json.Message);
            }
        }
    }
});

Object.extend(Ajax, {
    RegisterPage: function(url) {
        if (typeof (AjaxInfo) == "undefined") return null;
        if (url == window) url = this.getURL();
        var header = [['X-Ajax-Process', 'true']];
        var methods = Ajax.GetData(url, 'json', header);
        if (methods == null) return null;
        var sb = new StringBuilder("var Ajax_class=__Class.create();\r\n");
        sb.Append("Object.extend(Ajax_class.prototype, ");
        sb.Append("Object.extend(new AjaxClass(), {\r\n");
        sb.Append("\turl : '" + url + "',\r\n");
        for (var i = 0; i < methods.length; i++) {
            var method = methods[i];
            sb.Append("\t" + method.Name + " : function(");
            var sp = new StringBuilder("{\r\n");
            var isContent = false;
            if (method.Paramters.length > 0) {
                isContent = true;
                for (var p = 0; p < method.Paramters.length; p++) {
                    var paramter = method.Paramters[p];
                    if (p == method.Paramters.length - 1) {
                        sp.Append("\t\t\t\t" + paramter + " : Ajax.toJSON(param)");
                    } else {
                        sp.Append("\t\t\t\t" + paramter + " : Ajax.toJSON(" + paramter + ")");
                    }
                    sb.Append(paramter);
                    if (p < method.Paramters.length - 1) {
                        sp.Append(",\r\n");
                        sb.Append(",");
                    }
                }
            }
            sp.Append("\r\n\t\t\t}");
            if (method.Async) {
                if (isContent) sb.Append(",callback){\r\n");
                else sb.Append("callback)\r\n\t{\r\n");
            }
            else {
                sb.Append(")\r\n\t{\r\n");
            }
            if (isContent) {
                sb.Append("\t\tvar param=[],pm=[];\r\n");
                sb.Append("\t\tpm.addRange(arguments);\r\n");
                sb.Append("\t\t\if(pm.length>" + method.Paramters.length + "){\r\n");
                sb.Append("\t\t\tparam.addRange(pm);\r\n");
                if (method.Async) {
                    sb.Append("\t\t\tif(typeof(arguments[arguments.length-1])=='function')\r\n");
                    sb.Append("\t\t\t\tcallback=arguments[arguments.length-1];\r\n");
                }
                if (method.Paramters.length > 1) {
                    sb.Append("\t\t\tparam.splice(0," + (method.Paramters.length - 1) + ");\r\n");
                }
                sb.Append("\t\t} else {\r\n");
                sb.Append("\t\t\tparam=" + method.Paramters[method.Paramters.length - 1] + ";\r\n");
                sb.Append("\t\t}\r\n\r\n");
                sb.Append("\t\tvar args = " + sp.ToString() + ";\r\n");
            }
            else sb.Append("\t\tvar args = null;\r\n");
            sb.Append("\r\n\t\tthis.clearHeader();");
            if (header) {
                for (var j = 0; j < header.length; j++) {
                    sb.Append("\r\n\t\tthis.addHeader('" + header[j][0] + "','" + header[j][1] + "');");
                }
            }
            sb.Append("\r\n\t\tthis.addHeader('X-Ajax-Key','" + AjaxInfo.key + "');");
            sb.Append("\r\n\t\treturn this.invoke('" + method.Name + "'");
            sb.Append(",args");
            if (isContent) sb.Append(",'POST'");
            else sb.Append(",'GET'");
            if (method.Async) sb.Append(",true");
            else sb.Append(",false");
            if (method.Async) sb.Append(",callback");
            sb.Append(");\r\n\t}");
            if (i < methods.length - 1) sb.Append(",\r\n");
        }
        sb.Append("\r\n}));\r\n");
        try {
            eval(sb.ToString());
            var ajax = new Ajax_class();
            return ajax;
        }
        catch (e) { return null; }
    },
    /*obj is callback,id,element
    * path is controlpath
    * args is control params
    * option is callback,template,interval and cache
    */
    UpdatePanel: function(obj, path, args, option) {
        if (typeof (AjaxInfo) == "undefined") return;
        var url = this.getURL();
        var op = {
            callback: null,
            template: null,
            interval: null,
            cache: false
        };
        Object.extend(op, option);
        var header = [];
        header.add(['X-Ajax-Process', 'true']);
        header.add(['X-Ajax-Load', 'true']);
        header.add(['X-Ajax-Path', path]);
        header.add(['X-Ajax-Key', AjaxInfo.key]);
        if (op.template) {
            header.add(['X-Ajax-Template', op.template]);
        }
        var type = args ? 'POST' : 'GET';
        function update() {
            var updateURL = url + ';' + path;
            var key = updateURL;
            if (args) key += ';' + Ajax.toQueryString(args);

            if (Ajax.cache[key] && op.cache) {
                fillElement(obj, Ajax.cache[key]);
                return;
            }
            Ajax.Send(url, args, type, true, header, function(xmlHttp) {
                var json = Ajax.getData(xmlHttp, 'json');
                if (json) {
                    if (json.Success) {
                        if (op.cache) Ajax.cache[key] = json.Message;
                        fillElement(obj, json.Message);
                    } else {
                        alert(json.Message);
                    }
                }
                if (op.callback) op.callback(xmlHttp);
            });
        };

        function fillElement(el, html) {
            if (!el) return;

            //如果使用模板方式，则进行解析
            if (op.template) {
                try {
                    var json = eval('(' + html + ')');
                    html = json.jst.process(json.data);
                } catch (e) { }
            }

            //用正则表达式匹配ajax返回的html中是否有<script>，如果存在则取出标签内部的内容。
            var reg = /<script[^>]*>[\s\S]*?<\/script>/ig;
            if (!html.match(reg)) {
                fillHTML(el, html);
                return;
            }

            //用正则表达式匹配ajax返回的html中是否有onload,如果存在则取出内容。 
            var reg_onload = /<body onload="([^<]*)">/ig;
            var match_onload = html.match(reg_onload);
            var matchs = html.match(reg);

            //将剩下的html祛除<script>部分，插入模版页
            html = html.replace(reg, ""); 
            fillHTML(el, html);
            
            if (matchs != null) {
                var myscript = "";
                for (var i = 0; i < matchs.length; i++) {
                    var tag = /<script[^>]*>|<\/script>|<!--|-->/ig;
                    myscript += matchs[i].replace(tag, "");
                }
                var script = $create("script"); //在模版页创建新的<script>标签
                script.text = myscript; //给新的script标签赋值
                $tag("head")[0].appendChild(script); //把该标签加入<head>

                //5秒后移除此script标签
                setTimeout(function() {
                    $tag("head")[0].removeChild(script)
                }, 5000);
            }

            if (match_onload != null) {
                eval(match_onload[0]); //如果存在onload方法，则调用；
            }
        };

        function fillHTML(element, html) {
            if (typeof (element) == 'function') element(html);
            else {
                if (typeof (element) == 'string') element = $get(element);
                var nn = element.nodeName.toUpperCase();
                if ("INPUT|TEXTAREA".indexOf(nn) > -1) element.value = html;
                else element.innerHTML = html;
            }
        };

        if (op.interval) {
            update(); setInterval(function() { update(); }, value);
        }
        else update();
    }
});

