using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Core.Remoting
{
    /// <summary>
    /// Remoting Channel Type
    /// </summary>
    public enum RemotingChannelType
    {
        /// <summary>
        /// TCP
        /// </summary>
        TCP,
        /// <summary>
        /// HTTP
        /// </summary>
        HTTP
    }

    /// <summary>
    /// Remoting Data Type
    /// </summary>
    public enum RemotingDataType
    {
        /// <summary>
        /// 二进制
        /// </summary>
        BINARY,
        /// <summary>
        /// json格式
        /// </summary>
        JSON,
        /// <summary>
        /// xml格式
        /// </summary>
        XML
    }
}
