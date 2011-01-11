using System;
using System.Runtime.Remoting;

namespace MySoft.Remoting
{
    /// <summary>
    /// ҵ��ģ��ʵ����
    /// </summary>
    [Serializable]
    public class ServiceModule : ServiceProfile
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
