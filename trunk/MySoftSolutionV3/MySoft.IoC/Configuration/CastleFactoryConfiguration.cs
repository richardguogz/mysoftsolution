using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace MySoft.IoC.Configuration
{
    /// <summary>
    /// The service factory configuration.
    /// </summary>
    public class CastleFactoryConfiguration : ConfigurationBase
    {
        private IDictionary<string, ServerNode> nodes;
        private CastleFactoryType type = CastleFactoryType.Local;
        private string defaultKey;                                  //Ĭ�Ϸ���
        private string appname;                                     //host����
        private bool throwError = true;                             //�׳��쳣

        /// <summary>
        /// ʵ����CastleFactoryConfiguration
        /// </summary>
        public CastleFactoryConfiguration()
        {
            this.nodes = new Dictionary<string, ServerNode>();
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
        /// <param name="xmlnode"></param>
        public void LoadValuesFromConfigurationXml(XmlNode xmlnode)
        {
            if (xmlnode == null) return;

            XmlAttributeCollection attribute = xmlnode.Attributes;

            if (attribute["type"] != null && attribute["type"].Value.Trim() != string.Empty)
                type = (CastleFactoryType)Enum.Parse(typeof(CastleFactoryType), attribute["type"].Value, true);

            if (attribute["throwError"] != null && attribute["throwError"].Value.Trim() != string.Empty)
                throwError = Convert.ToBoolean(attribute["throwError"].Value);

            if (attribute["default"] != null && attribute["default"].Value.Trim() != string.Empty)
                defaultKey = attribute["default"].Value;

            if (attribute["appname"] != null && attribute["appname"].Value.Trim() != string.Empty)
                appname = attribute["appname"].Value;

            foreach (XmlNode child in xmlnode.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Comment) continue;

                XmlAttributeCollection childattribute = child.Attributes;
                if (child.Name == "serverNode")
                {
                    var node = new ServerNode();
                    node.Key = childattribute["key"].Value;
                    node.IP = childattribute["ip"].Value;
                    node.Port = Convert.ToInt32(childattribute["port"].Value);

                    //��ʱʱ�䣬Ĭ��Ϊ1����
                    if (childattribute["timeout"] != null && childattribute["timeout"].Value.Trim() != string.Empty)
                        node.Timeout = Convert.ToInt32(childattribute["timeout"].Value);

                    //������ӳ�
                    if (childattribute["maxpool"] != null && childattribute["maxpool"].Value.Trim() != string.Empty)
                        node.MaxPool = Convert.ToInt32(childattribute["maxpool"].Value);

                    if (childattribute["encrypt"] != null && childattribute["encrypt"].Value.Trim() != string.Empty)
                        node.Encrypt = Convert.ToBoolean(childattribute["encrypt"].Value);

                    if (childattribute["compress"] != null && childattribute["compress"].Value.Trim() != string.Empty)
                        node.Compress = Convert.ToBoolean(childattribute["compress"].Value);

                    if (childattribute["invoke"] != null && childattribute["invoke"].Value.Trim() != string.Empty)
                        node.Invoke = Convert.ToBoolean(childattribute["invoke"].Value);

                    //����Ĭ�ϵķ���
                    if (string.IsNullOrEmpty(defaultKey))
                    {
                        defaultKey = node.Key;
                    }

                    if (nodes.ContainsKey(node.Key))
                        throw new WarningException("Already exists server node ��" + node.Key + "��.");

                    nodes[node.Key] = node;
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
                    throw new WarningException("Not find the default service node ��" + defaultKey + "����");
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
        /// Gets or sets the nodes
        /// </summary>
        /// <value>The nodes.</value>
        public IDictionary<string, ServerNode> Nodes
        {
            get { return nodes; }
            set { nodes = value; }
        }
    }
}
