using System;
using System.Data;
using MySoft.Logger;

namespace MySoft.Data
{
    /// <summary>
    /// ִ�в������־�Ľӿ�
    /// </summary>
    public interface IExcutingLog : ILog
    {
        /// <summary>
        /// ��ʼִ����������Ƿ���Ҫִ��
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameter"></param>
        bool BeginExcute(string cmdText, SQLParameter[] parameter);

        /// <summary>
        /// ����ִ������
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameter"></param>
        /// <param name="result"></param>
        /// <param name="elapsedTime"></param>
        void EndExcute(string cmdText, SQLParameter[] parameter, object result, int elapsedTime);
    }
}
