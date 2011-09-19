using System;
using System.Runtime.Remoting;

namespace MySoft.Remoting
{
    /// <summary>
    /// ҵ��ģ��ʵ����
    /// </summary>
    [Serializable]
    public class ServiceModule
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

        private string _Name;

        /// <summary>
        /// ģ������
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _AssemblyName;

        /// <summary>
        /// ���������ַ���
        /// </summary>
        public string AssemblyName
        {
            get { return _AssemblyName; }
            set { _AssemblyName = value; }
        }

        private string _ClassName;

        /// <summary>
        /// ��������
        /// </summary>
        public string ClassName
        {
            get { return _ClassName; }
            set { _ClassName = value; }
        }
    }
}