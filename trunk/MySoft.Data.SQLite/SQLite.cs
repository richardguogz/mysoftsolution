using System;
using System.Data;
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
        protected override string AutoIncrementValue
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
        /// ����DbCommand
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        protected override DbCommand PrepareCommand(DbCommand cmd)
        {
            //�滻ϵͳ����ֵ
            cmd.CommandText = cmd.CommandText.Replace("getdate()", "current_timestamp");

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
