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

    /// <summary>
    /// 服务器状态信息
    /// </summary>
    [Serializable]
    public class ServerStatus : SecondStatus
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
                    return Math.Round((base.DataFlow * 1.0) / (totalSeconds * 1.0), 4);
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
                    return Math.Round((base.RequestCount * 1.0) / (totalSeconds * 1.0), 4);
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
                if (base.RequestCount > 0)
                    return Math.Round((base.ElapsedTime * 1.0) / (base.RequestCount * 1.0), 4);
                else
                    return 0;
            }
        }

        /// <summary>
        /// 平均成功数（每秒）
        /// </summary>
        public double AverageSuccessCount
        {
            get
            {
                if (totalSeconds > 0)
                    return Math.Round((base.SuccessCount * 1.0) / (totalSeconds * 1.0), 4);
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
                    return Math.Round((base.ErrorCount * 1.0) / (totalSeconds * 1.0), 4);
                else
                    return 0;
            }
        }
    }
}
