using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MySoft.Net.Client;
using System.Net.Sockets;
using MySoft.Net.Sockets;
using MySoft.IoC.Configuration;

namespace MySoft.IoC.Message
{
    /// <summary>
    /// 服务消息
    /// </summary>
    public class ServiceMessage
    {
        public event ServiceMessageEventHandler SendCallback;

        private SocketClientManager manager;
        private bool isConnected = false;
        private string node;
        private string ip;
        private int port;

        public ServiceMessage(RemoteNode node)
        {
            this.node = node.Key;
            this.ip = node.IP;
            this.port = node.Port;

            manager = new SocketClientManager();
            manager.OnConnected += new ConnectionEventHandler(SocketClientManager_OnConnected);
            manager.OnDisconnected += new DisconnectionEventHandler(SocketClientManager_OnDisconnected);
            manager.OnReceived += new ReceiveEventHandler(SocketClientManager_OnReceived);
        }

        /// <summary>
        /// 是否连接
        /// </summary>
        public bool IsConnected
        {
            get { return isConnected; }
        }

        /// <summary>
        /// 发送数据包
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool Send(object data, TimeSpan timeout)
        {
            //如果连接断开，直接抛出异常
            if (!isConnected)
            {
                //尝试连接到服务器
                isConnected = manager.Client.ConnectTo(ip, port, timeout);
                if (!isConnected)
                {
                    throw new WarningException(string.Format("Can't connect to server ({0}:{1})！Remote node : {2}", ip, port, node));
                }
            }

            if (data == null) return false;
            byte[] buffer = BufferFormat.FormatFCA(data);
            return manager.Client.SendData(buffer);
        }

        #region Socket消息委托

        void SocketClientManager_OnReceived(byte[] buffer, Socket socket)
        {
            BufferReader read = new BufferReader(buffer);

            int length;
            int cmd;

            if (read.ReadInt32(out length) && read.ReadInt32(out cmd) && length == read.Length)
            {
                if (cmd == 10000) //返回数据包
                {
                    object responseObject;
                    if (read.ReadObject(out responseObject))
                    {
                        var result = responseObject as ResponseMessage;
                        if (SendCallback != null)
                        {
                            var args = new ServiceMessageEventArgs
                            {
                                Result = result,
                                Socket = manager.Client.Socket
                            };

                            SendCallback(this, args);
                        }
                    }
                }
            }
        }

        void SocketClientManager_OnDisconnected(string message, Socket socket)
        {
            //断开服务器
            isConnected = false;

            //断开套接字
            socket.Disconnect(true);
        }

        void SocketClientManager_OnConnected(string message, bool connected, Socket socket)
        {
            //连上服务器
            this.isConnected = connected;
        }

        #endregion
    }
}
