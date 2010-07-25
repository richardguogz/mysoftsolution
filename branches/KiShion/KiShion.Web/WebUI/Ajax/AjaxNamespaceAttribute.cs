using System;
using System.Collections.Generic;
using System.Text;

namespace KiShion.Web.UI
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
