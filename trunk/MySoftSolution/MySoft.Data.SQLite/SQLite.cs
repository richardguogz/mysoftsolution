using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Odbc;
using System.Data.Common;
using System.Data.SQLite;

namespace MySoft.Data.SQLite
{
    /// <summary>
    /// SQLite ����
    /// </summary>
    public class SQLiteProvider : DbProvider
    {
        public SQLiteProvider(string connectionString)
            : base(connectionString, SQLiteFactory.Instance, '[', ']', '?')
        {
        }

        /// <summary>
        /// �Ƿ�֧��������
        /// </summary>
        protected override bool SupportBatch
        {
            get { return true; }
        }

        /// <summary>
        /// �����Զ�ID��sql���
        /// </summary>
        protected override string RowAutoID
        {
            get { return "select last_insert_rowid()"; }
        }


        /// <summary>
        /// ����DbParameter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        protected override DbParameter CreateParameter(string parameterName, object val)
        {
            SQLiteParameter p = new SQLiteParameter();
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
        /// ϵ�л�SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected override string Serialization(string sql)
        {
            sql = sql.Replace("substring(", "substr(")
                .Replace("len(", "length(")
                .Replace("getdate()", "datetime('now')");

            if (sql.Contains("trim("))
            {
                throw new NotSupportedException("Sqlite provider does not support Trim() function.");
            }
            if (sql.Contains("charindex("))
            {
                throw new NotSupportedException("Sqlite provider does not support IndexOf() function.");
            }
            if (sql.Contains("replace("))
            {
                throw new NotSupportedException("Sqlite provider does not support Replace() function.");
            }
            if (sql.Contains("datepart("))
            {
                throw new NotSupportedException("Sqlite provider does not support GetYear()/GetMonth()/GetDay() functions.");
            }

            return base.Serialization(sql);
        }

        /// <summary>
        /// ����DbCommand
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        protected override DbCommand PrepareCommand(DbCommand cmd)
        {
            foreach (SQLiteParameter p in cmd.Parameters)
            {
                if (p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.ReturnValue) continue;
                if (p.Value == DBNull.Value) continue;

                //SQLite����������
                cmd.CommandText = cmd.CommandText.Replace(p.ParameterName, "?");
                p.ParameterName = "?";
            }

            return base.PrepareCommand(cmd);
        }

        /// <summary>
        /// ������ҳ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="itemCount"></param>
        /// <param name="skipCount"></param>
        /// <returns></returns>
        protected override QuerySection<T> CreatePageQuery<T>(QuerySection<T> query, int itemCount, int skipCount)
        {
            if (skipCount == 0)
            {
                ((IPaging)query).End("limit " + itemCount);
                return query;
            }
            else
            {
                string suffix = string.Format("limit {0} offset {1}", itemCount, skipCount);
                ((IPaging)query).End(suffix);
                return query;
            }
        }
    }
}
