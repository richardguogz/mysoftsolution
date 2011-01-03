using System.Collections.Generic;
using System.Threading;

namespace MySoft.Task
{
    /// <summary>
    /// �����̳߳�
    /// </summary>
    public class TaskThreadPool
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly TaskThreadPool Instance = new TaskThreadPool();

        private Dictionary<string, Thread> _Threads;

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, Thread> Threads
        {
            get { return _Threads; }
            set { _Threads = value; }
        }

        Dictionary<string, Job> jobs = TaskConfiguration.GetConfig().Jobs;

        private TaskThreadPool()
        {
            _Threads = new Dictionary<string, Thread>();

            Thread thread = new Thread(RunTask);
            thread.Start();
        }

        void RunTask()
        {
            //����߳�״̬
            foreach (KeyValuePair<string, Job> kvp in jobs)
            {
                Job job = kvp.Value;

                if (_Threads.ContainsKey(job.Name))
                {
                    Thread thread = _Threads[job.Name];

                    if (thread.ThreadState == ThreadState.Stopped && job.State == JobState.Running)
                    {
                        thread.Start();
                    }
                }
            }

            //����̳߳أ�ɾ�������߳�
            foreach (KeyValuePair<string, Thread> kvp in _Threads)
            {
                if (!jobs.ContainsKey(kvp.Key))
                {
                    _Threads.Remove(kvp.Key);
                }
            }

            //���10��
            Thread.Sleep(10000);
        }
    }
}
