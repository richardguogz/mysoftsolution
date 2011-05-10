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
using MySoft.Installer;

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
            try
            {
                SimpleLog.Instance.WriteLog("正在启动服务......");
                if (service == null)
                {
                    throw new Exception("IServiceRun服务加载失败，服务未能正常启动！");
                }
                service.Start();
                SimpleLog.Instance.WriteLog("服务启动成功！");
            }
            catch (Exception ex)
            {
                SimpleLog.Instance.WriteLog(ex);
                throw ex;
            }
        }

        protected override void OnStop()
        {
            try
            {
                SimpleLog.Instance.WriteLog("正在停止服务......");
                if (service == null)
                {
                    throw new Exception("IServiceRun服务加载失败，服务未能正常停止！");
                }
                service.Stop();
                SimpleLog.Instance.WriteLog("服务成功停止！");
            }
            catch (Exception ex)
            {
                SimpleLog.Instance.WriteLog(ex);
                throw ex;
            }
        }
    }
}
