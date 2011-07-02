using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySoft.Data;
using System.Threading;

namespace MySoft.PlatformService.UserService
{
    public class UserService : IUserService, Castle.Core.IStartable
    {
        public int GetUserID()
        {
            Thread.Sleep(100);

            if (Environment.TickCount % 100 == 0)
                throw new MySoftException(ExceptionType.Unknown, Environment.TickCount + "错误！");

            return 1;
        }

        public void SetUser(UserInfo user, ref int userid)
        {
            //throw new Exception("asdfsad");
            //Console.WriteLine(user.Name);

            userid = userid + 1;
        }

        public UserInfo GetUserInfo(string username, out int userid)
        {
            userid = username.Length;

            var user = new UserInfo()
            {
                Name = username,
                Description = string.Format("您的用户名为：{0}", username)
            };

            return user;
        }

        public DataTable GetDataTable()
        {
            DbProvider p = DbProviderFactory.CreateDbProvider(DbProviderType.SqlServer9, "server=(local);Database=Shumi.LiveChat;Uid=sa;Pwd=shumiwang;");
            DbSession dbSession = new DbSession(p);

            var data = dbSession.FromSql("select * from t_company").ToTable().OriginalData;
            return data;
        }

        #region IStartable 成员

        public void Start()
        {
            Console.WriteLine("autostart");
        }

        public void Stop()
        {
            Console.WriteLine("autostop");
        }

        #endregion

        #region IUserService 成员


        public IList<UserInfo> GetUsers()
        {
            return new List<UserInfo>
            {
                new UserInfo(),
                new UserInfo()
            };
        }

        public IDictionary<string, UserInfo> GetDictUsers()
        {
            return new Dictionary<string, UserInfo>();
        }

        #endregion
    }
}
