using System;
using System.Collections.Generic;
using System.Text;
using MySoft.IoC;
using System.Collections;
using MySoft.Remoting;

namespace MySoft.IoC.Hosts.ServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            //CastleFactoryConfiguration config = CastleFactoryConfiguration.GetConfig();
            CastleFactory.Create().OnLog += new LogEventHandler(Program_OnLog);
            CastleFactory.Create().OnError += new ErrorLogEventHandler(Program_OnError);

            Console.WriteLine("Service host started...");
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
