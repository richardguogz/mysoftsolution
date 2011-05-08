using System;
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
    }
}
