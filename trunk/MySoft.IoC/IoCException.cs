using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Core;

namespace MySoft.IoC
{
    /// <summary>
    /// IoC异常
    /// </summary>
    [Serializable]
    public class IoCException : MySoftException
    {
        /// <summary>
        /// 普通异常的构造方法
        /// </summary>
        /// <param name="message"></param>
        public IoCException(string message) : base(ExceptionType.IoCException, message) { }

        /// <summary>
        /// 内嵌异常的构造方法
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public IoCException(string message, Exception ex) : base(ExceptionType.IoCException, message, ex) { }
    }
}
