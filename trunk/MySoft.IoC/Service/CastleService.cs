using System;
using System.Net.Sockets;
using MySoft.Net.Server;
using MySoft.Net.Sockets;
using System.Net;

namespace MySoft.IoC
{
    /// <summary>
    /// Castle服务
    /// </summary>
    public class CastleService : IDisposable, ILogable, IErrorLogable
    {
        private IServiceContainer container;
        private CastleServiceConfiguration config;
        private SocketServerManager manager;

        /// <summary>
        /// 实例化CastleService
        /// </summary>
        /// <param name="config"></param>
        public CastleService(CastleServiceConfiguration config)
        {
            this.config = config;
            this.container = new SimpleServiceContainer();
            this.container.OnError += new ErrorLogEventHandler(container_OnError);
            this.container.OnLog += new LogEventHandler(container_OnLog);

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
        /// 获取服务的URI地址
        /// </summary>
        public string URI
        {
            get { return string.Format("{0}://{1}", manager.Server.Sock.ProtocolType, manager.Server.Sock.LocalEndPoint).ToLower(); }
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
            if (OnLog != null) OnLog(string.Format("User connection {0}！", socketAsync.AcceptSocket.RemoteEndPoint));
            else Console.WriteLine("User connection {0}！", socketAsync.AcceptSocket.RemoteEndPoint);
            return true;
        }

        void SocketServerManager_OnMessageOutput(object sender, LogOutEventArgs e)
        {
            if (OnLog != null) OnLog(string.Format("{0} ==> {1}", e.MessageType, e.Message));
            else Console.WriteLine("{0} <==> {1}", e.MessageType, e.Message);
        }

        void SocketServerManager_OnDisconnected(int error, SocketAsyncEventArgs socketAsync)
        {
            if (OnLog != null) OnLog(string.Format("User Disconnect {0}！", socketAsync.AcceptSocket.RemoteEndPoint));
            else Console.WriteLine("User Disconnect {0}！", socketAsync.AcceptSocket.RemoteEndPoint);
            socketAsync.UserToken = null;
            socketAsync.AcceptSocket.Close();
        }

        void SocketServerManager_OnBinaryInput(byte[] buffer, SocketAsyncEventArgs socketAsync)
        {
            try
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

                            //设置客户端IP
                            request.RequestAddress = socketAsync.AcceptSocket.RemoteEndPoint.ToString();

                            //获取返回的消息
                            ResponseMessage response = container.CallService(request);

                            if (requestObject != null)
                            {
                                //如果超时，则不返回数据
                                if (response.Expiration > DateTime.Now)
                                {
                                    //发送数据到服务端
                                    manager.Server.SendData(socketAsync.AcceptSocket, BufferFormat.FormatFCA(response));
                                }
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
            catch (Exception ex)
            {
                if (OnError != null) OnError(new IoCException(ex.Message, ex));
            }
        }

        #endregion
    }
}
