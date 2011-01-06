using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Remoting;

namespace MySoft.IoC
{
    /// <summary>
    /// CastleService
    /// </summary>
    public class CastleConfigService : RemotingServerHelper
    {
        private CastleFactoryConfiguration config;
        public CastleConfigService(CastleFactoryConfiguration config)
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
            base.PublishWellKnownServiceInstance(config.ServiceMQName, typeof(IServiceMQ), mq, System.Runtime.Remoting.WellKnownObjectMode.SingleCall);
        }
    }
}
