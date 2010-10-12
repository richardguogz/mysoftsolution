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

    /// <summary>
    /// 压缩方式
    /// </summary>
    public enum CompressType
    {
        /// <summary>
        /// 不压缩
        /// </summary>
        NONE,
        /// <summary>
        /// GZIP压缩方式
        /// </summary>
        GZIP,
        /// <summary>
        /// 7ZIP压缩方式
        /// </summary>
        ZIP7,
        /// <summary>
        /// AUTO压缩方式
        /// </summary>
        AUTO
    }
}
