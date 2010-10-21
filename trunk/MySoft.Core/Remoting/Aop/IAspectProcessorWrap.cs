using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Remoting.Aop
{
    /// <summary>
    /// IAspectProcessorWrap 对某一Aspect类型和对应的AspectClassArgument进行封装
    /// </summary>
    public interface IAspectProcessorWrap
    {
        Type AspectProcessorType { get; } //返回的是IAspect的实现
        object AspectClassArgument { get; }

        /// <summary>
        /// 当一个方法没有被某个方面的AspectSwitcherAttribute修饰时，是否启用该方面
        /// </summary>
        AspectSwitcherState DefaultAspectSwitcherState { get; }
    }
}
