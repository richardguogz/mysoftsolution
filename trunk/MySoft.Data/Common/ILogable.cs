using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Core;
using System.Data;

namespace MySoft.Data
{
    /// <summary>
    /// A delegate used for exception.
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="log"></param>
    public delegate void ExceptionLogHandler(Exception exception, string log);

    /// <summary>
    /// 数据处理事件委托
    /// </summary>
    /// <param name="command"></param>
    public delegate void ExcutingHandler(IDbCommand command);

    interface IExcutingCommand : ILogable
    {
        /// <summary>
        /// OnDbException event;
        /// </summary>
        event ExceptionLogHandler OnExceptionLog;

        /// <summary>
        /// 开始事件
        /// </summary>
        event ExcutingHandler OnStart;

        /// <summary>
        /// 结束事件
        /// </summary>
        event ExcutingHandler OnEnd;
    }
}
