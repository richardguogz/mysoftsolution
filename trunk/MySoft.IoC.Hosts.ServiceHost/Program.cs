using System;
using System.Collections.Generic;
using System.Text;
using MySoft.IoC.Service;
using MySoft.Core;

namespace MySoft.IoC.Hosts.ServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            //ServiceFactoryConfiguration config = ServiceFactoryConfiguration.GetConfig();
            //LogHandler logger = (config.Debug ? new LogHandler(Console.WriteLine) : null);
            //ServiceFactory.Create().OnLog += logger;

            MySoft.Core.Service.ServiceFactory.Create(); 

            //Console.WriteLine("Service host started...");
            //Console.WriteLine("Logger Status: " + (config.Debug ? "On" : "Off"));
            //Console.WriteLine("Press any key to exit and stop host...");
            //Console.ReadLine();
        }
    }
}
