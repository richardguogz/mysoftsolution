using System;
using System.Data;
using MySoft.Logger;

namespace MySoft.Data
{
    /// <summary>
    /// ִ�в������־�Ľӿ�
    /// </summary>
    public interface IExecuteLog : ILog
    {
        /// <summary>
        /// ��ʼִ����������Ƿ���Ҫִ��
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameter"></param>
        bool Begin(string cmdText, SQLParameter[] parameter);

        /// <summary>
        /// ����ִ������
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameter"></param>
        /// <param name="result"></param>
        /// <param name="elapsedTime"></param>
        void End(string cmdText, SQLParameter[] parameter, object result, int elapsedTime);
    }
}
