using System.Collections;
using System.Runtime.Remoting.Channels;

namespace MySoft.Remoting.CompressionSink
{
    public class ZipServerSinkProvider : IServerChannelSinkProvider
    {
        private IServerChannelSinkProvider _nextProvider;
        private ZipSinkType _zipType = ZipSinkType.GZip;

        public ZipServerSinkProvider(ZipSinkType zipType)
        {
            _zipType = zipType;
        }

        public ZipServerSinkProvider(IDictionary properties, ICollection providerData)
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

        public IServerChannelSink CreateSink(IChannelReceiver channel)
        {
            // create other sinks in the chain
            IServerChannelSink next = _nextProvider.CreateSink(channel);

            // put our sink on top of the chain and return it				
            return new ZipServerSink(next, _zipType);
        }

        public void GetChannelData(IChannelDataStore channelData)
        {
            // not yet needed
        }

    }
}
