using System;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.IO;

namespace CompressionSink
{

    public class CompressionServerSink : BaseChannelSinkWithProperties, IServerChannelSink //,IChannelSinkBase
    {

        private IServerChannelSink _nextSink;

        public CompressionServerSink(IServerChannelSink next)
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

        //�첽��ʽ	
        public void AsyncProcessResponse(IServerResponseChannelSinkStack sinkStack,
            object state,
            IMessage msg,
            ITransportHeaders headers,
            Stream stream)
        {
            //����ͻ���ָ����ѹ���ı�־��ѹ����������Ĭ��Ϊ��ѹ��
            object compress = headers["Compress"];
            if (compress != null && compress.ToString() == "True")
            {
                // ѹ��������
                stream = CompressionHelper.getCompressedStreamCopy(stream);
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
                // ��ѹ�����������,using NZipLib
                Stream localrequestStream = CompressionHelper.getUncompressedStreamCopy(requestStream);

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
                responseStream =
                    CompressionHelper.getCompressedStreamCopy(localresponseStream);

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
