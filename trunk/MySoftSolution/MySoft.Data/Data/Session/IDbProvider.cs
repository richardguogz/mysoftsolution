using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace MySoft.Data
{
    interface IDbProvider : ILogable
    {
        #region ��������

        void AddParameter(DbCommand cmd, DbParameter parameter);
        void AddInputParameter(DbCommand cmd, string parameterName, DbType dbType, object value);
        void AddOutputParameter(DbCommand cmd, string parameterName, DbType dbType, int size);
        void AddInputOutputParameter(DbCommand cmd, string parameterName, DbType dbType, object value, int size);
        void AddReturnValueParameter(DbCommand cmd, string parameterName, DbType dbType);
        DbParameter GetParameter(DbCommand cmd, string parameterName);


        /// <summary>
        /// ����DbConnection
        /// </summary>
        /// <returns></returns>
        DbConnection CreateConnection();

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        DbParameter CreateParameter(string paramterName);

        #endregion

        int ExecuteNonQuery(DbCommand cmd, DbTrans trans);
        SourceReader ExecuteReader(DbCommand cmd, DbTrans trans);
        DataTable ExecuteDataTable(DbCommand cmd, DbTrans trans);
        object ExecuteScalar(DbCommand cmd, DbTrans trans);
    }
}
