using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace MySoft.Core.Remoting
{
    /// <summary>
    /// 
    /// </summary>
    public class RemotingServerConfigurationHandler : IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler ≥…‘±

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="configContext"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            RemotingServerConfiguration config = new RemotingServerConfiguration();
            config.LoadValuesFromConfigurationXml(section);
            return config;
        }

        #endregion
    }
}
