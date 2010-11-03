using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Core
{
    /// <summary>
    /// A delegate used for exception.
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="log"></param>
    public delegate void ErrorLogEventHandler(Exception exception, string log);

    /// <summary>
    /// Mark a implementing class as loggable.
    /// </summary>
    public interface IErrorLogable
    {
        /// <summary>
        /// OnError event.
        /// </summary>
        event ErrorLogEventHandler OnError;
    }
}
