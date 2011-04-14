using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySoft.Data;

namespace MySoft.IoC.Dll
{
    public class UserService : IUserService
    {
        public UserInfo GetUserInfo(string username)
        {
            //if (username.Length % 2 == 0)
            //{
            //    throw new Exception(username + " =>  出错啦！");
            //}

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
