using System;
using System.Collections.Generic;
using MySoft.Logger;
using MySoft.Remoting.Configuration;

namespace MySoft.Remoting
{
    /// <summary>
    /// Remoting�ͻ��˹�����
    /// </summary>
    /// <typeparam name="T">һ��Ϊ�ӿ�����</typeparam>
    public class RemotingClientUtil<T> : ILogable
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly RemotingClientUtil<T> Instance = new RemotingClientUtil<T>();
        RemotingClientConfiguration _RemotingConfiguration;
        Dictionary<string, T> _RemoteObjects = new Dictionary<string, T>();

        /// <summary>
        /// Remoting Configuration
        /// </summary>
        public RemotingClientConfiguration RemotingConfiguration
        {
            get { return _RemotingConfiguration; }
            set { _RemotingConfiguration = value; }
        }

        private RemotingClientUtil()
        {
            _RemotingConfiguration = RemotingClientConfiguration.GetConfig();

            if (_RemotingConfiguration == null) return;

            Dictionary<string, RemotingHost> hosts = _RemotingConfiguration.RemotingHosts;

            //��������Զ�̶���������ص��ڴ�
            foreach (KeyValuePair<string, RemotingHost> kvp in hosts)
            {
                RemotingHost host = kvp.Value;
                LoadModulesByHost(host);
            }

            //���ÿ���ͻ��˵Ŀ��÷�����
            RemotingHostCheck.Instance.DoCheck();

            //��������������������־
            if (_RemotingConfiguration.IsCheckServer)
            {
                System.Threading.Thread thread = new System.Threading.Thread(DoWork);
                //thread.IsBackground = true;
                thread.Start();
            }
        }

        void DoWork()
        {
            while (true)
            {
                if (OnLog != null)
                {
                    try
                    {
                        lock (RemotingHostCheck.Instance.CheckLog)
                        {
                            foreach (string log in RemotingHostCheck.Instance.CheckLog)
                            {
                                OnLog(log, LogType.Information);
                            }

                            RemotingHostCheck.Instance.CheckLog.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        OnLog(ex.Message, LogType.Error);
                    }
                }

                //ÿ��10������һ����־
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }

        /// <summary>
        /// ����Զ�̶������ͻ���ģ��
        /// </summary>
        /// <param name="host"></param>
        public void LoadModulesByHost(RemotingHost host)
        {
            string serverUrl = RemotingHostCheck.Instance.GetUsableServerUrl(host);

            foreach (KeyValuePair<string, string> m in host.Modules)
            {
                string objectUrl = _RemotingConfiguration.GetRemoteObjectUrl(serverUrl, m.Value);
                T instance = (T)Activator.GetObject(typeof(T), objectUrl);

                string key = string.Format("{0}${1}${2}", host.Name, serverUrl, m.Key);
                _RemoteObjects[key] = instance;
            }
        }

        /// <summary>
        /// ��ȡԶ�̶���
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="remoteObjectName"></param>
        /// <returns></returns>
        public T GetRemotingObject(string hostName, string remoteObjectName)
        {
            RemotingHost host = _RemotingConfiguration.RemotingHosts[hostName];
            string serverUrl = RemotingHostCheck.Instance.GetUsableServerUrl(host);
            string key = string.Format("{0}${1}${2}", host.Name, serverUrl, remoteObjectName);

            if (!_RemoteObjects.ContainsKey(key))
            {
                string objectUrl = _RemotingConfiguration.GetRemoteObjectUrl(serverUrl, remoteObjectName);
                T instance = (T)Activator.GetObject(typeof(T), objectUrl);
                _RemoteObjects[key] = instance;

                return instance;
            }

            return _RemoteObjects[key];
        }

        /// <summary>
        /// ��ȡԶ�̶���Ĭ��Ϊ��һ��RemotingClient��Ĭ�Ϸ�������
        /// </summary>
        /// <param name="remoteObjectName"></param>
        /// <returns></returns>
        public T GetRemotingObject(string remoteObjectName)
        {
            RemotingHost host = null;

            foreach (KeyValuePair<string, RemotingHost> kvp in _RemotingConfiguration.RemotingHosts)
            {
                host = kvp.Value;
                break;
            }

            if (host != null)
            {
                return GetRemotingObject(host.Name, remoteObjectName);
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// ��ȡ֪������ͻ��˴���ʵ��
        /// </summary>
        /// <param name="objectUrl"></param>
        /// <returns></returns>
        public T GetWellKnownClientInstance(string objectUrl)
        {
            if (!_RemoteObjects.ContainsKey(objectUrl))
            {
                T instance = (T)Activator.GetObject(typeof(T), objectUrl);

                _RemoteObjects.Add(objectUrl, instance);

                return instance;
            }

            return _RemoteObjects[objectUrl];
        }

        /// <summary>
        /// Remoting����������
        /// </summary>
        /// <param name="serverUrl">Remoting��������ַ �����磺tcp://127.0.0.1:8888��</param>
        /// <returns>Remoting������ʱ��</returns>
        public string RemotingServerTest(string serverUrl)
        {
            IRemotingTest t = RemotingClientUtil<IRemotingTest>.Instance.GetWellKnownClientInstance(serverUrl.TrimEnd('/') + "/RemotingTest");
            return t.GetDate();
        }

        /// <summary>
        /// OnLog event.
        /// </summary>
        public event LogEventHandler OnLog;
    }
}
