using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySoft.Net.Sockets;

namespace MySoft.IoC
{
    /// <summary>
    /// The response msg.
    /// </summary>
    [Serializable]
    [BufferType(10000)]
    public class ResponseMessage : RequestBase
    {
        private byte[] keys;

        /// <summary>
        /// Gets or sets the keys of the service.
        /// </summary>
        public byte[] Keys
        {
            get
            {
                return keys;
            }
            set
            {
                keys = value;
            }
        }

        private byte[] data;

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public byte[] Data
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

        private Exception exception;

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception
        {
            get
            {
                return exception;
            }
            set
            {
                exception = value;
            }
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public override string Message
        {
            get
            {
                //如果发生了异常
                if (exception != null)
                {
                    return "Occured error: " + exception.Message;
                }

                if (data == null)
                {
                    return "Data is empty.";
                }
                else
                {
                    return string.Format("Packet size (return:{1} compress:{2} encrypt:{3}): {0} bytes.", data.Length, base.ReturnType, base.Compress, base.Encrypt);
                }
            }
        }
    }
}