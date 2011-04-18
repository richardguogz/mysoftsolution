using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Net.Sockets;
using System.Net.Sockets;

namespace MySoft.Net.Server
{
    /// <summary>
    /// string host, int port, int maxconnectcout, int maxbuffersize
    /// </summary>
    public class SocketServerConfiguration
    {
        /// <summary>
        /// 主机信息，可以为localhost或any
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 侦听端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 最大连接数
        /// </summary>
        public int MaxConnectCount { get; set; }

        /// <summary>
        /// 最大缓冲大小
        /// </summary>
        public int MaxBufferSize { get; set; }
    }

    /// <summary>
    /// 默认Socket服务端
    /// </summary>
    public class SocketServerManager
    {
        /// <summary>
        /// 数据包接收
        /// </summary>
        public event BinaryInputEventHandler OnBinaryInput;

        /// <summary>
        /// 消息输入
        /// </summary>
        public event MessageInputEventHandler OnMessageInput;

        /// <summary>
        /// 消息输出
        /// </summary>
        public event EventHandler<LogOutEventArgs> OnMessageOutput;

        /// <summary>
        /// 连接筛选
        /// </summary>
        public event ConnectionFilterEventHandler OnConnectFilter;

        /// <summary>
        /// 数据包缓冲类
        /// </summary>
        public BufferList BuffListManger { get; set; }

        /// <summary>
        /// SOCKETSERVER对象
        /// </summary>
        public SocketServer Server { get; set; }

        /// <summary>
        /// 实例化Socket服务端管理器
        /// </summary>
        public SocketServerManager()
        {
            //初始化数据包缓冲区,并设置了最大数据包尽可能的大 
            BuffListManger = new BufferList(409600);

            Server = new SocketServer();
            Server.OnBinaryInput += new BinaryInputEventHandler(Server_OnBinaryInput);
            Server.OnMessageInput += new MessageInputEventHandler(Server_OnMessageInput);
            Server.OnMessageOutput += new EventHandler<LogOutEventArgs>(Server_OnMessageOutput);
            Server.OnConnectFilter += new ConnectionFilterEventHandler(Server_OnConnectFilter);
        }

        /// <summary>
        /// 实例化Socket服务端管理器
        /// </summary>
        /// <param name="config"></param>
        public SocketServerManager(SocketServerConfiguration config)
        {
            //初始化数据包缓冲区,并设置了最大数据包尽可能的大 
            BuffListManger = new BufferList(409600);

            Server = new SocketServer(config.Host, config.Port, config.MaxConnectCount, config.MaxBufferSize);
            Server.OnBinaryInput += new BinaryInputEventHandler(Server_OnBinaryInput);
            Server.OnMessageInput += new MessageInputEventHandler(Server_OnMessageInput);
            Server.OnMessageOutput += new EventHandler<LogOutEventArgs>(Server_OnMessageOutput);
            Server.OnConnectFilter += new ConnectionFilterEventHandler(Server_OnConnectFilter);
        }

        bool Server_OnConnectFilter(SocketAsyncEventArgs socketAsync)
        {
            if (OnConnectFilter != null)
                return OnConnectFilter(socketAsync);

            return false;
        }

        void Server_OnMessageOutput(object sender, LogOutEventArgs e)
        {
            if (OnMessageOutput != null)
                OnMessageOutput(sender, e);
        }

        void Server_OnMessageInput(string message, int error, SocketAsyncEventArgs socketAsync)
        {
            if (OnMessageInput != null)
                OnMessageInput(message, error, socketAsync);
        }

        void Server_OnBinaryInput(byte[] buffer, SocketAsyncEventArgs socketAsync)
        {
            //如果链接断开，则直接返回
            if (!socketAsync.AcceptSocket.Connected) return;

            if (socketAsync.UserToken == null) //如果此SOCKET绑定的对象为NULL
            {
                //注意这里为了 简单 所以就绑定了个 BuffList 类，本来这里应该绑定用户类对象，
                //并在用户类里面建立 初始化 一个 BuffList 类，这样就能通过用户类保存更多的信息了。
                //比如用户名，权限等等
                socketAsync.UserToken = new BufferList(4096);
            }

            //BuffList 数据包组合类 如果不想丢数据就用这个类吧
            BufferList bufflist = socketAsync.UserToken as BufferList;

            lock (socketAsync)
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

                    datax = null;
                }
            }
        }
    }
}
