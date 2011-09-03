using System;
using System.Diagnostics;

namespace MySoft.Logger
{
    /// <summary>
    /// A delegate used for log.
    /// </summary>
    /// <param name="log">The msg to write to log.</param>
    public delegate void LogEventHandler(string log, LogType type);

    /// <summary>
    /// Mark a implementing class as loggable.
    /// </summary>
    public interface ILogable
    {
        /// <summary>
        /// OnLog event.
        /// </summary>
        event LogEventHandler OnLog;
    }

    /// <summary>
    /// ��־����
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// �����¼�
        /// </summary>
        Error,
        /// <summary>
        /// �����¼�
        /// </summary>
        Warning,
        /// <summary>
        /// ��Ϣ�¼�
        /// </summary>
        Information
    }
}
