using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySoft.Net.Sockets;

namespace MySoft.Net.Services
{
    /// <summary>
    /// Socket客户端工厂
    /// </summary>
    public class ClientFactory
    {
        private ClientFactory instance;
        private Dictionary<Guid, ResponseMessage> responses = new Dictionary<Guid, ResponseMessage>();

        public ClientFactory()
        {
            SocketClientManager.OnConnected += new ConnectionEventHandler(SocketClientManager_OnConnected);
            SocketClientManager.OnDisconnected += new DisconnectionEventHandler(SocketClientManager_OnDisconnected);
            SocketClientManager.OnReceived += new ReceiveEventHandler(SocketClientManager_OnReceived);
        }

        public ClientFactory Create()
        {
            if (instance == null)
            {
                instance = new ClientFactory();
            }

            return instance;
        }

        void SocketClientManager_OnReceived(byte[] buffer)
        {
            //throw new NotImplementedException();

            BufferRead read = new BufferRead(buffer);

            int length;
            int cmd;

            if (read.ReadInt32(out length) && read.ReadInt32(out cmd) && length == read.Length)
            {
                switch (cmd)
                {
                    case -20000: //自定义的数据包，输出数据
                        {
                            object responseObject;
                            if (read.ReadObject(out responseObject))
                            {
                                ResponseMessage result = responseObject as ResponseMessage;
                                responses[result.Request.MessageID] = result;
                            }
                        }
                        break;
                }
            }
        }

        void SocketClientManager_OnDisconnected(string message)
        {
            //throw new NotImplementedException();
        }

        void SocketClientManager_OnConnected(string message, bool connect)
        {
            //throw new NotImplementedException();
        }
    }
}
