using System;
using System.Data;

namespace MySoft.Data
{
    /// <summary>
    /// 执行并输出日志的接口
    /// </summary>
    public interface IExcutingLog
    {
        /// <summary>
        /// 开始执行命令
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameter"></param>
        void StartExcute(string cmdText, SQLParameter[] parameter);

        /// <summary>
        /// 结束执行命令
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameter"></param>
        /// <param name="result"></param>
        /// <param name="elapsedTime"></param>
        void EndExcute(string cmdText, SQLParameter[] parameter, object result, double elapsedTime);

        /// <summary>
        /// 执行的错误信息
        /// </summary>
        /// <param name="exception"></param>
        void ExcuteError(Exception exception);

        /// <summary>
        /// 执行的sql日志
        /// </summary>
        /// <param name="log"></param>
        void ExcuteLog(string log);
    }
}
