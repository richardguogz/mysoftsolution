using System;

namespace MySoft
{
    /// <summary>
    /// Attribute used to mark service interfaces.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class ServiceContractAttribute : Attribute
    {
        private int timeout = -1;
        /// <summary>
        /// 响应超时时间
        /// </summary>
        public int Timeout
        {
            get
            {
                return timeout;
            }
            set
            {
                timeout = value;
            }
        }

        /// <summary>
        /// 实例化ServiceContractAttribute
        /// </summary>
        public ServiceContractAttribute() { }

        /// <summary>
        /// 实例化ServiceContractAttribute
        /// </summary>
        /// <param name="timeout"></param>
        public ServiceContractAttribute(int timeout)
        {
            this.timeout = timeout;
        }
    }
}
