using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Core;

namespace MySoft.Web
{
    /// <summary>
    /// Web异常
    /// </summary>
    [Serializable]
    public class WebException : MySoftException
    {
        /// <summary>
        /// 普通异常的构造方法
        /// </summary>
        /// <param name="message"></param>
        public WebException(string message) : base(ExceptionType.WebException, message) { }

        /// <summary>
        /// 内嵌异常的构造方法
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public WebException(string message, Exception ex) : base(ExceptionType.WebException, message, ex) { }
    }
}
