﻿using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Remoting;
using System.Data;
using MySoft.IoC;

namespace MySoft.PlatformService.UserService
{
    //[ServiceContract(Timeout = 1000, Format = ResponseFormat.Json, Compress = CompressType.GZip)]
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract(CacheTime = 30000)]
        UserInfo GetUserInfo(string username, string user, out int userid);

        DataTable GetDataTable();

        void SetUser(UserInfo user);

        int GetUserID();
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    [Serializable]
    public class UserInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string FullName
        {
            get
            {
                return typeof(Data.DataHelper).FullName;
            }
        }
    }
}
