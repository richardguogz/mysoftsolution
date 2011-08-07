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

    /// <summary>
    /// 时间状态集合
    /// </summary>
    [Serializable]
    public class TimeStatusCollection : Dictionary<string, TimeStatus>
    {
        private int maxCount;
        public TimeStatusCollection(int maxCount)
        {
            this.maxCount = maxCount;
        }

        /// <summary>
        /// 获取或创建
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public TimeStatus GetOrCreate(DateTime value)
        {
            lock (this)
            {
                string key = value.ToString("yyyyMMddHHmmss");
                if (!base.ContainsKey(key))
                {
                    //如果总数大于传入的总数
                    if (base.Count >= maxCount)
                    {
                        var firstKey = base.Keys.FirstOrDefault();
                        if (firstKey != null) base.Remove(firstKey);
                    }

                    base[key] = new TimeStatus { CounterTime = value };
                }

                return base[key];
            }
        }

        /// <summary>
        /// 返回列表
        /// </summary>
        /// <returns></returns>
        public IList<TimeStatus> ToList()
        {
            lock (this)
            {
                return base.Values.ToList();
            }
        }

        /// <summary>
        /// 获取最后一条
        /// </summary>
        /// <returns></returns>
        public TimeStatus GetLast()
        {
            lock (this)
            {
                var status = base.Values.LastOrDefault();
                if (status == null)
                    return new TimeStatus { CounterTime = DateTime.Now };
                else
                    return status;
            }
        }
    }
}

