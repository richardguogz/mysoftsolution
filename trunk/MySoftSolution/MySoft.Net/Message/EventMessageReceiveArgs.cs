namespace MySoft.Net.Message
{
    using System;

    public class EventMessageReceiveArgs : EventArgs
    {
        private IMessageChannel mChannel;
        private IMessage mMessage;

        public EventMessageReceiveArgs(IMessage message, IMessageChannel mc)
        {
            this.mMessage = message;
            this.mChannel = mc;
        }

        public IMessageChannel Channel
        {
            get
            {
                return this.mChannel;
            }
        }

        public IMessage Message
        {
            get
            {
                return this.mMessage;
            }
        }
    }
}

