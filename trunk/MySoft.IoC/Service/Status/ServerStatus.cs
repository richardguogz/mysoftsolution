using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySoft.Net.Sockets;
using System.Net;

namespace MySoft.IoC
{
    /// <summary>
    /// 服务器状态信息
    /// </summary>
    [Serializable]
    public class ServerStatus
    {
        private IList<EndPoint> clients;
        /// <summary>
        /// 客户信息
        /// </summary>
        public IList<EndPoint> Clients
        {
            get
            {
                return clients;
            }
            set
            {
                clients = value;
            }
        }
    }
}
