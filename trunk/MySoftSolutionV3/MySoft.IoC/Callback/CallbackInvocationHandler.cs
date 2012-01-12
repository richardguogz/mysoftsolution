﻿using System;
using System.Net.Sockets;
using MySoft.Communication.Scs.Communication;
using MySoft.Communication.Scs.Server;
using MySoft.IoC.Messages;

namespace MySoft.IoC
{
    /// <summary>
    /// 回调代理
    /// </summary>
    internal class CallbackInvocationHandler : IProxyInvocationHandler
    {
        private Type callType;
        private IScsServerClient client;
        private int timeout = 60;
        public CallbackInvocationHandler(Type callType, IScsServerClient client, int timeout)
        {
            this.callType = callType;
            this.client = client;
            this.timeout = timeout;
        }

        #region IProxyInvocationHandler 成员

        /// <summary>
        /// 响应消息
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object Invoke(object proxy, System.Reflection.MethodInfo method, object[] parameters)
        {
            if (client.CommunicationState == CommunicationStates.Connected)
            {
                var message = new CallbackMessage { ServiceType = callType, MethodName = method.ToString(), Parameters = parameters };

                //发送i回调消息
                client.SendMessage(new ScsCallbackMessage(message));

                //返回null
                return null;
            }
            else
            {
                throw new SocketException((int)SocketError.ConnectionAborted);
            }
        }

        #endregion
    }
}
