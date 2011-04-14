
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
}
