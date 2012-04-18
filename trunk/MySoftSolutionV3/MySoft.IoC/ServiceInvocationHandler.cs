using System;
using System.Collections.Generic;
using MySoft.IoC.Configuration;
using MySoft.IoC.Messages;
using MySoft.IoC.Cache;
using System.Diagnostics;

namespace MySoft.IoC
{
    /// <summary>
    /// The base impl class of the service interface, this class is used by service factory to emit service interface impl automatically at runtime.
    /// </summary>
    public class ServiceInvocationHandler : IProxyInvocationHandler
    {
        private CastleFactoryConfiguration config;
        private IDictionary<string, int> cacheTimes;
        protected IServiceContainer container;
        private IService service;
        private Type serviceType;
        private IServiceCache cache;
        private IServiceLog logger;
        private string hostName;
        private string ipAddress;

        /// <summary>
        ///  Initializes a new instance of the <see cref="ServiceInvocationHandler"/> class.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="container"></param>
        /// <param name="service"></param>
        /// <param name="serviceType"></param>
        /// <param name="cache"></param>
        public ServiceInvocationHandler(CastleFactoryConfiguration config, IServiceContainer container, IService service, Type serviceType, IServiceCache cache, IServiceLog logger)
        {
            this.config = config;
            this.container = container;
            this.serviceType = serviceType;
            this.service = service;
            this.cache = cache;
            this.logger = logger;

            this.hostName = DnsHelper.GetHostName();
            this.ipAddress = DnsHelper.GetIPAddress();

            this.cacheTimes = new Dictionary<string, int>();
            var methods = CoreHelper.GetMethodsFromType(serviceType);
            foreach (var method in methods)
            {
                var contract = CoreHelper.GetMemberAttribute<OperationContractAttribute>(method);
                if (contract != null && contract.CacheTime > 0)
                    cacheTimes[method.ToString()] = contract.CacheTime;
            }
        }

        #region IInvocationHandler 成员

        /// <summary>
        /// 响应委托
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object Invoke(object proxy, System.Reflection.MethodInfo method, object[] parameters)
        {
            object returnValue = null;

            var hashtable = ServiceConfig.CreateParameters(method, parameters);
            string cacheKey = ServiceConfig.GetCacheKey(serviceType, method, hashtable);
            var cacheValue = cache.Get<CacheObject>(cacheKey);

            //缓存无值
            if (cacheValue == null)
            {
                //调用方法
                var resMsg = InvokeMethod(method, hashtable);

                if (resMsg != null)
                {
                    returnValue = resMsg.Value;

                    //处理参数
                    ServiceConfig.SetParameterValue(method, parameters, resMsg.Parameters);

                    //如果需要缓存，则存入本地缓存
                    if (returnValue != null && cacheTimes.ContainsKey(method.ToString()))
                    {
                        int cacheTime = cacheTimes[method.ToString()];
                        cacheValue = new CacheObject
                        {
                            Value = resMsg.Value,
                            Parameters = resMsg.Parameters
                        };

                        cache.Insert(cacheKey, cacheValue, cacheTime);
                    }
                }
            }
            else
            {
                //处理返回值
                returnValue = cacheValue.Value;

                //处理参数
                ServiceConfig.SetParameterValue(method, parameters, cacheValue.Parameters);
            }

            //返回结果
            return returnValue;
        }

        /// <summary>
        /// 调用方法返回
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private ResponseMessage InvokeMethod(System.Reflection.MethodInfo method, ParameterCollection parameters)
        {
            #region 设置请求信息

            var reqMsg = new RequestMessage
            {
                AppName = config.AppName,                       //应用名称
                HostName = hostName,                            //客户端名称
                IPAddress = ipAddress,                          //客户端IP地址
                ReturnType = method.ReturnType,                 //返回类型
                ServiceName = serviceType.FullName,             //服务名称
                MethodName = method.ToString(),                 //方法名称
                TransactionId = Guid.NewGuid(),                 //传输ID号
                Parameters = parameters                         //设置参数
            };

            //设置调用方法
            (reqMsg as IInvoking).MethodInfo = method;

            //设置是否缓存
            if (cacheTimes.ContainsKey(method.ToString()))
            {
                reqMsg.IsCaching = true;
            }

            #endregion

            //调用服务
            return CallService(reqMsg);
        }

        /// <summary>
        /// Calls the service.
        /// </summary>
        /// <param name="reqMsg">Name of the sub service.</param>
        /// <returns>The result.</returns>
        protected virtual ResponseMessage CallService(RequestMessage reqMsg)
        {
            ResponseMessage resMsg = null;

            try
            {
                //写日志开始
                if (logger != null)
                {
                    try
                    {
                        var callMsg = new CallMessage
                        {
                            AppName = reqMsg.AppName,
                            IPAddress = reqMsg.IPAddress,
                            HostName = reqMsg.HostName,
                            ServiceName = reqMsg.ServiceName,
                            MethodName = reqMsg.MethodName
                        };

                        //开始调用
                        logger.Begin(callMsg, reqMsg.Parameters);
                    }
                    catch
                    {
                    }
                }

                //开始计时
                var watch = Stopwatch.StartNew();

                //调用服务
                resMsg = service.CallService(reqMsg);

                watch.Stop();

                //写日志结束
                if (logger != null)
                {
                    try
                    {
                        var returnMsg = new ReturnMessage
                        {
                            ServiceName = resMsg.ServiceName,
                            MethodName = reqMsg.MethodName,
                            Count = resMsg.Count,
                            Error = resMsg.Error,
                            Value = resMsg.Value
                        };

                        //结束调用
                        logger.End(returnMsg, resMsg.Parameters, watch.ElapsedMilliseconds);
                    }
                    catch
                    {
                    }
                }

                //如果有异常，向外抛出
                if (resMsg.IsError) throw resMsg.Error;
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                if (config.ThrowError)
                    throw ex;
                else
                    container.WriteError(ex);
            }

            return resMsg;
        }

        #endregion
    }
}
