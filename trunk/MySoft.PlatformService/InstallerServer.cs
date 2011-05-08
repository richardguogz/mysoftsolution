using System.Collections;
using System.Configuration.Install;
using System.ServiceProcess;
using MySoft.Installer.Configuration;
using System;
using System.IO;
using System.Threading;

namespace MySoft.PlatformService
{
    /// <summary>
    /// 安装服务类
    /// </summary>
    public class InstallerServer
    {
        private InstallerConfiguration config;
        private IServiceRun service;
        private TransactedInstaller ti;

        public InstallerServer()
        {
            #region 动态加载服务

            try
            {
                //读取配置节
                this.config = InstallerConfiguration.GetConfig();

                var type = Type.GetType(config.ServiceType);
                if (type == null) throw new Exception("加载服务失败！");
                this.service = (IServiceRun)Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
                else
                {
                    Console.WriteLine(ex.Message);
                }
            }

            #endregion

            this.ti = new TransactedInstaller();
            this.ti.BeforeInstall += new InstallEventHandler((obj, state) => { Console.WriteLine("服务正在安装......"); });
            this.ti.AfterInstall += new InstallEventHandler((obj, state) => { Console.WriteLine("服务安装完成！"); });
            this.ti.BeforeUninstall += new InstallEventHandler((obj, state) => { Console.WriteLine("服务正在卸载......"); });
            this.ti.AfterUninstall += new InstallEventHandler((obj, state) => { Console.WriteLine("服务卸载完成！"); });

            InitInstaller();
        }

        private void InitInstaller()
        {
            BusinessInstaller installer = new BusinessInstaller(config);
            ti.Installers.Add(installer);
            string logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "installer.log");
            string path = string.Format("/assemblypath={0}", System.Reflection.Assembly.GetExecutingAssembly().Location);
            string[] cmd = { path };
            InstallContext context = new InstallContext(logFile, cmd);
            ti.Context = context;
        }

        /// <summary>
        /// 当前服务
        /// </summary>
        public IServiceRun Service
        {
            get { return service; }
        }

        private void PrintInstallMessage()
        {
            Console.WriteLine("服务尚未安装,请安装window服务,输入'/?'查看帮助！");
        }

        /// <summary>
        /// 从控制台运行
        /// </summary>
        public void StartConsole()
        {
            if (config == null) return;
            ServiceController controller = InstallerUtils.LookupService(config.ServiceName);
            if (controller != null)
            {
                if (controller.Status == ServiceControllerStatus.Running)
                {
                    Console.WriteLine("服务已经启动,不能控制台启动,请先停止服务后再执行该命令！");
                    return;
                }
            }

            service.StartMode = StartMode.Console;
            service.Start();

            Console.WriteLine("控制台已经启动......");
        }

        /// <summary>
        /// 停止控制台运行
        /// </summary>
        public void StopConsole()
        {
            if (config == null) return;
            service.StartMode = StartMode.Console;
            service.Stop();

            Console.WriteLine("控制台已经退出......");
        }

        /// <summary>
        /// 运行服务服务
        /// </summary>
        public void StartService()
        {
            if (config == null) return;
            ServiceController controller = InstallerUtils.LookupService(config.ServiceName);
            if (controller != null)
            {
                if (controller.Status == ServiceControllerStatus.Stopped)
                {
                    Console.WriteLine("正在启动服务......");
                    try
                    {
                        controller.Start();

                        Thread.Sleep(1000);
                        controller = InstallerUtils.LookupService(config.ServiceName);
                        if (controller.Status == ServiceControllerStatus.Running)
                            Console.WriteLine("启动服务成功！");
                        else
                            Console.WriteLine("启动服务失败！");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("服务已经启动！");
                }
            }
            else
            {
                PrintInstallMessage();
            }
        }

        /// <summary>
        /// 停止服务服务
        /// </summary>
        public void StopService()
        {
            if (config == null) return;
            ServiceController controller = InstallerUtils.LookupService(config.ServiceName);
            if (controller != null)
            {
                if (controller.Status == ServiceControllerStatus.Running)
                {
                    Console.WriteLine("正在停止服务.....");
                    try
                    {
                        controller.Stop();

                        Thread.Sleep(1000);
                        controller = InstallerUtils.LookupService(config.ServiceName);
                        if (controller.Status == ServiceControllerStatus.Stopped)
                            Console.WriteLine("停止服务成功！");
                        else
                            Console.WriteLine("停止服务失败！");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("服务已经停止！");
                }
            }
            else
            {
                PrintInstallMessage();
            }
        }

        /// <summary>
        /// 安装服务器为window服务
        /// </summary>
        public void InstallService()
        {
            if (config == null) return;
            ServiceController controller = InstallerUtils.LookupService(config.ServiceName);
            if (controller == null)
            {
                ti.Install(new Hashtable());
            }
            else
            {
                Console.WriteLine("服务已经存在,请先卸载后再执行此命令！");
            }
        }

        /// <summary>
        /// 卸载window服务
        /// </summary>
        public void UninstallService()
        {
            if (config == null) return;
            ServiceController controller = InstallerUtils.LookupService(config.ServiceName);
            if (controller != null)
            {
                //卸载之前先停止服务
                if (controller.Status == ServiceControllerStatus.Running)
                {
                    StopService();
                }

                ti.Uninstall(null);
            }
            else
            {
                PrintInstallMessage();
            }
        }
    }
}