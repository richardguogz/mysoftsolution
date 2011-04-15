using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Net.Client;
using MySoft.Net.Sockets;

namespace MySoft.Net.Services
{
    /// <summary>
    /// 默认Socket客户端
    /// </summary>
    public static class SocketClientManager
    {
        /// <summary>
        /// 数据包接收
        /// </summary>
        public static event ReceiveEventHandler OnReceived;

        /// <summary>
        /// 连上服务器
        /// </summary>
        public static event ConnectionEventHandler OnConnected;

        /// <summary>
        /// 断开连接
        /// </summary>
        public static event DisconnectionEventHandler OnDisconnected;

        /// <summary>
        /// 数据包缓冲类
        /// </summary>
        public static BufferList BuffListManger { get; set; }

        /// <summary>
        /// SOCKETCLIENT对象
        /// </summary>
        public static SocketClient Client { get; set; }

        static SocketClientManager()
        {
            //初始化数据包缓冲区,并设置了最大数据包尽可能的大 
            BuffListManger = new BufferList(400000);

            Client = new SocketClient();
            Client.OnReceived += new ReceiveEventHandler(Client_OnReceived);
            Client.OnConnected += new ConnectionEventHandler(Client_OnConnected);
            Client.OnDisconnected += new DisconnectionEventHandler(Client_OnDisconnected);
        }

        static void Client_OnConnected(string message, bool connect)
        {
            if (OnConnected != null)
                OnConnected(message, connect);
        }

        static void Client_OnDisconnected(string message)
        {
            if (OnDisconnected != null)
                OnDisconnected(message);
        }

        static void Client_OnReceived(byte[] buffer)
        {
            List<byte[]> datax;

            ////整理从服务器上收到的数据包
            if (BuffListManger.InsertByteArray(buffer, 4, out datax))
            {
                if (OnReceived != null)
                {
                    foreach (byte[] mdata in datax)
                    {
                        OnReceived(mdata);
                    }
                }
            }
        }
    }
}
