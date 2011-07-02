using System;
using System.Collections;
using System.Xml.Serialization;

namespace MySoft.Web.Configuration
{
    /// <summary>
    /// The UpdateRuleCollection models a set of UpdateRules in the Web.config file.
    /// </summary>
    /// <remarks>
    /// The UpdateRuleCollection is expressed in XML as:
    /// <code>
    /// &lt;UpdateRule&gt;
    ///   &lt;LookFor&gt;<i>pattern to search for</i>&lt;/LookFor&gt;
    ///   &lt;SendTo&gt;<i>string to redirect to</i>&lt;/LookFor&gt;
    /// &lt;UpdateRule&gt;
    /// &lt;UpdateRule&gt;
    ///   &lt;LookFor&gt;<i>pattern to search for</i>&lt;/LookFor&gt;
    ///   &lt;SendTo&gt;<i>string to redirect to</i>&lt;/LookFor&gt;
    /// &lt;UpdateRule&gt;
    /// ...
    /// &lt;UpdateRule&gt;
    ///   &lt;LookFor&gt;<i>pattern to search for</i>&lt;/LookFor&gt;
    ///   &lt;SendTo&gt;<i>string to redirect to</i>&lt;/LookFor&gt;
    /// &lt;UpdateRule&gt;
    /// </code>
    /// </remarks>
    [Serializable]
    public class UpdateRuleCollection : CollectionBase
    {
        /// <summary>
        /// Adds a new UpdateRule to the collection.
        /// </summary>
        /// <param name="r">A UpdateRule instance.</param>
        public virtual void Add(UpdateRule r)
        {
            this.InnerList.Add(r);
        }

        /// <summary>
        /// Gets or sets a UpdateRule at a specified ordinal index.
        /// </summary>
        public UpdateRule this[int index]
        {
            get
            {
                return (UpdateRule)this.InnerList[index];
            }
            set
            {
                this.InnerList[index] = value;
            }
        }
    }
}
