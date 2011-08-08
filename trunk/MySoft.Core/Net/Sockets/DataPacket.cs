using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.Net.Sockets
{
    /// <summary>
    /// 数据包信息
    /// </summary>
    [Serializable]
    public class DataPacket : IDisposable
    {
        private Guid packetID;
        /// <summary>
        /// 数据包ID
        /// </summary>
        public Guid PacketID
        {
            get { return packetID; }
            set { packetID = value; }
        }

        private object packetObject;
        /// <summary>
        /// 数据包对象
        /// </summary>
        public object PacketObject
        {
            get { return packetObject; }
            set { packetObject = value; }
        }

        /// <summary>
        /// 翻译资源
        /// </summary>
        public void Dispose()
        {
            this.packetObject = null;
        }
    }
}
