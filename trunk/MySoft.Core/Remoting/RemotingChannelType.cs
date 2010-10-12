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
        /// ������
        /// </summary>
        BINARY,
        /// <summary>
        /// json��ʽ
        /// </summary>
        JSON,
        /// <summary>
        /// xml��ʽ
        /// </summary>
        XML
    }

    /// <summary>
    /// ѹ����ʽ
    /// </summary>
    public enum CompressType
    {
        /// <summary>
        /// ��ѹ��
        /// </summary>
        NONE,
        /// <summary>
        /// GZIPѹ����ʽ
        /// </summary>
        GZIP,
        /// <summary>
        /// 7ZIPѹ����ʽ
        /// </summary>
        ZIP7,
        /// <summary>
        /// AUTOѹ����ʽ
        /// </summary>
        AUTO
    }
}
