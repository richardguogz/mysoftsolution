using System;
using System.Configuration;
using System.Xml;

namespace MySoft.IoC.Configuration
{
    /// <summary>
    /// The service factory configuration.
    /// </summary>
    public class CastleServiceConfiguration : ConfigurationBase
    {
        private string host = "any";
        private int port = 8888;
        private int httpPort = 8080;
        private bool httpEnabled = false;
        private string cacheType;
        private bool encrypt = false;
        private bool compress = false;
        private int timeout = ServiceConfig.DEFAULT_SERVER_TIMEOUT;
        private int minuteCalls = ServiceConfig.DEFAULT_MINUTE_CALL_NUMBER;        //Ĭ��Ϊÿ���ӵ���1000�Σ��������쳣
        private int recordNums = ServiceConfig.DEFAULT_RECORD_NUMBER;              //Ĭ�ϼ�¼3600��   //��¼������Ĭ��Ϊ3600����1Сʱ��¼

        /// <summary>
        /// ��ȡԶ�̶�������
        /// </summary>
        /// <returns></returns>
        public static CastleServiceConfiguration GetConfig()
        {
            string key = "mysoft.framework/castleService";
            CastleServiceConfiguration obj = CacheHelper.Get<CastleServiceConfiguration>(key);
            if (obj == null)
            {
                var tmp = ConfigurationManager.GetSection(key);
                obj = tmp as CastleServiceConfiguration;
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

            if (xmlnode["host"] != null && xmlnode["host"].Value.Trim() != string.Empty)
                host = xmlnode["host"].Value;

            if (xmlnode["port"] != null && xmlnode["port"].Value.Trim() != string.Empty)
                port = Convert.ToInt32(xmlnode["port"].Value);

            if (xmlnode["encrypt"] != null && xmlnode["encrypt"].Value.Trim() != string.Empty)
                encrypt = Convert.ToBoolean(xmlnode["encrypt"].Value);

            if (xmlnode["compress"] != null && xmlnode["compress"].Value.Trim() != string.Empty)
                compress = Convert.ToBoolean(xmlnode["compress"].Value);

            if (xmlnode["timeout"] != null && xmlnode["timeout"].Value.Trim() != string.Empty)
                timeout = Convert.ToInt32(xmlnode["timeout"].Value);

            if (xmlnode["recordNums"] != null && xmlnode["recordNums"].Value.Trim() != string.Empty)
                recordNums = Convert.ToInt32(xmlnode["recordNums"].Value);

            if (xmlnode["minuteCalls"] != null && xmlnode["minuteCalls"].Value.Trim() != string.Empty)
                minuteCalls = Convert.ToInt32(xmlnode["minuteCalls"].Value);

            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Comment) continue;

                XmlAttributeCollection childnode = child.Attributes;
                if (child.Name == "httpServer")
                {
                    httpPort = Convert.ToInt32(childnode["port"].Value);
                    httpEnabled = Convert.ToBoolean(childnode["enabled"].Value);
                }
                else if (child.Name == "serverCache")
                {
                    if (childnode["cacheType"] != null && childnode["cacheType"].Value.Trim() != string.Empty)
                        cacheType = childnode["cacheType"].Value;
                }
            }
        }

        #region Http����

        /// <summary>
        /// Gets or sets the httpport
        /// </summary>
        public int HttpPort
        {
            get { return httpPort; }
            set { httpPort = value; }
        }

        /// <summary>
        /// Gets or sets the httpenabled
        /// </summary>
        public bool HttpEnabled
        {
            get { return httpEnabled; }
            set { httpEnabled = value; }
        }

        #endregion

        /// <summary>
        /// Gets or sets the cacheType
        /// </summary>
        public string CacheType
        {
            get { return cacheType; }
            set { cacheType = value; }
        }

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>The host.</value>
        public string Host
        {
            get { return host; }
            set { host = value; }
        }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        /// <summary>
        /// Gets or sets the encrypt.
        /// </summary>
        /// <value>The encrypt.</value>
        public bool Encrypt
        {
            get { return encrypt; }
            set { encrypt = value; }
        }

        /// <summary>
        /// Gets or sets the compress.
        /// </summary>
        /// <value>The format.</value>
        public bool Compress
        {
            get { return compress; }
            set { compress = value; }
        }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        public int Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }

        /// <summary>
        /// Gets or sets the recordNums
        /// </summary>
        /// <value>The recordNums.</value>
        public int RecordNums
        {
            get { return recordNums; }
            set { recordNums = value; }
        }

        /// <summary>
        /// Gets or sets the minuteCalls
        /// </summary>
        public int MinuteCalls
        {
            get { return minuteCalls; }
            set { minuteCalls = value; }
        }
    }
}
