using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySoft.Net.Sockets;
using MySoft.Net.Server;

namespace MySoft.Net.Services
{
    /// <summary>
    /// Socket服务端工厂
    /// </summary>
    public class ServerFactory : IDisposable
    {
        private ServerFactory instance;
        private Dictionary<Guid, RequestMessage> requests = new Dictionary<Guid, RequestMessage>();

        public ServerFactory()
        {
            SocketServerManager.OnConnectFilter += new ConnectionFilterEventHandler(SocketServerManager_OnConnectFilter);
            SocketServerManager.OnBinaryInput += new BinaryInputEventHandler(SocketServerManager_OnBinaryInput);
            SocketServerManager.OnMessageInput += new MessageInputEventHandler(SocketServerManager_OnMessageInput);
            SocketServerManager.OnMessageOutput += new EventHandler<LogOutEventArgs>(SocketServerManager_OnMessageOutput);

            SocketServerManager.Start();
        }

        ~ServerFactory()
        {
            SocketServerManager.Stop();
        }

        public void Dispose()
        {
            SocketServerManager.Stop();
        }

        public ServerFactory Create()
        {
            if (instance == null)
            {
                instance = new ServerFactory();
            }

            return instance;
        }

        bool SocketServerManager_OnConnectFilter(System.Net.Sockets.SocketAsyncEventArgs socketAsync)
        {
            socketAsync.UserToken = true;

            Console.WriteLine("User connection {0}！", socketAsync.AcceptSocket.RemoteEndPoint);

            return true;
        }

        void SocketServerManager_OnMessageOutput(object sender, LogOutEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void SocketServerManager_OnMessageInput(string message, System.Net.Sockets.SocketAsyncEventArgs socketAsync, int error)
        {
            //throw new NotImplementedException();
        }

        void SocketServerManager_OnBinaryInput(byte[] buffer, System.Net.Sockets.SocketAsyncEventArgs socketAsync)
        {
            BufferRead read = new BufferRead(buffer);

            int length;
            int cmd;

            if (read.ReadInt32(out length) && read.ReadInt32(out cmd) && length == read.Length)
            {
                switch (cmd)
                {
                    case -10000: //自定义的数据包，输入请求
                        {
                            object requestObject;
                            if (read.ReadObject(out requestObject))
                            {
                                RequestMessage result = requestObject as RequestMessage;
                                requests[result.MessageID] = result;
                            }
                        }
                        break;
                }
            }
        }
    }
}
