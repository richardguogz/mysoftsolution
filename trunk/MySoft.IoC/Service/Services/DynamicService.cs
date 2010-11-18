using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using MySoft.Core;
using MySoft.Remoting;

namespace MySoft.IoC.Services
{
    /// <summary>
    /// The dynamic service.
    /// </summary>
    public class DynamicService : BaseService
    {
        /// <summary>
        /// The default  message expire minutes.
        /// </summary>
        public static int DefaultExpireMinutes = 30;
        private IServiceContainer container;
        private Type serviceInterfaceType;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicService"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="serviceInterfaceType">Type of the service interface.</param>
        public DynamicService(IServiceContainer container, Type serviceInterfaceType)
            : base(serviceInterfaceType.FullName, container.MQ)
        {
            this.container = container;
            this.OnLog += container.WriteLog;
            this.OnError += container.WriteError;
            this.serviceInterfaceType = serviceInterfaceType;
        }


        /// <summary>
        /// Runs the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns>The msg.</returns>
        protected override ResponseMessage Run(RequestMessage msg)
        {
            if (container == null || msg == null)
            {
                return null;
            }

            object service = null;
            try
            {
                service = container[serviceInterfaceType];
            }
            catch { }
            if (service == null) return null;

            MethodInfo mi = null;
            foreach (MethodInfo item in serviceInterfaceType.GetMethods())
            {
                if (msg.SubServiceName == item.ToString())
                {
                    mi = item;
                    break;
                }
            }

            if (mi == null)
            {
                foreach (Type inheritedInterface in serviceInterfaceType.GetInterfaces())
                {
                    foreach (MethodInfo item in inheritedInterface.GetMethods())
                    {
                        if (item.ToString() == msg.SubServiceName)
                        {
                            mi = item;
                            break;
                        }
                    }
                }
            }

            if (mi == null)
            {
                throw new Exception("Method not found " + msg.SubServiceName + "!");
            }

            ParameterInfo[] pis = mi.GetParameters();
            object[] parms = new object[pis.Length];

            for (int i = 0; i < pis.Length; i++)
            {
                Type type = pis[i].ParameterType;

                object val = SerializationManager.DeserializeJson(type, msg.Parameters[pis[i].Name]);

                parms[i] = val;
            }

            ResponseMessage resMsg = new ResponseMessage();
            resMsg.Request = msg;
            resMsg.Expiration = DateTime.Now.AddMinutes(DefaultExpireMinutes);
            resMsg.MessageId = Guid.NewGuid();
            resMsg.ServiceName = serviceInterfaceType.FullName;
            resMsg.SubServiceName = mi.Name;
            resMsg.Timestamp = DateTime.Now;
            resMsg.TransactionId = msg.TransactionId;

            //返回拦截服务
            service = AspectManager.GetService(service);

            object returnValue = DynamicCalls.GetMethodInvoker(mi).Invoke(service, parms);
            //returnValue = mi.Invoke(service, parms);

            if (returnValue != null)
            {
                Type returnType = mi.ReturnType;
                switch (container.Transfer)
                {
                    case TransferType.Binary:
                        byte[] buffer = SerializationManager.SerializeBin(returnValue);

                        //将数据进行压缩
                        if (container.Compress != CompressType.None)
                        {
                            switch (container.Compress)
                            {
                                case CompressType.Zip:
                                    resMsg.Data = CompressionManager.Compress7Zip(buffer);
                                    break;
                                case CompressType.GZip:
                                    resMsg.Data = CompressionManager.CompressGZip(buffer);
                                    break;
                            }
                        }
                        else
                        {
                            resMsg.Data = buffer;
                        }
                        break;
                    case TransferType.Json:
                        string jsonString = SerializationManager.SerializeJson(returnValue);

                        //将数据进行压缩
                        if (container.Compress != CompressType.None)
                        {
                            switch (container.Compress)
                            {
                                case CompressType.Zip:
                                    resMsg.Data = CompressionManager.Compress7Zip(jsonString);
                                    break;
                                case CompressType.GZip:
                                    resMsg.Data = CompressionManager.CompressGZip(jsonString);
                                    break;
                            }
                        }
                        else
                        {
                            resMsg.Data = jsonString;
                        }
                        break;
                    case TransferType.Xml:
                        string xmlString = SerializationManager.SerializeXml(returnValue);

                        //将数据进行压缩
                        if (container.Compress != CompressType.None)
                        {
                            switch (container.Compress)
                            {
                                case CompressType.Zip:
                                    resMsg.Data = CompressionManager.Compress7Zip(xmlString);
                                    break;
                                case CompressType.GZip:
                                    resMsg.Data = CompressionManager.CompressGZip(xmlString);
                                    break;
                            }
                        }
                        else
                        {
                            resMsg.Data = xmlString;
                        }
                        break;
                }
            }

            return resMsg;
        }
    }
}
