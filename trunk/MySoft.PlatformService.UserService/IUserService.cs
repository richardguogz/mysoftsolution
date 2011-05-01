using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Remoting;
using System.Data;

namespace MySoft.PlatformService.UserService
{
    //[ServiceContract(Timeout = 1000, Format = ResponseFormat.Json, Compress = CompressType.GZip)]
    [ServiceContract(Timeout = 2000)]
    public interface IUserService
    {
        UserInfo GetUserInfo(string username, out int userid);

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
