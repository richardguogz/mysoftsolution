using System;
using System.Collections.Generic;
using System.Text;
using MySoft.IoC.Service;
using MySoft.Core.Service;
using MySoft.Core;

namespace MySoft.IoC.Hosts.ServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            CastleFactoryConfiguration config = CastleFactoryConfiguration.GetConfig();
            LogEventHandler logger = (config.Debug ? new LogEventHandler(Console.WriteLine) : null);
            CastleFactory.Create().OnLog += logger;

            ServiceFactory.Create();

            Console.WriteLine("Service host started...");
            Console.WriteLine("Logger Status: " + (config.Debug ? "On" : "Off"));
            Console.WriteLine("Press any key to exit and stop host...");
            Console.ReadLine();
        }
    }
}
