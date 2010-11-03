using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

using Castle.Windsor;
using MySoft.Core;

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

        private ResponseMessage Run(string serviceName, RequestMessage msg)
        {
            ResponseMessage retMsg = null;

            //SerializationManager.Serialize(msg)
            if (OnLog != null) OnLog(string.Format("Run reqMsg for {0} to service mq. -->(name:{1} parameters:{2})", serviceName, msg.SubServiceName, msg.Parameters.SerializedData));
            Guid tid = mq.SendRequestToQueue(serviceName, msg);
            for (int i = 0; i < maxTryNum; i++)
            {
                retMsg = mq.ReceieveResponseFromQueue(tid);
                if (retMsg == null)
                {
                    if (OnLog != null) OnLog(string.Format("Try {0} Run (name:{1} parameters:{2})...", (i + 1), msg.SubServiceName, msg.Parameters.SerializedData));
                    Thread.Sleep(100);
                }
                else
                {
                    break;
                }
            }

            if (retMsg != null)
            {
                //SerializationManager.Serialize(retMsg)
                if (OnLog != null) OnLog(string.Format("Result: {0}", retMsg.Message));
            }
            else
            {
                //delete the reqMsg message from mq if no service host can process it
                mq.ReceiveRequestFromQueue(msg.TransactionId);
            }

            return retMsg;
        }

        public ResponseMessage CallMethod(string serviceName, RequestMessage msg)
        {
            //SerializationManager.Serialize(msg)
            if (OnLog != null) OnLog(string.Format("Receive reqMsg for service:{0}. -->(name:{1} parameters:{2})", serviceName, msg.SubServiceName, msg.Parameters.SerializedData));

            long t1 = System.Environment.TickCount;
            ResponseMessage retMsg = Run(serviceName, msg);
            long t2 = System.Environment.TickCount - t1;
            if (OnLog != null) OnLog("Spent time: (" + t2.ToString() + ") ms");

            return retMsg;
        }

        #region ILogable Members

        public event LogEventHandler OnLog;

        #endregion
    }
}
