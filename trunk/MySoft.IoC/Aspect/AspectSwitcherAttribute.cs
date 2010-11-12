using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.IoC
{
    /// <summary>
    /// 切面方法选择器
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class  AspectSwitcherAttribute : Attribute
    {
        private bool useAspect = false;

        public AspectSwitcherAttribute(bool useAop)
        {
            this.useAspect = useAop;
        }

        /// <summary>
        /// 是否使用切面处理
        /// </summary>
        public bool UseAspect
        {
            get
            {
                return this.useAspect;
            }
        }
    }
}
