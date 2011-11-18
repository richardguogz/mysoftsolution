﻿using System;

namespace MySoft.IoC.Status
{
    /// <summary>
    /// 每秒服务器状态信息
    /// </summary>
    [Serializable]
    public abstract class SecondStatus
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

        /// <summary>
        /// 请求数
        /// </summary>
        public int RequestCount
        {
            get
            {
                return successCount + errorCount;
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

        /// <summary>
        /// 平均数据流量（每次请求）
        /// </summary>
        public double AverageDataFlow
        {
            get
            {
                if (this.RequestCount > 0)
                    return Math.Round((dataFlow * 1.0) / (this.RequestCount * 1.0), 4);
                else
                    return 0;
            }
        }

        /// <summary>
        /// 平均耗时（每次请求）
        /// </summary>
        public double AverageElapsedTime
        {
            get
            {
                if (this.RequestCount > 0)
                    return Math.Round((elapsedTime * 1.0) / (this.RequestCount * 1.0), 4);
                else
                    return 0;
            }
        }
    }
}