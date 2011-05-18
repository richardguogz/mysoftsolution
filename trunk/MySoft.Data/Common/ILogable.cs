using System;
using System.Data;

namespace MySoft.Data
{
    /// <summary>
    /// ִ�в������־�Ľӿ�
    /// </summary>
    public interface IExcutingLog
    {
        /// <summary>
        /// ��ʼִ����������Ƿ���Ҫִ��
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameter"></param>
        bool StartExcute(string cmdText, SQLParameter[] parameter);

        /// <summary>
        /// ����ִ������
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameter"></param>
        /// <param name="result"></param>
        /// <param name="elapsedTime"></param>
        void EndExcute(string cmdText, SQLParameter[] parameter, object result, int elapsedTime);

        /// <summary>
        /// ִ�еĴ�����Ϣ
        /// </summary>
        /// <param name="exception"></param>
        void ExcuteError(Exception exception);
    }
}
