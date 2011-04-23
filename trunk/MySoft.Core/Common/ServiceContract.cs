using System;

namespace MySoft
{
    /// <summary>
    /// 数据格式
    /// </summary>
    public enum ResponseFormat
    {
        /// <summary>
        /// 二进制
        /// </summary>
        Binary,
        /// <summary>
        /// json格式
        /// </summary>
        Json,
        /// <summary>
        /// xml格式
        /// </summary>
        Xml
    }

    /// <summary>
    /// Attribute used to mark service interfaces.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class ServiceContractAttribute : Attribute
    {
        private ResponseFormat format;
        /// <summary>
        /// 响应格式
        /// </summary>
        public ResponseFormat Format
        {
            get
            {
                return format;
            }
            set
            {
                format = value;
            }
        }

        private int timeout;
        /// <summary>
        /// 响应时间
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

        public ServiceContractAttribute()
        {
            this.format = ResponseFormat.Binary;
        }

        public ServiceContractAttribute(ResponseFormat format)
            : this()
        {
            this.format = format;
        }
    }
}
