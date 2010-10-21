using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Remoting
{
    /// <summary>
    /// Զ�̷�����ʵ��
    /// </summary>
    [Serializable]
    public class RemotingServer
    {
        private string _ServerName;

        /// <summary>
        /// ������������������������������
        /// </summary>
        public string ServerName
        {
            get { return _ServerName; }
            set { _ServerName = value; }
        }

        private string _ServerUrl;

        /// <summary>
        /// ��ȡԶ��ҵ�����Url���磺tcp://127.0.0.1:8888/NetValue��
        /// </summary>
        public string ServerUrl
        {
            get { return _ServerUrl; }
            set { _ServerUrl = value; }
        }
    }
}
