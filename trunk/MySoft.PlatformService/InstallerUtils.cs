﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace MySoft.PlatformService
{
    public static class InstallerUtils
    {
        public static ServiceController LookupService(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController s in services)
            {
                if (s.ServiceName.ToLower() == serviceName.ToLower())
                {
                    return s;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取服务列表
        /// </summary>
        /// <returns></returns>
        public static IList<ServiceInformation> GetServiceList(string contains)
        {
            IList<ServiceInformation> servicelist = new List<ServiceInformation>();
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController s in services)
            {
                if (s.ServiceName != null && s.ServiceName.ToLower().Contains(contains.ToLower()))
                {
                    servicelist.Add(new ServiceInformation(s.ServiceName));
                }
                else if (s.DisplayName != null && s.DisplayName.ToLower().Contains(contains.ToLower()))
                {
                    servicelist.Add(new ServiceInformation(s.ServiceName));
                }
            }

            return servicelist;
        }
    }
}
