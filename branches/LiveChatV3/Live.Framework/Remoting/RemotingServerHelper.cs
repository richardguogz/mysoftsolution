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
    /// Remoting����˰�����
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
        /// ��־�����¼�
        /// </summary>
        public event LogHandler OnLog;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="channelType">ͨ������</param>
        /// <param name="serverAddress">��������ַ</param>
        /// <param name="port">�˿�</param>
        public RemotingServerHelper(RemotingChannelType channelType, string serverAddress, int port)
        {
            this.channelType = channelType;
            this.serverAddress = serverAddress;
            this.port = port;

            Init(channelType, port);
        }

        /// <summary>
        /// ���캯����ͨ�������ļ�ʵ����
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
                IPHostEntry hostEntry = Dns.Resolve(Environment.MachineName);///todo:Dns.Resolve(Environment.MachineName)��ʱ
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
                //ʹ�ö����Ƹ�ʽ��
                BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
                BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();

                //���÷����л�����ΪFull��֧��Զ�̴��������������֧�ֵ���������
                serverProvider.TypeFilterLevel = TypeFilterLevel.Full;

                IDictionary props = new Hashtable();
                props["name"] = AppDomain.CurrentDomain.FriendlyName;
                props["port"] = port;

                //��ʼ��ͨ����ע��
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
        /// ����֪����������ʵ��
        /// </summary>
        /// <param name="notifyName">Զ�̶��󷢲�ʱ������</param>
        /// <param name="interfaceType">�ӿ����ͣ�����Remotingʹ�û��ڽӿڵķ�ʽ��ÿ��Զ�̶�������ǻ��ڽӿڵ�ʵ�֣��ýӿ���Ҫ�ڿͻ���ʹ�ã�</param>
        /// <param name="instance">Զ�̶���ʵ��</param>
        /// <param name="mode">Զ�̶��󼤻ʽ</param>
        public void PublishWellKnownServiceInstance(string notifyName, Type interfaceType, MarshalByRefObject instance, WellKnownObjectMode mode)
        {
            RemotingConfiguration.RegisterWellKnownServiceType(interfaceType, BuildUrl(notifyName), mode);
            ObjRef objRef = RemotingServices.Marshal(instance, notifyName);
            WriteLog(string.Format("Զ�̶���[{0}]�����ɹ���URL��{1}", instance.ToString(), BuildUrl(notifyName)));
        }

        /// <summary>
        /// ����֪�������������ʵ����Զ�̶������������ļ��ж��壩
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
                        WriteLog(string.Format("Զ�̶���[{0}]�����ɹ���URL��{1}", m.ClassName, BuildUrl(m.ClassName)));
                    }
                    else
                    {
                        WriteLog(string.Format("Զ�̶���[{0}]����ʧ�ܣ�URL��{1}", m.ClassName, BuildUrl(m.ClassName)));
                    }
                    //Assembly assembly = Assembly.Load(m.AssemblyName);
                    //Type t = assembly.GetType(m.Type);
                    //object obj = Activator.CreateInstance(t);
                    //RemotingServices.Marshal((MarshalByRefObject)obj, m.Type);
                }
            }

            PublishRemotingManageModule();
        }

        //����Remoting�������ģ��
        private void PublishRemotingManageModule()
        {
            //����Remoting����ģ��
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(RemotingTest), "RemotingTest", WellKnownObjectMode.Singleton);

            //����Remoting������־�ļ�����ģ��
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(RemotingLogFileManager), "RemotingLogFileManager", WellKnownObjectMode.Singleton);
        }

        /// <summary>
        /// �����ͻ��˼������
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        public void PublishActivatedService(Type serviceType)
        {
            RemotingConfiguration.RegisterActivatedServiceType(serviceType);
            WriteLog(string.Format("�ͻ��˼������[{0}]ע��ɹ��������˿ڣ�{1}", serviceType.ToString(), port));
        }

        #region IDisposable Members

        /// <summary>
        /// ע��ͨ�����ͷ���Դ
        /// </summary>
        public void Dispose()
        {
            ChannelServices.UnregisterChannel(serviceChannel);
        }

        #endregion
    }
}
