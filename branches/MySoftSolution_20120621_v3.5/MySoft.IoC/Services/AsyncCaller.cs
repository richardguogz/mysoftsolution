﻿using System;
using System.Threading;
using MySoft.IoC.Messages;

namespace MySoft.IoC.Services
{
    /// <summary>
    /// 异步调用器
    /// </summary>
    internal class AsyncCaller
    {
        private Semaphore semaphore;

        /// <summary>
        /// 实例化AsyncCaller
        /// </summary>
        /// <param name="maxCaller"></param>
        public AsyncCaller(int maxCaller)
        {
            this.semaphore = new Semaphore(maxCaller, maxCaller);
        }

        /// <summary>
        /// 异步调用服务
        /// </summary>
        /// <param name="service"></param>
        /// <param name="context"></param>
        /// <param name="reqMsg"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public ResponseItem AsyncRun(IService service, OperationContext context, RequestMessage reqMsg, TimeSpan timeout)
        {
            //请求一个控制器
            semaphore.WaitOne();

            try
            {
                //异步处理器
                var handler = new AsyncHandler(service, context, reqMsg);

                //Invoke响应
                var ar = handler.BeginDoTask(null, null);

                //超时返回
                if (!ar.AsyncWaitHandle.WaitOne(timeout, false))
                {
                    return GetTimeoutResponse(reqMsg, timeout);
                }

                return handler.EndDoTask(ar);
            }
            finally
            {
                //释放一个控制器
                semaphore.Release();
            }
        }

        /// <summary>
        /// 异步调用服务
        /// </summary>
        /// <param name="service"></param>
        /// <param name="context"></param>
        /// <param name="reqMsg"></param>
        /// <returns></returns>
        public ResponseItem SyncRun(IService service, OperationContext context, RequestMessage reqMsg)
        {
            //请求一个控制器
            semaphore.WaitOne();

            try
            {
                //异步处理器
                var handler = new AsyncHandler(service, context, reqMsg);

                //同步响应
                return handler.DoTask();
            }
            finally
            {
                //释放一个控制器
                semaphore.Release();
            }
        }

        /// <summary>
        /// 获取超时响应信息
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        private ResponseItem GetTimeoutResponse(RequestMessage reqMsg, TimeSpan timeout)
        {
            var title = string.Format("Async invoke service ({0}, {1}) timeout ({2}) ms.", reqMsg.ServiceName, reqMsg.MethodName, (int)timeout.TotalMilliseconds);

            //获取异常
            var resMsg = IoCHelper.GetResponse(reqMsg, new TimeoutException(title));

            return new ResponseItem(resMsg);
        }
    }
}