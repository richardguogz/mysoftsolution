using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MySoft.Data
{
    /// <summary>
    /// MySoft异常类
    /// </summary>
    public class MySoftException : Exception
    {
        public MySoftException() { }
        public MySoftException(string message) : base(message) { }
        public MySoftException(string message, Exception inner) : base(message, inner) { }
        protected MySoftException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// MySoft验证异常类
    /// </summary>
    public class ValidateException : MySoftException
    {
        public ValidateException() { }
        public ValidateException(string message) : base(message) { }
        public ValidateException(string message, Exception inner) : base(message, inner) { }
        protected ValidateException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
