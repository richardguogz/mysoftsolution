using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using MySoft.IoC.Configuration;
using MySoft.Logger;
using MySoft.Threading;

namespace MySoft.IoC
{
    /// <summary>
    /// 服务代理
    /// </summary>
    public class ProxyService : IService
    {
        private Dictionary<Guid, ResponseMessage> responses = new Dictionary<Guid, ResponseMessage>();
        private ILog logger;
        private RemoteNode node;
        private ServiceMessagePool reqPool;
        private SmartThreadPool pool;

        public ProxyService(ILog logger, RemoteNode node)
        {
            this.logger = logger;
            this.node = node;

            #region socket通讯

            //实例化线程池
            pool = new SmartThreadPool(30 * 1000, 500, 10);

            //实例化服务池
            reqPool = new ServiceMessagePool(node.MaxPool);
            for (int i = 0; i < node.MaxPool; i++)
            {
                var request = new ServiceMessage(node);
                request.SendCallback += new ServiceMessageEventHandler(client_SendMessage);

                //请求端入栈
                reqPool.Push(request);
            }

            #endregion
        }

        void client_SendMessage(object sender, ServiceMessageEventArgs message)
        {
            var response = message.Result;

            //如果未过期，则加入队列中
            if (response.Expiration >= DateTime.Now)
            {
                lock (responses)
                {
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
        public ResponseMessage CallService(RequestMessage reqMsg, double logtime)
        {
            //如果池为空
            if (reqPool.Count == 0)
            {
                throw new Exception("Service pool is empty！");
            }

            //从池中弹出一个可用请求
            var request = reqPool.Pop();

            try
            {
                //发送数据包到服务端
                bool isSend = request.Send(reqMsg, (int)(reqMsg.Timeout * 1000));

                if (isSend)
                {
                    //开始计时
                    Stopwatch watch = Stopwatch.StartNew();

                    //获取消息
                    var resMsg = GetResponse(reqMsg, watch);

                    //如果数据不为空
                    if (resMsg.Data != null)
                    {
                        watch.Stop();

                        //如果时间超过预定，则输出日志
                        if (watch.ElapsedMilliseconds > logtime * 1000)
                        {
                            //SerializationManager.Serialize(retMsg)
                            string log = string.Format("【{7}】Call ({0}:{1}) remote service ({2},{3}). {5}\r\nMessage ==> {6}\r\nParameters ==> {4}", node.IP, node.Port, resMsg.ServiceName, resMsg.SubServiceName, resMsg.Parameters.SerializedData, "Spent time: (" + watch.ElapsedMilliseconds + ") ms.", resMsg.Message, resMsg.TransactionId);
                            log = string.Format("Elapsed time more than {0} ms, {1}", logtime * 1000, log);
                            logger.WriteLog(log, LogType.Warning);
                        }
                    }

                    return resMsg;
                }
                else
                {
                    throw new IoCException(string.Format("Send data to ({0}:{1}) failure！", node.IP, node.Port))
                    {
                        ExceptionHeader = string.Format("Application \"{0}\" occurs error. ==> Comes from {1}({2}).", reqMsg.AppName, reqMsg.HostName, reqMsg.IPAddress)
                    };
                }
            }
            finally
            {
                //将SocketRequest入栈
                reqPool.Push(request);
            }
        }

        private ResponseMessage GetResponse(RequestMessage reqMsg, Stopwatch watch)
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

            if (!pool.WaitForIdle((int)(reqMsg.Timeout * 1000)))
            {
                if (!wir.IsCompleted) wir.Cancel(true);
                watch.Stop();

                throw new IoCException(string.Format("【{5}】Call ({0}:{1}) remote service ({2},{3}) failure. timeout ({4} ms)！", node.IP, node.Port, reqMsg.ServiceName, reqMsg.SubServiceName, watch.ElapsedMilliseconds, reqMsg.TransactionId))
                {
                    ExceptionHeader = string.Format("Application \"{0}\" occurs error. ==> Comes from {1}({2}).", reqMsg.AppName, reqMsg.HostName, reqMsg.IPAddress)
                };
            }

            //从线程获取返回信息，超时等待
            return wir.GetResult();
        }

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

        #region IService 成员

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName
        {
            get { return typeof(ProxyService).FullName; }
        }

        #endregion
    }
}