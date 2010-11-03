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
using MySoft.Core;

namespace MySoft.Remoting
{
    /// <summary>
    /// The Remoting Client Helper
    /// </summary>
    public sealed class RemotingClientHelper : IDisposable, ILogable
    {
        #region Private Members

        private RemotingChannelType channelType;
        private string serverAddress;
        private int serverPort;
        private int callbackPort;
        private IChannel clientChannel;

        private void WriteLog(string log)
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
        /// Initializes a new instance of the <see cref="RemotingClientHelper"/> class.
        /// </summary>
        /// <param name="channelType">Type of the channel.</param>
        /// <param name="serverAddress">The server address.</param>
        /// <param name="serverPort">The server port.</param>
        /// <param name="callbackPort">The callback port.</param>
        public RemotingClientHelper(RemotingChannelType channelType, string serverAddress, int serverPort, int callbackPort)
        {
            this.channelType = channelType;
            this.serverAddress = serverAddress;
            this.serverPort = serverPort;
            this.callbackPort = callbackPort;

            Init();
        }

        /// <summary>
        /// Inits this instance.
        /// </summary>
        private void Init()
        {
            if (clientChannel == null)
            {
                try
                {
                    IDictionary props = new Hashtable();
                    props["name"] = AppDomain.CurrentDomain.FriendlyName;
                    props["port"] = callbackPort;
                    props["timeout"] = 1000 * 60 * 5; //Ĭ��5���ӳ�ʱ

                    if (channelType == RemotingChannelType.Tcp)
                    {
                        BinaryClientFormatterSinkProvider binaryClientSinkProvider = new BinaryClientFormatterSinkProvider();
                        BinaryServerFormatterSinkProvider binaryServerSinkProvider = new BinaryServerFormatterSinkProvider();
                        binaryServerSinkProvider.TypeFilterLevel = TypeFilterLevel.Full;

                        clientChannel = new TcpChannel(props, binaryClientSinkProvider, binaryServerSinkProvider);
                    }
                    else
                    {
                        SoapClientFormatterSinkProvider soapClientSinkProvider = new SoapClientFormatterSinkProvider();
                        SoapServerFormatterSinkProvider soapServerSinkProvider = new SoapServerFormatterSinkProvider();
                        soapServerSinkProvider.TypeFilterLevel = TypeFilterLevel.Full;

                        clientChannel = new HttpChannel(props, soapClientSinkProvider, soapServerSinkProvider);
                    }

                    ChannelServices.RegisterChannel(clientChannel, false);
                }
                catch (Exception ex)
                {
                    throw new RemotingException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Gets the well known client instance.
        /// </summary>
        /// <param name="notifyName">Name of the notify.</param>
        /// <returns>Te instance.</returns>
        public T GetWellKnownClientInstance<T>(string notifyName)
        {
            string url = BuildUrl(notifyName);
            RemotingConfiguration.RegisterWellKnownClientType(typeof(T), url);
            T instance = (T)Activator.GetObject(typeof(T), url);
            if (instance != null)
            {
                WriteLog(instance.ToString() + " proxy created!");
            }

            return instance;
        }

        /// <summary>
        /// Creates the activated client instance.
        /// </summary>
        /// <returns>Te instance.</returns>
        public T CreateActivatedClientInstance<T>()
            where T : new()
        {
            if (!IsClientTypeRegistered(typeof(T)))
            {
                RegisterActivatedClientType(typeof(T));
            }

            T instance = new T();
            if (instance != null)
            {
                WriteLog(instance.ToString() + " proxy created!");
            }

            return instance;
        }

        /// <summary>
        /// Determines whether the specified client type is registered.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	<c>true</c> if is registered; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsClientTypeRegistered(Type type)
        {
            bool alreadyRegistered = false;
            foreach (ActivatedClientTypeEntry item in RemotingConfiguration.GetRegisteredActivatedClientTypes())
            {
                if (item.ObjectType == type)
                {
                    alreadyRegistered = true;
                    break;
                }
            }
            return alreadyRegistered;
        }

        /// <summary>
        /// Registers the type of the activated client.
        /// </summary>
        /// <param name="type">The type.</param>
        public void RegisterActivatedClientType(Type type)
        {
            string url = BuildUrl(string.Empty);
            RemotingConfiguration.RegisterActivatedClientType(type, url);
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            ChannelServices.UnregisterChannel(clientChannel);
        }

        #endregion

        #region ILogable Members

        /// <summary>
        /// OnLog event.
        /// </summary>
        public event LogEventHandler OnLog;

        #endregion
    }
}
