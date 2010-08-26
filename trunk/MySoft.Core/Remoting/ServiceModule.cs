using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using MySoft.Core.Service;

namespace MySoft.Core.Remoting
{
    /// <summary>
    /// ҵ��ģ��ʵ����
    /// </summary>
    [Serializable]
    public class ServiceModule : ServiceBase
    {
        private WellKnownObjectMode _Mode = WellKnownObjectMode.SingleCall;

        /// <summary>
        /// ���󼤻ʽ��SingleCall����SingleTon��
        /// </summary>
        public WellKnownObjectMode Mode
        {
            get { return _Mode; }
            set { _Mode = value; }
        }
    }
}
