using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Core;
using System.Data;

namespace MySoft.Data
{
    /// <summary>
    /// 数据处理事件委托
    /// </summary>
    /// <param name="command"></param>
    public delegate void ExcutingEventHandler(IDbCommand command);

    interface IExcutingCommand : ILogable
    {
        /// <summary>
        /// OnDbException event;
        /// </summary>
        event ExceptionLogEventHandler OnError;

        /// <summary>
        /// 开始事件
        /// </summary>
        event ExcutingEventHandler OnStart;

        /// <summary>
        /// 结束事件
        /// </summary>
        event ExcutingEventHandler OnEnd;
    }
}
