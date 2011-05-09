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

            InitColor();
            Console.Title = "PlatformService Installer";

            if (!string.IsNullOrEmpty(optionalArgs))
            {
                switch (optionalArgs.ToLower())
                {
                    case "/?":
                        PrintHelp();
                        break;
                    case "/list":
                        {
                            string contains = null;
                            if (args.Length == 2) contains = args[1].Trim();
                            server.ListService(contains);
                        }
                        break;
                    case "/console":
                        {
                            if (server.StartConsole())
                            {
                                Console.ReadLine();
                            }
                        }
                        break;
                    case "/start":
                        {
                            string service = null;
                            if (args.Length == 2) service = args[1].Trim();
                            server.StartService(service);
                        }
                        break;
                    case "/stop":
                        {
                            string service = null;
                            if (args.Length == 2) service = args[1].Trim();
                            server.StopService(service);
                        }
                        break;
                    case "/restart":
                        {
                            string service = null;
                            if (args.Length == 2) service = args[1].Trim();
                            server.StopService(service);
                            server.StartService(service);
                        }
                        break;
                    case "/install":
                        server.InstallService();
                        break;
                    case "/uninstall":
                        server.UninstallService();
                        break;
                    default:
                        Console.WriteLine("输入的命令无效，输入/?显示帮助！");
                        break;
                }
            }

            InitColor();
        }

        static void PrintHelp()
        {
            Console.WriteLine("请输入命令启动相关操作:");
            Console.WriteLine("------------------------------------");
            Console.WriteLine(@"/? : 显示帮助");
            Console.WriteLine(@"/console : 启动控制台");
            Console.WriteLine(@"/start [服务名称]: 启动服务");
            Console.WriteLine(@"/stop [服务名称]: 停止服务");
            Console.WriteLine(@"/restart : 重启服务");
            Console.WriteLine(@"/install : 安装为window服务");
            Console.WriteLine(@"/uninstall : 卸载window服务");
            Console.WriteLine(@"/list [模糊名称]：列出服务");
            Console.WriteLine("------------------------------------");
        }

        static void InitColor()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
