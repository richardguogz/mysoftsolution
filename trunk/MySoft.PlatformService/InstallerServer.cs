using System.Collections;
using System.Configuration.Install;
using System.ServiceProcess;
using MySoft.Installer.Configuration;
using System;
using System.IO;
using System.Threading;
using MySoft.Installer;
using System.Reflection;

namespace MySoft.PlatformService
{
    /// <summary>
    /// 安装服务类
    /// </summary>
    public class InstallerServer
    {
        private InstallerConfiguration config;
        private IServiceRun service;

        public InstallerServer()
        {
            try
            {
                //读取配置节
                this.config = InstallerConfiguration.GetConfig();
                if (this.config == null)
                {
                    Console.WriteLine("加载服务安装配置节失败！");
                }
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
        }

        /// <summary>
        /// 当前服务
        /// </summary>
        public IServiceRun Service
        {
            get
            {
                if (config == null) return null;

                if (service == null)
                {
                    #region 动态加载服务

                    try
                    {
                        var type = Type.GetType(config.ServiceType);
                        if (type == null) throw new Exception(string.Format("加载服务{0}失败！", config.ServiceType));
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
                }

                return service;
            }
        }

        /// <summary>
        /// 列出服务
        /// </summary>
        public void ListService(string contains)
        {
            Console.WriteLine("正在读取服务信息......");
            if (string.IsNullOrEmpty(contains))
            {
                contains = "(Paltform Service)";
            }

            var list = InstallerUtils.GetServiceList(contains);
            if (list.Count == 0)
            {
                Console.WriteLine("未能读取到相关的服务信息......");
            }
            else
            {
                foreach (var controller in list)
                {
                    Console.WriteLine("------------------------------------------------------------------------");
                    Console.WriteLine("服务名：{0} ({1})", controller.ServiceName, controller.Status);
                    Console.WriteLine("显示名：{0}", controller.DisplayName);
                    Console.WriteLine("描  述：{0}", controller.Description);
                    Console.WriteLine("路  径：{0}", controller.ServicePath);
                }
                Console.WriteLine("------------------------------------------------------------------------");
            }
        }

        /// <summary>
        /// 从控制台运行
        /// </summary>
        public bool StartConsole()
        {
            if (config == null)
            {
                Console.WriteLine("无效的服务配置项！");
                return false;
            }

            ServiceController controller = InstallerUtils.LookupService(config.ServiceName);
            if (controller != null)
            {
                if (controller.Status == ServiceControllerStatus.Running)
                {
                    Console.WriteLine("服务已经启动,不能从控制台启动,请先停止服务后再执行该命令！");
                    return false;
                }
            }

            if (Service != null)
            {
                Service.StartMode = StartMode.Console;
                Service.Start();

                Console.WriteLine("控制台已经启动......");
                return true;
            }
            else
            {
                Console.WriteLine("无效的服务启动项！");
                return false;
            }
        }

        /// <summary>
        /// 运行服务服务
        /// </summary>
        public void StartService(string serviceName)
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                if (config == null)
                {
                    Console.WriteLine("无效的服务配置项！");
                    return;
                }
                serviceName = config.ServiceName;
            }

            ServiceController controller = InstallerUtils.LookupService(serviceName);
            if (controller != null)
            {
                if (controller.Status == ServiceControllerStatus.Stopped)
                {
                    Console.WriteLine("正在启动服务{0}......", serviceName);
                    try
                    {
                        controller.Start();
                        controller.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMinutes(1));
                        controller.Refresh();

                        if (controller.Status == ServiceControllerStatus.Running)
                            Console.WriteLine("启动服务{0}成功！", serviceName);
                        else
                            Console.WriteLine("启动服务{0}失败！", serviceName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("服务{0}已经启动！", serviceName);
                }
            }
            else
            {
                Console.WriteLine("不存在服务{0}！", serviceName);
            }
        }

        /// <summary>
        /// 停止服务服务
        /// </summary>
        public void StopService(string serviceName)
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                if (config == null)
                {
                    Console.WriteLine("无效的服务配置项！");
                    return;
                }
                serviceName = config.ServiceName;
            }

            ServiceController controller = InstallerUtils.LookupService(serviceName);
            if (controller != null)
            {
                if (controller.Status == ServiceControllerStatus.Running)
                {
                    Console.WriteLine("正在停止服务{0}.....", serviceName);
                    try
                    {
                        controller.Stop();
                        controller.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMinutes(1));
                        controller.Refresh();

                        if (controller.Status == ServiceControllerStatus.Stopped)
                            Console.WriteLine("停止服务{0}成功！", serviceName);
                        else
                            Console.WriteLine("停止服务{0}失败！", serviceName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("服务{0}已经停止！", serviceName);
                }
            }
            else
            {
                Console.WriteLine("不存在服务{0}！", serviceName);
            }
        }

        /// <summary>
        /// 安装服务器为window服务
        /// </summary>
        public void InstallService()
        {
            if (config == null)
            {
                Console.WriteLine("无效的服务配置项！");
                return;
            }

            ServiceController controller = InstallerUtils.LookupService(config.ServiceName);
            if (controller == null)
            {
                try
                {
                    TransactedInstaller installer = GetTransactedInstaller();
                    installer.Install(new Hashtable());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("服务{0}已经存在,请先卸载后再执行此命令！", config.ServiceName);
            }
        }

        /// <summary>
        /// 卸载window服务
        /// </summary>
        public void UninstallService()
        {
            if (config == null)
            {
                Console.WriteLine("无效的服务配置项！");
                return;
            }

            ServiceController controller = InstallerUtils.LookupService(config.ServiceName);
            if (controller != null)
            {
                try
                {
                    TransactedInstaller installer = GetTransactedInstaller();
                    installer.Uninstall(null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("服务{0}尚未安装,输入'/?'查看帮助！", config.ServiceName);
            }
        }

        /// <summary>
        /// 获取当前的安装信息
        /// </summary>
        /// <returns></returns>
        private TransactedInstaller GetTransactedInstaller()
        {
            TransactedInstaller installer = new TransactedInstaller();
            installer.BeforeInstall += new InstallEventHandler((obj, state) => { Console.WriteLine("服务正在安装......"); });
            installer.AfterInstall += new InstallEventHandler((obj, state) => { Console.WriteLine("服务安装完成！"); });
            installer.BeforeUninstall += new InstallEventHandler((obj, state) => { Console.WriteLine("服务正在卸载......"); });
            installer.AfterUninstall += new InstallEventHandler((obj, state) => { Console.WriteLine("服务卸载完成！"); });

            BusinessInstaller businessInstaller = new BusinessInstaller(config);
            installer.Installers.Add(businessInstaller);
            string logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "installer.log");
            string path = string.Format("/assemblypath={0}", System.Reflection.Assembly.GetExecutingAssembly().Location);
            string[] cmd = { path };
            InstallContext context = new InstallContext(logFile, cmd);
            installer.Context = context;

            return installer;
        }
    }
}