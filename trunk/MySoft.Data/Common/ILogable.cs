using System.Data;
using MySoft.Core;

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
        event ErrorLogEventHandler OnError;

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
