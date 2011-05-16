using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.IoC
{
    /// <summary>
    /// 最高峰状态
    /// </summary>
    [Serializable]
    public class HighestStatus : SecondStatus
    {
        private DateTime dataFlowOccurTime;
        /// <summary>
        /// 最高流量发生时间
        /// </summary>
        public DateTime DataFlowOccurTime
        {
            get
            {
                return dataFlowOccurTime;
            }
            set
            {
                dataFlowOccurTime = value;
            }
        }

        private DateTime requestCountOccurTime;
        /// <summary>
        /// 最大请求发生时间
        /// </summary>
        public DateTime RequestCountOccurTime
        {
            get
            {
                return requestCountOccurTime;
            }
            set
            {
                requestCountOccurTime = value;
            }
        }

        private DateTime successCountOccurTime;
        /// <summary>
        /// 最多成功请求发生时间
        /// </summary>
        public DateTime SuccessCountOccurTime
        {
            get
            {
                return successCountOccurTime;
            }
            set
            {
                successCountOccurTime = value;
            }
        }

        private DateTime errorCountOccurTime;
        /// <summary>
        /// 最多错误请求发生时间
        /// </summary>
        public DateTime ErrorCountOccurTime
        {
            get
            {
                return errorCountOccurTime;
            }
            set
            {
                errorCountOccurTime = value;
            }
        }

        private DateTime elapsedTimeOccurTime;
        /// <summary>
        /// 最耗时请求发生时间
        /// </summary>
        public DateTime ElapsedTimeOccurTime
        {
            get
            {
                return elapsedTimeOccurTime;
            }
            set
            {
                elapsedTimeOccurTime = value;
            }
        }
    }
}
