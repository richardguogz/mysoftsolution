using System.Data;
using System.Data.Common;
using MySoft.Logger;
using MySoft.Cache;

namespace MySoft.Data
{
    interface IDbSession : IDbTrans
    {
        #region Trans����

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

        #region ע��Log

        void RegisterSqlLogger(LogEventHandler handler);
        void UnregisterSqlLogger(LogEventHandler handler);

        void RegisterSqlExceptionLogger(ErrorLogEventHandler handler);
        void UnregisterSqlExceptionLogger(ErrorLogEventHandler handler);

        void RegisterOnStartHandler(ExcutingEventHandler handler);
        void UnregisterOnStartHandler(ExcutingEventHandler handler);

        void RegisterOnEndHandler(ExcutingEventHandler handler);
        void UnregisterOnEndHandler(ExcutingEventHandler handler);

        #endregion

        #region ע�뻺��

        /// <summary>
        /// ע�뻺������
        /// </summary>
        /// <param name="cache"></param>
        void InjectCacheDependent(ICacheDependent cache);

        #endregion

        #region ϵ�л�����

        string Serialization(WhereClip where);
        string Serialization(OrderByClip order);

        #endregion
    }
}
