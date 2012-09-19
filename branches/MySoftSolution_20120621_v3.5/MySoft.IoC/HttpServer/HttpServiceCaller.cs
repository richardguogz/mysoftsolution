﻿using System;
using System.Collections;
using MySoft.IoC.Configuration;
using MySoft.IoC.Messages;

namespace MySoft.IoC.HttpServer
{
    /// <summary>
    /// Http服务调用
    /// </summary>
    public class HttpServiceCaller
    {
        private IServiceContainer container;
        private CastleServiceConfiguration config;
        private HttpCallerInfoCollection callers;

        /// <summary>
        /// HttpServiceCaller初始化
        /// </summary>
        /// <param name="config"></param>
        /// <param name="container"></param>
        public HttpServiceCaller(CastleServiceConfiguration config, IServiceContainer container)
        {
            this.config = config;
            this.container = container;
            this.callers = new HttpCallerInfoCollection();
        }

        /// <summary>
        /// 初始化Caller
        /// </summary>
        /// <param name="resolver"></param>
        public void InitCaller(IHttpApiResolver resolver)
        {
            //清理资源
            callers.Clear();

            //获取拥有ServiceContract约束的服务
            var types = container.GetServiceTypes<ServiceContractAttribute>();

            //初始化字典
            foreach (var type in types)
            {
                //状态服务跳过
                if (type == typeof(IStatusService)) continue;

                if (resolver != null)
                {
                    //添加方法
                    foreach (var httpApi in resolver.MethodResolver(type))
                    {
                        //添加一个新的Caller
                        AddNewCaller(type, httpApi);
                    }
                }
            }
        }

        private void AddNewCaller(Type serviceType, HttpApiMethod httpApi)
        {
            //将方法添加到字典
            var callerInfo = new HttpCallerInfo
            {
                CacheTime = httpApi.CacheTime,
                Service = serviceType,
                Method = httpApi.Method,
                TypeString = httpApi.Method.ReturnType == typeof(string),
                Description = httpApi.Description,
                Authorized = httpApi.Authorized,
                AuthParameter = httpApi.AuthParameter,
                HttpMethod = httpApi.HttpMethod
            };

            string fullName = httpApi.Name;
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

            callerInfo.CallerName = fullName;
            callers[fullName] = callerInfo;
        }

        /// <summary>
        /// 获取Http方法
        /// </summary>
        /// <returns></returns>
        public string GetDocument(string name)
        {
            var dicCaller = new HttpCallerInfoCollection();
            if (!string.IsNullOrEmpty(name))
            {
                if (callers.ContainsKey(name))
                    dicCaller[name] = callers[name];
            }
            else
                dicCaller = callers;

            var doc = new HttpDocument(dicCaller, config.HttpPort);
            return doc.MakeDocument(name);
        }

        /// <summary>
        /// 获取API文档
        /// </summary>
        /// <returns></returns>
        public string GetAPIText()
        {
            var array = new ArrayList();
            foreach (var kvp in callers)
            {
                array.Add(new
                {
                    Name = kvp.Value.CallerName,
                    Authorized = kvp.Value.Authorized,
                    TypeString = kvp.Value.TypeString
                });
            }

            //系列化json输出
            return SerializationManager.SerializeJson(array);
        }

        /// <summary>
        /// 获取调用信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public HttpCallerInfo GetCaller(string name)
        {
            if (callers.ContainsKey(name))
            {
                return callers[name];
            }

            return null;
        }

        /// <summary>
        /// 调用服务，并返回字符串
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string CallMethod(string name, string parameters)
        {
            if (callers.ContainsKey(name))
            {
                var caller = callers[name];
                var message = new InvokeMessage
                {
                    ServiceName = caller.Service.FullName,
                    MethodName = caller.Method.ToString(),
                    Parameters = parameters
                };

                string thisKey = string.Format("{0}${1}${2}", message.ServiceName, message.MethodName, message.Parameters);
                var cacheKey = string.Format("HttpServiceCaller_{0}", thisKey);

                var invokeData = CacheHelper.Get<InvokeData>(cacheKey);
                if (invokeData == null)
                {
                    //创建服务
                    var service = ParseService(message.ServiceName);

                    //使用Invoke方式调用
                    var invoke = new InvokeCaller("HttpServer", container, service);
                    invokeData = invoke.CallMethod(message);

                    //插入缓存
                    if (invokeData != null && caller.CacheTime > 0)
                    {
                        CacheHelper.Insert(cacheKey, invokeData, caller.CacheTime);
                    }
                }

                //如果缓存不为null，则返回缓存数据
                if (invokeData != null)
                {
                    return invokeData.Value;
                }
            }

            return "null";
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        private IService ParseService(string serviceName)
        {
            //处理数据返回InvokeData
            IService service = null;
            string serviceKey = "Service_" + serviceName;

            if (container.Kernel.HasComponent(serviceKey))
            {
                service = container.Resolve<IService>(serviceKey);
            }

            if (service == null)
            {
                string body = string.Format("The server【{1}({2})】not find matching service ({0})."
                    , serviceName, DnsHelper.GetHostName(), DnsHelper.GetIPAddress());

                //返回异常信息
                throw new WarningException(body);
            }

            return service;
        }
    }
}