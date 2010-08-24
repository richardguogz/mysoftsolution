using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Core.Remoting
{
    /// <summary>
    /// ���ÿ���ͻ��˵Ŀ��÷�����
    /// </summary>
    public class RemotingHostCheck : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly RemotingHostCheck Instance = new RemotingHostCheck();

        System.Timers.Timer timer = null;
        RemotingClientConfiguration cfg = null;
        IList<string> _CheckLog = new List<string>();

        /// <summary>
        /// �����������־
        /// </summary>
        public IList<string> CheckLog
        {
            get { return _CheckLog; }
            set { _CheckLog = value; }
        }

        /// <summary>
        /// ��ʼ���
        /// </summary>
        public void DoCheck()
        {
            cfg = RemotingClientConfiguration.GetConfig();

            if (cfg.IsCheckServer)
            {
                timer = new System.Timers.Timer(cfg.Interval);
                timer.Enabled = true;
                timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
                timer.Start();
            }
        }

        //���ÿ��RemotingHost��Ĭ�Ͽ��÷�����
        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            timer.Stop();

            Dictionary<string, RemotingHost> hosts = cfg.RemotingHosts;

            foreach (KeyValuePair<string, RemotingHost> kvp in hosts)
            {
                if (!kvp.Value.IsChecking)
                {
                    CheckRemotingHost(kvp.Value);
                }
            }

            timer.Start();
        }

        /// <summary>
        /// ��ȡ���õķ�������ַ
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public string GetUsableServerUrl(RemotingHost host)
        {
            if (AppDomain.CurrentDomain.GetData(host.Name) == null)
            {
                string url = host.Servers[host.DefaultServer].ServerUrl;
                AppDomain.CurrentDomain.SetData(host.Name, url);
            }

            return AppDomain.CurrentDomain.GetData(host.Name).ToString();
        }

        void CheckRemotingHost(RemotingHost host)
        {
            string objectUrl = string.Empty;
            RemotingServer defaultServer = host.Servers[host.DefaultServer];
            string usableServerUrl = this.GetUsableServerUrl(host);
            bool flag = false;  //�Ƿ���Ҫ���赱ǰ���÷�������־

            host.IsChecking = true;

            //���ȼ�鵱ǰ���÷�����
            try
            {
                objectUrl = cfg.GetRemoteObjectUrl(usableServerUrl, "RemotingTest");
                IRemotingTest t = (IRemotingTest)Activator.GetObject(typeof(RemotingTest), objectUrl);
                t.GetDate();
                WriteLog(host.Name, usableServerUrl, true, "ok");
            }
            catch (Exception ex)
            {
                flag = true;    //��Ҫ���赱ǰ���÷�������־
                WriteLog(host.Name, usableServerUrl, false, ex.Message);
            }

            //����ǰ���÷���������Ĭ�Ϸ����������ټ��Ĭ�Ϸ�������������ã���ԭ
            if (defaultServer.ServerUrl != usableServerUrl)
            {
                try
                {
                    objectUrl = cfg.GetRemoteObjectUrl(defaultServer.ServerUrl, "RemotingTest");
                    IRemotingTest t = (IRemotingTest)Activator.GetObject(typeof(RemotingTest), objectUrl);
                    t.GetDate();

                    AppDomain.CurrentDomain.SetData(host.Name, defaultServer.ServerUrl);
                    WriteLog(host.Name, defaultServer.ServerUrl, true, "ok");
                }
                catch (Exception ex)
                {
                    WriteLog(host.Name, defaultServer.ServerUrl, false, ex.Message);
                }
            }

            string serverUrl = string.Empty;

            //���������������������״̬
            foreach (KeyValuePair<string, RemotingServer> kvp in host.Servers)
            {
                serverUrl = kvp.Value.ServerUrl;

                if (serverUrl == usableServerUrl) continue;
                if (serverUrl == defaultServer.ServerUrl) continue;

                objectUrl = cfg.GetRemoteObjectUrl(serverUrl, "RemotingTest");

                try
                {
                    IRemotingTest t = (IRemotingTest)Activator.GetObject(typeof(RemotingTest), objectUrl);
                    t.GetDate();

                    if (flag)
                    {
                        AppDomain.CurrentDomain.SetData(host.Name, serverUrl);  //���赱ǰ���÷�����
                        flag = false;
                        WriteLog(host.Name, serverUrl, true, "ok");
                    }

                    WriteLog(host.Name, serverUrl, false, "ok");
                }
                catch (Exception ex)
                {
                    WriteLog(host.Name, serverUrl, false, ex.Message);
                }
            }

            host.IsChecking = false;
        }

        #region IDisposable ��Ա

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (timer != null)
            {
                timer.Stop();
            }
        }

        #endregion

        //д��־
        private void WriteLog(string hostName, string serverUrl, bool isCurrentServer, string msg)
        {
            //string key = string.Format("{0}${1}", hostName, serverUrl);
            string log = string.Format("RemotingHost��{0}����������{1}���Ƿ�ǰ��������{2}��״̬��{3}����¼ʱ�䣺{4}", hostName, serverUrl, isCurrentServer.ToString(), msg, DateTime.Now.ToString());

            lock (_CheckLog)
            {
                _CheckLog.Add(log);
            }
        }
    }
}
