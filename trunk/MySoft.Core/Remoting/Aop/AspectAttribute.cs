using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Proxies;

namespace MySoft.Remoting.Aop
{
    /// <summary>
    /// AspectAttribute 把被修饰类的实例委托给代理AspectChainProxy ，如此可以截获被修饰类的方法调用    
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AspectAttribute : ProxyAttribute
    {
        private Type[] theAspectProcessorWrapTypes = null;

        public AspectAttribute(params Type[] wrapTypes)
        {
            this.theAspectProcessorWrapTypes = wrapTypes;
        }

        #region CreateInstance
        /// <summary>
        ///    获得目标对象的自定义透明代理，该方法由系统调用
        /// </summary>
        public override MarshalByRefObject CreateInstance(Type serverType)//serverType是被AopProxyAttribute修饰的类
        {
            //未初始化的实例的默认透明代理
            MarshalByRefObject target = base.CreateInstance(serverType); //得到未初始化的实例（ctor未执行）
            object[] args = { target, serverType };

            //得到自定义的真实代理
            RealProxy rp = new AspectChainProxy(target, serverType, this.theAspectProcessorWrapTypes);//new AopControlProxy(target ,serverType) ;
            return (MarshalByRefObject)rp.GetTransparentProxy();
        }

        #endregion
    }
}
