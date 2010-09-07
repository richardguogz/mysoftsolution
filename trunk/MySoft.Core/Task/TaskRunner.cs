using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MySoft.Core.Task
{
    /// <summary>
    /// �ƻ�����ִ����
    /// </summary>
    public class TaskRunner : MarshalByRefObject, ILogable
    {
        private TaskConfiguration cfg;

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, Job> JobList
        {
            get { return cfg.Jobs; }
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="job"></param>
        public void AddJob(Job job)
        {
            if (!job.IsRegisterLog) job.OnLog += OnLog;

            cfg.Jobs.Add(job.Name, job);
        }

        /// <summary>
        /// ʵ����TaskRunner
        /// </summary>
        public TaskRunner()
        {
            this.cfg = new TaskConfiguration();
        }

        /// <summary>
        /// ʵ����TaskRunner
        /// </summary>
        public TaskRunner(TaskConfiguration cfg)
        {
            this.cfg = cfg;
        }

        /// <summary>
        /// ִ�мƻ�����
        /// </summary>
        public void RunSchemeTask()
        {
            if (cfg != null)
            {
                Dictionary<string, Job> jobs = cfg.Jobs;
                Dictionary<string, Thread> threads = TaskThreadPool.Instance.Threads;

                foreach (KeyValuePair<string, Job> kvp in jobs)
                {
                    Job job = kvp.Value;

                    if (!job.IsRegisterLog) job.OnLog += OnLog;

                    if (!threads.ContainsKey(job.Name))
                    {
                        job.State = JobState.Running;
                        Thread thread = new Thread(new ThreadStart(job.Execute));
                        thread.IsBackground = true;
                        threads.Add(job.Name, thread);
                        thread.Start();

                        WriteLog(string.Format("�ƻ�����[{0}]������������������{1}�����򼯣�{2}", job.Name, job.ClassName, job.AssemblyName));
                    }
                }
            }
            else
            {
                WriteLog("��ǰ�����ļ�û������Ҫִ�еļƻ�����");
            }
        }

        /// <summary>
        /// ����ָ������
        /// </summary>
        /// <param name="jobName"></param>
        public void Start(string jobName)
        {
            if (cfg == null) return;
            if (cfg.Jobs == null) return;
            if (cfg.Jobs.Count == 0) return;

            if (cfg.Jobs.ContainsKey(jobName))
            {
                Job job = cfg.Jobs[jobName];

                if (job.State == JobState.Stop)
                {
                    job.State = JobState.Running;

                    Thread t = TaskThreadPool.Instance.Threads[job.Name];
                    t = null;
                    t = new Thread(new ThreadStart(job.Execute));
                    t.Start();
                    TaskThreadPool.Instance.Threads[job.Name] = t;

                    WriteLog(string.Format("�ƻ�����[{0}]������������������{1}�����򼯣�{2}", job.Name, job.ClassName, job.AssemblyName));
                }
            }
        }

        /// <summary>
        /// ָֹͣ������
        /// </summary>
        /// <param name="jobName"></param>
        public void Stop(string jobName)
        {
            if (cfg == null) return;
            if (cfg.Jobs == null) return;
            if (cfg.Jobs.Count == 0) return;

            if (cfg.Jobs.ContainsKey(jobName))
            {
                Job job = cfg.Jobs[jobName];

                if (job.State == JobState.Running)
                {
                    job.State = JobState.Stop;

                    try
                    {
                        TaskThreadPool.Instance.Threads[job.Name].Abort();
                    }
                    catch { }

                    WriteLog(string.Format("�ƻ�����[{0}]��ֹͣ������������{1}�����򼯣�{2}", job.Name, job.ClassName, job.AssemblyName));
                }
            }
        }

        /// <summary>
        /// ������������
        /// </summary>
        public void StartAll()
        {
            foreach (KeyValuePair<string, Job> kvp in cfg.Jobs)
            {
                Start(kvp.Key);
            }
        }

        /// <summary>
        /// ֹͣ��������
        /// </summary>
        public void StopAll()
        {
            foreach (KeyValuePair<string, Job> kvp in cfg.Jobs)
            {
                Stop(kvp.Key);
            }
        }

        #region ILogable ��Ա

        /// <summary>
        /// 
        /// </summary>
        public event LogEventHandler OnLog;

        #endregion

        void WriteLog(string log)
        {
            if (OnLog != null)
            {
                OnLog(log);
            }
        }
    }
}
