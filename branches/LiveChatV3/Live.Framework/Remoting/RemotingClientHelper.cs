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
using LiveChat.Remoting;

namespace LiveChat.Remoting
{
    /// <summary>
    /// Remoting�ͻ��˰�����
    /// </summary>
    public sealed class RemotingClientHelper : IDisposable, ILogable
    {
        #region Private Members

        private RemotingChannelType channelType;
        private string serverAddress;
        private int serverPort;
        private int callbackPort;
        private IChannel clientChannel;

        private void WriteLog(string logMsg)
        {
            if (OnLog != null)
            {
                OnLog(logMsg);
            }
        }

        private string BuildUrl(string notifyName)
        {
            string url = string.Format("{0}://{1}:{2}/{3}", channelType.ToString().ToLower(), serverAddress, serverPort, notifyName);
            return url;
        }

        #endregion

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="channelType">ͨ������</param>
        /// <param name="serverAddress">��������ַ</param>
        /// <param name="serverPort">�������˿�</param>
        /// <param name="callbackPort">�ص��˿�</param>
        public RemotingClientHelper(RemotingChannelType channelType, string serverAddress, int serverPort, int callbackPort)
        {
            this.channelType = channelType;
            this.serverAddress = serverAddress;
            this.serverPort = serverPort;
            this.callbackPort = callbackPort;

            Init();
        }

        private void Init()
        {
            if (clientChannel == null)
            {
                BinaryServerFormatterSinkProvider serverProvider = new
                    BinaryServerFormatterSinkProvider();
                BinaryClientFormatterSinkProvider clientProvider = new
                    BinaryClientFormatterSinkProvider();

                //���÷����л�����ΪFull��֧��Զ�̴��������������֧�ֵ���������
                serverProvider.TypeFilterLevel = TypeFilterLevel.Full;

                IDictionary props = new Hashtable();
                props["name"] = AppDomain.CurrentDomain.FriendlyName;
                props["port"] = callbackPort;

                if (channelType == RemotingChannelType.TCP)
                {
                    clientChannel = new TcpChannel(props, clientProvider, serverProvider);
                }
                else
                {
                    clientChannel = new HttpChannel(props, clientProvider, serverProvider);
                }

                ChannelServices.RegisterChannel(clientChannel, false);
            }
        }

        /// <summary>
        /// ��ȡ֪������ͻ���ʵ��
        /// </summary>
        /// <param name="notifyName">Զ�̶��󷢲�ʱ������</param>
        public T GetWellKnownClientInstance<T>(string notifyName)
        {
            string url = BuildUrl(notifyName);
            RemotingConfiguration.RegisterWellKnownClientType(typeof(T), url);
            T instance = (T)Activator.GetObject(typeof(T), url);

            if (instance != null)
            {
                WriteLog(string.Format("�Ѵ���[{0}]�Ŀͻ��˴���",instance.ToString()));
            }

            return instance;
        }

        /// <summary>
        /// �����ͻ��˼�������ʵ��
        /// </summary>
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
                WriteLog(string.Format("�Ѵ���[{0}]�Ŀͻ��˴���", instance.ToString()));
            }

            return instance;
        }

        /// <summary>
        /// ���ָ���Ŀͻ��������Ƿ�ע��
        /// </summary>
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
        /// ע�ἤ��Ŀͻ�������
        /// </summary>
        public void RegisterActivatedClientType(Type type)
        {
            string url = BuildUrl(string.Empty);
            RemotingConfiguration.RegisterActivatedClientType(type, url);
        }

        #region IDisposable Members

        /// <summary>
        /// ע��ͨ��
        /// </summary>
        public void Dispose()
        {
            ChannelServices.UnregisterChannel(clientChannel);
        }

        #endregion

        #region ILogable Members

        /// <summary>
        /// ��־�����¼�
        /// </summary>
        public event LogHandler OnLog;

        #endregion
    }
}
