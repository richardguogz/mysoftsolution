/*
 * 北风之神SOCKET框架(ZYSocket)
 *  Borey Socket Frame(ZYSocket)
 *  by luyikk@126.com
 *  Updated 2010-12-26 
 */
using System;
using System.Net;
using System.Net.Sockets;

namespace MySoft.Net.Client
{
    /// <summary>
    /// 连接事件
    /// </summary>
    /// <param name="message"></param>
    /// <param name="isConnect"></param>
    public delegate void ConnectionEventHandler(string message, bool connect);

    /// <summary>
    /// 接收事件
    /// </summary>
    /// <param name="Data"></param>
    public delegate void ReceiveEventHandler(byte[] buffer);

    /// <summary>
    /// 退出事件
    /// </summary>
    /// <param name="message"></param>
    public delegate void DisconnectionEventHandler(string message);

    /// <summary>
    /// ZYSOCKET 客户端
    /// （一个简单的异步SOCKET客户端，性能不错。支持.NET 3.0以上版本。适用于silverlight)
    /// </summary>
    public class SocketClient
    {
        /// <summary>
        /// SOCKET对象
        /// </summary>
        private Socket sock;

        /// <summary>
        /// 连接成功事件
        /// </summary>
        public event ConnectionEventHandler OnConnected;

        /// <summary>
        /// 数据包进入事件
        /// </summary>
        public event ReceiveEventHandler OnReceived;

        /// <summary>
        /// 出错或断开触发事件
        /// </summary>
        public event DisconnectionEventHandler OnDisconnected;

        private System.Threading.AutoResetEvent wait = new System.Threading.AutoResetEvent(false);

        public SocketClient()
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        }

        private bool connect;

        /// <summary>
        /// 异步连接到指定的服务器
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public void BeginConnectionTo(string host, int port)
        {
            IPEndPoint myEnd = null;

            #region ipformat
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

            #endregion

            BeginConnectionTo(myEnd);
        }

        /// <summary>
        /// 异步连接到指定的服务器
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        public void BeginConnectionTo(IPAddress ipAddress, int port)
        {
            BeginConnectionTo(new IPEndPoint(ipAddress, port));
        }

        /// <summary>
        /// 异步连接到指定的服务器
        /// </summary>
        /// <param name="ipEndPoint"></param>
        public void BeginConnectionTo(IPEndPoint ipEndPoint)
        {

            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.RemoteEndPoint = ipEndPoint;
            e.Completed += new EventHandler<SocketAsyncEventArgs>(e_Completed);
            if (!sock.ConnectAsync(e))
            {
                eCompleted(e);
            }
        }

        /// <summary>
        /// 异步发送数据包
        /// </summary>
        /// <param name="data"></param>
        public void BeginSendTo(byte[] data)
        {
            this.sock.BeginSend(data, 0, data.Length, SocketFlags.None, delegate(IAsyncResult Result)
            {
                this.sock.EndSend(Result);
            }, null);
        }

        /// <summary>
        /// 连接到指定服务器
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool ConnectionTo(string host, int port)
        {
            IPEndPoint myEnd = null;

            #region ipformat
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

            #endregion

            return ConnectionTo(myEnd);
        }


        /// <summary>
        /// 连接到指定服务器
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool ConnectionTo(IPAddress ipAddress, int port)
        {
            return ConnectionTo(new IPEndPoint(ipAddress, port));
        }

        /// <summary>
        /// 连接到指定服务器
        /// </summary>
        /// <param name="ipEndPoint"></param>
        /// <returns></returns>
        public bool ConnectionTo(IPEndPoint ipEndPoint)
        {

            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.RemoteEndPoint = ipEndPoint;
            e.Completed += new EventHandler<SocketAsyncEventArgs>(e_Completed);
            if (!sock.ConnectAsync(e))
            {
                eCompleted(e);
            }

            wait.WaitOne();
            wait.Reset();

            return connect;
        }

        void e_Completed(object sender, SocketAsyncEventArgs e)
        {
            eCompleted(e);
        }

        void eCompleted(SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Connect:

                    if (e.SocketError == SocketError.Success)
                    {

                        connect = true;
                        wait.Set();

                        if (OnConnected != null)
                            OnConnected("连接服务器成功！", true);

                        byte[] data = new byte[4098];
                        e.SetBuffer(data, 0, data.Length);  //设置数据包

                        if (!sock.ReceiveAsync(e)) //开始读取数据包
                            eCompleted(e);
                    }
                    else
                    {
                        connect = false;
                        wait.Set();
                        if (OnConnected != null)
                            OnConnected("连接服务器失败！", false);
                    }
                    break;

                case SocketAsyncOperation.Receive:
                    if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
                    {
                        byte[] data = new byte[e.BytesTransferred];
                        Array.Copy(e.Buffer, 0, data, 0, data.Length);

                        byte[] dataLast = new byte[4098];
                        e.SetBuffer(dataLast, 0, dataLast.Length);

                        if (!sock.ReceiveAsync(e))
                            eCompleted(e);

                        if (OnReceived != null)
                            OnReceived(data);

                    }
                    else
                    {
                        if (OnDisconnected != null)
                            OnDisconnected("与服务器断开连接！");
                    }
                    break;

            }
        }

        /// <summary>
        /// 发送数据包
        /// </summary>
        /// <param name="data"></param>
        public void SendTo(byte[] data)
        {
            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.SetBuffer(data, 0, data.Length);
            sock.SendAsync(e);
        }

    }
}
