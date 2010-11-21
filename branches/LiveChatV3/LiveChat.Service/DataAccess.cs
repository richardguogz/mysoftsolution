using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data;

namespace LiveChat.Service
{
    /// <summary>
    /// 数据访问类
    /// </summary>
    internal class DataAccess
    {
        public static readonly FundLiveChat DbLiveChat = new FundLiveChat();
    }

    /// <summary>
    /// Chat数据库重载
    /// </summary>
    public class FundLiveChat : DbSession
    {
        public FundLiveChat()
            : base("FundLiveChat")
        {
#if DEBUG
            this.RegisterSqlLogger(delegate(string log)
            {
                Console.Write(log);
            });
#endif
        }
    }
}
