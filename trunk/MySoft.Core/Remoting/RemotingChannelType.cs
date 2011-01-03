
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

    /// <summary>
    /// ѹ����ʽ
    /// </summary>
    public enum CompressType
    {
        /// <summary>
        /// ��ѹ��
        /// </summary>
        None,
        /// <summary>
        /// GZIPѹ����ʽ
        /// </summary>
        GZip,
        /// <summary>
        /// 7ZIPѹ����ʽ
        /// </summary>
        Zip
    }
}
