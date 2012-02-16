﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MySoft.RESTful;
using MySoft.Net.HTTP;
using System.Collections.Specialized;

namespace MySoft.IoC.Http
{
    /// <summary>
    /// Http服务调用
    /// </summary>
    public class HttpServiceCaller
    {
        private IServiceContainer container;
        private IHttpAuthentication handler;
        private IDictionary<string, HttpCallerInfo> callers;
        private int port;

        /// <summary>
        /// HttpServiceCaller初始化
        /// </summary>
        /// <param name="container"></param>
        /// <param name="httpAuth"></param>
        /// <param name="port"></param>
        public HttpServiceCaller(IServiceContainer container, string httpAuth, int port)
        {
            this.container = container;
            this.port = port;
            this.callers = new Dictionary<string, HttpCallerInfo>();

            //加载httpAuth
            if (!string.IsNullOrEmpty(httpAuth))
            {
                Type type = Type.GetType(httpAuth);
                object instance = Activator.CreateInstance(type);
                this.handler = instance as IHttpAuthentication;
            }

            //初始化字典
            foreach (var serviceType in container.GetInterfaces<ServiceContractAttribute>())
            {
                var serviceAttr = CoreHelper.GetTypeAttribute<ServiceContractAttribute>(serviceType);
                var serviceName = serviceAttr.Name ?? serviceType.FullName;

                object instance = null;
                try { instance = container.Resolve(serviceType); }
                catch { }
                if (instance == null) continue;

                //添加方法
                foreach (var methodInfo in CoreHelper.GetMethodsFromType(serviceType))
                {
                    var description = serviceAttr.Description;
                    var methodAttr = CoreHelper.GetMemberAttribute<OperationContractAttribute>(methodInfo);
                    if (methodAttr != null && methodAttr.HttpEnabled)
                    {
                        string methodName = methodAttr.Name ?? methodInfo.Name;
                        string fullName = string.Format("{0}.{1}", serviceName, methodName);
                        if (!string.IsNullOrEmpty(methodAttr.Description))
                        {
                            if (string.IsNullOrEmpty(description))
                                description = methodAttr.Description;
                            else
                                description += " - " + methodAttr.Description;
                        }

                        //将方法添加到字典
                        var callerInfo = new HttpCallerInfo
                        {
                            ServiceName = string.Format("【{0}】\r\n{1}", serviceType.FullName, methodInfo.ToString()),
                            Method = methodInfo,
                            Instance = instance,
                            Authorized = methodAttr.Authorized,
                            AuthParameter = methodAttr.AuthParameter,
                            Description = description
                        };

                        if (!CheckGetSubmitType(methodInfo.GetParameters()))
                        {
                            callerInfo.IsPassCheck = false;
                            callerInfo.CheckMessage = string.Format("{0} business is not pass check, because the SubmitType of 'GET' parameters only suport primitive type.", fullName);
                        }
                        else
                        {
                            callerInfo.IsPassCheck = true;
                        }

                        if (callers.ContainsKey(fullName))
                        {
                            //处理重复的方法
                            for (int i = 0; i < 10000; i++)
                            {
                                var name = fullName + (i + 1);
                                if (!callers.ContainsKey(name))
                                {
                                    fullName = name;
                                    break;
                                }
                            }
                        }

                        callers[fullName] = callerInfo;
                    }
                }
            }
        }

        /// <summary>
        /// 获取Http方法
        /// </summary>
        /// <returns></returns>
        public string GetDocument()
        {
            var doc = new HttpDocument(callers, port);
            return doc.MakeDocument();
        }

        /// <summary>
        /// 调用服务，并返回字符串
        /// </summary>
        /// <param name="name"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public string CallMethod(string name, NameValueCollection collection)
        {
            if (callers.ContainsKey(name))
            {
                var caller = callers[name];
                if (!caller.IsPassCheck)
                {
                    throw new HTTPMessageException(caller.CheckMessage);
                }

                var parameters = new object[0];
                if (caller.Method.GetParameters().Length > 0)
                {
                    parameters = ParseParameters(collection, caller);
                }

                try
                {
                    var retVal = DynamicCalls.GetMethodInvoker(caller.Method).Invoke(caller.Instance, parameters);

                    if (retVal == null) return "{}";
                    return SerializationManager.SerializeJson(retVal, new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
                }
                catch (Exception ex)
                {
                    throw new HTTPMessageException(ex.Message);
                }
            }
            else
            {
                throw new HTTPMessageException(string.Format("Not found method {0}!", name));
            }
        }

        /// <summary>
        /// 处理参数
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        private object[] ParseParameters(NameValueCollection collection, HttpCallerInfo caller)
        {
            var jobject = ParameterHelper.Resolve(collection);
            if (caller.Authorized)
            {
                if (string.IsNullOrEmpty(caller.AuthParameter))
                {
                    throw new HTTPMessageException("AuthParameter is empty or not set correct.");
                }
                else
                {
                    if (handler != null)
                    {
                        //进行授权处理
                        try
                        {
                            //处理sessionKey
                            string authString = handler.Authorize(container, collection["sessionKey"]);
                            jobject[caller.AuthParameter] = authString;
                        }
                        catch (Exception ex)
                        {
                            throw new HTTPMessageException("SessionKey is empty or not set correct, authorized failed! Error: " + ex.Message);
                        }
                    }
                    else
                    {
                        throw new HTTPMessageException("Authorized failed ,httpAuth not set correct.");
                    }
                }
            }

            return ParameterHelper.Convert(jobject, caller.Method.GetParameters());
        }

        /// <summary>
        /// 检查Get类型的参数
        /// </summary>
        /// <param name="paramsInfo"></param>
        /// <returns></returns>
        private bool CheckGetSubmitType(ParameterInfo[] paramsInfo)
        {
            //如果参数为0
            if (paramsInfo.Length == 0) return true;

            bool result = true;
            StringBuilder sb = new StringBuilder();
            foreach (ParameterInfo p in paramsInfo)
            {
                if (!(p.ParameterType.IsValueType || p.ParameterType == typeof(string)))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }
    }
}
