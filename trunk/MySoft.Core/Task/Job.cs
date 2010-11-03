using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Reflection;
using MySoft.Core;

namespace MySoft.Task
{
    /// <summary>
    /// ����ʵ��
    /// </summary>
    [Serializable]
    public class Job : ILogable
    {
        /// <summary>
        /// �¼�������־
        /// </summary>
        public event LogEventHandler OnLog;

        /// <summary>
        /// �Ƿ�ע������־
        /// </summary>
        internal bool IsRegisterLog
        {
            get
            {
                return OnLog != null;
            }
        }

        private string _Name;

        /// <summary>
        /// ��������
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private DateTime _BeginDate;

        /// <summary>
        /// ����ʼ����
        /// </summary>
        public DateTime BeginDate
        {
            get { return _BeginDate; }
            set { _BeginDate = value; }
        }

        private DateTime _EndDate;

        /// <summary>
        /// �����������
        /// </summary>
        public DateTime EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; }
        }

        private string _BeginTime;

        /// <summary>
        /// ����ʼʱ��
        /// </summary>
        public string BeginTime
        {
            get { return _BeginTime; }
            set { _BeginTime = value; }
        }

        private string _EndTime;

        /// <summary>
        /// �������ʱ��
        /// </summary>
        public string EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }


        private int _Interval;

        /// <summary>
        /// ����ѭ��ִ��ʱ��������λ�����룩
        /// </summary>
        public int Interval
        {
            get { return _Interval; }
            set { _Interval = value; }
        }

        private string _AssemblyName;

        /// <summary>
        /// ��������
        /// </summary>
        public string AssemblyName
        {
            get { return _AssemblyName; }
            set { _AssemblyName = value; }
        }

        private string _ClassName;

        /// <summary>
        /// ����ȫ�ƣ�����ִ����ڷ����ڸ������涨�壩
        /// </summary>
        public string ClassName
        {
            get { return _ClassName; }
            set { _ClassName = value; }
        }

        private JobState _State = JobState.Stop;

        /// <summary>
        /// ����״̬
        /// </summary>
        public JobState State
        {
            get { return _State; }
            set { _State = value; }
        }

        private string _LatestRunTime;

        /// <summary>
        /// ���һ������ʱ��
        /// </summary>
        public string LatestRunTime
        {
            get { return _LatestRunTime; }
            set { _LatestRunTime = value; }
        }

        private bool _IsStopIfException = false;

        private MySoftException _LatestException = null;

        /// <summary>
        /// ��������ʱ����������쳣
        /// </summary>
        public MySoftException LatestException
        {
            get { return _LatestException; }
            set { _LatestException = value; }
        }

        private int _ExceptionCount = 0;

        /// <summary>
        /// �쳣����
        /// </summary>
        public int ExceptionCount
        {
            get { return _ExceptionCount; }
            set { _ExceptionCount = value; }
        }

        private object _Param;
        /// <summary>
        /// ����
        /// </summary>
        public object Param
        {
            get { return _Param; }
            set { _Param = value; }
        }


        /// <summary>
        /// ���ݵ�ǰʱ���ж������Ƿ���Ҫִ��
        /// </summary>
        public bool IsRun()
        {
            DateTime now = DateTime.Now;
            TimeSpan nowTime = now.TimeOfDay;
            TimeSpan theTime = TimeSpan.Zero;

            if (now < _BeginDate)
            {
                return false;
            }

            if (now > _EndDate)
            {
                return false;
            }

            theTime = TimeSpan.Parse(_BeginTime);
            if (nowTime < theTime)
            {
                return false;
            }

            theTime = TimeSpan.Parse(_EndTime);
            if (nowTime > theTime)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// ִ������
        /// </summary>
        public void Execute()
        {
            while (true)
            {
                if (this.IsRun() && this._State == JobState.Running)
                {
                    //��¼���ִ��ʱ��
                    _LatestRunTime = DateTime.Now.ToString();

                    try
                    {
                        WriteLog(string.Format("����ִ������[{0}]......", this.Name));

                        //ִ������
                        Assembly assembly = Assembly.Load(_AssemblyName);
                        Type type = assembly.GetType(_ClassName);
                        object obj = Activator.CreateInstance(type);
                        MethodInfo mi = type.GetMethod("Run");

                        if (_Param != null)
                        {
                            DynamicCalls.GetMethodInvoker(mi).Invoke(obj, new object[] { _Param });
                        }
                        else
                        {
                            DynamicCalls.GetMethodInvoker(mi).Invoke(obj, null);
                        }

                        WriteLog(string.Format("ִ������[{0}]�ɹ���", this.Name));
                    }
                    catch (Exception ex)
                    {
                        if (_IsStopIfException)
                        {
                            _State = JobState.Stop;
                            TaskThreadPool.Instance.Threads[_Name].Abort();
                        }

                        _ExceptionCount = _ExceptionCount + 1;
                        _LatestException = new MySoftException(ExceptionType.TaskException, "Task����ִ��ʧ�ܣ�", ex);

                        WriteLog(string.Format("ִ������[{0}]ʧ�ܣ�����{1}��", this.Name, ex.Message));
                    }
                }

                Thread.Sleep(_Interval);
            }
        }

        void WriteLog(string log)
        {
            if (OnLog != null)
            {
                OnLog(log);
            }
        }
    }
}
