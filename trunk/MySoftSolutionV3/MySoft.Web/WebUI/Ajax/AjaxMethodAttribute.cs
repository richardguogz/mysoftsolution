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
        /// �Զ��巽������
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

        private bool async;
        public bool Async
        {
            get
            {
                return async;
            }
            set
            {
                async = value;
            }
        }

        public AjaxMethodAttribute() { }

        public AjaxMethodAttribute(string name)
        {
            this.name = name;
        }
    }

    internal class AsyncMethodInfo
    {
        private bool async;
        public bool Async
        {
            get
            {
                return async;
            }
            set
            {
                async = value;
            }
        }

        private MethodInfo methodInfo;
        public MethodInfo MethodInfo
        {
            get
            {
                return methodInfo;
            }
            set
            {
                methodInfo = value;
            }
        }
    }

    internal class AjaxMethodInfo
    {
        /// <summary>
        /// ��������
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

        private bool async;
        public bool Async
        {
            get
            {
                return async;
            }
            set
            {
                async = value;
            }
        }

        /// <summary>
        /// �����б�
        /// </summary>
        private string[] paramters;
        public string[] Paramters
        {
            get
            {
                return paramters;
            }
            set
            {
                paramters = value;
            }
        }
    }
}