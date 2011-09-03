using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.Remoting;
using System.Xml;

namespace MySoft.Remoting.Configuration
{
    /// <summary>
    /// Remoting���������
    /// <example>
    /// <code>
    /// <configuration>
    ///     <configSections>
    /// 	    <sectionGroup name="mysoft.framework">
    /// 		    <section name="remotingServer" type="MySoft.Remoting.Configuration.RemotingServerConfigurationHandler, MySoft"/>
    /// 	    </sectionGroup>
    ///     </configSections>
    ///     <system.web>
    /// 	......
    ///     </system.web>
    ///     <mysoft.framework>
    /// 	    <remotingServer>
    ///             <server channelType="tcp" serverAddress="127.0.0.1" port="8888" compress="true"/>
    /// 		    <remoteObject name="����ֵ" assemblyName="Shumi.BLL" className="SB.NetValue" mode="singleton" />
    /// 		    <remoteObject name="����ֵ" assemblyName="Shumi.BLL" className="SB.NetValue" mode="singlecall" />
    /// 	    </remotingServer>
    ///     </mysoft.framework>
    /// </configuration>
    /// </code>
    /// </example>
    /// </summary>
    public class RemotingServerConfiguration : ConfigurationBase
    {
        /// <summary>
        /// ��ȡԶ�̶�������
        /// </summary>
        /// <returns></returns>
        public static RemotingServerConfiguration GetConfig()
        {
            string key = "mysoft.framework/remotingServer";
            RemotingServerConfiguration obj = CacheHelper.Get<RemotingServerConfiguration>(key);
            if (obj == null)
            {
                var tmp = ConfigurationManager.GetSection(key);
                obj = tmp as RemotingServerConfiguration;
                CacheHelper.Insert(key, obj, 60);
            }

            return obj;
        }

        List<ServiceModule> _Modules = new List<ServiceModule>();

        /// <summary>
        /// ��ȡԶ�̶���ҵ��ģ�鼯�ϣ������URL��Ϣ��Э�飬IP���˿ڣ�
        /// </summary>
        public List<ServiceModule> Modules
        {
            get { return _Modules; }
        }

        private RemotingChannelType _ChannelType = RemotingChannelType.Tcp;

        /// <summary>
        /// ͨ������
        /// </summary>
        public RemotingChannelType ChannelType
        {
            get { return _ChannelType; }
        }

        private string _ServerAddress;

        /// <summary>
        /// ��������ַ��IP����������
        /// </summary>
        public string ServerAddress
        {
            get { return _ServerAddress; }
        }

        private int _Port = 8888;

        /// <summary>
        /// �˿�
        /// </summary>
        public int Port
        {
            get { return _Port; }
            set { _Port = value; }
        }

        /// <summary>
        /// Remoting������Url���磺tcp://127.0.0.1:8888��
        /// </summary>
        public string ServerUrl
        {
            get
            {
                return string.Format("{0}://{1}:{2}", _ChannelType, _ServerAddress, _Port);
            }
        }

        /// <summary>
        /// ��ȡԶ��ҵ�����Url���磺tcp://127.0.0.1:8888/NetValue��
        /// </summary>
        /// <param name="remoteObjectUri">Զ�̶���Uri���磺NetValue��</param>
        /// <returns></returns>
        public string GetRemoteObjectUrl(string remoteObjectUri)
        {
            string url = string.Format("{0}://{1}:{2}/{3}", _ChannelType, _ServerAddress, _Port, remoteObjectUri);
            return url;
        }

        /// <summary>
        /// �������ļ���������ֵ
        /// </summary>
        /// <param name="node"></param>
        public void LoadValuesFromConfigurationXml(XmlNode node)
        {
            if (node == null) return;

            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.NodeType == XmlNodeType.Comment) continue;

                if (n.Name == "server")
                {
                    XmlAttributeCollection ac = n.Attributes;
                    this._ChannelType = ac["channelType"].Value.ToLower() == "tcp" ? RemotingChannelType.Tcp : RemotingChannelType.Http;
                    this._ServerAddress = ac["serverAddress"].Value;
                    this._Port = Convert.ToInt32(ac["port"].Value);
                }

                if (n.Name == "remoteObject")
                {
                    XmlAttributeCollection ac = n.Attributes;
                    ServiceModule module = new ServiceModule();
                    module.Name = ac["name"].Value;
                    module.AssemblyName = ac["assemblyName"].Value;
                    module.ClassName = ac["className"].Value;

                    if (string.IsNullOrEmpty(ac["mode"].Value))
                    {
                        module.Mode = ac["mode"].Value.ToLower() == "singleton" ? WellKnownObjectMode.Singleton : WellKnownObjectMode.SingleCall;
                    }

                    this._Modules.Add(module);
                }
            }
        }

    }
}