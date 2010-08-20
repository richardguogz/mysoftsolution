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
    }
}