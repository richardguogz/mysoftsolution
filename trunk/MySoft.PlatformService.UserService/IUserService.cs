﻿using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Remoting;
using System.Data;
using MySoft.IoC;

namespace MySoft.PlatformService.UserService
{
    //[ServiceContract(Timeout = 1000, Format = ResponseFormat.Json, Compress = CompressType.GZip)]
    [ServiceContract(AllowCache = true)]
    public interface IUserService
    {
        [OperationContract(CacheTime = 30000)]
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
