
namespace MySoft.Remoting
{
    /// <summary>
    /// Remoting Channel Type
    /// </summary>
    public enum RemotingChannelType
    {
        /// <summary>
        /// TCP
        /// </summary>
        Tcp,
        /// <summary>
        /// HTTP
        /// </summary>
        Http
    }

    /// <summary>
    /// Remoting Data Type
    /// </summary>
    public enum TransferType
    {
        /// <summary>
        /// 二进制
        /// </summary>
        Binary,
        /// <summary>
        /// json格式
        /// </summary>
        Json,
        /// <summary>
        /// xml格式
        /// </summary>
        Xml
    }

    /// <summary>
    /// 压缩方式
    /// </summary>
    public enum CompressType
    {
        /// <summary>
        /// 不压缩
        /// </summary>
        None,
        /// <summary>
        /// GZIP压缩方式
        /// </summary>
        GZip,
        /// <summary>
        /// 7ZIP压缩方式
        /// </summary>
        Zip
    }
}
