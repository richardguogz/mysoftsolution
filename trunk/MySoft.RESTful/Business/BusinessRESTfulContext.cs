﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using MySoft.RESTful.Business.Pool;
using MySoft.RESTful.Business.Register;
using Newtonsoft.Json.Linq;

namespace MySoft.RESTful.Business
{
    /// <summary>
    /// 默认服务上下文
    /// </summary>
    public class BusinessRESTfulContext : IRESTfulContext
    {
        #region IRESTfulContext 成员

        /// <summary>
        /// 业务池
        /// </summary>
        private IBusinessPool pool;

        /// <summary>
        /// 业务注册
        /// </summary>
        private IBusinessRegister register;

        /// <summary>
        /// 实例化BusinessRESTfulContext
        /// </summary>
        public BusinessRESTfulContext()
            : this(new DefaultBusinessPool(), new NativeBusinessRegister()) { }

        /// <summary>
        /// 实例化BusinessRESTfulContext
        /// </summary>
        /// <param name="pool"></param>
        public BusinessRESTfulContext(IBusinessPool pool)
            : this(pool, new NativeBusinessRegister()) { }

        /// <summary>
        /// 实例化BusinessRESTfulContext
        /// </summary>
        /// <param name="register"></param>
        public BusinessRESTfulContext(IBusinessRegister register)
            : this(new DefaultBusinessPool(), register) { }

        /// <summary>
        /// 实例化BusinessRESTfulContext
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="register"></param>
        public BusinessRESTfulContext(IBusinessPool pool, IBusinessRegister register)
        {
            this.pool = pool;
            this.register = register;
            Init();
        }

        private void Init()
        {
            register.Register(pool);
        }

        /// <summary>
        /// 是否需要认证
        /// </summary>
        /// <param name="format"></param>
        /// <param name="kind"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public bool IsAuthorized(ParameterFormat format, string kind, string method)
        {
            return pool.CheckAuthorized(format, kind, method);
        }

        /// <summary>
        /// 方法调用
        /// </summary>
        /// <param name="format"></param>
        /// <param name="kind"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object Invoke(ParameterFormat format, string kind, string method, string parameters)
        {
            WebOperationContext context = WebOperationContext.Current;
            JObject obj = new JObject();
            BusinessMethodModel metadata = pool.FindMethod(kind, method);

            try
            {
                if (metadata.SubmitType != (SubmitType)Enum.Parse(typeof(SubmitType), context.IncomingRequest.Method, true))
                {
                    throw new RESTfulException("Resources can only by the [" + metadata.SubmitType.ToString().ToUpper() + "] way to acquire!") { Code = RESTfulCode.BUSINESS_METHOD_CALL_TYPE_NOT_MATCH };
                }

                if (!metadata.IsPassCheck)
                {
                    throw new RESTfulException(metadata.CheckMessage) { Code = RESTfulCode.BUSINESS_METHOD_CALL_TYPE_NOT_MATCH };
                }

                if (!string.IsNullOrEmpty(parameters))
                {
                    obj = ParameterHelper.Resolve(parameters, format);
                }

                //解析QueryString
                var nvs = context.IncomingRequest.UriTemplateMatch.QueryParameters;
                if (nvs.Count > 0)
                {
                    var jo = ParameterHelper.Resolve(nvs);
                    foreach (var o in jo) obj[o.Key] = o.Value;
                }
            }
            catch (RESTfulException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new RESTfulException(String.Format("Fault parameters: {0}!", e.Message)) { Code = RESTfulCode.BUSINESS_METHOD_PARAMS_TYPE_NOT_MATCH };
            }

            object[] arguments = ParameterHelper.Convert(metadata.Parameters, obj);
            return DynamicCalls.GetMethodInvoker(metadata.Method)(metadata.Instance, arguments);
        }

        #endregion

        /// <summary>
        /// 生成API文档
        /// </summary>
        /// <returns></returns>
        public string MakeApiDocument(Uri requestUri)
        {
            #region 读取资源

            Assembly assm = this.GetType().Assembly;
            Stream helpStream = assm.GetManifestResourceStream("MySoft.RESTful.Template.help.htm");
            Stream helpitemStream = assm.GetManifestResourceStream("MySoft.RESTful.Template.helpitem.htm");

            StreamReader helpReader = new StreamReader(helpStream);
            StreamReader helpitemReader = new StreamReader(helpitemStream);

            string html = helpReader.ReadToEnd(); helpReader.Close();
            string item = helpitemReader.ReadToEnd(); helpitemReader.Close();

            #endregion

            string uri = string.Format("http://{0}/", requestUri.Authority.Replace("127.0.0.1", DnsHelper.GetIPAddress()));
            html = html.Replace("${uri}", uri);

            StringBuilder table = new StringBuilder();
            foreach (BusinessKindModel e in pool.KindMethods.Values)
            {
                StringBuilder items = new StringBuilder();
                int index = 0;
                foreach (BusinessMethodModel model in e.MethodModels.Values)
                {
                    string template = item;
                    if (index == 0)
                    {
                        template = template.Replace("${count}", e.MethodModels.Count.ToString());
                        template = template.Replace("${kind}", e.Name + "<br/>" + e.Description);
                    }
                    else
                    {
                        template = item.Substring(item.IndexOf("</td>") + 5);
                    }

                    var tempStr = model.Name + "<br/>" + model.Description;
                    if (!model.IsPassCheck)
                    {
                        tempStr = string.Format("<font color=\"red\" title=\"{0}\">{1}</font>", model.CheckMessage, tempStr);
                    }
                    template = template.Replace("${method}", tempStr);

                    StringBuilder buider = new StringBuilder();
                    List<string> plist = new List<string>();
                    foreach (var p in model.Parameters)
                    {
                        if (p.ParameterType == typeof(AuthenticationUser)) continue;

                        var s = String.Format("<{0}:{1}>", p.Name, p.ParameterType.FullName);
                        buider.AppendLine(HttpUtility.HtmlEncode(s)).AppendLine("<br/>");
                        if (!(p.ParameterType.IsClass && p.ParameterType != typeof(string)))
                        {
                            plist.Add(string.Format("{0}=[{0}]", p.Name).Replace('[', '{').Replace(']', '}'));
                        }
                    }

                    if (string.IsNullOrEmpty(buider.ToString()))
                        template = template.Replace("${parameter}", "&nbsp;");
                    else
                        template = template.Replace("${parameter}", buider.ToString());

                    template = template.Replace("${type}", model.SubmitType.ToString().ToUpper());

                    StringBuilder anchor = new StringBuilder();
                    anchor.AppendLine(CreateAnchorHtml(requestUri, uri, e, model, plist, model.SubmitType, "xml"));
                    anchor.AppendLine("<br/>");
                    anchor.AppendLine(CreateAnchorHtml(requestUri, uri, e, model, plist, model.SubmitType, "json"));
                    if (model.SubmitType == SubmitType.GET)
                    {
                        anchor.AppendLine("<br/>");
                        anchor.AppendLine(CreateAnchorHtml(requestUri, uri, e, model, plist, model.SubmitType, "jsonp"));
                    }

                    template = template.Replace("${uri}", anchor.ToString());
                    items.AppendLine(template);

                    index++;
                }

                table.AppendLine(items.ToString());
            }

            return html.Replace("${body}", table.ToString());
        }

        private string CreateAnchorHtml(Uri requestUri, string uri, BusinessKindModel e, BusinessMethodModel model, List<string> plist, SubmitType mode, string format)
        {
            string url = string.Empty;
            string method = mode.ToString().ToLower();
            if (plist.Count > 0)
                url = string.Format("{0}{1}.{2}/{3}.{4}?{5}", uri, method, format, e.Name, model.Name, string.Join("&", plist.ToArray()));
            else
                url = string.Format("{0}{1}.{2}/{3}.{4}", uri, method, format, e.Name, model.Name);

            if (!string.IsNullOrEmpty(requestUri.Query))
            {
                if (url.IndexOf('?') >= 0)
                    url += "&" + requestUri.Query.Substring(1);
                else
                    url += requestUri.Query;
            }

            if (format == "jsonp")
            {
                if (url.IndexOf('?') >= 0)
                    url += "&callback={callback}";
                else
                    url += "?callback={callback}";
            }

            url = string.Format("<a rel=\"operation\" target=\"_blank\" title=\"{0}\" href=\"{0}\">{1}</a> 处的服务", url, url.Replace(uri, "/"));
            return url;
        }
    }
}