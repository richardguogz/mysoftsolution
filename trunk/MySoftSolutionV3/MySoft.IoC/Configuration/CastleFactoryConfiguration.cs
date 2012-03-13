using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace MySoft.IoC.Configuration
{
    /// <summary>
    /// ���ݸ�ʽ
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// ������
        /// </summary>
        Binary,
        /// <summary>
        /// Json��ʽ
        /// </summary>
        Json
    }

    /// <summary>
    /// The service factory configuration.
    /// </summary>
    public class CastleFactoryConfiguration : ConfigurationBase
    {
        private IDictionary<string, RemoteNode> nodes;
        private CastleFactoryType type = CastleFactoryType.Local;
        private string defaultKey;                                  //Ĭ�Ϸ���
        private string appname;                                     //host����
        private bool throwError = true;                             //�׳��쳣
        private DataType format = DataType.Binary;                //���ݸ�ʽ��Ĭ��Ϊbinary��ʽ

        /// <summary>
        /// ʵ����CastleFactoryConfiguration
        /// </summary>
        public CastleFactoryConfiguration()
        {
            this.nodes = new Dictionary<string, RemoteNode>();
        }

        /// <summary>
        /// ��ȡԶ�̶�������
        /// </summary>
        /// <returns></returns>
        public static CastleFactoryConfiguration GetConfig()
        {
            string key = "mysoft.framework/castleFactory";
            CastleFactoryConfiguration obj = CacheHelper.Get<CastleFactoryConfiguration>(key);
            if (obj == null)
            {
                var tmp = ConfigurationManager.GetSection(key);
                obj = tmp as CastleFactoryConfiguration;
                CacheHelper.Permanent(key, obj);
            }

            return obj;
        }

        /// <summary>
        /// �������ļ���������ֵ
        /// </summary>
        /// <param name="node"></param>
        public void LoadValuesFromConfigurationXml(XmlNode node)
        {
            if (node == null) return;

            XmlAttributeCollection xmlnode = node.Attributes;

            if (xmlnode["type"] != null && xmlnode["type"].Value.Trim() != string.Empty)
                type = (CastleFactoryType)Enum.Parse(typeof(CastleFactoryType), xmlnode["type"].Value, true);

            if (xmlnode["throwError"] != null && xmlnode["throwError"].Value.Trim() != string.Empty)
                throwError = Convert.ToBoolean(xmlnode["throwError"].Value);

            if (xmlnode["format"] != null && xmlnode["format"].Value.Trim() != string.Empty)
                format = (DataType)Enum.Parse(typeof(DataType), xmlnode["format"].Value, true);

            if (xmlnode["default"] != null && xmlnode["default"].Value.Trim() != string.Empty)
                defaultKey = xmlnode["default"].Value;

            if (xmlnode["appname"] != null && xmlnode["appname"].Value.Trim() != string.Empty)
                appname = xmlnode["appname"].Value;

            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Comment) continue;

                XmlAttributeCollection childnode = child.Attributes;
                if (child.Name == "serverNode")
                {
                    RemoteNode remoteNode = new RemoteNode();
                    remoteNode.Key = childnode["key"].Value;
                    remoteNode.IP = childnode["ip"].Value;
                    remoteNode.Port = Convert.ToInt32(childnode["port"].Value);

                    //��ʱʱ�䣬Ĭ��Ϊ1����
                    if (childnode["timeout"] != null && childnode["timeout"].Value.Trim() != string.Empty)
                        remoteNode.Timeout = Convert.ToInt32(childnode["timeout"].Value);

                    //������ӳ�
                    if (childnode["maxpool"] != null && childnode["maxpool"].Value.Trim() != string.Empty)
                        remoteNode.MaxPool = Convert.ToInt32(childnode["maxpool"].Value);

                    if (childnode["encrypt"] != null && childnode["encrypt"].Value.Trim() != string.Empty)
                        remoteNode.Encrypt = Convert.ToBoolean(childnode["encrypt"].Value);

                    if (childnode["compress"] != null && childnode["compress"].Value.Trim() != string.Empty)
                        remoteNode.Compress = Convert.ToBoolean(childnode["compress"].Value);

                    //����Ĭ�ϵķ���
                    if (string.IsNullOrEmpty(defaultKey))
                    {
                        defaultKey = remoteNode.Key;
                    }

                    if (nodes.ContainsKey(remoteNode.Key))
                        throw new WarningException("Already exists server node ��" + remoteNode.Key + "��.");

                    nodes[remoteNode.Key] = remoteNode;
                }
            }

            if (type != CastleFactoryType.Local)
            {
                //���app����Ϊ��
                if (string.IsNullOrEmpty(appname))
                {
                    throw new WarningException("App name must be provided��");
                }

                //�ж��Ƿ������˷�����Ϣ
                if (nodes.Count == 0)
                {
                    throw new WarningException("Not configure any service node��");
                }

                //�ж��Ƿ����Ĭ�ϵķ���
                if (!nodes.ContainsKey(defaultKey))
                {
                    throw new WarningException("Not find the default service node [" + defaultKey + "]��");
                }
            }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public CastleFactoryType Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Gets or sets the app name.
        /// </summary>
        /// <value>The host name.</value>
        public string AppName
        {
            get { return appname; }
            set { appname = value; }
        }

        /// <summary>
        /// Gets or sets the default
        /// </summary>
        /// <value>The default.</value>
        public string Default
        {
            get { return defaultKey; }
            set { defaultKey = value; }
        }

        /// <summary>
        /// Gets or sets the throwError
        /// </summary>
        /// <value>The throwError.</value>
        public bool ThrowError
        {
            get { return throwError; }
            set { throwError = value; }
        }

        /// <summary>
        /// Gets or sets the format
        /// </summary>
        /// <value>The throwError.</value>
        public DataType DataType
        {
            get { return format; }
            set { format = value; }
        }

        /// <summary>
        /// Gets or sets the nodes
        /// </summary>
        /// <value>The nodes.</value>
        public IDictionary<string, RemoteNode> Nodes
        {
            get { return nodes; }
            set { nodes = value; }
        }
    }
}
