﻿using System;
using System.Collections.Generic;
using MySoft.IoC.Communication.Scs.Server;
using MySoft.IoC.Configuration;
using MySoft.IoC.Messages;
using MySoft.IoC.Services;

namespace MySoft.IoC
{
    /// <summary>
    /// 服务调用者
    /// </summary>
    internal class ServiceCaller : IDisposable
    {
        private IDictionary<string, Type> callbackTypes;
        private IDictionary<string, SyncCaller> syncCallers;
        private IServiceContainer container;

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
                        syncCallers[type.FullName] = new SyncCaller(service, null);
                    else
                        syncCallers[type.FullName] = new SyncCaller(service);
                }
            }
        }

        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public ResponseMessage InvokeResponse(IScsServerClient channel, ICaller e)
        {
            //获取上下文
            using (var context = GetOperationContext(channel, e.Caller))
            {
                //解析服务
                var syncCaller = GetAsyncCaller(e.Caller);

                byte[] buffer = null;

                //异步调用服务
                var resMsg = syncCaller.Run(context, e.Request, out buffer);

                //如果消息为null
                if (resMsg == null) e.Buffer = buffer;

                return resMsg;
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
                    string body = string.Format("The server not find matching service ({0}).", caller.ServiceName);

                    //获取异常
                    throw IoCHelper.GetException(caller, body);
                }

                return syncCallers[caller.ServiceName];
            }
        }

        #region IDisposable 成员

        public void Dispose()
        {
            callbackTypes.Clear();
            syncCallers.Clear();

            callbackTypes = null;
            syncCallers = null;
        }

        #endregion
    }
}
