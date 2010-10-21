using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using MySoft.Service;

namespace MySoft.Remoting
{
    /// <summary>
    /// 业务模块实体类
    /// </summary>
    [Serializable]
    public class ServiceModule : ServiceBase
    {
        private WellKnownObjectMode _Mode = WellKnownObjectMode.SingleCall;

        /// <summary>
        /// 对象激活方式（SingleCall或者SingleTon）
        /// </summary>
        public WellKnownObjectMode Mode
        {
            get { return _Mode; }
            set { _Mode = value; }
        }
    }
}
