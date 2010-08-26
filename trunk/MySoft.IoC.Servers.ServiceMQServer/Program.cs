using System;
using System.Collections.Generic;
using System.Text;
using MySoft.IoC.Service;
using MySoft.Core;
using MySoft.Core.Remoting;

namespace MySoft.IoC.Servers.ServiceMQServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceFactoryConfiguration config = ServiceFactoryConfiguration.GetConfig();

            LogHandler logger = (config.Debug ? new LogHandler(Console.WriteLine) : null);
            MemoryServiceMQ mq = new MemoryServiceMQ();
            mq.OnLog += logger;

            RemotingServiceHelper rh = new RemotingServiceHelper(config.Protocol, config.Server, config.Port);
            rh.OnLog += logger;
            rh.PublishWellKnownServiceInstance(config.ServiceMQName, typeof(IServiceMQ), mq, System.Runtime.Remoting.WellKnownObjectMode.Singleton);

            Console.WriteLine("Service MQ Server started...");
            Console.WriteLine("Logger Status: " + (config.Debug ? "On" : "Off"));
            Console.WriteLine("Press any key to exit and stop server...");
            Console.ReadLine();
        }
    }
}
