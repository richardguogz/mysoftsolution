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

        #region ע����Ϣ

        /// <summary>
        /// ע����ܵ�Handler
        /// </summary>
        /// <param name="handler"></param>
        void RegisterDecryptHandler(DecryptEventHandler handler);

        /// <summary>
        /// ע����־����
        /// </summary>
        /// <param name="logger"></param>
        void RegisterExcutingLog(IExcutingLog logger);

        /// <summary>
        /// ע�Ỻ������
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="timeout"></param>
        void RegisterCacheDependent(ICacheDependent cache, int timeout);

        #endregion

        #region ϵ�л�����

        string Serialization(WhereClip where);
        string Serialization(OrderByClip order);

        #endregion
    }
}
