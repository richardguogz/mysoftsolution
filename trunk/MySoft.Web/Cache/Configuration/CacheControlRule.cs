﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MySoft.Web.Configuration
{
    /// <summary>
    /// Represents a rewriter rule.  A rewriter rule is composed of a pattern to search for and a string to replace
    /// the pattern with (if matched).
    /// </summary>
    [Serializable]
    [XmlRoot("cacheControlRule")]
    public class CacheControlRule
    {
        private string cachePath;
        private int cacheTime;

        #region Public Properties
        /// <summary>
        /// Gets or sets the pattern to look for.
        /// </summary>
        /// <remarks><b>CacheName</b> is a regular expression pattern.  Therefore, you might need to escape
        /// characters in the pattern that are reserved characters in regular expression syntax (., ?, ^, $, etc.).
        /// <p />
        /// The pattern is searched for using the <b>System.Text.RegularExpression.Regex</b> class's <b>IsMatch()</b>
        /// method.  The pattern is case insensitive.</remarks>
        [XmlElement("cachePath")]
        public string CachePath
        {
            get
            {
                return cachePath;
            }
            set
            {
                cachePath = value;
            }
        }

        /// <summary>
        /// The string to replace the pattern with, if found.
        /// </summary>
        /// <remarks>The replacement string may use grouping symbols, like $1, $2, etc.  Specifically, the
        /// <b>System.Text.RegularExpression.Regex</b> class's <b>Replace()</b> method is used to replace
        /// the match in <see cref="CacheTime"/> with the value in <b>SendTo</b>.</remarks>
        [XmlElement("cacheTime")]
        public int CacheTime
        {
            get
            {
                return cacheTime;
            }
            set
            {
                cacheTime = value;
            }
        }

        #endregion
    }
}
