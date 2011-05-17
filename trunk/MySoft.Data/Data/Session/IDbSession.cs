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
        /// ���ó�ʱ��ʾ��־
        /// </summary>
        /// <param name="timeout"></param>
        void SetLogTimeout(double timeout);

        /// <summary>
        /// ע����־����
        /// </summary>
        /// <param name="logger"></param>
        void InjectExcutingLog(IExcutingLog logger);

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
