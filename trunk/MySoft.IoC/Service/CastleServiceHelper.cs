using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Remoting;
using System.Runtime.Remoting;

namespace MySoft.IoC
{
    /// <summary>
    /// CastleServiceHelper
    /// </summary>
    public class CastleServiceHelper : RemotingServerHelper
    {
        private CastleFactoryConfiguration config;
        public CastleServiceHelper(CastleFactoryConfiguration config)
            : base(config.Protocol, config.Server, config.Port)
        {
            this.config = config;
        }

        /// <summary>
        /// 发布服务实例
        /// </summary>
        /// <param name="mq"></param>
        public void PublishWellKnownServiceInstance(MemoryServiceMQ mq)
        {
            base.PublishWellKnownServiceInstance(config.ServiceMQName, typeof(IServiceMQ), mq, WellKnownObjectMode.SingleCall);
        }
    }
}
