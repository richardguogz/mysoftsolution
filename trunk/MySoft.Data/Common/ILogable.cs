using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Core;
using System.Data;

namespace MySoft.Data
{
    /// <summary>
    /// ���ݴ����¼�ί��
    /// </summary>
    /// <param name="command"></param>
    public delegate void ExcutingEventHandler(IDbCommand command);

    interface IExcutingCommand : ILogable
    {
        /// <summary>
        /// OnDbException event;
        /// </summary>
        event ExceptionLogEventHandler OnError;

        /// <summary>
        /// ��ʼ�¼�
        /// </summary>
        event ExcutingEventHandler OnStart;

        /// <summary>
        /// �����¼�
        /// </summary>
        event ExcutingEventHandler OnEnd;
    }
}
