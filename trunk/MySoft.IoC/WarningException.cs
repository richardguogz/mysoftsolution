using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.IoC
{
    /// <summary>
    /// 警告异常信息
    /// </summary>
    [Serializable]
    public class WarningException : IoCException
    {
        /// <summary>
        /// 普通异常的构造方法
        /// </summary>
        /// <param name="message"></param>
        public WarningException(string message)
            : base(message) { }
    }
}
