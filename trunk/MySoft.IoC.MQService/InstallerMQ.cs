using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Configuration;
using System.Xml;


namespace MySoft.IoC.MQService
{
    [RunInstaller(true)]
    public partial class InstallerMQ : System.Configuration.Install.Installer
    {
        private ServiceInstaller _ServiceInstaller;
        private ServiceProcessInstaller _ProcessInstaller;

        private String _UserName;
        private String _Password;
        private String _ServiceName;
        private String _Description;
        private String _Command;

        private static Boolean _Initialized = false;

        public InstallerMQ()
        {
            InitializeComponent();
            BeforeInstall += new InstallEventHandler(BeforeInstallEventHandler);
            BeforeUninstall += new InstallEventHandler(BeforeUninstallEventHandler);

            this._UserName = "";
            this._Password = "";
            this._ServiceName = "";
            this._Description = "MySoft组件MQ服务中心";
            this._Command = "";
        }

        private void Initialize()
        {
            try
            {
                String _ConfigPath = String.Concat(Environment.CurrentDirectory, "\\InstallConfig.xml");
                if (System.IO.File.Exists(_ConfigPath))
                {
                    XmlDocument _Xdc = new XmlDocument();
                    _Xdc.Load(_ConfigPath);

                    XmlNode _Root = _Xdc.DocumentElement;
                    if (_Root.Name == "Config")
                    {
                        if (_Root.Attributes.Count > 0)
                        {
                            _ServiceName = _Root.Attributes["ServiceName"].Value;
                            _UserName = _Root.Attributes["UserName"].Value;
                            _Password = _Root.Attributes["Password"].Value;
                            _Description = _Root.Attributes["Description"].Value;
                            _Command = _Root.Attributes["Command"].Value;
                        }
                    }
                }
                else
                {
                    Console.Write("通用配置文件不存在（Config.xml）,按任意键继续..");
                    Console.ReadLine();
                }

                //如果服务名为空，则在安装时输入服务名
                if (String.IsNullOrEmpty(_ServiceName))
                {
                    Console.WriteLine("\n\n---------------------------------------");
                    Console.WriteLine("请输入服务名称：\n");
                    _ServiceName = Console.ReadLine();
                }

                if (String.IsNullOrEmpty(_ServiceName))
                {
                    _ServiceName = "MySoft.IoC.MQService";
                }

                _ProcessInstaller = new ServiceProcessInstaller();
                _ServiceInstaller = new ServiceInstaller();

                //指定服务帐号类型
                if (String.IsNullOrEmpty(_UserName) ||
                    String.IsNullOrEmpty(_Password))
                {
                    _ProcessInstaller.Account = ServiceAccount.LocalSystem;
                }
                else
                {
                    _ProcessInstaller.Account = ServiceAccount.User;
                    _ProcessInstaller.Username = _UserName;
                    _ProcessInstaller.Password = _Password;
                }

                //服务启动类型
                _ServiceInstaller.StartType = ServiceStartMode.Automatic;
                _ServiceInstaller.ServiceName = _ServiceName;
                _ServiceInstaller.DisplayName = _ServiceName.Replace('.', ' ') + " (MQ服务中心)";

                Installers.Add(_ServiceInstaller);
                Installers.Add(_ProcessInstaller);
                _Initialized = true;

                Console.WriteLine("\n配置完成，按任意键开始安装...\n");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine(String.Format("\n安装服务名：{0}\n描述：{1}\n登录用户名：{2}\n密码：{3}\n安装后执行命令：{4}\n",
                    _ServiceName,
                    _Description,
                    _UserName,
                    _Password,
                    _Command));

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("初始化出错：\n" + ex.ToString());
                Console.ReadLine();
            }
        }

        public override void Install(IDictionary stateServer)
        {
            Microsoft.Win32.RegistryKey system,
            currentControlSet,
            services,
            service,
            config;
            try
            {
                base.Install(stateServer);

                system = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("System");

                currentControlSet = system.OpenSubKey("CurrentControlSet");

                services = currentControlSet.OpenSubKey("Services");

                service = services.OpenSubKey(this._ServiceInstaller.ServiceName, true);

                if (String.IsNullOrEmpty(_Description))
                {
                    service.SetValue("Description", this._ServiceInstaller.ServiceName + " Description");
                }
                else
                {
                    service.SetValue("Description", _Description);
                }

                config = service.CreateSubKey("Parameters");
                config.SetValue("Arguments", _Command);
                Console.WriteLine(service.GetValue("ImagePath"));
                string path = service.GetValue("ImagePath") + " " +
                this._ServiceInstaller.ServiceName;

                service.SetValue("ImagePath", path);
            }
            catch (Exception e)
            {
                Console.WriteLine("安装时出错：\n" + e.ToString());
            }
        }

        public override void Uninstall(IDictionary stateServer)
        {
            Microsoft.Win32.RegistryKey system,
            currentControlSet,
            services,
            service;
            try
            {
                system = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("System");
                currentControlSet = system.OpenSubKey("CurrentControlSet");
                services = currentControlSet.OpenSubKey("Services");
                service = services.OpenSubKey(this._ServiceInstaller.ServiceName, true);

                service.DeleteSubKeyTree("Parameters");
            }
            catch (Exception e)
            {
                Console.WriteLine("卸载时出错:\n" + e.ToString());
            }
            finally
            {
                base.Uninstall(stateServer);
            }
        }

        private void BeforeInstallEventHandler(object sender, InstallEventArgs e)
        {
            Console.WriteLine("\n开始安装");

            if (!_Initialized)
            {
                Initialize();
            }
        }

        private void BeforeUninstallEventHandler(object sender, InstallEventArgs e)
        {
            Console.WriteLine("\n开始卸载");

            if (!_Initialized)
            {
                Initialize();
            }
        }
    }
}
