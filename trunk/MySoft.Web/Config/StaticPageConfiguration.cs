using System;
using System.Configuration;
using System.Web;
using System.Xml.Serialization;

namespace MySoft.Web.Config
{
    /// <summary>
    /// Specifies the configuration settings in the Web.config for the StaticPageRule.
    /// </summary>
    [XmlRoot("StaticPageConfig")]
    [Serializable]
    public class StaticPageConfiguration
    {
        // private member variables
        private StaticPageRuleCollection rules;			// an instance of the StaticPageRuleCollection class...

        /// <summary>
        /// GetConfig() returns an instance of the <b>StaticPageConfiguration</b> class with the values populated from
        /// the Web.config file.  It uses XML deserialization to convert the XML structure in Web.config into
        /// a <b>StaticPageConfiguration</b> instance.
        /// </summary>
        /// <returns>A <see cref="StaticPageConfiguration"/> instance.</returns>
        public static StaticPageConfiguration GetConfig()
        {
            if (HttpContext.Current.Cache["StaticPageConfig"] == null)
                HttpContext.Current.Cache.Insert("StaticPageConfig", ConfigurationManager.GetSection("StaticPageConfig"));

            return (StaticPageConfiguration)HttpContext.Current.Cache["StaticPageConfig"];
        }

        #region Public Properties
        /// <summary>
        /// A <see cref="StaticPageRuleCollection"/> instance that provides access to a set of <see cref="StaticPageRule"/>s.
        /// </summary>
        public StaticPageRuleCollection Rules
        {
            get
            {
                return rules;
            }
            set
            {
                rules = value;
            }
        }
        #endregion
    }
}
