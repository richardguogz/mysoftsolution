using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySoft.Data;
using System.Threading;

namespace MySoft.PlatformService.UserService
{
    public class UserService : IUserService
    {
        public UserInfo GetUserInfo(string username, out int userid)
        {
            userid = username.Length;

            //if (username.Length % 2 == 0)
            //{
            //    throw new Exception(username + " =>  出错啦！");
            //}

            //if (username.Length % 2 == 0)
            //{
            //    Thread.Sleep(2000);
            //}
            //else
            //{
            //    Thread.Sleep(1000);
            //}

            Thread.Sleep(1000);

            //throw new Exception(username + " =>  出错啦！");

            return new UserInfo()
            {
                Name = username,
                Description = string.Format("{0} --> 您的用户名为：{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), username)
            };
        }

        public DataTable GetDataTable()
        {
            DbProvider p = DbProviderFactory.CreateDbProvider(DbProviderType.SqlServer9, "server=(local);Database=Shumi.LiveChat;Uid=sa;Pwd=shumiwang;");
            DbSession dbSession = new DbSession(p);

            var data = dbSession.FromSql("select * from t_company").ToTable().OriginalData;
            return data;
        }
    }
}
