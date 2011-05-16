using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySoft.IoC
{
    /// <summary>
    /// 服务配置
    /// </summary>
    public class ServiceConfig
    {
        #region Const Members

        /// <summary>
        /// The default timeout number. 
        /// </summary>
        public const double DEFAULT_TIMEOUT_NUMBER = 30; //30秒

        /// <summary>
        /// The default cachetime number.
        /// </summary>
        public const double DEFAULT_CACHETIME_NUMBER = 60; //60秒

        /// <summary>
        /// The default logtime number.
        /// </summary>
        public const double DEFAULT_LOGTIME_NUMBER = 1; //1秒

        /// <summary>
        /// The default record number.
        /// </summary>
        public const int DEFAULT_RECORD_NUMBER = 3600; //3600次

        /// <summary>
        /// The default maxconnect number.
        /// </summary>
        public const int DEFAULT_MAXCONNECT_NUMBER = 10000;

        /// <summary>
        /// The default maxbuffer number.
        /// </summary>
        public const int DEFAULT_MAXBUFFER_NUMBER = 4096;

        /// <summary>
        /// The default maxpool number.
        /// </summary>
        public const int DEFAULT_CLIENTPOOL_NUMBER = 5;

        #endregion
    }
}
