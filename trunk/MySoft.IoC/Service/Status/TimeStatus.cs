using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.IoC
{
    /// <summary>
    /// 时间状态信息
    /// </summary>
    [Serializable]
    public class TimeStatus : SecondStatus
    {
        private DateTime counterTime;
        /// <summary>
        /// 记数时间
        /// </summary>
        public DateTime CounterTime
        {
            get
            {
                return counterTime;
            }
            set
            {
                counterTime = value;
            }
        }
    }
}

