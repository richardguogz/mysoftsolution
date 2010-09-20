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
    /// Remoting客户端帮助类
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
        /// 构造函数
        /// </summary>
        /// <param name="channelType">通道类型</param>
        /// <param name="serverAddress">服务器地址</param>
        /// <param name="serverPort">服务器端口</param>
        /// <param name="callbackPort">回调端口</param>
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

                //设置反序列化级别为Full，支持远程处理在所有情况下支持的所有类型
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
        /// 获取知名对象客户端实例
        /// </summary>
        /// <param name="notifyName">远程对象发布时的名称</param>
        public T GetWellKnownClientInstance<T>(string notifyName)
        {
            string url = BuildUrl(notifyName);
            RemotingConfiguration.RegisterWellKnownClientType(typeof(T), url);
            T instance = (T)Activator.GetObject(typeof(T), url);

            if (instance != null)
            {
                WriteLog(string.Format("已创建[{0}]的客户端代理",instance.ToString()));
            }

            return instance;
        }

        /// <summary>
        /// 创建客户端激活对象的实例
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
                WriteLog(string.Format("已创建[{0}]的客户端代理", instance.ToString()));
            }

            return instance;
        }

        /// <summary>
        /// 检测指定的客户端类型是否注册
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
        /// 注册激活的客户端类型
        /// </summary>
        public void RegisterActivatedClientType(Type type)
        {
            string url = BuildUrl(string.Empty);
            RemotingConfiguration.RegisterActivatedClientType(type, url);
        }

        #region IDisposable Members

        /// <summary>
        /// 注销通道
        /// </summary>
        public void Dispose()
        {
            ChannelServices.UnregisterChannel(clientChannel);
        }

        #endregion

        #region ILogable Members

        /// <summary>
        /// 日志处理事件
        /// </summary>
        public event LogHandler OnLog;

        #endregion
    }
}
