﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using MySoft.IoC.Callback;
using MySoft.IoC.Communication.Scs.Server;
using MySoft.IoC.Messages;
using MySoft.IoC.Services.Tasks;

namespace MySoft.IoC
{
    /// <summary>
    /// 服务调用者
    /// </summary>
    public class ServiceCaller
    {
        private IDictionary<string, Type> callbackTypes;
        private ServerStatusService status;

        /// <summary>
        /// 初始化ServiceCaller
        /// </summary>
        /// <param name="status"></param>
        public ServiceCaller(ServerStatusService status)
        {
            this.status = status;
            this.callbackTypes = new Dictionary<string, Type>();

            //初始化服务
            InitServiceCaller(status.Container);

            //注册状态服务
            var hashtable = new Dictionary<Type, object>();
            hashtable[typeof(IStatusService)] = status;

            //注册组件
            status.Container.RegisterComponents(hashtable);
        }

        private void InitServiceCaller(IServiceContainer container)
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
        /// <param name="client"></param>
        /// <param name="reqMsg"></param>
        /// <returns></returns>
        public void CallMethod(IScsServerClient client, RequestMessage reqMsg)
        {
            //创建Caller;
            var caller = CreateCaller(client, reqMsg);

            //获取上下文
            var context = GetOperationContext(client, caller);

            try
            {
                OperationContext.Current = context;

                //创建服务
                var service = ParseService(reqMsg);

                var resMsg = service.CallService(reqMsg);

                //发送消息
                SendMessage(client, reqMsg, resMsg, reqMsg.TransactionId.ToString());

                //调用参数
                //var callerArgs = new AsyncCallerArgs
                //{
                //    Context = context,
                //    Request = reqMsg,
                //};

                //var asyncCaller = new AsyncCaller(service, callerArgs);
                //asyncCaller.BeginDoTask(AsyncCallback, new ArrayList { asyncCaller, client, callerArgs });
            }
            catch (Exception ex)
            {
                //将异常信息写出
                status.Container.WriteError(ex);

                //处理异常
                var resMsg = IoCHelper.GetResponse(reqMsg, ex);

                //发送消息
                SendMessage(client, reqMsg, resMsg, reqMsg.TransactionId.ToString());
            }
            finally
            {
                OperationContext.Current = null;
            }
        }

        /// <summary>
        /// 回调方法
        /// </summary>
        /// <param name="ar"></param>
        private void AsyncCallback(IAsyncResult ar)
        {
            var arr = ar.AsyncState as ArrayList;

            try
            {
                var asyncCaller = arr[0] as AsyncCaller;
                var client = arr[1] as IScsServerClient;
                var asyncArgs = arr[2] as AsyncCallerArgs;

                //返回响应数据
                var resMsg = asyncCaller.EndDoTask(ar);

                //调用参数
                var callArgs = new CallEventArgs
                {
                    Caller = asyncArgs.Context.Caller,
                    ElapsedTime = asyncCaller.ElapsedMilliseconds,
                    Count = resMsg.Count,
                    Error = resMsg.Error,
                    Value = resMsg.Value
                };

                //响应计数
                NotifyEventArgs(callArgs);

                var reqMsg = asyncArgs.Request;

                //如果是Json方式调用，则需要处理异常
                if (resMsg.IsError && reqMsg.InvokeMethod)
                {
                    resMsg.Error = new ApplicationException(callArgs.Error.Message);
                }

                //发送消息
                SendMessage(client, reqMsg, resMsg, reqMsg.TransactionId.ToString());
            }
            catch (Exception ex)
            {
                //将异常信息写出
                status.Container.WriteError(ex);
            }
            finally
            {
                //清理资源
                arr = null;
            }
        }

        /// <summary>
        /// 响应计数事件
        /// </summary>
        /// <param name="callArgs"></param>
        private void NotifyEventArgs(CallEventArgs callArgs)
        {
            //调用计数
            ThreadPool.QueueUserWorkItem(state =>
            {
                try
                {
                    var arr = state as ArrayList;
                    var statusService = arr[0] as ServerStatusService;
                    var eventArgs = arr[1] as CallEventArgs;

                    //调用计数服务
                    statusService.Counter(eventArgs);

                    //响应消息
                    MessageCenter.Instance.Notify(eventArgs);
                }
                catch (Exception ex)
                {
                    //TODO
                }
            }, new ArrayList { status, callArgs });
        }

        /// <summary>
        /// 获取AppCaller
        /// </summary>
        /// <param name="client"></param>
        /// <param name="reqMsg"></param>
        /// <returns></returns>
        private AppCaller CreateCaller(IScsServerClient client, RequestMessage reqMsg)
        {
            //获取AppPath
            var appPath = (client.State == null) ? null : (client.State as AppClient).AppPath;

            //服务参数信息
            var caller = new AppCaller
            {
                AppPath = appPath,
                AppName = reqMsg.AppName,
                IPAddress = reqMsg.IPAddress,
                HostName = reqMsg.HostName,
                ServiceName = reqMsg.ServiceName,
                MethodName = reqMsg.MethodName,
                Parameters = reqMsg.Parameters.ToString(),
                CallTime = DateTime.Now
            };

            return caller;
        }

        /// <summary>
        /// 获取上下文
        /// </summary>
        /// <param name="client"></param>
        /// <param name="caller"></param>
        private OperationContext GetOperationContext(IScsServerClient client, AppCaller caller)
        {
            //实例化当前上下文
            Type callbackType = null;
            if (callbackTypes.ContainsKey(caller.ServiceName))
            {
                callbackType = callbackTypes[caller.ServiceName];
            }

            return new OperationContext(client, callbackType)
            {
                Container = status.Container,
                Caller = caller
            };
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <returns></returns>
        private IService ParseService(RequestMessage reqMsg)
        {
            IService service = null;
            string serviceKey = "Service_" + reqMsg.ServiceName;

            if (status.Container.Kernel.HasComponent(serviceKey))
            {
                service = status.Container.Resolve<IService>(serviceKey);
            }

            if (service == null)
            {
                string body = string.Format("The server【{1}({2})】not find matching service ({0})."
                    , reqMsg.ServiceName, DnsHelper.GetHostName(), DnsHelper.GetIPAddress());

                //获取异常
                throw IoCHelper.GetException(OperationContext.Current, reqMsg, body);
            }

            return service;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="client"></param>
        /// <param name="reqMsg"></param>
        /// <param name="resMsg"></param>
        /// <param name="messageId"></param>
        private void SendMessage(IScsServerClient client, RequestMessage reqMsg, ResponseMessage resMsg, string messageId)
        {
            try
            {
                var sendMsg = new ScsResultMessage(resMsg, messageId);

                //发送消息
                client.SendMessage(sendMsg);
            }
            catch (Exception ex)
            {
                //写异常日志
                status.Container.WriteError(ex);

                try
                {
                    resMsg = IoCHelper.GetResponse(reqMsg, ex);

                    var sendMsg = new ScsResultMessage(resMsg, messageId);

                    //发送消息
                    client.SendMessage(sendMsg);
                }
                catch
                {
                    //写异常日志
                    status.Container.WriteError(ex);
                }
            }
        }
    }
}
