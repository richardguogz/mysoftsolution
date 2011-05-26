using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace MySoft.Data.SqlServer
{
    /// <summary>
    /// SQL Server 2000 驱动
    /// </summary>
    public class SqlServerProvider : DbProvider
    {
        public SqlServerProvider(string connectionString)
            : this(connectionString, SqlClientFactory.Instance)
        {
        }

        public SqlServerProvider(string connectionString, System.Data.Common.DbProviderFactory dbFactory)
            : base(connectionString, dbFactory, '[', ']', '@')
        {
        }

        /// <summary>
        /// 是否支持批处理
        /// </summary>
        protected internal override bool SupportBatch
        {
            get { return true; }
        }

        /// <summary>
        /// 返回自动ID的sql语句
        /// </summary>
        protected override string AutoIncrementValue
        {
            get { return "select scope_identity()"; }
        }

        /// <summary>
        /// 获取参数类型
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected override object GetParameterType(DbParameter parameter)
        {
            return (parameter as SqlParameter).SqlDbType;
        }

        /// <summary>
        /// 创建DbParameter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        protected override DbParameter CreateParameter(string parameterName, object val)
        {
            SqlParameter p = new SqlParameter();
            p.ParameterName = parameterName;
            if (val == null || val == DBNull.Value)
            {
                p.Value = DBNull.Value;
            }
            else
            {
                if (val.GetType().IsEnum)
                {
                    p.Value = Convert.ToInt32(val);
                }
                else
                {
                    p.Value = val;
                }
            }
            return p;
        }

        /// <summary>
        /// 调整DbCommand
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        protected override void PrepareParameter(DbCommand cmd)
        {
            foreach (SqlParameter p in cmd.Parameters)
            {
                if (p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.ReturnValue) continue;
                if (p.Value == DBNull.Value) continue;

                if (p.DbType == DbType.String || p.DbType == DbType.AnsiString || p.DbType == DbType.AnsiStringFixedLength)
                {
                    if (p.Size > 4000)
                    {
                        p.SqlDbType = SqlDbType.NText;
                    }
                    else
                    {
                        p.SqlDbType = SqlDbType.NVarChar;
                    }
                }
                else if (p.DbType == DbType.Binary)
                {
                    if (p.Size > 8000)
                    {
                        p.SqlDbType = SqlDbType.Image;
                    }
                }
            }
        }
    }
}
