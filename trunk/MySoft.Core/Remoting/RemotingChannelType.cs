
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
        /// ������
        /// </summary>
        Binary,
        /// <summary>
        /// json��ʽ
        /// </summary>
        Json,
        /// <summary>
        /// xml��ʽ
        /// </summary>
        Xml
    }
}
