using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data;
using MySoft.Logger;

namespace LiveChat.Service
{
    /// <summary>
    /// 数据访问类
    /// </summary>
    internal class DataAccess
    {
        public static readonly DbSession DbLiveChat;

        static DataAccess()
        {
            DbLiveChat = new DbSession("FundLiveChat");
            DbLiveChat.RegisterExcutingLog(new LiveChatExcuting());
        }
    }

    /// <summary>
    /// 处理日志处理
    /// </summary>
    public class LiveChatExcuting : IExcutingLog
    {
        #region IExcutingLog 成员

        public void EndExcute(string cmdText, SQLParameter[] parameter, object result, int elapsedTime)
        {
            throw new NotImplementedException();
        }

        public bool StartExcute(string cmdText, SQLParameter[] parameter)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ILog 成员

        public void WriteError(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void WriteLog(string log, LogType type)
        {
#if DEBUG
            Console.WriteLine(log);
#endif
        }

        #endregion
    }
}
