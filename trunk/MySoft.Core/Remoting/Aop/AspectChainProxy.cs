using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;
using System.Collections;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Services;

namespace MySoft.Remoting.Aop
{
    /// <summary>
    /// 代理
    /// </summary>
    public class AspectChainProxy : RealProxy
    {
        private MarshalByRefObject target = null;
        private Type[] theAspectProcessorWrapTypes = null;
        private ArrayList aspectCallerList = new ArrayList();//集合中为AspectCaller实例


        public AspectChainProxy(MarshalByRefObject target, Type serverType, params Type[] aopProcessorWrapTypes)
            : base(serverType)
        {
            this.target = target;
            this.theAspectProcessorWrapTypes = aopProcessorWrapTypes;
        }

        #region Invoke
        public override IMessage Invoke(IMessage msg)
        {
            IMethodCallMessage call = (IMethodCallMessage)msg;
            this.FillAspectCallerList(call);

            //如果触发的是构造函数，此时target的构建还未开始
            IConstructionCallMessage ctor = call as IConstructionCallMessage;
            if (ctor != null)
            {
                //获取最底层的默认真实代理
                RealProxy default_proxy = RemotingServices.GetRealProxy(this.target);

                default_proxy.InitializeServerObject(ctor);
                MarshalByRefObject tp = (MarshalByRefObject)this.GetTransparentProxy(); //自定义的透明代理 this

                return EnterpriseServicesHelper.CreateConstructionReturnMessage(ctor, tp);
            }

            this.PreProcess(call);

            IMethodReturnMessage result_msg = RemotingServices.ExecuteMessage(this.target, call); //将消息转化为堆栈，并执行目标方法，方法完成后，再将堆栈转化为消息

            this.PostProcess(call, ref result_msg);

            return result_msg;
        }
        #endregion

        #region FillAspectCallerList
        private void FillAspectCallerList(IMethodCallMessage call)
        {
            this.aspectCallerList.Clear();

            if (this.theAspectProcessorWrapTypes == null)
            {
                return;
            }

            //显式启动了方面的WrapType
            ArrayList overtWrapTypeList = new ArrayList();


            //查询目标方法是否 "显式" 启用AOP的MethodAopSwitcherAttribute
            foreach (Attribute attr in call.MethodBase.GetCustomAttributes(false))
            {
                AspectSwitcherAttribute aspectSwitcher = attr as AspectSwitcherAttribute;
                if (aspectSwitcher == null)
                {
                    continue;
                }

                if (aspectSwitcher.DestAspectProcessorWrapType == null)
                {
                    continue;
                }

                overtWrapTypeList.Add(aspectSwitcher.DestAspectProcessorWrapType);

                if (!aspectSwitcher.UseAspect)
                {
                    continue;
                }

                IAspectProcessorWrap processorWrap = this.GetAspectProcessorWrap(aspectSwitcher.DestAspectProcessorWrapType);
                if (processorWrap == null)
                {
                    continue;
                }

                AspectCaller caller = new AspectCaller();
                caller.AspectMethodArgument = aspectSwitcher.AopArgument;
                caller.CurProcessorWrap = processorWrap;

                this.aspectCallerList.Add(caller);
            }

            //非显式启用的方面
            foreach (Type wrapType in this.theAspectProcessorWrapTypes)
            {
                bool passIt = this.WrapTypeIsInOvertWrapTypeList(wrapType, overtWrapTypeList);

                if (!passIt)
                {
                    IAspectProcessorWrap processorWrap = (IAspectProcessorWrap)Activator.CreateInstance(wrapType);
                    if (processorWrap.DefaultAspectSwitcherState == AspectSwitcherState.On)
                    {
                        AspectCaller caller = new AspectCaller();
                        caller.CurProcessorWrap = processorWrap;

                        this.aspectCallerList.Add(caller);
                    }
                }
            }
        }

        private bool WrapTypeIsInOvertWrapTypeList(Type wrapType, ArrayList overtWrapTypeList)
        {
            foreach (Type tempWrapType in overtWrapTypeList)
            {
                if (wrapType == tempWrapType)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region GetAspectProcessorWrap
        private IAspectProcessorWrap GetAspectProcessorWrap(Type aspectProcesserWrapType)
        {
            foreach (Type wrapType in this.theAspectProcessorWrapTypes)
            {
                if (wrapType == aspectProcesserWrapType)
                {
                    IAspectProcessorWrap wrap = (IAspectProcessorWrap)Activator.CreateInstance(wrapType);
                    return wrap;
                }
            }

            return null;
        }
        #endregion

        #region PreProcess ,PostProcess
        private void PreProcess(IMethodCallMessage requestMsg)
        {
            foreach (AspectCaller caller in this.aspectCallerList)
            {
                IAspect aspectProcessor = (IAspect)Activator.CreateInstance(caller.CurProcessorWrap.AspectProcessorType);
                if (aspectProcessor != null)
                {
                    aspectProcessor.PreProcess(requestMsg, caller.CurProcessorWrap.AspectClassArgument, caller.AspectMethodArgument);
                }
            }
        }

        private void PostProcess(IMethodCallMessage requestMsg, ref IMethodReturnMessage respond)
        {
            foreach (AspectCaller caller in this.aspectCallerList)
            {
                IAspect aspectProcessor = (IAspect)Activator.CreateInstance(caller.CurProcessorWrap.AspectProcessorType);
                if (aspectProcessor != null)
                {
                    aspectProcessor.PostProcess(requestMsg, ref respond, caller.CurProcessorWrap.AspectClassArgument, caller.AspectMethodArgument);
                }
            }
        }
        #endregion
    }
}
