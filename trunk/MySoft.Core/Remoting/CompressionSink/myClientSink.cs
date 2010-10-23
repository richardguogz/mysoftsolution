using System;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.IO;


namespace CompressionSink
{
    public class myClientSink : BaseChannelSinkWithProperties,
                                        IClientChannelSink
    {
        private IClientChannelSink _nextSink;

        public myClientSink(IClientChannelSink next)
        {
            _nextSink = next;
        }

        public IClientChannelSink NextChannelSink
        {
            get
            {
                return _nextSink;
            }
        }


        public void AsyncProcessRequest(IClientChannelSinkStack sinkStack,
                                        IMessage msg,
                                        ITransportHeaders headers,
                                        Stream stream)
        {

            headers["Compress"] = "True";
            // generate a compressed stream using NZipLib
            stream = myHelper.getCompressedStreamCopy(stream);

            // push onto stack and forward the request
            sinkStack.Push(this, null);
            _nextSink.AsyncProcessRequest(sinkStack, msg, headers, stream);
        }


        public void AsyncProcessResponse(IClientResponseChannelSinkStack sinkStack,
                                            object state,
                                            ITransportHeaders headers,
                                            Stream stream)
        {

            // deflate the response
            stream = myHelper.getUncompressedStreamCopy(stream);

            // forward the request
            sinkStack.AsyncProcessResponse(headers, stream);
        }


        public Stream GetRequestStream(IMessage msg, ITransportHeaders headers)
        {
            return _nextSink.GetRequestStream(msg, headers);
        }


        public void ProcessMessage(IMessage msg,
                                   ITransportHeaders requestHeaders,
                                   Stream requestStream,
                                   out ITransportHeaders responseHeaders,
                                   out Stream responseStream)
        {



            requestHeaders["Compress"] = "True";

            Stream localrequestStream =
                myHelper.getCompressedStreamCopy(requestStream);

            Stream localresponseStream;
            // forward the call to the next sink
            _nextSink.ProcessMessage(msg,
                                     requestHeaders,
                                     localrequestStream,
                                     out responseHeaders,
                                     out localresponseStream);

            // deflate the response
            responseStream = myHelper.getUncompressedStreamCopy(localresponseStream);



        }
    }
}
