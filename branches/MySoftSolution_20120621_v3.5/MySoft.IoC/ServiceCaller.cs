﻿using System;
using System.Collections.Generic;
using System.Threading;
using MySoft.IoC.Communication.Scs.Communication;
using MySoft.IoC.Communication.Scs.Server;
using MySoft.IoC.Configuration;
using MySoft.IoC.Messages;
using MySoft.IoC.Services;

namespace MySoft.IoC
{
    /// <summary>
    /// 服务调用者
    /// </summary>
    internal class ServiceCaller
    {
        private IDictionary<string, Type> callbackTypes;
        private IDictionary<string, SyncCaller> syncCallers;
        private IServiceContainer container;
        private Semaphore _semaphore;
        private TimeSpan _timeout;

        /// <summary>
        /// 初始化ServiceCaller
        /// </summary>
        /// <param name="config"></param>
        /// <param name="container"></param>
        public ServiceCaller(CastleServiceConfiguration config, IServiceContainer container)
        {
            this.container = container;
            this.callbackTypes = new Dictionary<string, Type>();
            this.syncCallers = new Dictionary<string, SyncCaller>();
            this._semaphore = new Semaphore(config.MaxCaller, config.MaxCaller);
            this._timeout = TimeSpan.FromSeconds(ServiceConfig.DEFAULT_CLIENT_TIMEOUT);

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

                IService service = null;
                string serviceKey = "Service_" + type.FullName;

                if (container.Kernel.HasComponent(serviceKey))
                {
                    service = container.Resolve<IService>(serviceKey);

                    //实例化SyncCaller
                    if (config.EnableCache)
                        syncCallers[type.FullName] = new SyncCaller(service, null, true);
                    else
                        syncCallers[type.FullName] = new SyncCaller(service, true);
                }
            }
        }

        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool InvokeResponse(IScsServerClient channel, CallerContext e)
        {
            if (!_semaphore.WaitOne(_timeout, false)) return false;

            //不是连接状态，直接返回
            if (channel.CommunicationState != CommunicationStates.Connected)
            {
                return false;
            }

            //获取上下文
            using (var context = GetOperationContext(channel, e.Caller))
            {
                try
                {
                    //解析服务
                    var syncCaller = GetAsyncCaller(e.Caller);

                    //异步调用服务
                    e.Message = syncCaller.Run(context, e.Request);
                }
                catch (Exception ex)
                {
                    //获取异常响应
                    e.Message = IoCHelper.GetResponse(e.Request, ex);
                }
                finally
                {
                    _semaphore.Release();
                }

                return e.Message != null;
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
        /// Gets the syncCaller.
        /// </summary>
        /// <param name="caller"></param>
        /// <returns></returns>
        private SyncCaller GetAsyncCaller(AppCaller caller)
        {
            lock (syncCallers)
            {
                if (!syncCallers.ContainsKey(caller.ServiceName))
                {
                    string body = string.Format("The server【{1}({2})】not find matching service ({0})."
                        , caller.ServiceName, DnsHelper.GetHostName(), DnsHelper.GetIPAddress());

                    //获取异常
                    throw IoCHelper.GetException(caller, body);
                }

                return syncCallers[caller.ServiceName];
            }
        }
    }
}
