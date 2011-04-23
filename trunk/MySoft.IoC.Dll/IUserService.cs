using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Remoting;
using System.Data;

namespace MySoft.IoC.Dll
{
    [ServiceContract(Timeout = 10000)]
    public interface IUserService
    {
        UserInfo GetUserInfo(string username);

        DataTable GetDataTable();
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    [Serializable]
    public class UserInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
