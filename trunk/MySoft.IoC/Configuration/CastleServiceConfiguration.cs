using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;
using MySoft.IoC;

namespace MySoft.IoC.Configuration
{
    /// <summary>
    /// The service factory configuration.
    /// </summary>
    public class CastleServiceConfiguration : ConfigurationBase
    {
        private string host = "any";
        private int port = 8888;
        private int logtime = 1000;             //超时多长输出日志，默认为1秒
        private int records = 3600;            //记录条数，默认为3600条，1小时记录
        private int maxconnect = SimpleServiceContainer.DEFAULT_MAXCONNECT_NUMBER;
        private int maxbuffer = SimpleServiceContainer.DEFAULT_MAXBUFFER_NUMBER;

        /// <summary>
        /// 获取远程对象配置
        /// </summary>
        /// <returns></returns>
        public static CastleServiceConfiguration GetConfig()
        {
            object obj = ConfigurationManager.GetSection("mysoft.framework/castleService");

            if (obj != null)
                return (CastleServiceConfiguration)obj;
            else
                return null;
        }

        /// <summary>
        /// 从配置文件加载配置值
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

            if (xmlnode["maxconnect"] != null && xmlnode["maxconnect"].Value.Trim() != string.Empty)
                maxconnect = Convert.ToInt32(xmlnode["maxconnect"].Value);

            if (xmlnode["maxbuffer"] != null && xmlnode["maxbuffer"].Value.Trim() != string.Empty)
                maxbuffer = Convert.ToInt32(xmlnode["maxbuffer"].Value);

            if (xmlnode["logtime"] != null && xmlnode["logtime"].Value.Trim() != string.Empty)
                logtime = Convert.ToInt32(xmlnode["logtime"].Value);

            if (xmlnode["records"] != null && xmlnode["records"].Value.Trim() != string.Empty)
                records = Convert.ToInt32(xmlnode["records"].Value);
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
        /// Gets or sets the maxconnect
        /// </summary>
        /// <value>The maxconnect.</value>
        public int MaxConnect
        {
            get { return maxconnect; }
            set { maxconnect = value; }
        }

        /// <summary>
        /// Gets or sets the maxbuffer
        /// </summary>
        /// <value>The maxbuffer.</value>
        public int MaxBuffer
        {
            get { return maxbuffer; }
            set { maxbuffer = value; }
        }

        /// <summary>
        /// Gets or sets the logtime
        /// </summary>
        /// <value>The logtime.</value>
        public int LogTime
        {
            get { return logtime; }
            set { logtime = value; }
        }

        /// <summary>
        /// Gets or sets the records
        /// </summary>
        /// <value>The records.</value>
        public int Records
        {
            get { return records; }
            set { records = value; }
        }
    }
}
