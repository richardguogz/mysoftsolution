using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MySoft.Core
{
    /// <summary>
    /// MySoft异常类
    /// </summary>
    [Serializable]
    public class MySoftException : Exception
    {
        public MySoftException() { }
        public MySoftException(string message) : base(message) { }
        public MySoftException(string message, Exception inner) : base(message, inner) { }
        protected MySoftException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

}