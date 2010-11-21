using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using LiveChat.Interface;
using LiveChat.Remoting;
using LiveChat.Service;
using LiveChat.Utils;
using LiveChat.Service.Manager;
using System.Runtime.Remoting;

namespace LiveChat.WindowsService
{
    public partial class LiveChatService : ServiceBase
    {
        public LiveChatService()
        {
            InitializeComponent();
            Thread.GetDomain().UnhandledException += new UnhandledExceptionEventHandler(LiveChatService_UnhandledException);
        }

        void LiveChatService_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //出错时保存会话和用户信息
            UserManager.Instance.SaveUser();
            SessionManager.Instance.SaveSessionAndMessage();

            srv_OnLog(e.ExceptionObject.ToString());
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            try
            {
                //远程抛出错误
                RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
                RemotingConfiguration.CustomErrorsEnabled(false);

                RemotingServerConfiguration cfg = RemotingServerConfiguration.GetConfig();

                if (cfg != null)
                {
                    srv_OnLog("开始发布远程对象......");
                    RemotingServerHelper srv = new RemotingServerHelper(cfg);
                    srv.OnLog += new LiveChat.Logger.LogHandler(srv_OnLog);
                    srv.PublishWellKnownServiceInstanceByConfig();
                    srv_OnLog("远程对象发布完成.");

                    //Thread.Sleep(30000);

                    //加载服务异常后的数据
                    srv_OnLog("正在加载初始化数据......");

                    CompanyManager.Instance.LoadCompany();
                    GroupManager.Instance.LoadGroup();
                    UserManager.Instance.LoadUser();
                    SessionManager.Instance.LoadSessionAndMessage();

                    srv_OnLog("初始化数据加载完成.");
                }
                else
                {
                    srv_OnLog("由于Remoting配置为空，本程序将不会提供Remoting服务");
                }

                srv_OnLog("聊天服务已启动");
            }
            catch (Exception ex)
            {
                srv_OnLog("聊天服务启动失败：" + ex.ToString());
            }
        }

        void srv_OnLog(string log)
        {
            try
            {
                Utils.Logger.Instance.WriteLog(log);
            }
            catch { }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        protected override void OnStop()
        {
            try
            {
                UserManager.Instance.SaveUser();
                SessionManager.Instance.SaveSessionAndMessage();
            }
            catch { }

            Utils.Logger.Instance.WriteLog("聊天服务已停止");
        }
    }
}
