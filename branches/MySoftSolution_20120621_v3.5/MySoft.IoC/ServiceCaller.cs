﻿using MySoft.IoC.Communication.Scs.Server;
using MySoft.IoC.Configuration;
using MySoft.IoC.Messages;
using MySoft.IoC.Services;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MySoft.IoC
{
    /// <summary>
    /// 服务调用者
    /// </summary>
    internal class ServiceCaller : IDisposable
    {
        private readonly IServiceContainer container;
        private IDictionary<string, Type> callbackTypes;
        private Semaphore semaphore;
        private TimeSpan timeout;

        /// <summary>
        /// 初始化ServiceCaller
        /// </summary>
        /// <param name="config"></param>
        /// <param name="container"></param>
        public ServiceCaller(CastleServiceConfiguration config, IServiceContainer container)
        {
            this.callbackTypes = new Dictionary<string, Type>();
            this.container = container;
            this.semaphore = new Semaphore(config.MaxCaller, config.MaxCaller);
            this.timeout = TimeSpan.FromSeconds(config.Timeout);

            //初始化服务
            Init(container, config);
        }

        private void Init(IServiceContainer container, CastleServiceConfiguration config)
        {
            callbackTypes[typeof(IStatusService).FullName] = typeof(IStatusListener);
            var types = container.GetServiceTypes<ServiceContractAttribute>();

            foreach (var type in types)
            {
                var contract = CoreHelper.GetMemberAttribute<ServiceContractAttribute>(type);
                if (contract != null && contract.CallbackType != null)
                {
                    callbackTypes[type.FullName] = contract.CallbackType;
                }
            }
        }

        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="appCaller"></param>
        /// <param name="reqMsg"></param>
        /// <returns></returns>
        public ResponseMessage HandleResponse(IScsServerClient channel, AppCaller appCaller, RequestMessage reqMsg)
        {
            //请求一个控制器
            semaphore.WaitOne();

            try
            {
                //解析服务
                var service = ParseService(appCaller);

                using (var caller = new AsyncCaller(service))
                {
                    //获取上下文
                    var context = GetOperationContext(channel, appCaller);

                    //判断是否为状态服务
                    if (reqMsg.ServiceName == typeof(IStatusService).FullName)
                        return caller.SyncRun(context, reqMsg);
                    else
                        return caller.AsyncRun(context, reqMsg, timeout);
                }
            }
            catch (Exception ex)
            {
                //获取异常响应信息
                return IoCHelper.GetResponse(reqMsg, ex);
            }
            finally
            {
                //释放一个控制器
                semaphore.Release();
            }
        }

        /// <summary>
        /// 获取上下文
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        private OperationContext GetOperationContext(IScsServerClient channel, AppCaller caller)
        {
            //实例化当前上下文
            Type callbackType = null;

            lock (callbackTypes)
            {
                if (callbackTypes.ContainsKey(caller.ServiceName))
                {
                    callbackType = callbackTypes[caller.ServiceName];
                }
            }

            return new OperationContext(channel, callbackType)
            {
                Container = container,
                Caller = caller
            };
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="caller"></param>
        /// <returns></returns>
        private IService ParseService(AppCaller caller)
        {
            string serviceKey = "Service_" + caller.ServiceName;

            //判断服务是否存在
            if (container.Kernel.HasComponent(serviceKey))
            {
                return container.Resolve<IService>(serviceKey);
            }
            else
            {
                string body = string.Format("The server not find matching service ({0}).", caller.ServiceName);

                //获取异常
                throw IoCHelper.GetException(caller, body);
            }
        }

        #region IDisposable 成员

        public void Dispose()
        {
            callbackTypes.Clear();
        }

        #endregion
    }
}
