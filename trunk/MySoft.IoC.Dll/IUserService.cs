using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Core.Remoting;
using System.Data;

namespace MySoft.IoC.Dll
{
    public interface IUserService : IServiceInterface
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
