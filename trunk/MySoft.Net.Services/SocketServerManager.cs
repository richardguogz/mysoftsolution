using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Net.Server;
using MySoft.Net.Sockets;
using System.Net.Sockets;

namespace MySoft.Net
{
    /// <summary>
    /// 默认Socket服务端
    /// </summary>
    public static class SocketServerManager
    {
        /// <summary>
        /// 数据包接收
        /// </summary>
        public static event BinaryInputEventHandler OnBinaryInput;

        /// <summary>
        /// 消息输入
        /// </summary>
        public static event MessageInputEventHandler OnMessageInput;

        /// <summary>
        /// 消息输出
        /// </summary>
        public static event EventHandler<LogOutEventArgs> OnMessageOutput;

        /// <summary>
        /// 连接筛选
        /// </summary>
        public static event ConnectionFilterEventHandler OnConnectFilter;

        /// <summary>
        /// 数据包缓冲类
        /// </summary>
        public static BufferList BuffListManger { get; set; }

        /// <summary>
        /// SOCKETSERVER对象
        /// </summary>
        public static SocketServer Server { get; set; }

        static SocketServerManager()
        {
            //初始化数据包缓冲区,并设置了最大数据包尽可能的大 
            BuffListManger = new BufferList(400000);

            Server = new SocketServer();
            Server.OnBinaryInput += new BinaryInputEventHandler(Server_OnBinaryInput);
            Server.OnMessageInput += new MessageInputEventHandler(Server_OnMessageInput);
            Server.OnMessageOutput += new EventHandler<LogOutEventArgs>(Server_OnMessageOutput);
            Server.OnConnectFilter += new ConnectionFilterEventHandler(Server_OnConnectFilter);
        }

        static bool Server_OnConnectFilter(SocketAsyncEventArgs socketAsync)
        {
            if (OnConnectFilter != null)
                return OnConnectFilter(socketAsync);

            return false;
        }

        static void Server_OnMessageOutput(object sender, LogOutEventArgs e)
        {
            if (OnMessageOutput != null)
                OnMessageOutput(sender, e);
        }

        static void Server_OnMessageInput(string message, SocketAsyncEventArgs socketAsync, int error)
        {
            if (OnMessageInput != null)
                OnMessageInput(message, socketAsync, error);
        }

        static void Server_OnBinaryInput(byte[] buffer, SocketAsyncEventArgs socketAsync)
        {
            if (socketAsync.UserToken != null)
            {
                List<byte[]> datax;

                ////整理从服务器上收到的数据包
                if (BuffListManger.InsertByteArray(buffer, 4, out datax))
                {
                    if (OnBinaryInput != null)
                    {
                        foreach (byte[] mdata in datax)
                        {
                            OnBinaryInput(mdata, socketAsync);
                        }
                    }
                }
            }
        }
    }
}
