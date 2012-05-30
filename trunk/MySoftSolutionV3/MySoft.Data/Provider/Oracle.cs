using System;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Text;
using MySoft.Data.Design;

namespace MySoft.Data.Oracle
{
    /// <summary>
    /// Oracle ����
    /// </summary>
    public class OracleProvider : DbProvider
    {
        public OracleProvider(string connectionString)
            : base(connectionString, OracleClientFactory.Instance, '"', '"', ':')
        {
        }

        /// <summary>
        /// �Ƿ�֧��������
        /// </summary>
        protected internal override bool SupportBatch
        {
            get { return true; }
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
            get { return "SELECT {0}.CURRVAL FROM DUAL"; }
        }

        /// <summary>
        /// ��ʽ��IdentityName
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override string GetAutoIncrement(string name)
        {
            return string.Format("{0}.NEXTVAL", name);
        }

        /// <summary>
        /// ����DbParameter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        protected override DbParameter CreateParameter(string parameterName, object val)
        {
            OracleParameter p = new OracleParameter();
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
        /// ����DbCommand
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        protected override void PrepareParameter(DbCommand cmd)
        {
            //��sqlת���ɴ�д
            cmd.CommandText = cmd.CommandText.ToUpper();

            //�滻ϵͳ����ֵ
            cmd.CommandText = cmd.CommandText.Replace("GETDATE()", "SYSDATE");

            foreach (OracleParameter p in cmd.Parameters)
            {
                p.ParameterName = p.ParameterName.ToUpper();
                if (p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.ReturnValue) continue;
                if (p.Value == DBNull.Value) continue;

                if (p.DbType == DbType.Guid)
                {
                    p.OracleType = OracleType.Char;
                    p.Size = 36;
                    p.Value = p.Value.ToString();
                }
                else if (p.DbType == DbType.String || p.DbType == DbType.AnsiString || p.DbType == DbType.AnsiStringFixedLength)
                {
                    if (p.Size > 4000)
                    {
                        p.OracleType = OracleType.NClob;
                    }
                    else
                    {
                        p.OracleType = OracleType.NVarChar;
                    }
                }
                else if (p.DbType == DbType.Binary)
                {
                    if (p.Size > 2000)
                    {
                        p.OracleType = OracleType.LongRaw;
                    }
                    else
                    {
                        p.OracleType = OracleType.Raw;
                    }
                }
                else if (p.DbType == DbType.Boolean)
                {
                    p.OracleType = OracleType.Number;
                    p.Size = 1;
                    p.Value = Convert.ToBoolean(p.Value) ? 1 : 0;
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
        protected internal override QuerySection<T> CreatePageQuery<T>(QuerySection<T> query, int itemCount, int skipCount)
        {
            if (skipCount == 0)
            {
                if (itemCount == 1 && string.IsNullOrEmpty(query.OrderString))
                {
                    query.PageWhere = new WhereClip("ROWNUM <= 1");
                    return query;
                }
                else
                {
                    QuerySection<T> jquery = query.SubQuery("TMP_TABLE");
                    jquery.Where(new WhereClip("ROWNUM <= " + itemCount));
                    jquery.Select(Field.All.At("TMP_TABLE"));

                    return jquery;
                }
            }
            else
            {
                //���û��ָ��Order ����ָ����key������
                if (string.IsNullOrEmpty(query.OrderString))
                {
                    Field pagingField = query.PagingField;

                    if ((IField)pagingField == null)
                    {
                        throw new DataException("�����õ�ǰʵ�������ֶλ�ָ������");
                    }

                    query.OrderBy(pagingField.Asc);
                }

                ((IPaging)query).Suffix(",ROW_NUMBER() OVER(" + query.OrderString + ") AS TMP__ROWID");
                query.OrderBy(OrderByClip.None);
                query.SetPagingField(null);

                QuerySection<T> jquery = query.SubQuery("TMP_TABLE");
                jquery.Where(new WhereClip("TMP__ROWID BETWEEN " + (skipCount + 1) + " AND " + (itemCount + skipCount)));
                jquery.Select(Field.All.At("TMP_TABLE"));

                return jquery;
            }
        }
    }
}
