using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MySoft.IoC
{
    /// <summary>
    /// The response msg.
    /// </summary>
    [Serializable]
    public class ResponseMessage : RequestMessage
    {
        private RequestMessage request;
        private object data;

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
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public object Data
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

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message
        {
            get
            {
                if (data == null)
                {
                    return "empty data";
                }

                if (data is byte[])
                {
                    return string.Format("packet size: {0} bytes", ((byte[])data).Length);
                }
                else if (data is Exception)
                {
                    return ErrorHelper.GetInnerException(data as Exception).Message;
                }
                else
                {
                    return data.ToString();
                }
            }
        }
    }
}