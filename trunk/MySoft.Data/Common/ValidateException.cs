using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Core;
using System.Runtime.Serialization;

namespace MySoft.Data
{
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
