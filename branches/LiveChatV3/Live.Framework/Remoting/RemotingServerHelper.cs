using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using LiveChat.Logger;
using System.Reflection;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.IO;

namespace LiveChat.Remoting
{
    /// <summary>
    /// Remoting服务端帮助类
    /// </summary>
    public sealed class RemotingServerHelper : IDisposable, ILogable
    {
        #region Private Members

        private RemotingChannelType channelType;
        private string serverAddress;
        private int port;
        private IChannel serviceChannel;

        private RemotingServerConfiguration cfg;

        private void WriteLog(string logMsg)
        {
            if (OnLog != null)
            {
                OnLog(logMsg);
            }
        }

        private string BuildUrl(string notifyName)
        {
            string url = string.Format("{0}://{1}:{2}/{3}", channelType.ToString().ToLower(), serverAddress, port, notifyName);
            return url;
        }

        #endregion

        /// <summary>
        /// 日志处理事件
        /// </summary>
        public event LogHandler OnLog;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="channelType">通道类型</param>
        /// <param name="serverAddress">服务器地址</param>
        /// <param name="port">端口</param>
        public RemotingServerHelper(RemotingChannelType channelType, string serverAddress, int port)
        {
            this.channelType = channelType;
            this.serverAddress = serverAddress;
            this.port = port;

            Init(channelType, port);
        }

        /// <summary>
        /// 构造函数，通过配置文件实例化
        /// </summary>
        /// <param name="cfg">RemotingServerConfiguration</param>
        public RemotingServerHelper(RemotingServerConfiguration cfg)
        {
            if (cfg != null)
            {
                this.cfg = cfg;
                this.channelType = cfg.ChannelType;
                this.serverAddress = cfg.ServerAddress;
                this.port = cfg.Port;

                Init(channelType, port);
            }
            else
            {
                string ip = "";
                IPHostEntry hostEntry = Dns.Resolve(Environment.MachineName);///todo:Dns.Resolve(Environment.MachineName)过时
                if (hostEntry.AddressList.Length > 0)
                    ip = hostEntry.AddressList[0].ToString();

                this.channelType = RemotingChannelType.TCP;
                this.serverAddress = ip;
                this.port = 8888;

                Init(channelType, port);
            }
        }

        private void Init(RemotingChannelType channelType, int port)
        {
            if (serviceChannel == null)
            {
                //使用二进制格式化
                BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
                BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();

                //设置反序列化级别为Full，支持远程处理在所有情况下支持的所有类型
                serverProvider.TypeFilterLevel = TypeFilterLevel.Full;

                IDictionary props = new Hashtable();
                props["name"] = AppDomain.CurrentDomain.FriendlyName;
                props["port"] = port;

                //初始化通道并注册
                if (channelType == RemotingChannelType.TCP)
                {
                    serviceChannel = new TcpChannel(props, clientProvider, serverProvider);

                }
                else
                {
                    serviceChannel = new HttpChannel(props, clientProvider, serverProvider);
                }

                ChannelServices.RegisterChannel(serviceChannel, false);
            }
        }

        /// <summary>
        /// 发布知名对象服务端实例
        /// </summary>
        /// <param name="notifyName">远程对象发布时的名称</param>
        /// <param name="interfaceType">接口类型（这里Remoting使用基于接口的方式，每个远程对象必须是基于接口的实现，该接口需要在客户端使用）</param>
        /// <param name="instance">远程对象实例</param>
        /// <param name="mode">远程对象激活方式</param>
        public void PublishWellKnownServiceInstance(string notifyName, Type interfaceType, MarshalByRefObject instance, WellKnownObjectMode mode)
        {
            RemotingConfiguration.RegisterWellKnownServiceType(interfaceType, BuildUrl(notifyName), mode);
            ObjRef objRef = RemotingServices.Marshal(instance, notifyName);
            WriteLog(string.Format("远程对象[{0}]发布成功，URL：{1}", instance.ToString(), BuildUrl(notifyName)));
        }

        /// <summary>
        /// 发布知名对象服务器端实例（远程对象已在配置文件中定义）
        /// </summary>
        public void PublishWellKnownServiceInstanceByConfig()
        {
            if (cfg != null)
            {
                List<ServiceModule> list = cfg.Modules;

                foreach (ServiceModule m in list)
                {
                    Assembly assembly = Assembly.Load(m.AssemblyName);
                    Type t = assembly.GetType(m.ClassName);

                    if (t != null)
                    {
                        RemotingConfiguration.RegisterWellKnownServiceType(t, m.ClassName, m.Mode);
                        WriteLog(string.Format("远程对象[{0}]发布成功，URL：{1}", m.ClassName, BuildUrl(m.ClassName)));
                    }
                    else
                    {
                        WriteLog(string.Format("远程对象[{0}]发布失败，URL：{1}", m.ClassName, BuildUrl(m.ClassName)));
                    }
                    //Assembly assembly = Assembly.Load(m.AssemblyName);
                    //Type t = assembly.GetType(m.Type);
                    //object obj = Activator.CreateInstance(t);
                    //RemotingServices.Marshal((MarshalByRefObject)obj, m.Type);
                }
            }

            PublishRemotingManageModule();
        }

        //发布Remoting服务管理模块
        private void PublishRemotingManageModule()
        {
            //发布Remoting测试模块
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(RemotingTest), "RemotingTest", WellKnownObjectMode.Singleton);

            //发布Remoting服务日志文件管理模块
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(RemotingLogFileManager), "RemotingLogFileManager", WellKnownObjectMode.Singleton);
        }

        /// <summary>
        /// 发布客户端激活对象
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        public void PublishActivatedService(Type serviceType)
        {
            RemotingConfiguration.RegisterActivatedServiceType(serviceType);
            WriteLog(string.Format("客户端激活对象[{0}]注册成功，监听端口：{1}", serviceType.ToString(), port));
        }

        #region IDisposable Members

        /// <summary>
        /// 注销通道，释放资源
        /// </summary>
        public void Dispose()
        {
            ChannelServices.UnregisterChannel(serviceChannel);
        }

        #endregion
    }
}
