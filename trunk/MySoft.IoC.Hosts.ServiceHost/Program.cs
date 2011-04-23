using System;
using System.Collections.Generic;
using System.Text;
using MySoft.IoC;
using System.Collections;
using MySoft.Remoting;
using MySoft.Net.Server;

namespace MySoft.IoC.Hosts.ServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            //CastleFactoryConfiguration config = CastleFactoryConfiguration.GetConfig();
            //CastleFactory.Create().OnLog += new LogEventHandler(Program_OnLog);
            //CastleFactory.Create().OnError += new ErrorLogEventHandler(Program_OnError);

            CastleFactoryConfiguration config = CastleFactoryConfiguration.GetConfig();
            CastleService server = new CastleService(config);
            server.OnLog += new LogEventHandler(Program_OnLog);
            server.OnError += new ErrorLogEventHandler(Program_OnError);
            server.Start();

            Console.WriteLine("Service host started...");
            Console.WriteLine("Server host -> tcp://{0}", server.Host.ToString());
            Console.WriteLine("Logger Status: On");
            Console.WriteLine("Press any key to exit and stop host...");
            Console.ReadLine();
        }

        static void Program_OnLog(string log)
        {
            string message = "[" + DateTime.Now.ToString() + "] " + log;
            Console.WriteLine(message);
        }

        static void Program_OnError(Exception exception)
        {
            string message = "[" + DateTime.Now.ToString() + "] " + exception.Message;
            Console.WriteLine(message);
        }
    }
}
