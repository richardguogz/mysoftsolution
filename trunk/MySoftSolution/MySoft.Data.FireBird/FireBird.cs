using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Text;
using FirebirdSql.Data.FirebirdClient;

namespace MySoft.Data.FireBird
{
    /// <summary>
    /// FireBird 驱动
    /// </summary>
    public class FireBirdProvider : DbProvider
    {
        public FireBirdProvider(string connectionString)
            : base(connectionString, FirebirdClientFactory.Instance, ' ', ' ', '@')
        {
        }

        /// <summary>
        /// 是否支持批处理
        /// </summary>
        protected override bool SupportBatch
        {
            get { return false; }
        }

        /// <summary>
        /// 是否使用自增列
        /// </summary>
        protected override bool UseAutoIncrement
        {
            get { return true; }
        }

        /// <summary>
        /// 返回自动ID的sql语句
        /// </summary>
        protected override string RowAutoID
        {
            get { return "select gen_id({0}, 1) from rdb$database"; }
        }

        /// <summary>
        /// 格式化IdentityName
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override string FormatIdentityName(string name)
        {
            return string.Format("gen_id({0}, 1)", name);
        }

        /// <summary>
        /// 获取输出日志
        /// </summary>
        /// <param name="command">The command.</param>
        protected override string GetLog(DbCommand command)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format("{0}\t{1}\t\r\n", command.CommandType, command.CommandText));
            if (command.Parameters != null && command.Parameters.Count > 0)
            {
                sb.Append("Parameters:\r\n");
                foreach (FbParameter p in command.Parameters)
                {
                    if (p.Size > 0)
                    {
                        if (p.Scale > 0)
                            sb.Append(string.Format("{0}[{2}][{3}({4},{5})] = {1}\r\n", p.ParameterName, p.Value, p.DbType, p.FbDbType, p.Size, p.Scale));
                        else
                            sb.Append(string.Format("{0}[{2}][{3}({4})] = {1}\r\n", p.ParameterName, p.Value, p.DbType, p.FbDbType, p.Size));
                    }
                    else
                        sb.Append(string.Format("{0}[{2}][{3}] = {1}\r\n", p.ParameterName, p.Value, p.DbType, p.FbDbType));
                }
            }
            sb.Append("\r\n");

            return sb.ToString();
        }

        /// <summary>
        /// 创建DbParameter
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
        /// 调整命令
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        protected override DbCommand PrepareCommand(DbCommand cmd)
        {
            //replace "N'" to "'"
            cmd.CommandText = cmd.CommandText.Replace("N'", "'");

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

            return base.PrepareCommand(cmd);
        }

        /// <summary>
        /// 创建分页
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
