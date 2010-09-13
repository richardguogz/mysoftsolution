using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Core.Remoting;

namespace MySoft.IoC.Dll
{
    public interface IUserService : IServiceInterface
    {
        string GetUserInfo(string username);
    }
}
