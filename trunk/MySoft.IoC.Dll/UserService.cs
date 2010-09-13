using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.IoC.Dll
{
    public class UserService : MarshalByRefObject
    {
        public string GetUserInfo(string username)
        {
            return string.Format("{0} --> 您的用户名为：{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), username);
        }
    }
}
