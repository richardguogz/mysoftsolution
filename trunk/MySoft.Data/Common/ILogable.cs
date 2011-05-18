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
        /// ��ʼִ������
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameter"></param>
        void StartExcute(string cmdText, SQLParameter[] parameter);

        /// <summary>
        /// ����ִ������
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameter"></param>
        /// <param name="result"></param>
        /// <param name="elapsedTime"></param>
        void EndExcute(string cmdText, SQLParameter[] parameter, object result, double elapsedTime);

        /// <summary>
        /// ִ�еĴ�����Ϣ
        /// </summary>
        /// <param name="exception"></param>
        void ExcuteError(Exception exception);

        /// <summary>
        /// ִ�е�sql��־
        /// </summary>
        /// <param name="log"></param>
        void ExcuteLog(string log);
    }
}
