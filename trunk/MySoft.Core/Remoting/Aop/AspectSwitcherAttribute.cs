using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Aop
{
    /// <summary>
    /// Aop选择器
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AspectSwitcherAttribute : Attribute
    {
        private bool useAspect = false;
        private object theAopArgument = null;
        private Type destAspectProcessorWrapType = null;

        public AspectSwitcherAttribute(Type destAspectWrapType, bool useAop)
        {
            this.destAspectProcessorWrapType = destAspectWrapType;
            this.useAspect = useAop;
        }

        public AspectSwitcherAttribute(Type destAspectWrapType, bool useAop, object aopArg)
        {
            this.useAspect = useAop;
            this.theAopArgument = aopArg;
            this.destAspectProcessorWrapType = destAspectWrapType;
        }

        /// <summary>
        /// 使用切面
        /// </summary>
        public bool UseAspect
        {
            get
            {
                return this.useAspect;
            }
        }

        /// <summary>
        /// Aop参数
        /// </summary>
        public object AopArgument
        {
            get
            {
                return this.theAopArgument;
            }
        }

        /// <summary>
        /// 处理类型
        /// </summary>
        public Type DestAspectProcessorWrapType
        {
            get
            {
                return this.destAspectProcessorWrapType;
            }
        }
    }
}
