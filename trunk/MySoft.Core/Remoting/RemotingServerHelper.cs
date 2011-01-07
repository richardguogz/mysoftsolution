using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Text;

namespace MySoft.Remoting
{
    /// <summary>
    /// The Remoting Service Helper.
    /// </summary>
    public sealed class RemotingServiceHelper : RemotingServerHelper
    {
        private RemotingServerConfiguration config;
        public RemotingServiceHelper(RemotingServerConfiguration config)
            : base(config.ChannelType, config.ServerAddress, config.Port)
        {
            this.config = config;
        }

        /// <summary>
        /// ����֪�������������ʵ����Զ�̶������������ļ��ж��壩
        /// </summary>
        public void PublishWellKnownServiceInstance()
        {
            if (config != null)
            {
                List<ServiceModule> list = config.Modules;

                foreach (ServiceModule m in list)
                {
                    Assembly assembly = Assembly.Load(m.AssemblyName);
                    Type t = assembly.GetType(m.ClassName);

                    if (t == null)
                        WriteLog("(" + m.AssemblyName + " --> " + m.ClassName + ") loading failed! ");
                    else
                        PublishWellKnownServiceInstance(m.Name, t, m.Mode);
                }
            }

            PublishRemotingManageModule();
        }

        //����Remoting�������ģ��
        private void PublishRemotingManageModule()
        {
            //����Remoting����ģ��
            PublishWellKnownServiceInstance("RemotingTest", typeof(RemotingTest), WellKnownObjectMode.Singleton);

            //����Remoting������־�ļ�����ģ��
            PublishWellKnownServiceInstance("RemotingLogFileManager", typeof(RemotingLogFileManager), WellKnownObjectMode.Singleton);
        }
    }

    /// <summary>
    /// The Remoting Service Helper.
    /// </summary>
    public class RemotingServerHelper : IDisposable, ILogable
    {
        #region Private Members

        private RemotingChannelType channelType;
        private string serverAddress;
        private int serverPort;
        private IChannel serviceChannel;

        protected void WriteLog(string log)
        {
            if (OnLog != null)
            {
                OnLog(log);
            }
        }

        private string BuildUrl(string notifyName)
        {
            StringBuilder url = new StringBuilder();
            url.Append(channelType.ToString().ToLower());
            url.Append("://");
            url.Append(serverAddress);
            url.Append(":");
            url.Append(serverPort.ToString());
            url.Append("/" + notifyName);

            return url.ToString();
        }

        #endregion

        /// <summary>
        /// OnLog event.
        /// </summary>
        public event LogEventHandler OnLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemotingServerHelper"/> class.
        /// </summary>
        /// <param name="channelType">Type of the channel.</param>
        /// <param name="serverAddress">The server address.</param>
        /// <param name="serverPort">The server port.</param>
        public RemotingServerHelper(RemotingChannelType channelType, string serverAddress, int serverPort)
        {
            this.channelType = channelType;
            this.serverAddress = serverAddress;
            this.serverPort = serverPort;
            Init();
        }

        private void Init()
        {
            if (serviceChannel == null)
            {
                //ʹ�ö����Ƹ�ʽ��
                BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
                BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();

                //���÷����л�����ΪFull��֧��Զ�̴��������������֧�ֵ���������
                serverProvider.TypeFilterLevel = TypeFilterLevel.Full;

                string name = AppDomain.CurrentDomain.FriendlyName + Environment.MachineName;
                IDictionary props = new Hashtable();
                props["name"] = name;
                props["port"] = serverPort;

                //��ʼ��ͨ����ע��
                if (channelType == RemotingChannelType.Tcp)
                {
                    serviceChannel = new TcpChannel(props, clientProvider, serverProvider);
                }
                else
                {
                    serviceChannel = new HttpChannel(props, clientProvider, serverProvider);
                }

                //�ж��ŵ��Ƿ�ע��
                if (ChannelServices.GetChannel(name) == null)
                {
                    ChannelServices.RegisterChannel(serviceChannel, false);
                }
            }
        }

        /// <summary>
        /// Publishes the well known service instance.
        /// </summary>
        /// <param name="notifyName">Name of the notify.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="mode">The mode.</param>
        public void PublishWellKnownServiceInstance(string notifyName, Type interfaceType, WellKnownObjectMode mode)
        {
            PublishWellKnownServiceInstance(notifyName, interfaceType, null, mode);
        }

        /// <summary>
        /// Publishes the well known service instance.
        /// </summary>
        /// <param name="notifyName">Name of the notify.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="mode">The mode.</param>
        public void PublishWellKnownServiceInstance(string notifyName, Type interfaceType, MarshalByRefObject instance, WellKnownObjectMode mode)
        {
            WriteLog("Instance URL --> " + BuildUrl(notifyName));

            RemotingConfiguration.RegisterWellKnownServiceType(interfaceType, BuildUrl(notifyName), mode);
            ObjRef objRef = RemotingServices.Marshal(instance, notifyName);

            if (instance == null)
                WriteLog("(" + BuildUrl(notifyName) + ") start listening at port: " + serverPort);
            else
                WriteLog("(" + instance.ToString() + ") start listening at port: " + serverPort);
        }

        /// <summary>
        /// Publishes the activated service.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        public void PublishActivatedService(Type serviceType)
        {
            WriteLog("Registered Activated Service --> " + serviceType.ToString());
            RemotingConfiguration.RegisterActivatedServiceType(serviceType);
            WriteLog("[" + DateTime.Now.ToString() + "] Start listening at port: " + serverPort);
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            ChannelServices.UnregisterChannel(serviceChannel);
        }

        #endregion
    }
}
