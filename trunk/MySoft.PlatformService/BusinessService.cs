using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using MySoft.IoC;
using MySoft.Logger;
using System.Configuration;
using System.Threading;

namespace MySoft.PlatformService
{
    public partial class BusinessService : ServiceBase
    {
        private readonly IServiceRun service;
        public BusinessService(IServiceRun service)
        {
            if (service != null)
            {
                this.service = service;
                this.service.StartMode = StartMode.Service;
            }

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (service == null)
            {
                throw new Exception("IServiceRun服务加载失败，服务未能正常启动！");
            }
            service.Start();
        }

        protected override void OnStop()
        {
            if (service == null)
            {
                throw new Exception("IServiceRun服务加载失败，服务未能正常停止！");
            }
            service.Stop();
        }
    }
}
