﻿using System;
using System.Net.Sockets;
using MySoft.Net.Server;
using MySoft.Net.Sockets;

namespace MySoft.IoC
{
    /// <summary>
    /// Castle服务
    /// </summary>
    public class CastleService : IDisposable, ILogable, IErrorLogable
    {
        private IServiceContainer container;
        private CastleFactoryConfiguration config;
        private SocketServerManager manager;

        /// <summary>
        /// 实例化CastleService
        /// </summary>
        /// <param name="config"></param>
        public CastleService(CastleFactoryConfiguration config)
        {
            this.config = config;
            this.container = new SimpleServiceContainer();
            this.container.OnError += new ErrorLogEventHandler(container_OnError);
            this.container.OnLog += new LogEventHandler(container_OnLog);

            SocketServerConfiguration ssc = new SocketServerConfiguration
            {
                Host = config.Server,
                Port = config.Port,
                MaxConnectCount = 10000,
                MaxBufferSize = 10000
            };

            //实例化Socket服务
            manager = new SocketServerManager(ssc);
            manager.OnConnectFilter += new ConnectionFilterEventHandler(SocketServerManager_OnConnectFilter);
            manager.OnBinaryInput += new BinaryInputEventHandler(SocketServerManager_OnBinaryInput);
            manager.OnMessageInput += new MessageInputEventHandler(SocketServerManager_OnMessageInput);
            manager.OnMessageOutput += new EventHandler<LogOutEventArgs>(SocketServerManager_OnMessageOutput);
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
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            manager.Server.Stop();
        }

        #endregion

        void container_OnLog(string log)
        {
            if (OnLog != null) OnLog(log);
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
            Console.WriteLine("User connection {0}！", socketAsync.AcceptSocket.RemoteEndPoint);
            return true;
        }

        void SocketServerManager_OnMessageOutput(object sender, LogOutEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        void SocketServerManager_OnMessageInput(string message, int error, SocketAsyncEventArgs socketAsync)
        {
            Console.WriteLine(message);
            socketAsync.UserToken = null;
            socketAsync.AcceptSocket.Close();
        }

        void SocketServerManager_OnBinaryInput(byte[] buffer, SocketAsyncEventArgs socketAsync)
        {
            BufferRead read = new BufferRead(buffer);

            int length;
            int cmd;

            if (read.ReadInt32(out length) && read.ReadInt32(out cmd) && length == read.Length)
            {
                if (cmd == -10000)//自定义的数据包，输入请求
                {
                    object requestObject;
                    if (read.ReadObject(out requestObject))
                    {
                        RequestMessage request = requestObject as RequestMessage;

                        //获取返回的消息
                        ResponseMessage response = container.CallService(request);

                        if (requestObject != null)
                        {
                            //发送数据到服务端
                            manager.Server.SendData(socketAsync.AcceptSocket, BufferFormat.FormatFCA(response));
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

        #endregion
    }
}
