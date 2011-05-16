using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.IoC
{
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
