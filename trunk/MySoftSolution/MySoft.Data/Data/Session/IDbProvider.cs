using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace MySoft.Data
{
    interface IDbProvider : ILogable
    {
        #region 参数操作

        void AddParameter(DbCommand cmd, DbParameter parameter);
        void AddInputParameter(DbCommand cmd, string parameterName, DbType dbType, object value);
        void AddOutputParameter(DbCommand cmd, string parameterName, DbType dbType, int size);
        void AddInputOutputParameter(DbCommand cmd, string parameterName, DbType dbType, object value, int size);
        void AddReturnValueParameter(DbCommand cmd, string parameterName, DbType dbType);
        DbParameter GetParameter(DbCommand cmd, string parameterName);

        #endregion

        int ExecuteNonQuery(DbCommand cmd, DbTrans trans);
        SourceReader ExecuteReader(DbCommand cmd, DbTrans trans);
        DataTable ExecuteDataTable(DbCommand cmd, DbTrans trans);
        object ExecuteScalar(DbCommand cmd, DbTrans trans);

        /// <summary>
        /// 获取CommandText
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        string GetCommandText(DbCommand cmd);
    }
}
