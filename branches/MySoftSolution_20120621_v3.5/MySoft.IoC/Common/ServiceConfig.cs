﻿
namespace MySoft.IoC
{
    /// <summary>
    /// 服务配置
    /// </summary>
    public class ServiceConfig
    {
        #region Const Members

        /// <summary>
        /// The default record hour
        /// </summary>
        public const int DEFAULT_RECORD_HOUR = 24; //24小时

        /// <summary>
        /// The server default max caller
        /// </summary>
        public const int DEFAULT_SERVER_MAXCALLER = 1000; //默认并发数1000

        /// <summary>
        /// The client default max caller
        /// </summary>
        public const int DEFAULT_CLIENT_MAXCALLER = 100; //默认并发数100

        /// <summary>
        /// The default client call timeout number. 
        /// </summary>
        public const int DEFAULT_CLIENT_TIMEOUT = 30; //30秒

        /// <summary>
        /// The default pool number.
        /// </summary>
        public const int DEFAULT_CLIENT_MAXPOOL = 100; //默认为100

        /// <summary>
        /// The current framework version.
        /// </summary>
        public const string CURRENT_FRAMEWORK_VERSION = "v3.8"; //当前版本

        #endregion
    }
}