using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MySoft.IoC.Service
{
    /// <summary>
    /// The response msg.
    /// </summary>
    [Serializable]
    public class ResponseMessage : RequestMessage
    {
        private RequestMessage request;
        private string text;
        private DataSet data;
        private Type type;

        /// <summary>
        /// Gets or sets the request.
        /// </summary>
        /// <value>The request.</value>
        public RequestMessage Request
        {
            get
            {
                return request;
            }
            set
            {
                request = value;
            }
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message
        {
            get
            {
                if (type == null)
                    return text;
                else if (type == typeof(DataSet))
                    return "[DataSet]";

                return text;
            }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public Type Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public DataSet Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }
    }
}