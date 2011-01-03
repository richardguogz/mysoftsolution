using System.Collections;
using System.Runtime.Remoting.Channels;

namespace CompressionSink
{
    public class myClientSinkProvider : IClientChannelSinkProvider
    {
        private IClientChannelSinkProvider _nextProvider;

        public myClientSinkProvider(IDictionary properties, ICollection providerData)
        {
            // not yet needed
        }

        public IClientChannelSinkProvider Next
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

        public IClientChannelSink CreateSink(IChannelSender channel, string url, object remoteChannelData)
        {
            // create other sinks in the chain
            IClientChannelSink next = _nextProvider.CreateSink(channel,
                url,
                remoteChannelData);

            // put our sink on top of the chain and return it				
            return new myClientSink(next);
        }
    }
}
