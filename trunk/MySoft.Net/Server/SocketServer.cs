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

namespace MySoft.Net.Server
{

    /// <summary>
    /// 连接的代理
    /// </summary>
    /// <param name="socketAsync"></param>
    public delegate bool ConnectionEventHandler(SocketAsyncEventArgs socketAsync);

    /// <summary>
    /// 数据包输入代理
    /// </summary>
    /// <param name="data">输入包</param>
    /// <param name="socketAsync"></param>
    public delegate void BinaryInputEventHandler(byte[] data, SocketAsyncEventArgs socketAsync);

    /// <summary>
    /// 异常错误通常是用户断开的代理
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="socketAsync"></param>
    /// <param name="erorr">错误代码</param>
    public delegate void MessageInputEventHandler(string message, SocketAsyncEventArgs socketAsync, int erorr);

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
                catch
                {
                }

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
        public event ConnectionEventHandler OnConnected;

        /// <summary>
        /// 数据输入处理
        /// </summary>
        public event BinaryInputEventHandler OnBinaryInput;

        /// <summary>
        /// 异常错误通常是用户断开处理
        /// </summary>
        public event MessageInputEventHandler OnMessageInput;

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
        private int MaxConnectCout;

        /// <summary>
        /// 最大用户连接数
        /// </summary>
        public int GetMaxUserConnect
        {
            get
            {
                return MaxConnectCout;
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
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <param name="maxconnectcout"></param>
        /// <param name="maxbuffersize"></param>
        public SocketServer(IPAddress ipAddress, int port, int maxconnectcout, int maxbuffersize)
        {
            this.IPEndPoint = new IPEndPoint(ipAddress, port);
            this.MaxBufferSize = maxbuffersize;
            this.MaxConnectCout = maxconnectcout;


            this.reset = new System.Threading.AutoResetEvent[1];
            reset[0] = new System.Threading.AutoResetEvent(false);

            Run();
        }

        /// <summary>
        /// 实例化SocketServer类
        /// </summary>
        /// <param name="ipEndPoint"></param>
        /// <param name="maxconnectcout"></param>
        /// <param name="maxbuffersize"></param>
        public SocketServer(IPEndPoint ipEndPoint, int maxconnectcout, int maxbuffersize)
        {
            this.IPEndPoint = ipEndPoint;
            this.MaxBufferSize = maxbuffersize;
            this.MaxConnectCout = maxconnectcout;


            this.reset = new System.Threading.AutoResetEvent[1];
            reset[0] = new System.Threading.AutoResetEvent(false);

            Run();
        }

        /// <summary>
        /// 实例化SocketServer类
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="maxconnectcout"></param>
        /// <param name="maxbuffersize"></param>
        public SocketServer(string host, int port, int maxconnectcout, int maxbuffersize)
        {
            this.IPEndPoint = GetIPEndPoint(host, port);
            this.MaxBufferSize = maxbuffersize;
            this.MaxConnectCout = maxconnectcout;

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
            this.MaxConnectCout = SocketConfig.ReadInt32("MaxConnectCout");

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
                        if (!s.IsIPv6LinkLocal)
                            myEnd = new IPEndPoint(s, port);
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
                            if (!s.IsIPv6LinkLocal)
                                myEnd = new IPEndPoint(s, port);
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
                throw new ObjectDisposedException("SocketServer is Disposed");
            }

            sock = new Socket(IPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);

            sock.Bind(IPEndPoint);
            sock.Listen(20);
            SendTimeout = 1000;
            ReceiveTimeout = 1000;

            BuffManagers = new BufferManager(MaxConnectCout * MaxBufferSize, MaxBufferSize);
            BuffManagers.Init();

            SocketAsynPool = new SocketAsyncEventArgsPool(MaxConnectCout);

            for (int i = 0; i < MaxConnectCout; i++)
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
                LogOutEvent(null, LogType.Error, "The MaxUserCout！");
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

                    if (this.OnConnected != null)
                    {
                        if (!this.OnConnected(e))
                        {
                            LogOutEvent(null, LogType.Error, string.Format("The Socket Not Connect {0}！", e.AcceptSocket.RemoteEndPoint));
                            e.AcceptSocket = null;
                            SocketAsynPool.Push(e);

                            return;
                        }
                        else
                        {
                            //连接成功处理
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
                byte[] data = new byte[e.BytesTransferred];

                Array.Copy(e.Buffer, e.Offset, data, 0, data.Length);

                if (this.OnBinaryInput != null)
                    this.OnBinaryInput(data, e);

                if (!e.AcceptSocket.ReceiveAsync(e))
                {
                    BeginReceive(e);
                }
            }
            else
            {
                string message = string.Format("User Disconnect :{0}！", e.AcceptSocket.RemoteEndPoint.ToString());

                LogOutEvent(null, LogType.Error, message);

                if (OnMessageInput != null)
                {
                    OnMessageInput(message, e, 0);
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
        /// <param name="data"></param>
        public void SendData(Socket sock, byte[] data)
        {
            if (sock != null && sock.Connected)
                sock.BeginSend(data, 0, data.Length, SocketFlags.None, AsynCallBack, sock);

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
                sock.Shutdown(SocketShutdown.Both);
                sock.EndDisconnect(result);
            }
        }

    }

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 错误
        /// </summary>
        Error,
        /// <summary>
        /// 成功
        /// </summary>
        Success
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
