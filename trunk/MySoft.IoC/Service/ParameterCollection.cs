using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace MySoft.IoC
{
    /// <summary>
    /// The parameter collection type used by request msg.
    /// </summary>
    [Serializable]
    public class ParameterCollection
    {
        private Hashtable parmValues = new Hashtable();

        /// <summary>
        /// Gets or sets the serialized data.
        /// </summary>
        /// <value>The serialized data.</value>
        public string SerializedData
        {
            get
            {
                JObject json = new JObject();
                foreach (string key in parmValues.Keys)
                {
                    //�����ݽ���ϵ�л�
                    var jsonString = SerializationManager.SerializeJson(parmValues[key]);

                    //��ӵ�json����
                    json.Add(key, JToken.Parse(jsonString));
                }

                return json.ToString(Formatting.None);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterCollection"/> class.
        /// </summary>
        public ParameterCollection() { }

        /// <summary>
        /// Gets or sets the <see cref="System.String"/> with the specified param name.
        /// </summary>
        /// <value></value>
        public object this[string paramName]
        {
            get
            {
                if (parmValues.ContainsKey(paramName))
                {
                    return parmValues[paramName];
                }
                return null;
            }
            set
            {
                //if (value == null) return;
                parmValues[paramName] = value;
            }
        }

        /// <summary>
        /// Removes the specified param name.
        /// </summary>
        /// <param name="paramName">Name of the param.</param>
        public void Remove(string paramName)
        {
            if (parmValues.ContainsKey(paramName))
            {
                parmValues.Remove(paramName);
            }
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            parmValues.Clear();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return SerializedData;
        }
    }
}
