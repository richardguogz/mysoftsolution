﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using MySoft.IoC.Configuration;
using MySoft.Logger;
using MySoft.Net.Server;
using MySoft.Net.Sockets;
using System.Threading;

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
        private ServerStatus status;

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

            this.container = new SimpleServiceContainer(config.LogTimeout, hashTypes);
            this.container.OnError += new ErrorLogEventHandler(container_OnError);
            this.container.OnLog += new LogEventHandler(container_OnLog);
            this.clients = new List<EndPoint>();
            this.status = new ServerStatus();

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
                    status.TotalSeconds++;

                    //每秒处理一次
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
            get { return string.Format("{0}://{1}/", manager.Server.Sock.ProtocolType, manager.Server.Sock.LocalEndPoint).ToLower(); }
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
            if (OnLog != null) OnLog(log, type);
        }

        void container_OnError(Exception exception)
        {
            if (OnError != null) OnError(exception);
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
            else Console.WriteLine("User connection {0}！", socketAsync.AcceptSocket.RemoteEndPoint);

            //将地址加入到列表中
            clients.Add(socketAsync.AcceptSocket.RemoteEndPoint);

            return true;
        }

        void SocketServerManager_OnMessageOutput(object sender, LogOutEventArgs e)
        {
            if (OnLog != null) OnLog(string.Format("{0} ==> {1}", e.MessageType, e.Message), e.MessageType);
            else Console.WriteLine("{0} <==> {1}", e.MessageType, e.Message);
        }

        void SocketServerManager_OnDisconnected(int error, SocketAsyncEventArgs socketAsync)
        {
            if (OnError != null) OnLog(string.Format("User Disconnect {0}！", socketAsync.AcceptSocket.RemoteEndPoint), LogType.Error);
            else Console.WriteLine("User Disconnect {0}！", socketAsync.AcceptSocket.RemoteEndPoint);
            socketAsync.UserToken = null;

            //将地址从列表中移除
            clients.Remove(socketAsync.AcceptSocket.RemoteEndPoint);

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

            int t1 = Environment.TickCount;
            try
            {
                //处理请求数
                status.RequestCount++;

                //获取返回的消息
                response = container.CallService(request);
            }
            catch (Exception ex)
            {
                //处理错误数
                status.ErrorCount++;

                if (OnError != null) OnError(new IoCException(ex.Message, ex));
            }
            finally
            {
                int t2 = Environment.TickCount - t1;

                //处理时间
                status.ElapsedTime += t2;
            }

            if (response != null)
            {
                byte[] data = BufferFormat.FormatFCA(response);

                //处理流量
                status.DataFlow += data.Length;

                //发送数据到服务端
                manager.Server.SendData(socketAsync.AcceptSocket, data);
            }
        }

        #endregion

        #region IStatusService 成员

        /// <summary>
        /// 服务状态信息
        /// </summary>
        /// <returns></returns>
        public ServerStatus GetServerStatus()
        {
            return status;
        }

        /// <summary>
        /// 获取连接终结点信息
        /// </summary>
        /// <returns></returns>
        public IList<EndPoint> GetEndPoints()
        {
            return clients;
        }

        #endregion
    }
}
