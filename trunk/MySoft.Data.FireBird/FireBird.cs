using System;
using System.Data;
using System.Data.Common;
using FirebirdSql.Data.FirebirdClient;

namespace MySoft.Data.FireBird
{
    /// <summary>
    /// FireBird ����
    /// </summary>
    public class FireBirdProvider : DbProvider
    {
        public FireBirdProvider(string connectionString)
            : base(connectionString, FirebirdClientFactory.Instance, ' ', ' ', '@')
        {
        }

        /// <summary>
        /// �Ƿ�֧��������
        /// </summary>
        protected override bool SupportBatch
        {
            get { return false; }
        }

        /// <summary>
        /// �Ƿ�ʹ��������
        /// </summary>
        protected override bool AllowAutoIncrement
        {
            get { return true; }
        }

        /// <summary>
        /// �����Զ�ID��sql���
        /// </summary>
        protected override string AutoIncrementValue
        {
            get { return "select gen_id({0}, 1) from rdb$database"; }
        }

        /// <summary>
        /// ��ʽ��IdentityName
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override string GetAutoIncrement(string name)
        {
            return string.Format("gen_id({0}, 1)", name);
        }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected override object GetParameterType(DbParameter parameter)
        {
            return (parameter as FbParameter).FbDbType;
        }

        /// <summary>
        /// ����DbParameter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        protected override DbParameter CreateParameter(string parameterName, object val)
        {
            FbParameter p = new FbParameter();
            p.ParameterName = parameterName;
            if (val == null || val == DBNull.Value)
            {
                p.Value = DBNull.Value;
            }
            else
            {
                if (val is Enum)
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
        /// ��������
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        protected override void PrepareParameter(DbCommand cmd)
        {
            //replace "N'" to "'"
            cmd.CommandText = cmd.CommandText.Replace("N'", "'");

            //�滻ϵͳ����ֵ
            cmd.CommandText = cmd.CommandText.Replace("getdate()", "current_timestamp");

            foreach (FbParameter p in cmd.Parameters)
            {
                if (p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.ReturnValue) continue;
                if (p.Value == DBNull.Value) continue;

                if (p.DbType == DbType.String || p.DbType == DbType.AnsiString || p.DbType == DbType.AnsiStringFixedLength)
                {
                    if (p.Size > 4000)
                    {
                        p.FbDbType = FbDbType.Text;
                    }
                    else
                    {
                        p.FbDbType = FbDbType.VarChar;
                    }
                }
            }
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
                ((IPaging)query).Prefix("first " + itemCount);
                return query;
            }
            else
            {
                string prefix = string.Format("first {0} skip {1}", itemCount, skipCount);
                ((IPaging)query).Prefix(prefix);
                return query;
            }
        }
    }
}
