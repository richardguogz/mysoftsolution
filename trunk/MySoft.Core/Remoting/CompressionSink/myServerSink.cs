using System.IO;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;

namespace CompressionSink
{

    public class myServerSink : BaseChannelSinkWithProperties, IServerChannelSink
    {

        private IServerChannelSink _nextSink;

        public myServerSink(IServerChannelSink next)
        {
            _nextSink = next;
        }

        public IServerChannelSink NextChannelSink
        {
            get
            {
                return _nextSink;
            }
        }

        public void AsyncProcessResponse(IServerResponseChannelSinkStack sinkStack,
            object state,
            IMessage msg,
            ITransportHeaders headers,
            Stream stream)
        {


            //如果客户端指定了压缩的标志，压缩数据流，默认为不压缩
            object compress = headers["Compress"];
            if (compress != null && compress.ToString() == "True")
            {
                // 压缩数据流
                stream = myHelper.getCompressedStreamCopy(stream);
                // forwarding to the stack for further processing
                sinkStack.AsyncProcessResponse(msg, headers, stream);

            }
            else
            {
                _nextSink.AsyncProcessResponse(sinkStack, state, msg, headers, stream);
            }

        }

        public Stream GetResponseStream(IServerResponseChannelSinkStack sinkStack,
            object state,
            IMessage msg,
            ITransportHeaders headers)
        {
            return null;
        }

        public ServerProcessing ProcessMessage(IServerChannelSinkStack sinkStack,
            IMessage requestMsg,
            ITransportHeaders requestHeaders,
            Stream requestStream,
            out IMessage responseMsg,
            out ITransportHeaders responseHeaders,
            out Stream responseStream)
        {

            object compress = requestHeaders["Compress"];
            if (compress != null && compress.ToString() == "True")
            {
                // 解压请求的数据流,using NZipLib
                Stream localrequestStream = myHelper.getUncompressedStreamCopy(requestStream);

                // pushing onto stack and forwarding the call
                sinkStack.Push(this, null);

                Stream localresponseStream;
                ServerProcessing srvProc = _nextSink.ProcessMessage(sinkStack,
                    requestMsg,
                    requestHeaders,
                    localrequestStream,
                    out responseMsg,
                    out responseHeaders,
                    out localresponseStream);

                // compressing the response
                responseStream = myHelper.getCompressedStreamCopy(localresponseStream);

                // returning status information
                return srvProc;



            }
            else
            {
                //System.Net.IPAddress ip = requestHeaders[CommonTransportKeys.IPAddress] as System.Net.IPAddress;
                return _nextSink.ProcessMessage(sinkStack, requestMsg, requestHeaders, requestStream, out responseMsg, out responseHeaders, out responseStream);
            }
        }
    }
}
