using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Core;

namespace MySoft.Data
{
    /// <summary>
    /// A delegate used for exception.
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="log"></param>
    public delegate void ExceptionLogHandler(Exception exception, string log);

    interface IExceptionLogable : ILogable
    {

        /// <summary>
        /// OnDbException event;
        /// </summary>
        event ExceptionLogHandler OnExceptionLog;
    }
}
