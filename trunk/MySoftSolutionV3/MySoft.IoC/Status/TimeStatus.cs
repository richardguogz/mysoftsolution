﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.IoC.Status
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
    public class TimeStatusCollection
    {
        private int maxCount;
        private IDictionary<string, TimeStatus> dictStatus;
        public TimeStatusCollection(int maxCount)
        {
            this.maxCount = maxCount;
            this.dictStatus = new Dictionary<string, TimeStatus>();
        }

        /// <summary>
        /// 获取或创建
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public TimeStatus GetOrCreate(DateTime value)
        {
            lock (dictStatus)
            {
                string key = value.ToString("yyyyMMddHHmmss");
                if (!dictStatus.ContainsKey(key))
                {
                    //如果总数大于传入的总数
                    if (dictStatus.Count >= maxCount)
                    {
                        var firstKey = dictStatus.Keys.FirstOrDefault();
                        if (firstKey != null) dictStatus.Remove(firstKey);
                    }

                    dictStatus[key] = new TimeStatus { CounterTime = value };
                }

                return dictStatus[key];
            }
        }

        /// <summary>
        /// 返回列表
        /// </summary>
        /// <returns></returns>
        public IList<TimeStatus> ToList()
        {
            lock (dictStatus)
            {
                return dictStatus.Values.ToList();
            }
        }

        /// <summary>
        /// 获取最后一条
        /// </summary>
        /// <returns></returns>
        public TimeStatus GetLast()
        {
            lock (dictStatus)
            {
                var status = dictStatus.Values.LastOrDefault();
                if (status == null)
                    return new TimeStatus { CounterTime = DateTime.Now };
                else
                    return status;
            }
        }

        /// <summary>
        /// 清除字典中的数据
        /// </summary>
        public void Clear()
        {
            lock (dictStatus)
            {
                dictStatus.Clear();
            }
        }
    }
}

