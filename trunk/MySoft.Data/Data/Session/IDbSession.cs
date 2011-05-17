using System.Data;
using System.Data.Common;
using MySoft.Logger;
using MySoft.Cache;

namespace MySoft.Data
{
    interface IDbSession : IDbTrans
    {
        #region Trans操作

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

        #region 注入信息

        /// <summary>
        /// 注册解密的Handler
        /// </summary>
        /// <param name="handler"></param>
        void RegisterDecryptHandler(DecryptEventHandler handler);

        /// <summary>
        /// 设置超时显示日志
        /// </summary>
        /// <param name="timeout"></param>
        void SetLogTimeout(double timeout);

        /// <summary>
        /// 注册日志依赖
        /// </summary>
        /// <param name="logger"></param>
        void InjectExcutingLog(IExcutingLog logger);

        /// <summary>
        /// 注入缓存依赖
        /// </summary>
        /// <param name="cache"></param>
        void InjectCacheDependent(ICacheDependent cache);

        #endregion

        #region 系列化操作

        string Serialization(WhereClip where);
        string Serialization(OrderByClip order);

        #endregion
    }
}
