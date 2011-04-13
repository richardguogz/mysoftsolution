using System.Collections;
using System.Runtime.Remoting.Channels;

namespace MySoft.Remoting.CompressionSink
{
    public class ZipClientSinkProvider : IClientChannelSinkProvider
    {
        private IClientChannelSinkProvider _nextProvider;
        private ZipSinkType _zipType = ZipSinkType.GZip;

        public ZipClientSinkProvider(ZipSinkType zipType)
        {
            _zipType = zipType;
        }

        public ZipClientSinkProvider(IDictionary properties, ICollection providerData)
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

        public ZipSinkType ZipType
        {
            get
            {
                return _zipType;
            }
            set
            {
                _zipType = value;
            }
        }

        public IClientChannelSink CreateSink(IChannelSender channel, string url, object remoteChannelData)
        {
            // create other sinks in the chain
            IClientChannelSink next = _nextProvider.CreateSink(channel,
                url,
                remoteChannelData);

            // put our sink on top of the chain and return it				
            return new ZipClientSink(next, _zipType);
        }
    }
}
