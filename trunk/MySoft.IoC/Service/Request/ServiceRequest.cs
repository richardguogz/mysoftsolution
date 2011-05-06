using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MySoft.Net.Client;
using System.Net.Sockets;
using MySoft.Net.Sockets;

namespace MySoft.IoC
{
    /// <summary>
    /// 服务请求
    /// </summary>
    public class ServiceRequest<T>
    {
        public event SendMessageEventHandler<T> SendCallback;

        private SocketClientManager manager;
        private bool connected = false;
        private string ip;
        private int port;

        public ServiceRequest(string ip, int port)
        {
            this.ip = ip;
            this.port = port;

            manager = new SocketClientManager();
            manager.OnConnected += new ConnectionEventHandler(SocketClientManager_OnConnected);
            manager.OnDisconnected += new DisconnectionEventHandler(SocketClientManager_OnDisconnected);
            manager.OnReceived += new ReceiveEventHandler(SocketClientManager_OnReceived);

            //开始连接到服务器
            StartConnectThread();
        }

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool Connected
        {
            get
            {
                return connected;
            }
        }

        /// <summary>
        /// 返回通讯的Socket对象
        /// </summary>
        public Socket Socket
        {
            get
            {
                return manager.Client.Socket;
            }
        }

        /// <summary>
        /// 发送数据包
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public bool Send(byte[] buffer)
        {
            return manager.Client.SendData(buffer);
        }

        /// <summary>
        /// 连接到服务器
        /// </summary>
        private void StartConnectThread()
        {
            //启动检测线程
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    if (!connected)
                    {
                        //尝试连接到服务器
                        manager.Client.BeginConnectTo(ip, port);
                    }

                    //等待5秒
                    Thread.Sleep(5000);
                }
            });

            thread.IsBackground = true;
            thread.Start();
        }

        #region Socket消息委托

        void SocketClientManager_OnReceived(byte[] buffer, Socket socket)
        {
            BufferRead read = new BufferRead(buffer);

            int length;
            int cmd;

            if (read.ReadInt32(out length) && read.ReadInt32(out cmd) && length == read.Length)
            {
                if (cmd == 10000) //返回数据包
                {
                    object responseObject;
                    if (read.ReadObject(out responseObject))
                    {
                        T result = (T)responseObject;
                        if (SendCallback != null)
                        {
                            var args = new ServiceRequestEventArgs<T>
                            {
                                Response = result,
                                Request = this
                            };

                            SendCallback(args);
                        }
                    }
                }
            }
        }

        void SocketClientManager_OnDisconnected(string message, Socket socket)
        {
            //断开服务器
            connected = false;

            //断开套接字
            socket.Disconnect(true);
        }

        void SocketClientManager_OnConnected(string message, bool connected, Socket socket)
        {
            //连上服务器
            this.connected = connected;
        }

        #endregion
    }
}
