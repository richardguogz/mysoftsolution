using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.IO;
using System.Reflection;
using System.Web;
using System.Collections.Specialized;
using System.Web.Caching;
using System.Threading;

namespace MySoft.Web.UI
{
    /// <summary>
    /// Ajax相关信息
    /// </summary>
    public class AjaxInfo
    {
        public System.Web.UI.Page CurrentPage { get; private set; }
        public bool EnableAjaxCallback { get; set; }
        public bool EnableAjaxTemplate { get; set; }

        public AjaxInfo(System.Web.UI.Page currentPage)
        {
            this.CurrentPage = currentPage;
        }
    }

    /// <summary>
    /// Ajax调用类
    /// </summary>
    public class AjaxRequest
    {
        private AjaxInfo info;
        public AjaxRequest(AjaxInfo info)
        {
            this.info = info;
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        public void SendRequest()
        {
            try
            {
                bool AjaxProcess = WebHelper.GetRequestParam<bool>(info.CurrentPage.Request, "X-Ajax-Process", false);
                if (!AjaxProcess)
                {
                    WebUtils.RegisterPageCssFile(info.CurrentPage, info.CurrentPage.ClientScript.GetWebResourceUrl(typeof(AjaxPage), "MySoft.Web.Resources.pager.css"));

                    //需要启用模板加载
                    if (info.EnableAjaxTemplate)
                    {
                        WebUtils.RegisterPageJsFile(info.CurrentPage, info.CurrentPage.ClientScript.GetWebResourceUrl(typeof(AjaxPage), "MySoft.Web.Resources.template.js"));
                    }

                    WebUtils.RegisterPageForAjax(info.CurrentPage, info.CurrentPage.Request.Path);
                }
                else
                {
                    //只有启用Ajax，才调用初始化方法
                    if (info.CurrentPage is IAjaxInitEventHandler)
                        (info.CurrentPage as IAjaxInitEventHandler).OnAjaxInit();

                    if (info.CurrentPage is IAjaxProcessEventHandler)
                        (info.CurrentPage as IAjaxProcessEventHandler).OnAjaxProcess(GetCallbackParams());

                    bool AjaxLoad = WebHelper.GetRequestParam<bool>(info.CurrentPage.Request, "X-Ajax-Load", false);
                    string AjaxKey = WebHelper.GetRequestParam<string>(info.CurrentPage.Request, "X-Ajax-Key", Guid.NewGuid().ToString());

                    if (AjaxLoad)
                    {
                        string AjaxControlPath = WebHelper.GetRequestParam<string>(info.CurrentPage.Request, "X-Ajax-Path", null);
                        string AjaxTemplatePath = WebHelper.GetRequestParam<string>(info.CurrentPage.Request, "X-Ajax-Template", null);

                        if (CheckHeader(AjaxKey))
                        {
                            AjaxCallbackParam param = LoadAjaxControl(AjaxControlPath, AjaxTemplatePath);

                            //将param写入Response流
                            WriteToBuffer(param);
                        }
                        else
                        {
                            throw new AjaxException("Control \"" + AjaxControlPath + "\" Is Load Error！");
                        }
                    }
                    else
                    {
                        string AjaxMethodName = WebHelper.GetRequestParam<string>(info.CurrentPage.Request, "X-Ajax-Method", null);

                        if (AjaxMethodName != null)
                        {
                            if (CheckHeader(AjaxKey))
                            {
                                AjaxCallbackParam value = InvokeMethod(info.CurrentPage, AjaxMethodName);

                                //将value写入Response流
                                WriteToBuffer(value);
                            }
                            else
                            {
                                throw new AjaxException("Method \"" + AjaxMethodName + "\" Is Invoke Error！");
                            }
                        }
                        else
                        {
                            WriteAjaxMethods(info.CurrentPage.GetType());
                        }
                    }
                }
            }
            catch (ThreadAbortException) { }
            catch (AjaxException ex)
            {
                AjaxCallbackParam param = new AjaxCallbackParam(ex.Message);
                param.Success = false;

                WriteToBuffer(param);
            }
            catch (Exception ex)
            {
                throw new AjaxException(ex.Message, ex);
            }
        }

        #region 私有方法

        /// <summary>
        /// Called when [ajax template pre render].
        /// </summary>
        /// <param name="templatePath"></param>
        /// <returns></returns>
        private string LoadTemplate(string templatePath)
        {
            try
            {
                if (templatePath == null) return null;
                string html = GetCache(templatePath, string.Empty);
                if (html == null)
                {
                    Control control = info.CurrentPage.LoadControl(templatePath.ToLower().EndsWith(".ascx") ? templatePath : templatePath + ".ascx");
                    if (control != null)
                    {
                        if (control is IAjaxInitEventHandler)
                            (control as IAjaxInitEventHandler).OnAjaxInit();

                        if (control is IAjaxProcessEventHandler)
                            (control as IAjaxProcessEventHandler).OnAjaxProcess(GetCallbackParams());

                        StringBuilder sb = new StringBuilder();
                        control.RenderControl(new HtmlTextWriter(new StringWriter(sb)));
                        html = sb.ToString();
                        SetCache(templatePath, string.Empty, html);
                    }
                }
                return html;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Ajax类的处理

        private void WriteAjaxMethods(Type ajaxType)
        {
            Dictionary<string, AsyncMethodInfo> ajaxMethods = AjaxMethodHelper.GetAjaxMethods(ajaxType);
            List<AjaxMethodInfo> methodInfoList = new List<AjaxMethodInfo>();
            List<string> paramList = new List<string>();
            foreach (string key in ajaxMethods.Keys)
            {
                paramList.Clear();
                AjaxMethodInfo methodInfo = new AjaxMethodInfo();
                methodInfo.Name = key;
                foreach (ParameterInfo pi in ajaxMethods[key].MethodInfo.GetParameters())
                {
                    paramList.Add(pi.Name);
                }
                methodInfo.Async = ajaxMethods[key].Async;
                methodInfo.Paramters = paramList.ToArray();
                methodInfoList.Add(methodInfo);
            }

            WriteToBuffer(methodInfoList.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invokeObject"></param>
        /// <param name="MethodName"></param>
        /// <returns></returns>
        private AjaxCallbackParam InvokeMethod(object invokeObject, string MethodName)
        {
            try
            {
                Dictionary<string, AsyncMethodInfo> ajaxMethods = AjaxMethodHelper.GetAjaxMethods(invokeObject.GetType());
                if (ajaxMethods.ContainsKey(MethodName))
                {
                    ParameterInfo[] parameters = ajaxMethods[MethodName].MethodInfo.GetParameters();
                    List<object> list = new List<object>();
                    foreach (ParameterInfo p in parameters)
                    {
                        object obj = GetObject(p.Name, p.ParameterType);
                        list.Add(obj);
                    }

                    MethodInfo method = ajaxMethods[MethodName].MethodInfo;
                    FastInvokeHandler handler = DynamicCalls.GetMethodInvoker(method);
                    object value = handler.Invoke(invokeObject, list.ToArray());
                    return new AjaxCallbackParam(value);
                }
                else
                {
                    throw new AjaxException(string.Format("未找到服务器端方法{0}！", MethodName));
                }
            }
            catch (Exception ex)
            {
                throw new AjaxException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 加载控件返回String
        /// </summary>
        /// <param name="controlPath"></param>
        /// <returns></returns>
        private AjaxCallbackParam LoadAjaxControl(string controlPath)
        {
            return LoadAjaxControl(controlPath, null);
        }

        /// <summary>
        /// 加载控件返回String
        /// </summary>
        /// <param name="controlPath"></param>
        /// <param name="templatePath"></param>
        /// <returns></returns>
        private AjaxCallbackParam LoadAjaxControl(string controlPath, string templatePath)
        {
            try
            {
                string path = controlPath.ToLower().EndsWith(".ascx") ? controlPath : controlPath + ".ascx";

                //从缓存读取数据
                string html = GetCache(path, info.CurrentPage.Request.Form.ToString());
                if (html == null)
                {
                    Control control = info.CurrentPage.LoadControl(path);
                    if (control != null)
                    {
                        if (control is IAjaxInitEventHandler)
                            (control as IAjaxInitEventHandler).OnAjaxInit();

                        if (control is IAjaxProcessEventHandler)
                            (control as IAjaxProcessEventHandler).OnAjaxProcess(GetCallbackParams());

                        StringBuilder sb = new StringBuilder();
                        control.RenderControl(new HtmlTextWriter(new StringWriter(sb)));
                        html = sb.ToString();

                        if (info.EnableAjaxTemplate && templatePath != null)
                        {
                            string templateString = LoadTemplate(templatePath);
                            html = "{ data : " + html + ",\r\njst : " + WebUtils.Serialize(templateString) + " }";
                        }

                        //将数据放入缓存
                        SetCache(path, info.CurrentPage.Request.Form.ToString(), html);
                    }
                }

                return new AjaxCallbackParam(html);
            }
            catch (Exception ex)
            {
                throw new AjaxException(ex.Message, ex);
            }
        }

        private bool CheckHeader(string AjaxKey)
        {
            string ajaxKey = "AjaxProcess";
            bool ret = AjaxKey == WebUtils.MD5Encrypt(ajaxKey);

            return ret;
        }

        /// <summary>
        /// 获取控件的cache
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private string GetCache(string path, string parameter)
        {
            try
            {
                path = path.ToLower();
                string url = info.CurrentPage.Request.Url.PathAndQuery;
                string key = url + "," + path + (parameter != string.Empty ? "?" + parameter : null);

                if (CacheControlConfiguration.GetSection().Config.Enable)
                {
                    object obj = HttpContext.Current.Cache[key];
                    if (obj != null) return obj.ToString();
                }
                return null;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 设置控件的cache
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parameter"></param>
        /// <param name="html"></param>
        private void SetCache(string path, string parameter, string html)
        {
            try
            {
                if (html == null) return;
                path = path.ToLower();
                string url = info.CurrentPage.Request.Url.PathAndQuery;
                string key = url + "," + path + (parameter != string.Empty ? "?" + parameter : null);

                if (CacheControlConfiguration.GetSection().Config.Enable)
                {
                    Dictionary<string, int> rules = CacheControlConfiguration.GetSection().Rules;
                    if (rules.ContainsKey(path))
                    {
                        HttpContext.Current.Cache.Insert(key, html, null, DateTime.Now.AddSeconds(rules[path]), Cache.NoSlidingExpiration);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 获取页面参数
        /// </summary>
        /// <param name="eventArgument"></param>
        private CallbackParams GetCallbackParams()
        {
            CallbackParams callbackParams = new CallbackParams();
            NameValueCollection eventArgument = info.CurrentPage.Request.Form;
            if (eventArgument.Count > 0)
            {
                string[] keys = eventArgument.AllKeys;
                foreach (string key in keys)
                {
                    callbackParams[key] = new CallbackParam(eventArgument[key]);
                }
            }
            return callbackParams;
        }
        #endregion

        #region 将数据写入页面流中

        /// <summary>
        /// 将字符串反系列化成对象
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="paramsKey">传入key值获取对象</param>
        /// <returns></returns>
        private TObject GetObject<TObject>(string paramsKey)
        {
            return WebHelper.StrongTyped<TObject>(GetObject(paramsKey, typeof(TObject)));
        }

        /// <summary>
        /// 将字符串反系列化成对象
        /// </summary>
        private object GetObject(string paramsKey, Type type)
        {
            return WebUtils.Deserialize(WebHelper.GetRequestParam<string>(info.CurrentPage.Request, paramsKey, ""), type);
        }

        /// <summary>
        /// 将数据写入页面流
        /// </summary>
        /// <param name="param"></param>
        private void WriteToBuffer(AjaxCallbackParam param)
        {
            info.CurrentPage.Response.Clear();

            if (param != null)
                info.CurrentPage.Response.Write(WebUtils.Serialize(param));
            else
                info.CurrentPage.Response.ContentType = "image/gif";

            info.CurrentPage.Response.Cache.SetNoStore();
            info.CurrentPage.Response.Flush();
            info.CurrentPage.Response.End();
        }

        /// <summary>
        /// 将数据写入页面流
        /// </summary>
        /// <param name="methods"></param>
        private void WriteToBuffer(AjaxMethodInfo[] methods)
        {
            info.CurrentPage.Response.Clear();
            info.CurrentPage.Response.Write(WebUtils.Serialize(methods));
            info.CurrentPage.Response.Cache.SetNoStore();
            info.CurrentPage.Response.Flush();
            info.CurrentPage.Response.End();
        }

        /// <summary>
        /// 将数据写入页面流
        /// </summary>
        /// <param name="html"></param>
        private void WriteToBuffer(string html)
        {
            info.CurrentPage.Response.Clear();
            info.CurrentPage.Response.Write(html);
            info.CurrentPage.Response.Cache.SetNoStore();
            info.CurrentPage.Response.Flush();
            info.CurrentPage.Response.End();
        }

        #endregion
    }
}
