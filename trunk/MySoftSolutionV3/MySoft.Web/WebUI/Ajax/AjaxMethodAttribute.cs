#region usings

using System;
using System.Reflection;

#endregion

namespace MySoft.Web.UI
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AjaxMethodAttribute : Attribute
    {
        /// <summary>
        /// ��������
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// �Ƿ��첽
        /// </summary>
        public bool Async { get; set; }

        /// <summary>
        /// ʵ����AjaxMethodAttribute
        /// </summary>
        public AjaxMethodAttribute() { }

        /// <summary>
        /// ʵ����AjaxMethodAttribute
        /// </summary>
        /// <param name="name"></param>
        public AjaxMethodAttribute(string name)
        {
            this.Name = name;
        }
    }

    internal class AjaxMethodInfo
    {
        /// <summary>
        /// ��������
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// �Ƿ��첽
        /// </summary>
        public bool Async { get; set; }

        /// <summary>
        /// �����б�
        /// </summary>
        public string[] Paramters { get; set; }

        public AjaxMethodInfo()
        {
            this.Paramters = new string[0];
        }
    }
}