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
                ServicesToRun = new ServiceBase[] { new BusinessService(server.WindowsService) };
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
                            string status = null;
                            if (args.Length >= 2) contains = args[1].Trim();
                            if (args.Length >= 3) status = args[2].Trim();
                            server.ListService(contains, status);
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
            Console.WriteLine("请输入命令启动相关操作,[]表示可选参数:");
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine(@"/? : 显示帮助信息");
            Console.WriteLine(@"/list [-服务名称] [-status]：模糊查询服务");
            Console.WriteLine("(status取值为running、stopped、paused)");
            Console.WriteLine(@"/start [-服务名称]: 启动指定服务");
            Console.WriteLine(@"/stop [-服务名称]: 停止指定服务");
            Console.WriteLine(@"/restart [-服务名称]: 重启指定服务");
            Console.WriteLine(@"/console : 启动控制台 (仅当前配置有效)");
            Console.WriteLine(@"/install : 安装为window服务 (仅当前配置有效)");
            Console.WriteLine(@"/uninstall : 卸载window服务 (仅当前配置有效)");
            Console.WriteLine("----------------------------------------------");
        }

        static void InitColor()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
