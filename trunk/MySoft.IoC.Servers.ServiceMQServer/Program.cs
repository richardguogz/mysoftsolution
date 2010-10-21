using System;
using System.Collections.Generic;
using System.Text;
using MySoft.IoC;
using MySoft.Core;
using MySoft.Remoting;

namespace MySoft.IoC.Servers.ServiceMQServer
{
    class Program
    {
        static void Main(string[] args)
        {
            CastleFactoryConfiguration config = CastleFactoryConfiguration.GetConfig();

            LogEventHandler logger = (config.Debug ? new LogEventHandler(Console.WriteLine) : null);
            MemoryServiceMQ mq = new MemoryServiceMQ();
            mq.OnLog += logger;

            CastleServiceHelper cs = new CastleServiceHelper(config);
            cs.OnLog += logger;
            cs.PublishWellKnownServiceInstance(mq);

            Console.WriteLine("Service MQ Server started...");
            Console.WriteLine("Logger Status: " + (config.Debug ? "On" : "Off"));
            Console.WriteLine("Press any key to exit and stop server...");
            Console.ReadLine();
        }
    }
}
