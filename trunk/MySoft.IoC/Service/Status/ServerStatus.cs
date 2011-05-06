using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySoft.Net.Sockets;
using System.Net;

namespace MySoft.IoC
{
    /// <summary>
    /// 每秒服务器状态信息
    /// </summary>
    [Serializable]
    public class TimeServerStatus
    {
        protected long dataFlow;
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

        protected int requestCount;
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

        protected int errorCount;
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

        protected long elapsedTime;
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

    /// <summary>
    /// 服务器状态信息
    /// </summary>
    [Serializable]
    public class ServerStatus : TimeServerStatus
    {
        private int totalSeconds;
        /// <summary>
        /// 运行总时间
        /// </summary>
        public int TotalSeconds
        {
            get
            {
                return totalSeconds;
            }
            set
            {
                lock (this)
                {
                    totalSeconds = value;
                }
            }
        }

        /// <summary>
        /// 平均数据流量（每秒）
        /// </summary>
        public double AverageDataFlow
        {
            get
            {
                if (totalSeconds > 0)
                    return Math.Round((dataFlow * 1.0) / (totalSeconds * 1.0), 4);
                else
                    return 0;
            }
        }

        /// <summary>
        /// 平均请求数（每秒）
        /// </summary>
        public double AverageRequestCount
        {
            get
            {
                if (totalSeconds > 0)
                    return Math.Round((requestCount * 1.0) / (totalSeconds * 1.0), 4);
                else
                    return 0;
            }
        }

        /// <summary>
        /// 平均耗时
        /// </summary>
        public double AverageElapsedTime
        {
            get
            {
                if (requestCount > 0)
                    return Math.Round((elapsedTime * 1.0) / (requestCount * 1.0), 4);
                else
                    return 0;
            }
        }

        /// <summary>
        /// 平均错误数（每秒）
        /// </summary>
        public double AverageErrorCount
        {
            get
            {
                if (totalSeconds > 0)
                    return Math.Round((errorCount * 1.0) / (totalSeconds * 1.0), 4);
                else
                    return 0;
            }
        }
    }
}
