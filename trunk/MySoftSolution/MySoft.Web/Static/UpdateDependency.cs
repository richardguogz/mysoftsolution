using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Web
{
    /// <summary>
    /// �������������� SlidingUpdateTime �� AbsoluteUpdateTime 
    /// </summary>
    public interface IUpdateDependency
    {
        /// <summary>
        /// ��������
        /// </summary>
        UpdateType UpdateType { get; set; }

        /// <summary>
        /// �ж��Ƿ���Ҫ����
        /// </summary>
        /// <param name="currentDate"></param>
        /// <returns></returns>
        bool HasUpdate(DateTime currentDate);

        /// <summary>
        /// ������ʱ��
        /// </summary>
        DateTime LastUpdateTime { set; }
    }

    /// <summary>
    /// ��������
    /// </summary>
    public enum UpdateType
    {
        /// <summary>
        /// ֻ����һ��
        /// </summary>
        None,
        /// <summary>
        /// ÿ�궨ʱ����һ��
        /// </summary>
        Year,
        /// <summary>
        /// ÿ�¶�ʱ����һ��
        /// </summary>
        Month,
        /// <summary>
        /// ÿ�춨ʱ����һ��
        /// </summary>
        Day
    }

    /// <summary>
    /// ��ʱ���ɻ���
    /// </summary>
    public abstract class UpdateTime : IUpdateDependency
    {
        /// <summary>
        /// ����Ƿ���Ҫ����
        /// </summary>
        /// <returns></returns>
        public abstract bool HasUpdate(DateTime currentDate);

        /// <summary>
        /// ��������
        /// </summary>
        public abstract UpdateType UpdateType { get; set; }

        protected DateTime lastUpdateTime;
        /// <summary>
        /// ������ʱ��
        /// </summary>
        public DateTime LastUpdateTime
        {
            get { return lastUpdateTime; }
            set { lastUpdateTime = value; }
        }
    }

    /// <summary>
    /// ��ʱ���ɲ���
    /// </summary>
    public sealed class SlidingParamInfo
    {
        private DateTime beginDateTime;
        private DateTime endDateTime;

        public SlidingParamInfo(DateTime beginDateTime, DateTime endDateTime)
        {
            this.beginDateTime = beginDateTime;
            this.endDateTime = endDateTime;
        }

        /// <summary>
        /// ��⵱ǰ����ʱ���Ƿ������õķ�Χ��
        /// </summary>
        /// <param name="updateTime"></param>
        /// <returns></returns>
        internal bool CheckUpdate(UpdateType updateType, DateTime updateTime)
        {
            switch (updateType)
            {
                case UpdateType.Year:
                    if (beginDateTime.Year != updateTime.Year)
                    {
                        beginDateTime = beginDateTime.AddYears(1);
                        endDateTime = endDateTime.AddYears(1);
                    }
                    break;
                case UpdateType.Month:
                    if (beginDateTime.Month != updateTime.Month)
                    {
                        beginDateTime = beginDateTime.AddMonths(1);
                        endDateTime = endDateTime.AddMonths(1);
                    }
                    break;
                case UpdateType.Day:
                    if (beginDateTime.Day != updateTime.Day)
                    {
                        beginDateTime = beginDateTime.AddDays(1);
                        endDateTime = endDateTime.AddDays(1);
                    }
                    break;
            }

            return updateTime.Ticks >= beginDateTime.Ticks && updateTime.Ticks <= endDateTime.Ticks;
        }
    }

    /// <summary>
    /// ��ʱ���ɲ��ԣ������ʱ�䣩
    /// </summary>
    public sealed class SlidingUpdateTime : UpdateTime
    {
        private TimeSpan slidingTimeSpan;
        private SlidingParamInfo[] slidingTimeParams;

        private UpdateType updateType;
        /// <summary>
        /// ��������
        /// </summary>
        public override UpdateType UpdateType
        {
            get { return updateType; }
            set { updateType = value; }
        }

        /// <summary>
        /// ʱ����
        /// </summary>
        public TimeSpan SlidingTimeSpan
        {
            get { return slidingTimeSpan; }
            set { slidingTimeSpan = value; }
        }

        /// <summary>
        /// ��ʱ����ʱ��β���
        /// </summary>
        public SlidingParamInfo[] SlidingTimeParams
        {
            get { return slidingTimeParams; }
            set { slidingTimeParams = value; }
        }

        public SlidingUpdateTime() { }

        public SlidingUpdateTime(TimeSpan slidingTimeSpan)
        {
            this.slidingTimeSpan = slidingTimeSpan;
            this.updateType = UpdateType.None;
        }

        public SlidingUpdateTime(TimeSpan slidingTimeSpan, DateTime lastUpdateTime)
            : this(slidingTimeSpan)
        {
            this.lastUpdateTime = lastUpdateTime;
        }

        public SlidingUpdateTime(TimeSpan slidingTimeSpan, params SlidingParamInfo[] slidingTimeParams)
            : this(slidingTimeSpan)
        {
            this.slidingTimeParams = slidingTimeParams;
        }

        public SlidingUpdateTime(TimeSpan slidingTimeSpan, DateTime lastUpdateTime, params SlidingParamInfo[] slidingTimeParams)
            : this(slidingTimeSpan, lastUpdateTime)
        {
            this.slidingTimeParams = slidingTimeParams;
        }

        #region �����Ͳ���

        public SlidingUpdateTime(UpdateType updateType, TimeSpan slidingTimeSpan)
            : this(slidingTimeSpan)
        {
            this.updateType = updateType;
        }

        public SlidingUpdateTime(UpdateType updateType, TimeSpan slidingTimeSpan, DateTime lastUpdateTime)
            : this(slidingTimeSpan, lastUpdateTime)
        {
            this.updateType = updateType;
        }

        public SlidingUpdateTime(UpdateType updateType, TimeSpan slidingTimeSpan, params SlidingParamInfo[] slidingTimeParams)
            : this(slidingTimeSpan, slidingTimeParams)
        {
            this.updateType = updateType;
        }

        public SlidingUpdateTime(UpdateType updateType, TimeSpan slidingTimeSpan, DateTime lastUpdateTime, params SlidingParamInfo[] slidingTimeParams)
            : this(slidingTimeSpan, lastUpdateTime, slidingTimeParams)
        {
            this.updateType = updateType;
        }

        #endregion

        public override bool HasUpdate(DateTime currentDate)
        {
            DateTime updateTime = lastUpdateTime.Add(slidingTimeSpan);
            bool isUpdate = currentDate.Ticks >= updateTime.Ticks;
            if (isUpdate && lastUpdateTime != DateTime.MinValue)
            {
                if (slidingTimeParams != null)
                {
                    foreach (SlidingParamInfo slidingTimeParam in slidingTimeParams)
                    {
                        if (slidingTimeParam.CheckUpdate(updateType, currentDate)) return true;
                    }
                    isUpdate = false;
                }
            }
            return isUpdate;
        }
    }

    /// <summary>
    /// ����ʱ�����ɲ���
    /// </summary>
    public sealed class AbsoluteUpdateTime : UpdateTime
    {
        private DateTime[] absoluteDateTimes;
        private UpdateType updateType;
        /// <summary>
        /// ��������
        /// </summary>
        public override UpdateType UpdateType
        {
            get { return updateType; }
            set { updateType = value; }
        }

        /// <summary>
        /// ʱ����
        /// </summary>
        public DateTime[] AbsoluteDateTimes
        {
            get { return absoluteDateTimes; }
            set { absoluteDateTimes = value; }
        }

        public AbsoluteUpdateTime() { }

        public AbsoluteUpdateTime(params DateTime[] absoluteDateTimes)
        {
            this.absoluteDateTimes = absoluteDateTimes;
            this.updateType = UpdateType.None;
        }

        public AbsoluteUpdateTime(UpdateType updateType, params DateTime[] absoluteDateTimes)
            : this(absoluteDateTimes)
        {
            this.updateType = updateType;
        }

        public override bool HasUpdate(DateTime currentDate)
        {
            int index = 0;
            bool isUpdate = false;
            foreach (DateTime absoluteDateTime in absoluteDateTimes)
            {
                isUpdate = currentDate.Ticks >= absoluteDateTime.Ticks;
                if (isUpdate)
                {
                    switch (updateType)
                    {
                        case UpdateType.Year:
                            absoluteDateTimes[index] = absoluteDateTime.AddYears(1);
                            break;
                        case UpdateType.Month:
                            absoluteDateTimes[index] = absoluteDateTime.AddMonths(1);
                            break;
                        case UpdateType.Day:
                            absoluteDateTimes[index] = absoluteDateTime.AddDays(1);
                            break;
                        case UpdateType.None:
                            absoluteDateTimes[index] = DateTime.MaxValue;
                            break;
                    }
                    break;
                }
                index++;
            }
            return isUpdate;
        }
    }
}
