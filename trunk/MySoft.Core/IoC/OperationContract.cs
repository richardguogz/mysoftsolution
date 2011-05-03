using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.IoC
{
    /// <summary>
    /// Attribute used to mark service interfaces.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class OperationContractAttribute : Attribute
    {
        private int timeout = -1;
        /// <summary>
        /// 超时时间（单位：ms）
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

        private int cacheTime = -1;
        /// <summary>
        /// 缓存时间（单位：ms）
        /// </summary>
        public int CacheTime
        {
            get
            {
                return cacheTime;
            }
            set
            {
                cacheTime = value;
            }
        }

        /// <summary>
        /// 实例化OperationContractAttribute
        /// </summary>
        public OperationContractAttribute() { }

        /// <summary>
        /// 实例化OperationContractAttribute
        /// </summary>
        /// <param name="cacheTime"></param>
        public OperationContractAttribute(int cacheTime)
        {
            this.cacheTime = cacheTime;
        }
    }

}
