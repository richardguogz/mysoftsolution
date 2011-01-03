using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace MySoft.Core
{
    /// <summary>
    /// DnsHelper
    /// </summary>
    public static class DnsHelper
    {
        /// <summary>
        /// 用户的机器名
        /// </summary>
        public static string GetHostName()
        {
            return System.Net.Dns.GetHostName();
        }

        /// <summary>
        /// 获得本机局域网IP地址   
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {
            System.Net.IPAddress addr;
            addr = new System.Net.IPAddress(Dns.GetHostByName(Dns.GetHostName()).AddressList[0].Address);
            return addr.ToString();
        }

        /// <summary>
        /// 获得拨号动态分配IP地址   
        /// </summary>
        /// <returns></returns>
        public static string GetDynamicIPAddress()
        {
            System.Net.IPAddress addr;
            addr = new System.Net.IPAddress(Dns.GetHostByName(Dns.GetHostName()).AddressList[1].Address);
            return addr.ToString();
        }
    }
}
