/*
 * 北风之神SOCKET框架(ZYSocket)
 *  Borey Socket Frame(ZYSocket)
 *  by luyikk@126.com
 *  Updated 2010-12-26 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using MySoft.Net.Sockets;
using MySoft.Logger;

namespace MySoft.Net.Server
{
    /// <summary>
    /// 连接的代理
    /// </summary>
    /// <param name="socketAsync"></param>
    public delegate bool ConnectionFilterEventHandler(SocketAsyncEventArgs socketAsync);

    /// <summary>
    /// 断开连接的代理
    /// </summary>
    /// <param name="error">错误代码</param>
    /// <param name="socketAsync"></param>
    public delegate void DisconnectionEventHandler(int error, SocketAsyncEventArgs socketAsync);

    /// <summary>
    /// 数据包输入代理
    /// </summary>
    /// <param name="buffer">输入包</param>
    /// <param name="socketAsync"></param>
    public delegate void BinaryInputEventHandler(byte[] buffer, SocketAsyncEventArgs socketAsync);

    /// <summary>
    /// ZYSOCKET框架 服务器端
    ///（通过6W个连接测试。理论上支持10W个连接，可谓.NET最强SOCKET模型）
    /// </summary>
    public class SocketServer : IDisposable
    {

        #region 释放
        /// <summary>
        /// 用来确定是否以释放
        /// </summary>
        private bool isDisposed;


        ~SocketServer()
        {
            this.Dispose(false);

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed || disposing)
            {
                try
                {
                    sock.Shutdown(SocketShutdown.Both);
                    sock.Close();

                    for (int i = 0; i < SocketAsynPool.Count; i++)
                    {
                        SocketAsyncEventArgs args = SocketAsynPool.Pop();

                        BuffManagers.FreeBuffer(args);
                    }
                }
                catch { }

                isDisposed = true;
            }
        }
        #endregion

        /// <summary>
        /// 数据包管理
        /// </summary>
        private BufferManager BuffManagers;

        /// <summary>
        /// Socket异步对象池
        /// </summary>
        private SocketAsyncEventArgsPool SocketAsynPool;

        /// <summary>
        /// SOCK对象
        /// </summary>
        private Socket sock;

        /// <summary>
        /// Socket对象
        /// </summary>
        public Socket Sock
        {
            get { return sock; }
        }

        /// <summary>
        /// 连接传入处理
        /// </summary>
        public event ConnectionFilterEventHandler OnConnectFilter;

        /// <summary>
        /// 异常错误通常是用户断开处理
        /// </summary>
        public event DisconnectionEventHandler OnDisconnected;

        /// <summary>
        /// 数据输入处理
        /// </summary>
        public event BinaryInputEventHandler OnBinaryInput;

        private System.Threading.AutoResetEvent[] reset;

        /// <summary>
        /// 是否关闭SOCKET Delay算法
        /// </summary>
        public bool NoDelay
        {
            get
            {
                return sock.NoDelay;
            }

            set
            {
                sock.NoDelay = value;
            }
        }

        /// <summary>
        /// SOCKET 的  ReceiveTimeout属性
        /// </summary>
        public int ReceiveTimeout
        {
            get
            {
                return sock.ReceiveTimeout;
            }
            set
            {
                sock.ReceiveTimeout = value;

            }
        }

        /// <summary>
        /// SOCKET 的 SendTimeout
        /// </summary>
        public int SendTimeout
        {
            get
            {
                return sock.SendTimeout;
            }

            set
            {
                sock.SendTimeout = value;
            }

        }

        /// <summary>
        /// 接收包大小
        /// </summary>
        private int MaxBufferSize;

        public int GetMaxBufferSize
        {
            get
            {
                return MaxBufferSize;
            }
        }

        /// <summary>
        /// 最大用户连接
        /// </summary>
        private int MaxConnectCount;

        /// <summary>
        /// 最大用户连接数
        /// </summary>
        public int GetMaxUserConnect
        {
            get
            {
                return MaxConnectCount;
            }
        }


        /// <summary>
        /// IP端点
        /// </summary>
        private IPEndPoint IPEndPoint;

        #region 消息输出

        /// <summary>
        /// 输出消息
        /// </summary>
        public event EventHandler<LogOutEventArgs> OnMessageOutput;

        /// <summary>
        /// 输出消息
        /// </summary>
        /// <param name="o"></param>
        /// <param name="type"></param>
        /// <param name="message"></param>
        protected void LogOutEvent(Object sender, LogType type, string message)
        {
            if (OnMessageOutput != null)
                OnMessageOutput.BeginInvoke(sender, new LogOutEventArgs(type, message), new AsyncCallback(CallBackEvent), OnMessageOutput);
        }
        /// <summary>
        /// 事件处理完的回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void CallBackEvent(IAsyncResult ar)
        {
            EventHandler<LogOutEventArgs> onMessageOutput = ar.AsyncState as EventHandler<LogOutEventArgs>;
            if (onMessageOutput != null)
                onMessageOutput.EndInvoke(ar);
        }
        #endregion

        /// <summary>
        /// 实例化SocketServer类
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <param name="port"></param>
        /// <param name="maxconnectcount"></param>
        /// <param name="maxbuffersize"></param>
        public SocketServer(IPAddress ipaddress, int port, int maxconnectcount, int maxbuffersize)
        {
            this.IPEndPoint = new IPEndPoint(ipaddress, port);
            this.MaxBufferSize = maxbuffersize;
            this.MaxConnectCount = maxconnectcount;


            this.reset = new System.Threading.AutoResetEvent[1];
            reset[0] = new System.Threading.AutoResetEvent(false);

            Run();
        }

        /// <summary>
        /// 实例化SocketServer类
        /// </summary>
        /// <param name="ipendpoint"></param>
        /// <param name="maxconnectcount"></param>
        /// <param name="maxbuffersize"></param>
        public SocketServer(IPEndPoint ipendpoint, int maxconnectcount, int maxbuffersize)
        {
            this.IPEndPoint = ipendpoint;
            this.MaxBufferSize = maxbuffersize;
            this.MaxConnectCount = maxconnectcount;


            this.reset = new System.Threading.AutoResetEvent[1];
            reset[0] = new System.Threading.AutoResetEvent(false);

            Run();
        }

        /// <summary>
        /// 实例化SocketServer类
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="maxconnectcount"></param>
        /// <param name="maxbuffersize"></param>
        public SocketServer(string host, int port, int maxconnectcount, int maxbuffersize)
        {
            this.IPEndPoint = GetIPEndPoint(host, port);
            this.MaxBufferSize = maxbuffersize;
            this.MaxConnectCount = maxconnectcount;

            this.reset = new System.Threading.AutoResetEvent[1];
            reset[0] = new System.Threading.AutoResetEvent(false);

            Run();
        }

        public SocketServer()
        {
            int port = SocketConfig.ReadInt32("Port");
            string host = SocketConfig.ReadString("Host");

            this.IPEndPoint = GetIPEndPoint(host, port);
            this.MaxBufferSize = SocketConfig.ReadInt32("MaxBufferSize");
            this.MaxConnectCount = SocketConfig.ReadInt32("MaxConnectCount");

            this.reset = new System.Threading.AutoResetEvent[1];
            reset[0] = new System.Threading.AutoResetEvent(false);

            Run();
        }

        private IPEndPoint GetIPEndPoint(string host, int port)
        {
            IPEndPoint myEnd = new IPEndPoint(IPAddress.Any, port);

            if (!host.Equals("any", StringComparison.CurrentCultureIgnoreCase))
            {
                if (!String.IsNullOrEmpty(host))
                {
                    IPHostEntry p = Dns.GetHostEntry(Dns.GetHostName());

                    foreach (IPAddress s in p.AddressList)
                    {
                        if (s.AddressFamily == AddressFamily.InterNetwork)
                        {
                            myEnd = new IPEndPoint(s, port);
                            break;
                        }
                    }

                }
                else
                {
                    try
                    {
                        myEnd = new IPEndPoint(IPAddress.Parse(host), port);
                    }
                    catch (FormatException)
                    {
                        IPHostEntry p = Dns.GetHostEntry(Dns.GetHostName());

                        foreach (IPAddress s in p.AddressList)
                        {
                            if (s.AddressFamily == AddressFamily.InterNetwork)
                            {
                                myEnd = new IPEndPoint(s, port);
                                break;
                            }
                        }
                    }

                }
            }

            return myEnd;
        }

        /// <summary>
        /// 启动
        /// </summary>
        private void Run()
        {
            if (isDisposed == true)
            {
                throw new ObjectDisposedException("SocketServer is Disposed！");
            }

            sock = new Socket(IPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);

            sock.Bind(IPEndPoint);
            sock.Listen(20);
            SendTimeout = 1000;
            ReceiveTimeout = 1000;

            int maxValue = int.MaxValue;
            if (MaxConnectCount * MaxBufferSize < int.MaxValue)
            {
                maxValue = MaxConnectCount * MaxBufferSize;
            }

            BuffManagers = new BufferManager(maxValue, MaxBufferSize);
            BuffManagers.Init();

            SocketAsynPool = new SocketAsyncEventArgsPool(MaxConnectCount);

            for (int i = 0; i < MaxConnectCount; i++)
            {
                SocketAsyncEventArgs socketasyn = new SocketAsyncEventArgs();
                socketasyn.Completed += new EventHandler<SocketAsyncEventArgs>(Asyn_Completed);
                SocketAsynPool.Push(socketasyn);
            }

            Accept();
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            reset[0].Set();

        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            reset[0].Reset();
        }

        void Accept()
        {
            if (SocketAsynPool.Count > 0)
            {
                SocketAsyncEventArgs sockasyn = SocketAsynPool.Pop();
                if (!Sock.AcceptAsync(sockasyn))
                {
                    BeginAccep(sockasyn);
                }
            }
            else
            {
                LogOutEvent(null, LogType.Error, "The MaxUserCount！");
            }
        }

        void BeginAccep(SocketAsyncEventArgs e)
        {
            try
            {
                if (e.SocketError == SocketError.Success)
                {

                    System.Threading.WaitHandle.WaitAll(reset);
                    reset[0].Set();

                    if (this.OnConnectFilter != null)
                    {
                        if (!this.OnConnectFilter(e))
                        {
                            LogOutEvent(null, LogType.Error, string.Format("The Socket Not Connect {0}！", e.AcceptSocket.RemoteEndPoint));
                            e.AcceptSocket = null;
                            SocketAsynPool.Push(e);

                            return;
                        }
                        else
                        {
                            //连接成功处理
                            LogOutEvent(null, LogType.Information, string.Format("The Socket Connect {0}！", e.AcceptSocket.RemoteEndPoint));
                        }
                    }

                    if (BuffManagers.SetBuffer(e))
                    {
                        if (!e.AcceptSocket.ReceiveAsync(e))
                        {
                            BeginReceive(e);
                        }

                    }

                }
                else
                {
                    e.AcceptSocket = null;
                    SocketAsynPool.Push(e);
                    LogOutEvent(null, LogType.Error, "Not Accep！");
                }
            }
            finally
            {
                Accept();
            }
        }

        void BeginReceive(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
            {
                byte[] buffer = new byte[e.BytesTransferred];

                Array.Copy(e.Buffer, e.Offset, buffer, 0, buffer.Length);

                if (this.OnBinaryInput != null)
                    this.OnBinaryInput(buffer, e);

                if (!e.AcceptSocket.ReceiveAsync(e))
                {
                    BeginReceive(e);
                }
            }
            else
            {
                string message = string.Format("User Disconnect {0}！", e.AcceptSocket.RemoteEndPoint);
                LogOutEvent(null, LogType.Error, message);

                if (OnDisconnected != null)
                {
                    OnDisconnected(-1, e);
                }

                e.AcceptSocket = null;
                BuffManagers.FreeBuffer(e);
                SocketAsynPool.Push(e);
                if (SocketAsynPool.Count == 1)
                {
                    Accept();
                }
            }

        }

        void Asyn_Completed(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Accept:
                    BeginAccep(e);
                    break;
                case SocketAsyncOperation.Receive:
                    BeginReceive(e);
                    break;

            }

        }

        /// <summary>
        /// 发送数据包
        /// </summary>
        /// <param name="sock"></param>
        /// <param name="buffer"></param>
        public void SendData(Socket sock, byte[] buffer)
        {
            if (sock != null && sock.Connected)
            {
                try
                {
                    sock.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, AsynCallBack, sock);
                }
                catch
                {
                }
            }
        }

        void AsynCallBack(IAsyncResult result)
        {
            try
            {
                Socket sock = result.AsyncState as Socket;

                if (sock != null)
                {
                    sock.EndSend(result);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 断开此SOCKET
        /// </summary>
        /// <param name="sock"></param>
        public void Disconnect(Socket socks)
        {
            try
            {
                if (sock != null)
                    socks.BeginDisconnect(false, AsynCallBackDisconnect, socks);
            }
            catch (ObjectDisposedException)
            {
            }

        }

        void AsynCallBackDisconnect(IAsyncResult result)
        {
            Socket sock = result.AsyncState as Socket;

            if (sock != null)
            {
                try
                {
                    sock.Shutdown(SocketShutdown.Both);
                    sock.EndDisconnect(result);
                }
                catch
                {
                }
            }
        }

    }

    /// <summary>
    /// 日志输出事件参数
    /// </summary>
    public class LogOutEventArgs : EventArgs
    {
        /// <summary>
        /// 消息类型
        /// </summary>     
        private LogType messageType;

        /// <summary>
        /// 消息类型
        /// </summary>  
        public LogType MessageType
        {
            get { return messageType; }
        }

        /// <summary>
        /// 消息
        /// </summary>
        private string message;

        public string Message
        {
            get { return message; }
        }

        public LogOutEventArgs(LogType messagetype, string message)
        {
            this.messageType = messagetype;
            this.message = message;
        }
    }
}
