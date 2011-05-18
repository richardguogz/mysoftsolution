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
        /// 开始执行命令，返回是否需要执行
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameter"></param>
        bool StartExcute(string cmdText, SQLParameter[] parameter);

        /// <summary>
        /// 结束执行命令
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameter"></param>
        /// <param name="result"></param>
        /// <param name="elapsedTime"></param>
        void EndExcute(string cmdText, SQLParameter[] parameter, object result, int elapsedTime);

        /// <summary>
        /// 执行的错误信息
        /// </summary>
        /// <param name="exception"></param>
        void ExcuteError(Exception exception);
    }
}
