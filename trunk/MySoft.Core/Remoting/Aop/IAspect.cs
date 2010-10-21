using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Messaging;

namespace MySoft.Remoting.Aop
{
    /// <summary>
    /// 切面接口
    /// </summary>
    public interface IAspect
    {
        /// <summary>
        /// 处理之前
        /// </summary>
        /// <param name="requestMsg"></param>
        /// <param name="aspectClassArgument"></param>
        /// <param name="aspectMethodArgument"></param>
        void PreProcess(IMethodCallMessage requestMsg, object aspectClassArgument, object aspectMethodArgument);

        /// <summary>
        /// 处理时
        /// </summary>
        /// <param name="requestMsg"></param>
        /// <param name="respond"></param>
        /// <param name="aspectClassArgument"></param>
        /// <param name="aspectMethodArgument"></param>
        void PostProcess(IMethodCallMessage requestMsg, ref IMethodReturnMessage respond, object aspectClassArgument, object aspectMethodArgument);
    }
}
