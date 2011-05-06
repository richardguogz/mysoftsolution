using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using Castle.Windsor;
using MySoft.Net.Client;
using System.Net.Sockets;
using MySoft.Net.Sockets;
using System.Collections;
using MySoft.Threading;
using MySoft.Logger;

namespace MySoft.IoC
{
    internal class ServiceProxy : IServiceProxy
    {
        private Dictionary<Guid, ResponseMessage> responses = new Dictionary<Guid, ResponseMessage>();
        private SocketClientConfiguration config;
        private ServiceRequestPool<ResponseMessage> requestPool;
        private string displayName;
        private SmartThreadPool pool;

        public ServiceProxy(SocketClientConfiguration config, string displayName)
        {
            this.config = config;
            this.displayName = displayName;

            #region socket通讯

            //实例化线程池
            pool = new SmartThreadPool(30 * 1000, 100, 10);

            //实例化服务池
            requestPool = new ServiceRequestPool<ResponseMessage>(config.Pools);
            for (int i = 0; i < config.Pools; i++)
            {
                var request = new ServiceRequest<ResponseMessage>(config.IP, config.Port);
                request.SendCallback += new SendMessageEventHandler<ResponseMessage>(client_SendMessage);

                //请求端入栈
                requestPool.Push(request);
            }

            #endregion
        }

        void client_SendMessage(ServiceRequestEventArgs<ResponseMessage> message)
        {
            //如果未过期，则加入队列中
            if (message.Response.Expiration >= DateTime.Now)
            {
                lock (responses)
                {
                    var response = message.Response;
                    responses[response.TransactionId] = response;
                }
            }
        }

        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <param name="logtime"></param>
        /// <returns></returns>
        public ResponseMessage CallMethod(RequestMessage reqMsg, int logtime)
        {
            //如果池为空，则检测
            if (requestPool.Count == 0)
            {
                CheckPool(reqMsg.Timeout);
            }

            var request = requestPool.Pop();
            if (request == null) return null;

            try
            {
                //如果连接断开，直接抛出异常
                if (!request.Connected)
                {
                    throw new IoCException(string.Format("Can't connect to server ({0}:{1})！service: {2}", config.IP, config.Port, displayName));
                }

                //发送数据包到服务端
                bool isSend = request.Send(BufferFormat.FormatFCA(reqMsg));

                if (isSend)
                {
                    //开始计时
                    int t1 = System.Environment.TickCount;

                    //获取消息
                    ResponseMessage resMsg = GetResponse(reqMsg, t1);

                    if (resMsg == null)
                    {
                        throw new IoCException(string.Format("【{4}】Call ({0}:{1}) remote service ({2},{3}) failure. result is empty！", config.IP, config.Port, reqMsg.ServiceName, reqMsg.SubServiceName, reqMsg.TransactionId));
                    }

                    //如果发生了异常，则抛出异常
                    if (resMsg.Exception != null)
                    {
                        throw resMsg.Exception;
                    }

                    int t2 = System.Environment.TickCount - t1;

                    //如果时间超过预定，则输出日志
                    if (t2 > logtime)
                    {
                        //SerializationManager.Serialize(retMsg)
                        if (OnLog != null) OnLog(string.Format("【{7}】Call ({0}:{1}) remote service ({2},{3}). ==> {4} {5} <==> {6}", config.IP, config.Port, resMsg.ServiceName, resMsg.SubServiceName, resMsg.Parameters,
                            "Spent time: (" + t2.ToString() + ") ms.", resMsg.Message, resMsg.TransactionId), LogType.Warning);
                    }

                    return resMsg;
                }
                else
                {
                    throw new IoCException(string.Format("Send data to ({0}:{1}) failure！", config.IP, config.Port));
                }
            }
            finally
            {
                //将SocketRequest入栈
                requestPool.Push(request);
            }
        }

        private ResponseMessage GetResponse(RequestMessage reqMsg, int beginTick)
        {
            //启动线程来处理数据
            IWorkItemResult<ResponseMessage> wir = pool.QueueWorkItem(state =>
            {
                ResponseMessage resMsg = null;
                RequestMessage request = state as RequestMessage;

                while (true)
                {
                    resMsg = GetData<ResponseMessage>(responses, request.TransactionId);

                    //如果有数据返回，则响应
                    if (resMsg != null) break;

                    //防止cpu使用率过高
                    Thread.Sleep(1);
                }

                return resMsg;

            }, reqMsg);

            if (!pool.WaitForIdle(reqMsg.Timeout))
            {
                if (!wir.IsCompleted) wir.Cancel(true);
                int timeout = System.Environment.TickCount - beginTick;
                throw new IoCException(string.Format("【{5}】Call ({0}:{1}) remote service ({2},{3}) failure. timeout ({4} ms)！", config.IP, config.Port, reqMsg.ServiceName, reqMsg.SubServiceName, timeout, reqMsg.TransactionId));
            }

            //从线程获取返回信息，超时等待
            return wir.GetResult();
        }

        private void CheckPool(int timeout)
        {
            //启动线程来处理数据
            IWorkItemResult wir = pool.QueueWorkItem(() =>
            {
                while (requestPool.Count == 0)
                {
                    //防止cpu使用率过高
                    Thread.Sleep(1);
                }
            });

            //等待1秒，如果没有池可用，则返回
            if (!pool.WaitForIdle(timeout))
            {
                if (!wir.IsCompleted) wir.Cancel(true);
                throw new IoCException("Socket pool is empty！");
            }
        }

        #region ILogable Members

        public event LogEventHandler OnLog;

        #endregion

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="map"></param>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        private TResult GetData<TResult>(IDictionary map, Guid transactionId)
        {
            if (map.Contains(transactionId))
            {
                lock (map)
                {
                    if (map.Contains(transactionId))
                    {
                        object retObj = map[transactionId];
                        map.Remove(transactionId);
                        return (TResult)retObj;
                    }
                }
            }

            return default(TResult);
        }
    }
}