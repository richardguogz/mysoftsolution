using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Data.Design
{
    /// <summary>
    /// �ֶ�������Ϣ
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public sealed class DescriptionAttribute : Attribute
    {
        private string description;
        public string Description
        {
            get
            {
                return description;
            }
        }

        public DescriptionAttribute(string description)
        {
            this.description = description;
        }
    }
}
