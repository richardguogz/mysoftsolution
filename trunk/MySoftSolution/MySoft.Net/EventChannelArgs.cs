namespace MySoft.Net
{
    using System;

    public class EventChannelArgs : EventArgs
    {
        private IChannel mChannel;

        public EventChannelArgs(IChannel channel)
        {
            this.mChannel = channel;
        }

        public IChannel Channel
        {
            get
            {
                return this.mChannel;
            }
        }
    }
}

