using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace LiveChat.Service
{
    /// <summary>
    /// 服务配置类
    /// </summary>
    public class ServiceConfig
    {
        public static readonly string FileUploadUrl = ConfigurationManager.AppSettings["FileUploadUrl"];
        public static readonly int MessageSaveInterval = Convert.ToInt32(ConfigurationManager.AppSettings["MessageSaveInterval"]);
        public static readonly int TimeoutExitInterval = Convert.ToInt32(ConfigurationManager.AppSettings["TimeoutExitInterval"]);
    }
}
