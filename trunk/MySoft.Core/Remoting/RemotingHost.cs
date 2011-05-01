using System;
using System.Collections.Generic;

namespace MySoft.Remoting
{
    /// <summary>
    /// RemotingHostʵ����
    /// </summary>
    [Serializable]
    public class RemotingHost
    {
        private string _Name;

        /// <summary>
        /// RemotingHost Name
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _DefaultServer;

        /// <summary>
        /// Ĭ�Ϸ�����
        /// </summary>
        public string DefaultServer
        {
            get { return _DefaultServer; }
            set { _DefaultServer = value; }
        }

        private Dictionary<string, RemotingServer> _Servers;

        /// <summary>
        /// Remoting����������
        /// </summary>
        public Dictionary<string, RemotingServer> Servers
        {
            get { return _Servers; }
            set { _Servers = value; }
        }

        Dictionary<string, string> _Modules;

        /// <summary>
        /// Զ�̶���ҵ��ģ�鼯��
        /// </summary>
        public Dictionary<string, string> Modules
        {
            get { return _Modules; }
            set { _Modules = value; }
        }

        Boolean _IsChecking;

        /// <summary>
        /// �÷������Ƿ����ڱ����
        /// </summary>
        public Boolean IsChecking
        {
            get { return _IsChecking; }
            set { _IsChecking = value; }
        }
    }
}
