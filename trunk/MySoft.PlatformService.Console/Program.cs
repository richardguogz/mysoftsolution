using System;
using System.Collections.Generic;
using System.Text;
using MySoft.IoC;
using System.Collections;
using MySoft.Remoting;
using MySoft.Net.Server;
using MySoft.IoC.Configuration;
using MySoft.Logger;

namespace MySoft.PlatformService.Console
{
    class Program
    {
        private static readonly object syncobj = new object();
        static void Main(string[] args)
        {
            System.Console.WriteLine("Service ready started...");

            CastleServiceConfiguration config = CastleServiceConfiguration.GetConfig();
            CastleService server = new CastleService(config);
            server.OnLog += new LogEventHandler(Program_OnLog);
            server.OnError += new ErrorLogEventHandler(Program_OnError);
            server.Start();

            System.Console.WriteLine("Server host -> {0}", server.ServerUrl);
            System.Console.WriteLine("Logger status: On  -> Show log time£º{0} ms", config.LogTimeout);
            System.Console.WriteLine("Press any key to exit and stop service...");
            System.Console.ReadLine();
        }

        static void Program_OnLog(string log, LogType type)
        {
            lock (syncobj)
            {
                string message = "[" + DateTime.Now.ToString() + "] " + log;
                if (type == LogType.Error)
                    System.Console.ForegroundColor = ConsoleColor.Red;
                else if (type == LogType.Warning)
                    System.Console.ForegroundColor = ConsoleColor.Blue;
                else
                    System.Console.ForegroundColor = ConsoleColor.White;
                System.Console.WriteLine(message);
            }
        }

        static void Program_OnError(Exception exception)
        {
            lock (syncobj)
            {
                string message = "[" + DateTime.Now.ToString() + "] " + exception.Message;
                if (exception.InnerException != null)
                {
                    message += "\r\n" + exception.InnerException.Message;
                }
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine(message);
            }
        }
    }
}
