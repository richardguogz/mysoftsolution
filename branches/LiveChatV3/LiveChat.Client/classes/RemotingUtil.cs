using System;
using System.Collections.Generic;
using System.Web;
using LiveChat.Interface;
using LiveChat.Remoting;
using LiveChat.Utils;

namespace LiveChat.Client
{
    public abstract class RemotingUtil
    {
        public static RemotingInterfaceType GetRemotingObject<RemotingInterfaceType>(string remotingObjectName)
        {
            try
            {
                RemotingInterfaceType obj = RemotingClientUtil<RemotingInterfaceType>.Instance.GetRemotingObject(remotingObjectName);
                return obj;
            }
            catch (Exception ex)
            {
                throw new LiveChatException("创建Remoting远程对象出错！", ex);
            }
        }

        /// <summary>
        /// 创建客服服务
        /// </summary>
        /// <returns></returns>
        public static ISeatService GetRemotingSeatService()
        {
            return GetRemotingObject<ISeatService>("SeatService");
        }

        /// <summary>
        /// 创建用户服务
        /// </summary>
        /// <returns></returns>
        public static IUserService GetRemotingUserService()
        {
            return GetRemotingObject<IUserService>("UserService");
        }
    }
}
