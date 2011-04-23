using System;

namespace MySoft
{
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

        private CompressType compress;
        /// <summary>
        /// 压缩方式
        /// </summary>
        public CompressType Compress
        {
            get
            {
                return compress;
            }
            set
            {
                compress = value;
            }
        }

        private int timeout = -1;
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
            this.compress = CompressType.None;
        }

        public ServiceContractAttribute(ResponseFormat format)
            : this()
        {
            this.format = format;
        }

        public ServiceContractAttribute(CompressType compress)
            : this()
        {
            this.compress = compress;
        }

        public ServiceContractAttribute(ResponseFormat format, CompressType compress)
            : this()
        {
            this.format = format;
            this.compress = compress;
        }
    }
}
