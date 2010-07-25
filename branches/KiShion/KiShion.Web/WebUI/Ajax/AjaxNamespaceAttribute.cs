using System;
using System.Collections.Generic;
using System.Text;

namespace KiShion.Web.UI
{
    /// <summary>
    /// 自定义输出命名空间
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AjaxNamespaceAttribute : Attribute
    {
        /// <summary>
        /// 客户端程序集名称
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
        /// 实例化Ajax输出命名空间
        /// </summary>
        /// <param name="name"></param>
        public AjaxNamespaceAttribute(string name)
        {
            this.name = name;
        }
    }
}
