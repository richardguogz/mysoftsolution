using System;

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

    /// <summary>
    /// Attribute used to mark service interfaces.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class ServiceContractAttribute : Attribute
    {
        private bool allowCache = false;
        /// <summary>
        /// 是否允许缓存
        /// </summary>
        public bool AllowCache
        {
            get
            {
                return allowCache;
            }
            set
            {
                allowCache = value;
            }
        }

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
        /// 实例化ServiceContractAttribute
        /// </summary>
        public ServiceContractAttribute() { }

        /// <summary>
        /// 实例化ServiceContractAttribute
        /// </summary>
        /// <param name="allowCache"></param>
        public ServiceContractAttribute(bool allowCache)
        {
            this.allowCache = allowCache;
        }

        /// <summary>
        /// 实例化ServiceContractAttribute
        /// </summary>
        /// <param name="cacheTime"></param>
        public ServiceContractAttribute(int cacheTime)
        {
            this.allowCache = true;
            this.cacheTime = cacheTime;
        }
    }
}
