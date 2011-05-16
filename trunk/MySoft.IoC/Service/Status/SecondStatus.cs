using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.IoC
{
    /// <summary>
    /// 每秒服务器状态信息
    /// </summary>
    [Serializable]
    public class SecondStatus
    {
        private long dataFlow;
        /// <summary>
        /// 数据流量
        /// </summary>
        public long DataFlow
        {
            get
            {
                return dataFlow;
            }
            set
            {
                lock (this)
                {
                    dataFlow = value;
                }
            }
        }

        private int requestCount;
        /// <summary>
        /// 请求数
        /// </summary>
        public int RequestCount
        {
            get
            {
                return requestCount;
            }
            set
            {
                lock (this)
                {
                    requestCount = value;
                }
            }
        }

        private int successCount;
        /// <summary>
        /// 成功计数
        /// </summary>
        public int SuccessCount
        {
            get
            {
                return successCount;
            }
            set
            {
                lock (this)
                {
                    successCount = value;
                }
            }
        }

        private int errorCount;
        /// <summary>
        /// 错误数
        /// </summary>
        public int ErrorCount
        {
            get
            {
                return errorCount;
            }
            set
            {
                lock (this)
                {
                    errorCount = value;
                }
            }
        }

        private long elapsedTime;
        /// <summary>
        /// 总耗时
        /// </summary>
        public long ElapsedTime
        {
            get
            {
                return elapsedTime;
            }
            set
            {
                lock (this)
                {
                    elapsedTime = value;
                }
            }
        }
    }
}
