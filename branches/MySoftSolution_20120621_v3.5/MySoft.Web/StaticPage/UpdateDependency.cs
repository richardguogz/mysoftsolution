using System;

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
        bool HasUpdate(DateTime currentDate, int inMinutes);

        /// <summary>
        /// ������ʱ��
        /// </summary>
        DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// �Ƿ���³ɹ�
        /// </summary>
        bool UpdateSuccess { get; set; }
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
    public abstract class AbstractUpdateDependency : IUpdateDependency
    {
        /// <summary>
        /// ����Ƿ���Ҫ����
        /// </summary>
        /// <returns></returns>
        public abstract bool HasUpdate(DateTime currentDate, int inMinutes);

        protected UpdateType updateType;
        /// <summary>
        /// ��������
        /// </summary>
        public UpdateType UpdateType
        {
            get { return updateType; }
            set { updateType = value; }
        }

        protected DateTime lastUpdateTime = DateTime.Now;
        /// <summary>
        /// ������ʱ��
        /// </summary>
        public DateTime LastUpdateTime
        {
            get { return lastUpdateTime; }
            set { lastUpdateTime = value; }
        }

        protected bool updateSuccess;
        /// <summary>
        /// �Ƿ���³ɹ�
        /// </summary>
        public bool UpdateSuccess
        {
            get { return updateSuccess; }
            set { updateSuccess = value; }
        }

        public AbstractUpdateDependency()
        {
            this.updateType = UpdateType.Day;
            this.updateSuccess = true;
        }
    }

    /// <summary>
    /// ��ʱ���ɲ��ԣ����ڿ�������
    /// </summary>
    public sealed class SlidingDateTimeRegion
    {
        private DateTime beginDateTime;
        private DateTime endDateTime;

        public SlidingDateTimeRegion(DateTime beginDateTime, DateTime endDateTime)
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
            var checkTime = updateTime;
            var beginTime = beginDateTime;
            var endTime = endDateTime;

            switch (updateType)
            {
                case UpdateType.Year:
                    checkTime = DateTime.Parse(checkTime.ToString("1900-MM-dd HH:mm:ss"));
                    beginTime = DateTime.Parse(beginTime.ToString("1900-MM-dd HH:mm:ss"));
                    endTime = DateTime.Parse(endTime.ToString("1900-MM-dd HH:mm:ss"));
                    break;
                case UpdateType.Month:
                    checkTime = DateTime.Parse(checkTime.ToString("1900-01-dd HH:mm:ss"));
                    beginTime = DateTime.Parse(beginTime.ToString("1900-01-dd HH:mm:ss"));
                    endTime = DateTime.Parse(endTime.ToString("1900-01-dd HH:mm:ss"));
                    break;
                case UpdateType.Day:
                    checkTime = DateTime.Parse(checkTime.ToString("1900-01-01 HH:mm:ss"));
                    beginTime = DateTime.Parse(beginTime.ToString("1900-01-01 HH:mm:ss"));
                    endTime = DateTime.Parse(endTime.ToString("1900-01-01 HH:mm:ss"));
                    break;
            }

            //�ж��Ƿ���������
            return checkTime.Ticks >= beginTime.Ticks && checkTime.Ticks <= endTime.Ticks;
        }
    }

    /// <summary>
    /// ��ʱ���ɲ��ԣ������ʱ�䣩
    /// </summary>
    public sealed class SlidingUpdateDependency : AbstractUpdateDependency
    {
        private TimeSpan slidingTimeSpan;
        private SlidingDateTimeRegion[] slidingTimeParams;

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
        public SlidingDateTimeRegion[] SlidingTimeParams
        {
            get { return slidingTimeParams; }
            set { slidingTimeParams = value; }
        }

        public SlidingUpdateDependency() { }

        public SlidingUpdateDependency(TimeSpan slidingTimeSpan)
        {
            this.slidingTimeSpan = slidingTimeSpan;
        }

        public SlidingUpdateDependency(TimeSpan slidingTimeSpan, DateTime lastUpdateTime)
            : this(slidingTimeSpan)
        {
            this.lastUpdateTime = lastUpdateTime;
        }

        public SlidingUpdateDependency(TimeSpan slidingTimeSpan, params SlidingDateTimeRegion[] slidingTimeParams)
            : this(slidingTimeSpan)
        {
            this.slidingTimeParams = slidingTimeParams;
        }

        public SlidingUpdateDependency(TimeSpan slidingTimeSpan, DateTime lastUpdateTime, params SlidingDateTimeRegion[] slidingTimeParams)
            : this(slidingTimeSpan, lastUpdateTime)
        {
            this.slidingTimeParams = slidingTimeParams;
        }

        #region �����Ͳ���

        public SlidingUpdateDependency(UpdateType updateType, TimeSpan slidingTimeSpan)
            : this(slidingTimeSpan)
        {
            this.updateType = updateType;
        }

        public SlidingUpdateDependency(UpdateType updateType, TimeSpan slidingTimeSpan, DateTime lastUpdateTime)
            : this(slidingTimeSpan, lastUpdateTime)
        {
            this.updateType = updateType;
        }

        public SlidingUpdateDependency(UpdateType updateType, TimeSpan slidingTimeSpan, params SlidingDateTimeRegion[] slidingTimeParams)
            : this(slidingTimeSpan, slidingTimeParams)
        {
            this.updateType = updateType;
        }

        public SlidingUpdateDependency(UpdateType updateType, TimeSpan slidingTimeSpan, DateTime lastUpdateTime, params SlidingDateTimeRegion[] slidingTimeParams)
            : this(slidingTimeSpan, lastUpdateTime, slidingTimeParams)
        {
            this.updateType = updateType;
        }

        #endregion

        public override bool HasUpdate(DateTime currentDate, int inMinutes)
        {
            //����ʱ��Ϊ������ʱ�䣬ֱ�ӷ���true
            if (currentDate == DateTime.MaxValue) return true;

            //�������ʧ�ܣ��ж�ʱ��󷵻�true
            if (!updateSuccess && currentDate.Ticks > lastUpdateTime.Ticks)
                return true;

            if (!updateSuccess) return false;

            DateTime updateTime = lastUpdateTime.Add(slidingTimeSpan);

            var span = currentDate.Subtract(updateTime);
            bool isUpdate = span.Ticks > 0 && span.TotalMinutes < inMinutes;
            if (isUpdate)
            {
                if (slidingTimeParams != null && slidingTimeParams.Length > 0)
                {
                    isUpdate = false;
                    foreach (SlidingDateTimeRegion slidingTimeParam in slidingTimeParams)
                    {
                        isUpdate = slidingTimeParam.CheckUpdate(updateType, currentDate);
                        if (isUpdate) break;
                    }
                }
            }
            return isUpdate;
        }
    }

    /// <summary>
    /// ����ʱ�����ɲ���
    /// </summary>
    public sealed class AbsoluteUpdateDependency : AbstractUpdateDependency
    {
        private DateTime[] absoluteDateTimes;

        /// <summary>
        /// ʱ����
        /// </summary>
        public DateTime[] AbsoluteDateTimes
        {
            get { return absoluteDateTimes; }
            set { absoluteDateTimes = value; }
        }

        public AbsoluteUpdateDependency() { }

        public AbsoluteUpdateDependency(params DateTime[] absoluteDateTimes)
        {
            this.absoluteDateTimes = absoluteDateTimes;
        }

        public AbsoluteUpdateDependency(UpdateType updateType, params DateTime[] absoluteDateTimes)
            : this(absoluteDateTimes)
        {
            this.updateType = updateType;
        }

        public override bool HasUpdate(DateTime currentDate, int inMinutes)
        {
            //����ʱ��Ϊ������ʱ�䣬ֱ�ӷ���true
            if (currentDate == DateTime.MaxValue) return true;

            //�������ʧ�ܣ��ж�ʱ��󷵻�true
            if (!updateSuccess && currentDate.Ticks > lastUpdateTime.Ticks)
                return true;

            if (!updateSuccess) return false;

            bool isUpdate = false;
            var checkTime = currentDate;

            foreach (DateTime absoluteDateTime in absoluteDateTimes)
            {
                var absoluteTime = absoluteDateTime;

                switch (updateType)
                {
                    case UpdateType.Year:
                        checkTime = DateTime.Parse(checkTime.ToString("1900-MM-dd HH:mm:ss"));
                        absoluteTime = DateTime.Parse(absoluteTime.ToString("1900-MM-dd HH:mm:ss"));
                        break;
                    case UpdateType.Month:
                        checkTime = DateTime.Parse(checkTime.ToString("1900-01-dd HH:mm:ss"));
                        absoluteTime = DateTime.Parse(absoluteTime.ToString("1900-01-dd HH:mm:ss"));
                        break;
                    case UpdateType.Day:
                        checkTime = DateTime.Parse(checkTime.ToString("1900-01-01 HH:mm:ss"));
                        absoluteTime = DateTime.Parse(absoluteTime.ToString("1900-01-01 HH:mm:ss"));
                        break;
                }

                var span = checkTime.Subtract(absoluteTime);
                isUpdate = span.Ticks > 0 && span.TotalMinutes < inMinutes;
                if (isUpdate) break;
            }

            return isUpdate;
        }
    }
}
