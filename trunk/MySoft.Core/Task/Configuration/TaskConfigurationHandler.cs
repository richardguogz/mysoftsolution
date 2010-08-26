using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace MySoft.Core.Task
{
    /// <summary>
    /// 
    /// </summary>
    public class TaskConfigurationHandler : IConfigurationSectionHandler
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
            TaskConfiguration config = new TaskConfiguration();
            config.LoadValuesFromConfigurationXml(section);
            return config;
        }

        #endregion
    }
}
