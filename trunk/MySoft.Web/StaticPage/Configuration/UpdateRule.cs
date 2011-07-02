using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MySoft.Web.Configuration
{
    /// <summary>
    /// 更新规则
    /// </summary>
    [Serializable]
    public class UpdateRule
    {
        private string searchFor, replaceTo;

        /// <summary>
        /// 搜索从
        /// </summary>
        [XmlAttribute("searchFor")]
        public string SearchFor
        {
            get
            {
                return searchFor;
            }
            set
            {
                searchFor = value;
            }
        }

        /// <summary>
        /// 替换成
        /// </summary>
        [XmlAttribute("replaceTo")]
        public string ReplaceTo
        {
            get
            {
                return replaceTo;
            }
            set
            {
                replaceTo = value;
            }
        }
    }
}
