﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ServiceModel;
using System.Net;
using System.ServiceModel.Web;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;
using System.ServiceModel.Activation;
using MySoft.RESTful.Business;

namespace MySoft.RESTful
{
    /// <summary>
    /// 默认的RESTful服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DefaultRESTfulService : IRESTfulService
    {
        /// <summary>
        /// 上下文处理
        /// </summary>
        public IRESTfulContext Context { get; set; }

        /// <summary>
        /// 实例化DefaultRESTfulService
        /// </summary>
        public DefaultRESTfulService()
        {
            //创建上下文
            this.Context = new BusinessRESTfulContext();
        }

        #region IRESTfulService 成员

        /// <summary>
        /// 实现Post方式Json响应
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Stream PostJsonEntry(string kind, string method, Stream parameters)
        {
            return GetResponseStream(ParameterFormat.Json, kind, method, parameters);
        }

        /// <summary>
        /// 实现Delete方式Json响应
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Stream DeleteJsonEntry(string kind, string method, Stream parameters)
        {
            return GetResponseStream(ParameterFormat.Json, kind, method, parameters);
        }

        /// <summary>
        /// 实现Put方式Json响应
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Stream PutJsonEntry(string kind, string method, Stream parameters)
        {
            return GetResponseStream(ParameterFormat.Json, kind, method, parameters);
        }

        /// <summary>
        /// 实现Get方式Json响应
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public Stream GetJsonEntry(string kind, string method)
        {
            return GetResponseStream(ParameterFormat.Json, kind, method);
        }

        /// <summary>
        /// 实现Post方式Xml响应
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Stream PostXmlEntry(string kind, string method, Stream parameters)
        {
            return GetResponseStream(ParameterFormat.Xml, kind, method, parameters);
        }

        /// <summary>
        /// 实现Delete方式Xml响应
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Stream DeleteXmlEntry(string kind, string method, Stream parameters)
        {
            return GetResponseStream(ParameterFormat.Xml, kind, method, parameters);
        }

        /// <summary>
        /// 实现Put方式Xml响应
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Stream PutXmlEntry(string kind, string method, Stream parameters)
        {
            return GetResponseStream(ParameterFormat.Xml, kind, method, parameters);
        }

        /// <summary>
        /// 实现Get方式Xml响应
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public Stream GetXmlEntry(string kind, string method)
        {
            return GetResponseStream(ParameterFormat.Xml, kind, method);
        }

        /// <summary>
        /// 实现Get方式Jsonp响应
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public Stream GetEntryCallBack(string kind, string method)
        {
            var request = WebOperationContext.Current.IncomingRequest;
            var response = WebOperationContext.Current.OutgoingResponse;

            NameValueCollection values = request.UriTemplateMatch.QueryParameters;
            var callback = values["callback"];
            string result = string.Empty;

            if (string.IsNullOrEmpty(callback))
            {
                var ret = new RESTfulResult { Code = (int)RESTfulCode.OK, Message = "Not found [callback] parameter!" };
                //throw new WebFaultException<RESTfulResult>(ret, HttpStatusCode.Forbidden);
                response.StatusCode = HttpStatusCode.Forbidden;
                response.ContentType = "application/json;charset=utf-8";
                result = SerializationManager.SerializeJson(ret);
            }
            else
            {
                result = GetResponseString(ParameterFormat.Json, kind, method, null) as string;
                response.ContentType = "application/javascript;charset=utf-8";
                result = string.Format("{0}({1});", callback, result ?? "{}");
            }

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(result));
            return stream;
        }

        /// <summary>
        /// 获取方法的html文档
        /// </summary>
        /// <returns></returns>
        public Stream GetMethodHtml()
        {
            var request = WebOperationContext.Current.IncomingRequest;
            var html = Context.MakeApiDocument(request.UriTemplateMatch.RequestUri);
            var response = WebOperationContext.Current.OutgoingResponse;
            response.ContentType = "text/html;charset=utf-8";
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(html));
            return stream;
        }

        #endregion

        private Stream GetResponseStream(ParameterFormat format, string kind, string method, Stream parameters)
        {
            StreamReader sr = new StreamReader(parameters);
            string data = sr.ReadToEnd();
            sr.Close();

            string result = GetResponseString(format, kind, method, data) as string;
            if (string.IsNullOrEmpty(result)) return new MemoryStream();
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(result)); ;
            return stream;
        }

        private Stream GetResponseStream(ParameterFormat format, string kind, string method)
        {
            string result = GetResponseString(format, kind, method, null) as string;
            if (string.IsNullOrEmpty(result)) return new MemoryStream();
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(result)); ;
            return stream;
        }

        private string GetResponseString(ParameterFormat format, string kind, string method, string parameters)
        {
            var request = WebOperationContext.Current.IncomingRequest;
            var response = WebOperationContext.Current.OutgoingResponse;

            if (format == ParameterFormat.Json)
                response.ContentType = "application/json;charset=utf-8";
            else if (format == ParameterFormat.Xml)
                response.ContentType = "text/xml;charset=utf-8";

            object result = null;

            //进行认证处理
            RESTfulResult authResult = AuthenticationUtils.Authorize();

            //认证成功
            if (authResult.Code == (int)RESTfulCode.OK)
            {
                try
                {
                    result = Context.Invoke(format, kind, method, parameters);
                    if (result == null)
                    {
                        if (format == ParameterFormat.Json)
                            result = "{}";
                        else
                            response.ContentType = "text/plain";

                        return result as string;
                    }
                }
                catch (RESTfulException e)
                {
                    result = new RESTfulResult { Code = (int)e.Code, Message = e.Message };
                    //result = new WebFaultException<RESTfulResult>(ret, HttpStatusCode.BadRequest);
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
                catch (Exception e)
                {
                    result = new RESTfulResult { Code = (int)RESTfulCode.BUSINESS_ERROR_CODE, Message = e.Message };
                    //result = new WebFaultException<RESTfulResult>(ret, HttpStatusCode.ExpectationFailed);
                    response.StatusCode = HttpStatusCode.ExpectationFailed;
                }
            }
            else
            {
                result = authResult;
            }

            ISerializer serializer = SerializerFactory.Create(format);
            try
            {
                result = serializer.Serialize(result);
                return result.ToString();
            }
            catch (Exception ex)
            {
                //如果系列化失败
                result = new RESTfulResult { Code = (int)RESTfulCode.BUSINESS_ERROR_CODE, Message = ex.Message };
                result = serializer.Serialize(result);
                return result.ToString();
            }
        }
    }
}