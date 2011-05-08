using System.ServiceProcess;
using System;
using MySoft.Logger;
using MySoft.Installer.Configuration;

namespace MySoft.PlatformService
{
    static class Program
    {
        static void Main(string[] args)
        {
            InstallerServer server = new InstallerServer();
            string optionalArgs = string.Empty;

            // 运行服务
            if (args.Length == 0)
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new BusinessService(server.Service) };
                ServiceBase.Run(ServicesToRun);
                return;
            }
            else
            {
                optionalArgs = args[0];
            }

            Console.Title = "PlatformService Installer";
            bool isExit = false;
            while (!isExit)
            {
                if (string.IsNullOrEmpty(optionalArgs))
                {
                    optionalArgs = Console.ReadLine();
                }

                //如果未接收到参数，则继续
                if (string.IsNullOrEmpty(optionalArgs))
                {
                    continue;
                }

                switch (optionalArgs.ToLower())
                {
                    case "/?":
                        PrintHelp();
                        break;
                    case "/exit":
                        isExit = true;
                        break;
                    case "/console":
                        server.StartConsole();
                        Console.ReadLine();
                        server.StopConsole();
                        break;
                    case "/start":
                        server.StartService();
                        break;
                    case "/stop":
                        server.StopService();
                        break;
                    case "/install":
                        server.InstallService();
                        break;
                    case "/uninstall":
                        server.UninstallService();
                        break;
                }

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
                optionalArgs = string.Empty;
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("请输入命令启动相关操作:");
            Console.WriteLine("------------------------------------");
            Console.WriteLine(@"/console : 启动控制台");
            Console.WriteLine(@"/exit : 退出控制台");
            Console.WriteLine(@"/start : 启动服务");
            Console.WriteLine(@"/stop : 停止服务");
            Console.WriteLine(@"/install : 安装为window服务");
            Console.WriteLine(@"/uninstall : 卸载window服务");
            Console.WriteLine(@"/? : 显示帮助");
            Console.WriteLine("------------------------------------");
        }
    }
}
