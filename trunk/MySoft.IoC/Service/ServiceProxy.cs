using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using Castle.Windsor;

namespace MySoft.IoC
{
    internal sealed class ServiceProxy : ILogable
    {
        private int maxTryNum;
        private IServiceMQ mq;

        public int MaxTryNum
        {
            get
            {
                return maxTryNum;
            }
            set
            {
                maxTryNum = value;
            }
        }

        public ServiceProxy(IServiceMQ mq, int maxTryNum)
        {
            this.mq = mq;
            this.maxTryNum = maxTryNum;
        }

        public ResponseMessage CallMethod(string serviceName, RequestMessage msg)
        {
            long t1 = System.Environment.TickCount;
            ResponseMessage retMsg = null;

            //SerializationManager.Serialize(msg)
            if (OnLog != null) OnLog(string.Format("Run reqMsg for ({0},{1}) to service mq. -->{2}", serviceName, msg.SubServiceName, msg.Parameters.SerializedData));
            Guid tid = mq.SendRequestToQueue(serviceName, msg);
            for (int i = 0; i < maxTryNum; i++)
            {
                retMsg = mq.ReceieveResponseFromQueue(tid);
                if (retMsg == null)
                {
                    //重新发送请求
                    msg.MessageId = Guid.NewGuid();
                    msg.TransactionId = Guid.NewGuid();
                    tid = mq.SendRequestToQueue(serviceName, msg);

                    if (OnLog != null) OnLog(string.Format("Try {0} running ({1},{2}) -->{3}", (i + 1), serviceName, msg.SubServiceName, msg.Parameters.SerializedData));
                }
                else
                {
                    break;
                }
            }

            long t2 = System.Environment.TickCount - t1;
            if (retMsg != null)
            {
                if (retMsg.Data is Exception)
                {
                    throw retMsg.Data as Exception;
                }

                //SerializationManager.Serialize(retMsg)
                if (OnLog != null) OnLog(string.Format("Result -->{0}\r\n{1}", retMsg.Message, "Spent time: (" + t2.ToString() + ") ms"));
            }
            else
            {
                //delete the reqMsg message from mq if no service host can process it
                mq.ReceiveRequestFromQueue(msg.TransactionId);
            }

            return retMsg;
        }

        #region ILogable Members

        public event LogEventHandler OnLog;

        #endregion
    }
}
