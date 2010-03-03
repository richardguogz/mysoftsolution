namespace MySoft.Net
{
    using System;

    public class EventChannelReceiveArgs : EventChannelArgs
    {
        private byte[] mData;

        public EventChannelReceiveArgs(IChannel channel) : base(channel)
        {
        }

        public byte[] Data
        {
            get
            {
                return this.mData;
            }
            set
            {
                this.mData = value;
            }
        }
    }
}

