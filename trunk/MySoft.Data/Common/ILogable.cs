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
        /// ��ʼִ������
        /// </summary>
        /// <param name="cmd"></param>
        void StartExcute(IDbCommand cmd);

        /// <summary>
        /// ����ִ������
        /// </summary>
        /// <param name="cmd"></param>
        void EndExcute(IDbCommand cmd);
    }
}
