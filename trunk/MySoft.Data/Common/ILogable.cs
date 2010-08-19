using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Core;
using System.Data;

namespace MySoft.Data
{
    /// <summary>
    /// A delegate used for exception.
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="log"></param>
    public delegate void ExceptionLogHandler(Exception exception, string log);

    /// <summary>
    /// ���ݴ����¼�ί��
    /// </summary>
    /// <param name="command"></param>
    public delegate void ExcutingHandler(IDbCommand command);

    interface IExcutingCommand : ILogable
    {
        /// <summary>
        /// OnDbException event;
        /// </summary>
        event ExceptionLogHandler OnExceptionLog;

        /// <summary>
        /// ��ʼ�¼�
        /// </summary>
        event ExcutingHandler OnStart;

        /// <summary>
        /// �����¼�
        /// </summary>
        event ExcutingHandler OnEnd;
    }
}
