﻿using System;
using MySoft.IoC.Configuration;
using MySoft.IoC.Messages;
using MySoft.IoC.Services;

namespace MySoft.IoC
{
    /// <summary>
    /// 调用者
    /// </summary>
    internal class InvokeCaller
    {
        private readonly CastleFactoryConfiguration config;
        private readonly IContainer container;
        private readonly IService service;
        private readonly AsyncCaller caller;
        private string hostName;
        private string ipAddress;

        /// <summary>
        /// 实例化InvokeCaller
        /// </summary>
        /// <param name="config"></param>
        /// <param name="container"></param>
        /// <param name="service"></param>
        /// <param name="caller"></param>
        public InvokeCaller(CastleFactoryConfiguration config, IContainer container, IService service, AsyncCaller caller)
        {
            this.config = config;
            this.container = container;
            this.service = service;
            this.caller = caller;

            this.hostName = DnsHelper.GetHostName();
            this.ipAddress = DnsHelper.GetIPAddress();
        }

        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public InvokeData InvokeResponse(InvokeMessage message)
        {
            #region 设置请求信息

            var reqMsg = new RequestMessage
            {
                InvokeMethod = true,
                AppName = config.AppName,                               //应用名称
                AppPath = AppDomain.CurrentDomain.BaseDirectory,        //应用路径
                HostName = hostName,                                    //客户端名称
                IPAddress = ipAddress,                                  //客户端IP地址
                ServiceName = message.ServiceName,                      //服务名称
                MethodName = message.MethodName,                        //方法名称
                EnableCache = config.EnableCache,                       //是否缓存
                CacheTime = message.CacheTime,                          //缓存时间
                TransactionId = Guid.NewGuid(),                         //Json字符串
                RespType = ResponseType.Json                            //数据类型
            };

            #endregion

            //给参数赋值
            reqMsg.Parameters["InvokeParameter"] = message.Parameters;

            //获取上下文
            using (var context = GetOperationContext(reqMsg))
            {
                //异步调用服务
                var resMsg = caller.Run(service, context, reqMsg).Message;

                //如果有异常，向外抛出
                if (resMsg.IsError) throw resMsg.Error;

                //返回数据
                return resMsg.Value as InvokeData;
            }
        }

        /// <summary>
        /// 获取上下文对象
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <returns></returns>
        private OperationContext GetOperationContext(RequestMessage reqMsg)
        {
            var caller = new AppCaller
            {
                AppPath = AppDomain.CurrentDomain.BaseDirectory,
                AppName = reqMsg.AppName,
                IPAddress = reqMsg.IPAddress,
                HostName = reqMsg.HostName,
                ServiceName = reqMsg.ServiceName,
                MethodName = reqMsg.MethodName,
                Parameters = reqMsg.Parameters.ToString(),
                CallTime = DateTime.Now
            };

            return new OperationContext
            {
                Container = container,
                Caller = caller
            };
        }
    }
}
