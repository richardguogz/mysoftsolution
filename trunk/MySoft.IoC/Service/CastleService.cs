using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using MySoft.IoC.Configuration;
using MySoft.Logger;
using MySoft.Net.Server;
using MySoft.Net.Sockets;
using System.Threading;
using System.Linq;
using System.Diagnostics;

namespace MySoft.IoC
{
    /// <summary>
    /// Castle服务
    /// </summary>
    public class CastleService : IStatusService, IDisposable, ILogable, IErrorLogable
    {
        private IServiceContainer container;
        private CastleServiceConfiguration config;
        private SocketServerManager manager;
        private IList<EndPoint> clients;
        private IList<TimeStatus> statuslist;
        private SecondStatus status;
        private HighestStatus highest;

        /// <summary>
        /// 服务容器
        /// </summary>
        public IServiceContainer Container
        {
            get { return container; }
        }

        /// <summary>
        /// 实例化CastleService
        /// </summary>
        /// <param name="config"></param>
        public CastleService(CastleServiceConfiguration config)
        {
            this.config = config;

            //注入内部的服务
            Hashtable hashTypes = new Hashtable();
            hashTypes[typeof(IStatusService)] = this;

            this.container = new SimpleServiceContainer(hashTypes);
            this.container.OnError += new ErrorLogEventHandler(container_OnError);
            this.container.OnLog += new LogEventHandler(container_OnLog);
            this.clients = new List<EndPoint>();
            this.statuslist = new List<TimeStatus>();
            this.highest = new HighestStatus();
            this.status = new SecondStatus();

            //服务器配置
            SocketServerConfiguration ssc = new SocketServerConfiguration
            {
                Host = config.Host,
                Port = config.Port,
                MaxConnectCount = config.MaxConnect,
                MaxBufferSize = config.MaxBuffer
            };

            //实例化Socket服务
            manager = new SocketServerManager(ssc);
            manager.OnConnectFilter += new ConnectionFilterEventHandler(SocketServerManager_OnConnectFilter);
            manager.OnDisconnected += new DisconnectionEventHandler(SocketServerManager_OnDisconnected);
            manager.OnBinaryInput += new BinaryInputEventHandler(SocketServerManager_OnBinaryInput);
            manager.OnMessageOutput += new EventHandler<LogOutEventArgs>(SocketServerManager_OnMessageOutput);

            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    //计算时间
                    if (status.RequestCount > 0)
                    {
                        TimeStatus tmpStatus = new TimeStatus
                        {
                            CounterTime = DateTime.Now,
                            RequestCount = status.RequestCount,
                            SuccessCount = status.SuccessCount,
                            ErrorCount = status.ErrorCount,
                            ElapsedTime = status.ElapsedTime,
                            DataFlow = status.DataFlow
                        };

                        //重新实例化状态
                        status = new SecondStatus();

                        //处理最高值 
                        #region 处理最高值

                        //流量
                        if (tmpStatus.DataFlow > highest.DataFlow)
                        {
                            highest.DataFlow = tmpStatus.DataFlow;
                            highest.DataFlowCounterTime = tmpStatus.CounterTime;
                        }

                        //请求
                        if (tmpStatus.RequestCount > highest.RequestCount)
                        {
                            highest.RequestCount = tmpStatus.RequestCount;
                            highest.RequestCountCounterTime = tmpStatus.CounterTime;
                        }

                        //成功
                        if (tmpStatus.SuccessCount > highest.SuccessCount)
                        {
                            highest.SuccessCount = tmpStatus.SuccessCount;
                            highest.SuccessCountCounterTime = tmpStatus.CounterTime;
                        }

                        //失败
                        if (tmpStatus.ErrorCount > highest.ErrorCount)
                        {
                            highest.ErrorCount = tmpStatus.ErrorCount;
                            highest.ErrorCountCounterTime = tmpStatus.CounterTime;
                        }

                        //耗时
                        if (tmpStatus.ElapsedTime > highest.ElapsedTime)
                        {
                            highest.ElapsedTime = tmpStatus.ElapsedTime;
                            highest.ElapsedTimeCounterTime = tmpStatus.CounterTime;
                        }

                        #endregion

                        //将状态添加到列表中
                        lock (statuslist)
                        {
                            if (statuslist.Count >= config.Records)
                            {
                                //移除第一条
                                statuslist.RemoveAt(0);
                            }

                            statuslist.Add(tmpStatus);
                        }
                    }

                    //每1秒处理一次
                    Thread.Sleep(1000);
                }
            });

            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            this.Stop();
        }

        #region 启动停止服务

        /// <summary>
        /// 启用服务
        /// </summary>
        public void Start()
        {
            manager.Server.Start();
        }

        /// <summary>
        /// 获取服务的ServerUrl地址
        /// </summary>
        public string ServerUrl
        {
            get
            {
                return string.Format("{0}://{1}/", manager.Server.Sock.ProtocolType, manager.Server.Sock.LocalEndPoint).ToLower();
            }
        }

        /// <summary>
        /// 最大连接数
        /// </summary>
        public int MaxConnect
        {
            get
            {
                return config.MaxConnect;
            }
        }

        /// <summary>
        /// 最大缓冲区
        /// </summary>
        public int MaxBuffer
        {
            get
            {
                return config.MaxConnect * config.MaxBuffer;
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            manager.Server.Stop();
        }

        #endregion

        void container_OnLog(string log, LogType type)
        {
            try { if (OnLog != null) OnLog(log, type); }
            catch { }
        }

        void container_OnError(Exception exception)
        {
            try { if (OnError != null) OnError(exception); }
            catch { }
        }

        #region ILogable Members

        /// <summary>
        /// OnLog event.
        /// </summary>
        public event LogEventHandler OnLog;

        #endregion

        #region IErrorLogable Members

        /// <summary>
        /// OnError event.
        /// </summary>
        public event ErrorLogEventHandler OnError;

        #endregion

        #region 侦听事件

        bool SocketServerManager_OnConnectFilter(SocketAsyncEventArgs socketAsync)
        {
            if (OnLog != null) OnLog(string.Format("User connection {0}！", socketAsync.AcceptSocket.RemoteEndPoint), LogType.Information);

            //将地址加入到列表中
            lock (clients)
            {
                clients.Add(socketAsync.AcceptSocket.RemoteEndPoint);
            }

            return true;
        }

        void SocketServerManager_OnMessageOutput(object sender, LogOutEventArgs e)
        {
            if (OnLog != null) OnLog(e.Message, e.MessageType);
        }

        void SocketServerManager_OnDisconnected(int error, SocketAsyncEventArgs socketAsync)
        {
            if (OnError != null) OnLog(string.Format("User Disconnect {0}！", socketAsync.AcceptSocket.RemoteEndPoint), LogType.Error);
            socketAsync.UserToken = null;

            //将地址从列表中移除
            lock (clients)
            {
                clients.Remove(socketAsync.AcceptSocket.RemoteEndPoint);
            }

            socketAsync.AcceptSocket.Close();
        }

        void SocketServerManager_OnBinaryInput(byte[] buffer, SocketAsyncEventArgs socketAsync)
        {
            BufferRead read = new BufferRead(buffer);

            int length;
            int cmd;

            if (read.ReadInt32(out length) && read.ReadInt32(out cmd) && length == read.Length)
            {
                if (cmd == -10000)//请求结果信息
                {
                    object requestObject;
                    if (read.ReadObject(out requestObject))
                    {
                        if (requestObject != null)
                        {
                            RequestMessage request = requestObject as RequestMessage;

                            //设置客户端IP
                            request.RequestAddress = socketAsync.AcceptSocket.RemoteEndPoint.ToString();

                            //发送响应信息
                            GetSendResponse(socketAsync, request);
                        }
                    }
                }
                else //现在还没登入 如果有其他命令的请求那么 断开连接
                {
                    manager.Server.Disconnect(socketAsync.AcceptSocket);
                }
            }
            else //无法读取数据包 断开连接
            {
                manager.Server.Disconnect(socketAsync.AcceptSocket);
            }
        }

        /// <summary>
        /// 获取响应信息并发送
        /// </summary>
        /// <param name="socketAsync"></param>
        /// <param name="request"></param>
        private void GetSendResponse(SocketAsyncEventArgs socketAsync, RequestMessage request)
        {
            ResponseMessage response = null;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                //处理请求数
                if (IsServiceCounter(request)) status.RequestCount++;

                //获取返回的消息
                response = container.CallService(request, config.LogTime);

                //处理错误数
                if (IsServiceCounter(request))
                {
                    if (response.Exception == null)
                        status.SuccessCount++;
                    else
                        status.ErrorCount++;
                }

                byte[] data = BufferFormat.FormatFCA(response);

                //计算流量
                if (IsServiceCounter(request)) status.DataFlow += data.Length;

                //发送数据到服务端
                manager.Server.SendData(socketAsync.AcceptSocket, data);
            }
            catch (Exception ex)
            {
                container_OnError(ex);
            }
            finally
            {
                watch.Stop();

                //处理时间
                if (IsServiceCounter(request)) status.ElapsedTime += watch.ElapsedMilliseconds;
            }
        }

        /// <summary>
        /// 判断是否需要计数
        /// </summary>
        /// <param name="request"></param>
        private bool IsServiceCounter(RequestMessage request)
        {
            if (request == null) return false;
            if (request.ServiceName == typeof(IStatusService).FullName) return false;

            return true;
        }

        #endregion

        #region IStatusService 成员

        /// <summary>
        /// 清除所有服务器状态
        /// </summary>
        public void ClearStatus()
        {
            lock (statuslist)
            {
                statuslist.Clear();
                highest = new HighestStatus();
            }
        }

        /// <summary>
        /// 服务状态信息
        /// </summary>
        /// <returns></returns>
        public ServerStatus GetServerStatus()
        {
            ServerStatus status = new ServerStatus
            {
                Highest = GetHighestStatus(),
                Latest = GetLatestStatus(),
                Summary = GetSummaryStatus()
            };

            return status;
        }

        /// <summary>
        /// 获取最后一次服务状态
        /// </summary>
        /// <returns></returns>
        public TimeStatus GetLatestStatus()
        {
            return statuslist.LastOrDefault();
        }

        /// <summary>
        /// 获取最高状态信息
        /// </summary>
        /// <returns></returns>
        public HighestStatus GetHighestStatus()
        {
            return highest;
        }

        /// <summary>
        /// 汇总状态信息
        /// </summary>
        /// <returns></returns>
        public SummaryStatus GetSummaryStatus()
        {
            //统计状态信息
            SummaryStatus status = new SummaryStatus
            {
                RunningSeconds = statuslist.Count,
                RequestCount = statuslist.Sum(p => p.RequestCount),
                SuccessCount = statuslist.Sum(p => p.SuccessCount),
                ErrorCount = statuslist.Sum(p => p.ErrorCount),
                ElapsedTime = statuslist.Sum(p => p.ElapsedTime),
                DataFlow = statuslist.Sum(p => p.DataFlow),
            };

            return status;
        }

        /// <summary>
        /// 获取服务状态列表
        /// </summary>
        /// <returns></returns>
        public IList<TimeStatus> GetTimeStatusList()
        {
            return statuslist;
        }

        /// <summary>
        /// 获取连接客户信息
        /// </summary>
        /// <returns></returns>
        public IList<ConnectInfo> GetConnectInfoList()
        {
            lock (clients)
            {
                var dict = clients.ToLookup(p => p.ToString().Split(':')[0]);
                IList<ConnectInfo> list = new List<ConnectInfo>();
                foreach (var item in dict)
                {
                    list.Add(new ConnectInfo { IP = item.Key, Count = item.Count() });
                }

                return list;
            }
        }

        #endregion
    }
}
