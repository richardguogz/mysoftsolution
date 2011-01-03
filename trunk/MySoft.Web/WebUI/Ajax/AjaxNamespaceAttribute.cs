using System;

namespace MySoft.Web.UI
{
    /// <summary>
    /// �Զ�����������ռ�
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AjaxNamespaceAttribute : Attribute
    {
        /// <summary>
        /// �ͻ��˳�������
        /// </summary>
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        /// <summary>
        /// ʵ����Ajax��������ռ�
        /// </summary>
        /// <param name="name"></param>
        public AjaxNamespaceAttribute(string name)
        {
            this.name = name;
        }
    }
}
