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
            System.Console.BackgroundColor = ConsoleColor.DarkBlue;
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine("Service ready started...");

            CastleServiceConfiguration config = CastleServiceConfiguration.GetConfig();
            CastleService server = new CastleService(config);
            server.OnLog += new LogEventHandler(Program_OnLog);
            server.OnError += new ErrorLogEventHandler(Program_OnError);
            server.Start();

            System.Console.WriteLine("Server host -> {0}", server.ServerUrl);
            System.Console.WriteLine("Logger status: On  -> Show log time: {0} seconds", config.LogTime);
            System.Console.WriteLine("Press any key to exit and stop service...");
            System.Console.ReadLine();
        }

        static void Program_OnLog(string log, LogType type)
        {
            lock (syncobj)
            {
                //string message = "[" + DateTime.Now.ToString() + "] " + log;
                //if (type == LogType.Error)
                //    System.Console.ForegroundColor = ConsoleColor.Red;
                //else if (type == LogType.Warning)
                //    System.Console.ForegroundColor = ConsoleColor.Yellow;
                //else
                //    System.Console.ForegroundColor = ConsoleColor.White;
                //System.Console.WriteLine(message);

                if (type == LogType.Warning)
                {
                    string message = string.Format("({0}) {1}", type, log);
                    SimpleLog.Instance.WriteLogWithSendMail(message, "maoyong@fund123.cn");
                }
            }
        }

        static void Program_OnError(Exception exception)
        {
            lock (syncobj)
            {
                //string message = "[" + DateTime.Now.ToString() + "] " + exception.Message;
                //if (exception.InnerException != null)
                //{
                //    message += "\r\n´íÎóÐÅÏ¢ => " + exception.InnerException.Message;
                //}
                //System.Console.ForegroundColor = ConsoleColor.Red;
                //System.Console.WriteLine(message);

                if (exception is IoCException)
                {
                    var ex = exception as IoCException;
                    if (string.IsNullOrEmpty(ex.ExceptionHeader))
                    {
                        ex.ExceptionHeader = string.Format("Error: {0}. Comes from {1}({2}).", ex.Message, DnsHelper.GetHostName(), DnsHelper.GetIPAddress());
                    }
                    exception = new Exception(ex.ExceptionHeader, exception);
                }
                SimpleLog.Instance.WriteLogWithSendMail(exception, "maoyong@fund123.cn");
            }
        }
    }
}
