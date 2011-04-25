﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace MySoft.IoC
{
    /// <summary>
    /// 服务配置类
    /// </summary>
    public class CastleServiceConfigurationHandler : IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler 成员

        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            CastleServiceConfiguration config = new CastleServiceConfiguration();
            config.LoadValuesFromConfigurationXml(section);
            return config;
        }

        #endregion
    }
}
