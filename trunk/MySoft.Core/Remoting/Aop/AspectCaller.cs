using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Aop
{
    /// <summary>
    /// AspectCaller 针对某一特定的方法，实施的一次Aspect调用
    /// </summary>
    public class AspectCaller
    {
        public IAspectProcessorWrap CurProcessorWrap = null;
        public object AspectMethodArgument = null;
    }
}
