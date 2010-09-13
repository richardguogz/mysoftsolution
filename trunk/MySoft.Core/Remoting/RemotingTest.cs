using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Core.Remoting
{
    /// <summary>
    /// Remoting测试类接口
    /// </summary>
    [ServiceContract]
    public interface IRemotingTest
    {
        /// <summary>
        /// 测试方法，获取Remoting服务器时间
        /// </summary>
        /// <returns></returns>
        string GetDate();
    }

    /// <summary>
    /// Remoting测试类，用来测试Remoting服务器是否运行正常
    /// </summary>
    public class RemotingTest : MarshalByRefObject, IRemotingTest
    {
        /// <summary>
        /// 测试方法，获取Remoting服务器时间
        /// </summary>
        /// <returns></returns>
        public string GetDate()
        {
            return DateTime.Now.ToString();
        }
    }
}
