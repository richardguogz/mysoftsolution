using System;
using System.Collections;
using MySoft.Json;

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
                JSONObject json = new JSONObject();
                foreach (string key in parmValues.Keys)
                {
                    json.put(key, parmValues[key]);
                }

                return json.ToString();
            }
            set
            {
                JSONObject json = new JSONObject(value);
                parmValues.Clear();
                IEnumerator en = json.keys();
                while (en.MoveNext())
                {
                    string key = (string)en.Current;
                    parmValues.Add(key, json.getString(key));
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterCollection"/> class.
        /// </summary>
        public ParameterCollection()
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="System.String"/> with the specified param name.
        /// </summary>
        /// <value></value>
        public string this[string paramName]
        {
            get
            {
                if (!parmValues.ContainsKey(paramName))
                {
                    return null;
                }
                else
                {
                    return (string)parmValues[paramName];
                }
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                object val = value;
                if (parmValues.ContainsKey(paramName))
                {
                    parmValues[paramName] = val;
                }
                else
                {
                    parmValues.Add(paramName, val);
                }
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
