namespace MySoft.Net
{
    using System;

    public class EventRunErrorArgs : EventChannelArgs
    {
        private Exception mError;

        public EventRunErrorArgs(IChannel channel)
            : base(channel)
        {
        }

        public Exception Error
        {
            get
            {
                return this.mError;
            }
            set
            {
                this.mError = value;
            }
        }
    }
}

