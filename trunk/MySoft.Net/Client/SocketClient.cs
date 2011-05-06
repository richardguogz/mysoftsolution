﻿/*
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
    /// <param name="socket"></param>
    public delegate void ConnectionEventHandler(string message, bool connected, Socket socket);

    /// <summary>
    /// 接收事件
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="socket"></param>
    public delegate void ReceiveEventHandler(byte[] buffer, Socket socket);

    /// <summary>
    /// 退出事件
    /// </summary>
    /// <param name="message"></param>
    /// <param name="socket"></param>
    public delegate void DisconnectionEventHandler(string message, Socket socket);

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
        /// SOCKET对象
        /// </summary>
        public Socket Socket
        {
            get { return socket; }
        }

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

        #region 连接服务器

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

            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.RemoteEndPoint = myEnd;
            e.Completed += new EventHandler<SocketAsyncEventArgs>(e_Completed);
            if (!socket.ConnectAsync(e))
            {
                eCompleted(e);
            }
        }

        /// <summary>
        /// 连接到指定服务器
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool ConnectTo(string host, int port)
        {
            BeginConnectTo(host, port);

            wait.WaitOne();
            wait.Reset();

            return connected;
        }

        #endregion

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
                            OnConnected("连接服务器成功！", true, socket);

                        byte[] data = new byte[4096];
                        e.SetBuffer(data, 0, data.Length);  //设置数据包

                        if (!socket.ReceiveAsync(e)) //开始读取数据包
                            eCompleted(e);
                    }
                    else
                    {
                        connected = false;
                        wait.Set();
                        if (OnConnected != null)
                            OnConnected("连接服务器失败！", false, socket);
                    }
                    break;
                case SocketAsyncOperation.Receive:
                    if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
                    {
                        byte[] data = new byte[e.BytesTransferred];
                        Array.Copy(e.Buffer, 0, data, 0, data.Length);

                        byte[] dataLast = new byte[4096];
                        e.SetBuffer(dataLast, 0, dataLast.Length);

                        if (!socket.ReceiveAsync(e))
                            eCompleted(e);

                        if (OnReceived != null)
                            OnReceived(data, socket);

                    }
                    else
                    {
                        if (OnDisconnected != null)
                            OnDisconnected("与服务器断开连接！", socket);
                    }
                    break;
            }
        }
    }
}
