using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Core.Remoting;

namespace MySoft.IoC.Service
{
    /// <summary>
    /// CastleService
    /// </summary>
    public class CastleService : RemotingServerHelper
    {
        private CastleFactoryConfiguration config;
        public CastleService(CastleFactoryConfiguration config)
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
