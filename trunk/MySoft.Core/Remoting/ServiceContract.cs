using System;

namespace MySoft.Remoting
{
    /// <summary>
    /// 数据格式
    /// </summary>
    public enum DataFormat
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default,
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
        private DataFormat format;
        /// <summary>
        /// 数据格式
        /// </summary>
        public DataFormat Format
        {
            get
            {
                return format;
            }
        }

        public ServiceContractAttribute()
        {
            this.format = DataFormat.Default;
        }

        public ServiceContractAttribute(DataFormat format)
        {
            this.format = format;
        }
    }
}
