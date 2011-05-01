using System.Data;
using System.Data.Common;
using MySoft.Logger;
using MySoft.Cache;

namespace MySoft.Data
{
    interface IDbSession : IDbTrans
    {
        #region Trans²Ù×÷

        void SetProvider(string connectName);
        void SetProvider(DbProvider dbProvider);

        DbTrans BeginTrans();
        DbTrans BeginTrans(IsolationLevel isolationLevel);
        DbTrans SetTransaction(DbTransaction trans);
        DbTrans SetConnection(DbConnection connection);
        DbTransaction BeginTransaction();
        DbTransaction BeginTransaction(IsolationLevel isolationLevel);
        DbConnection CreateConnection();
        DbParameter CreateParameter();
        #endregion

        #region ×¢²áLog

        void RegisterSqlLogger(LogEventHandler handler);
        void UnregisterSqlLogger(LogEventHandler handler);

        void RegisterSqlExceptionLogger(ErrorLogEventHandler handler);
        void UnregisterSqlExceptionLogger(ErrorLogEventHandler handler);

        void RegisterOnStartHandler(ExcutingEventHandler handler);
        void UnregisterOnStartHandler(ExcutingEventHandler handler);

        void RegisterOnEndHandler(ExcutingEventHandler handler);
        void UnregisterOnEndHandler(ExcutingEventHandler handler);

        #endregion

        #region ×¢Èë»º´æ

        /// <summary>
        /// ×¢Èë»º´æÒÀÀµ
        /// </summary>
        /// <param name="cache"></param>
        void InjectCacheDependent(ICacheDependent cache);

        #endregion

        #region ÏµÁÐ»¯²Ù×÷

        string Serialization(WhereClip where);
        string Serialization(OrderByClip order);

        #endregion
    }
}
