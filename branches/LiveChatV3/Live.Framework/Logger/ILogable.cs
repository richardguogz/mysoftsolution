using System;
using System.Collections.Generic;
using System.Text;

namespace LiveChat.Logger
{
    /// <summary>
    /// ��־����ί��
    /// </summary>
    /// <param name="logMsg">��־��Ϣ</param>
    public delegate void LogHandler(string logMsg);

    /// <summary>
    /// ��Ҫ��¼��־�������̳иýӿ�
    /// </summary>
    public interface ILogable
    {
        /// <summary>
        /// ��־�¼�
        /// </summary>
        event LogHandler OnLog;
    }
}
