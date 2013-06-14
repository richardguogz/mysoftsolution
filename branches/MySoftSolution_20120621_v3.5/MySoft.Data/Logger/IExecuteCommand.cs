using System;
using System.Data;
using MySoft.Logger;
using System.Data.Common;

namespace MySoft.Data.Logger
{
    /// <summary>
    /// ִ�в������־�Ľӿ�
    /// </summary>
    public interface IExecuteCommand
    {
        /// <summary>
        /// ��ʼִ����������Ƿ���Ҫִ��
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        void BeginExecute(DbCommand command);

        /// <summary>
        /// ����ִ������
        /// </summary>
        /// <param name="command"></param>
        /// <param name="retValue"></param>
        /// <param name="elapsedTime"></param>
        void EndExecute(DbCommand command, ReturnValue retValue, long elapsedTime);
    }
}
