using System;
using System.Runtime.Remoting.Channels;
using System.Collections;

namespace CompressionSink
{
    public class myServerSinkProvider : IServerChannelSinkProvider
    {
        private IServerChannelSinkProvider _nextProvider;

        public myServerSinkProvider(IDictionary properties, ICollection providerData)
        {
            // not yet needed
        }

        public IServerChannelSinkProvider Next
        {
            get
            {
                return _nextProvider;
            }
            set
            {
                _nextProvider = value;
            }
        }

        public IServerChannelSink CreateSink(IChannelReceiver channel)
        {
            // create other sinks in the chain
            IServerChannelSink next = _nextProvider.CreateSink(channel);

            // put our sink on top of the chain and return it				
            return new myServerSink(next);
        }

        public void GetChannelData(IChannelDataStore channelData)
        {
            // not yet needed
        }

    }
}
