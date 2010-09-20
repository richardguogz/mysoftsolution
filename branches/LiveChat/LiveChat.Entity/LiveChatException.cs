using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Permissions;
using System.Runtime.Serialization;

namespace LiveChat.Utils
{
    [Serializable]
    public class LiveChatException : Exception, ISerializable
    {
        public LiveChatException() { }
        public LiveChatException(string message) : base(message) { }
        public LiveChatException(string message, Exception inner) : base(message, inner) { }
        protected LiveChatException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
