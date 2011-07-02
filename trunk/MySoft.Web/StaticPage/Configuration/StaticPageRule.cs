using System;
using System.Xml.Serialization;

namespace MySoft.Web.Configuration
{
    /// <summary>
    /// Represents a rewriter rule.  A rewriter rule is composed of a pattern to search for and a string to replace
    /// the pattern with (if matched).
    /// </summary>
    [Serializable]
    [XmlRoot("rule")]
    public class StaticPageRule
    {
        // private member variables...
        private string lookFor, writeTo, validateString;
        private int timeout;
        private UpdateRule[] updates = new UpdateRule[0];

        #region Public Properties
        /// <summary>
        /// Gets or sets the pattern to look for.
        /// </summary>
        /// <remarks><b>LookFor</b> is a regular expression pattern.  Therefore, you might need to escape
        /// characters in the pattern that are reserved characters in regular expression syntax (., ?, ^, $, etc.).
        /// <p />
        /// The pattern is searched for using the <b>System.Text.RegularExpression.Regex</b> class's <b>IsMatch()</b>
        /// method.  The pattern is case insensitive.</remarks>
        [XmlAttribute("lookFor")]
        public string LookFor
        {
            get
            {
                return lookFor;
            }
            set
            {
                lookFor = value;
            }
        }

        /// <summary>
        /// The string to replace the pattern with, if found.
        /// </summary>
        /// <remarks>The replacement string may use grouping symbols, like $1, $2, etc.  Specifically, the
        /// <b>System.Text.RegularExpression.Regex</b> class's <b>Replace()</b> method is used to replace
        /// the match in <see cref="LookFor"/> with the value in <b>SendTo</b>.</remarks>
        [XmlAttribute("writeTo")]
        public string WriteTo
        {
            get
            {
                return writeTo;
            }
            set
            {
                writeTo = value;
            }
        }

        /// <summary>
        /// 过期时间，单位为秒
        /// </summary>
        [XmlAttribute("timeout")]
        public int Timeout
        {
            get
            {
                return timeout;
            }
            set
            {
                timeout = value;
            }
        }

        /// <summary>
        /// 验证字符串
        /// </summary>
        [XmlAttribute("validate")]
        public string ValidateString
        {
            get
            {
                return validateString;
            }
            set
            {
                validateString = value;
            }
        }

        /// <summary>
        /// 更新规则
        /// </summary>
        [XmlElement("update", typeof(UpdateRule))]
        public UpdateRule[] Updates
        {
            get
            {
                return updates;
            }
            set
            {
                updates = value;
            }
        }

        #endregion
    }
}
