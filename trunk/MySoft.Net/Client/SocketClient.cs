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
    /// <param name="connected"></param>
    /// <param name="socketAsync"></param>
    public delegate void ConnectionEventHandler(string message, bool connected, SocketAsyncEventArgs socketAsync);

    /// <summary>
    /// 接收事件
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="socketAsync"></param>
    public delegate void ReceiveEventHandler(byte[] buffer, SocketAsyncEventArgs socketAsync);

    /// <summary>
    /// 退出事件
    /// </summary>
    /// <param name="message"></param>
    /// <param name="socketAsync"></param>
    public delegate void DisconnectionEventHandler(string message, SocketAsyncEventArgs socketAsync);

    /// <summary>
    /// ZYSOCKET 客户端
    /// （一个简单的异步SOCKET客户端，性能不错。支持.NET 3.0以上版本。适用于silverlight)
    /// </summary>
    public class SocketClient
    {
        /// <summary>
        /// SOCKET对象
        /// </summary>
        private Socket socket;

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

        /// <summary>
        /// 实例化Socket客户端
        /// </summary>
        public SocketClient()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        }

        private bool connected;

        /// <summary>
        /// 异步连接到指定的服务器
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public void BeginConnectTo(string host, int port)
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

            BeginConnectTo(myEnd);
        }

        /// <summary>
        /// 异步连接到指定的服务器
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <param name="port"></param>
        public void BeginConnectTo(IPAddress ipaddress, int port)
        {
            BeginConnectTo(new IPEndPoint(ipaddress, port));
        }

        /// <summary>
        /// 异步连接到指定的服务器
        /// </summary>
        /// <param name="ipendpoint"></param>
        public void BeginConnectTo(IPEndPoint ipendpoint)
        {

            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.RemoteEndPoint = ipendpoint;
            e.Completed += new EventHandler<SocketAsyncEventArgs>(e_Completed);
            if (!socket.ConnectAsync(e))
            {
                eCompleted(e);
            }
        }

        /// <summary>
        /// 异步发送数据包
        /// </summary>
        /// <param name="buffer"></param>
        public bool SendData(byte[] buffer)
        {
            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.SetBuffer(buffer, 0, buffer.Length);
            return socket.SendAsync(e);
        }

        /// <summary>
        /// 连接到指定服务器
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool ConnectTo(string host, int port)
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

            return ConnectTo(myEnd);
        }


        /// <summary>
        /// 连接到指定服务器
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool ConnectTo(IPAddress ipaddress, int port)
        {
            return ConnectTo(new IPEndPoint(ipaddress, port));
        }

        /// <summary>
        /// 连接到指定服务器
        /// </summary>
        /// <param name="ipendpoint"></param>
        /// <returns></returns>
        public bool ConnectTo(IPEndPoint ipendpoint)
        {

            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.RemoteEndPoint = ipendpoint;
            e.Completed += new EventHandler<SocketAsyncEventArgs>(e_Completed);
            if (!socket.ConnectAsync(e))
            {
                eCompleted(e);
            }

            wait.WaitOne();
            wait.Reset();

            return connected;
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
                        connected = true;
                        wait.Set();

                        if (OnConnected != null)
                            OnConnected("连接服务器成功！", true, e);

                        byte[] data = new byte[4098];
                        e.SetBuffer(data, 0, data.Length);  //设置数据包

                        if (!socket.ReceiveAsync(e)) //开始读取数据包
                            eCompleted(e);
                    }
                    else if (e.SocketError == SocketError.IsConnected)
                    {
                        connected = true;
                        wait.Set();

                        if (OnConnected != null)
                            OnConnected("连接服务器成功！", true, e);
                    }
                    else
                    {
                        connected = false;
                        wait.Set();
                        if (OnConnected != null)
                            OnConnected("连接服务器失败！", false, e);
                    }
                    break;

                case SocketAsyncOperation.Receive:
                    if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
                    {
                        byte[] data = new byte[e.BytesTransferred];
                        Array.Copy(e.Buffer, 0, data, 0, data.Length);

                        byte[] dataLast = new byte[4098];
                        e.SetBuffer(dataLast, 0, dataLast.Length);

                        if (!socket.ReceiveAsync(e))
                            eCompleted(e);

                        if (OnReceived != null)
                            OnReceived(data, e);

                    }
                    else
                    {
                        if (OnDisconnected != null)
                            OnDisconnected("与服务器断开连接！", e);
                    }
                    break;

            }
        }
    }
}
