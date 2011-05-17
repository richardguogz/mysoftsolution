using System.Data;
using MySoft.Logger;

namespace MySoft.Data
{
    /// <summary>
    /// 执行并输出日志的接口
    /// </summary>
    public interface IExcutingLog : ILog
    {
        /// <summary>
        /// 开始执行命令
        /// </summary>
        /// <param name="cmd"></param>
        void StartExcute(IDbCommand cmd);

        /// <summary>
        /// 结束执行命令
        /// </summary>
        /// <param name="cmd"></param>
        void EndExcute(IDbCommand cmd);
    }
}
