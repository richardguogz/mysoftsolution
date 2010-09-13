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
using System.Reflection;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.IO;

namespace MySoft.Core.Remoting
{
    /// <summary>
    /// The Remoting Service Helper.
    /// </summary>
    public sealed class RemotingServerHelper : RemotingServiceHelper
    {
        private RemotingServerConfiguration cfg;
        public RemotingServerHelper(RemotingServerConfiguration cfg)
            : base(cfg.ChannelType, cfg.ServerAddress, cfg.Port)
        {
            this.cfg = cfg;
        }

        /// <summary>
        /// 发布知名对象服务器端实例（远程对象已在配置文件中定义）
        /// </summary>
        public void PublishWellKnownServiceInstance()
        {
            if (cfg != null)
            {
                List<ServiceModule> list = cfg.Modules;

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

        //发布Remoting服务管理模块
        private void PublishRemotingManageModule()
        {
            //发布Remoting测试模块
            PublishWellKnownServiceInstance("RemotingTest", typeof(RemotingTest), WellKnownObjectMode.Singleton);

            //发布Remoting服务日志文件管理模块
            PublishWellKnownServiceInstance("RemotingLogFileManager", typeof(RemotingLogFileManager), WellKnownObjectMode.Singleton);
        }
    }

    /// <summary>
    /// The Remoting Service Helper.
    /// </summary>
    public class RemotingServiceHelper : IDisposable, ILogable
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
        public RemotingServiceHelper(RemotingChannelType channelType, string serverAddress, int serverPort)
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
                BinaryServerFormatterSinkProvider serverProvider = new
                    BinaryServerFormatterSinkProvider();
                BinaryClientFormatterSinkProvider clientProvider = new
                    BinaryClientFormatterSinkProvider();
                serverProvider.TypeFilterLevel = TypeFilterLevel.Full;

                IDictionary props = new Hashtable();
                props["name"] = AppDomain.CurrentDomain.FriendlyName;
                props["port"] = serverPort;

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

    /// <summary>
    /// 
    /// </summary>
    public class ClientIPServerSinkProvider : IServerChannelSinkProvider
    {
        private IServerChannelSinkProvider next = null;

        public ClientIPServerSinkProvider()
        { }

        public ClientIPServerSinkProvider(IDictionary properties, ICollection providerData)
        { }

        public void GetChannelData(IChannelDataStore channelData)
        { }

        public IServerChannelSink CreateSink(IChannelReceiver channel)
        {
            IServerChannelSink nextSink = null;
            if (next != null)
            {
                nextSink = next.CreateSink(channel);
            }

            return new ClientIPServerSink(nextSink);
        }

        public IServerChannelSinkProvider Next
        {
            get { return next; }
            set { next = value; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ClientIPServerSink : BaseChannelObjectWithProperties, IServerChannelSink, IChannelSinkBase
    {

        private IServerChannelSink _next;

        public ClientIPServerSink(IServerChannelSink next)
        {
            _next = next;
        }

        public void AsyncProcessResponse(IServerResponseChannelSinkStack sinkStack, Object state, IMessage msg, ITransportHeaders headers, Stream stream)
        {
        }

        public Stream GetResponseStream(IServerResponseChannelSinkStack sinkStack, Object state, IMessage msg, ITransportHeaders headers)
        {
            return null;
        }

        public System.Runtime.Remoting.Channels.ServerProcessing ProcessMessage(IServerChannelSinkStack sinkStack, IMessage requestMsg, ITransportHeaders requestHeaders, Stream requestStream, out IMessage responseMsg, out ITransportHeaders responseHeaders, out Stream responseStream)
        {
            if (_next != null)
            {
                IPAddress ip = requestHeaders[CommonTransportKeys.IPAddress] as IPAddress;
                String requestUri = requestHeaders[CommonTransportKeys.RequestUri].ToString();
                Console.WriteLine(ip.ToString());
                CallContext.SetData("ClientIPAddress", ip);
                CallContext.SetData("RequestUri", requestUri);
                ServerProcessing spres = _next.ProcessMessage(sinkStack, requestMsg, requestHeaders, requestStream, out responseMsg, out responseHeaders, out responseStream);

                return spres;
            }
            else
            {
                responseMsg = null;
                responseHeaders = null;
                responseStream = null;

                return new ServerProcessing();
            }
        }

        public IServerChannelSink NextChannelSink
        {
            get { return _next; }
            set { _next = value; }
        }
    }
}
