using System;
using System.Collections.Generic;
using System.Text;
using MySoft.IoC;
using MySoft.Remoting;

namespace MySoft.IoC.Servers.ServiceMQServer
{
    class Program
    {
        static void Main(string[] args)
        {
            CastleFactoryConfiguration config = CastleFactoryConfiguration.GetConfig();

            LogHandler logger = Console.WriteLine;
            MemoryServiceMQ mq = new MemoryServiceMQ();
            mq.OnLog += new LogHandler(mq_OnLog);
            //mq.OnError += new ErrorLogHandler(mq_OnError);

            CastleServiceHelper cs = new CastleServiceHelper(config);
            cs.OnLog += logger;
            cs.PublishWellKnownServiceInstance(mq);

            Console.WriteLine("Service MQ Server started...");
            Console.WriteLine("Logger Status: On");
            Console.WriteLine("Press any key to exit and stop server...");
            Console.ReadLine();
        }

        static void mq_OnLog(string log)
        {
            string message = "[" + DateTime.Now.ToString() + "] " + log;
            Console.WriteLine(message);
        }

        static void mq_OnError(Exception exception)
        {
            string message = "[" + DateTime.Now.ToString() + "] " + exception.Message;
            Console.WriteLine(message);
        }
    }
}
