using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace MySoft.Data
{
    public class SqlSection : ISqlSection
    {
        private DbProvider dbProvider;
        private DbCommand dbCommand;
        private DbTrans dbTran;

        internal SqlSection(string sql, SQLParameter[] parameters, DbProvider dbProvider, DbTrans dbTran)
        {
            this.dbProvider = dbProvider;
            this.dbTran = dbTran;
            this.dbCommand = dbProvider.CreateSqlCommand(sql, parameters);
        }

        /// <summary>
        /// 增加多个参数到当前Sql命令
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public SqlSection AddParameters(params DbParameter[] parameters)
        {
            foreach (DbParameter parameter in parameters)
            {
                dbProvider.AddParameter(dbCommand, parameter);
            }
            return this;
        }

        /// <summary>
        /// 增加一个输入参数到当前Sql命令
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlSection AddInputParameter(string paramName, DbType dbType, object value)
        {
            dbProvider.AddInputParameter(dbCommand, paramName, dbType, value);
            return this;
        }

        /// <summary>
        /// 增加一个输入参数到当前Sql命令
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlSection AddInputParameter(string paramName, DbType dbType, int size, object value)
        {
            dbProvider.AddInputParameter(dbCommand, paramName, dbType, size, value);
            return this;
        }

        /// <summary>
        /// 执行当前Sql命令
        /// </summary>
        /// <returns></returns>
        public int Execute()
        {
            return dbProvider.ExecuteNonQuery(dbCommand, dbTran);
        }

        /// <summary>
        /// 执行当前Sql命令并返回一个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ToSingle<T>()
            where T : Entity
        {
            ISourceList<T> list = GetList<T>(dbCommand, dbTran);
            if (list.Count == 0)
            {
                return default(T);
            }
            else
            {
                return list[0];
            }
        }

        /// <summary>
        /// 执行当前Sql命令并返回一个List列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ISourceList<T> ToList<T>()
            where T : Entity
        {
            return GetList<T>(dbCommand, dbTran);
        }

        /// <summary>
        /// 执行当前Proc并返回一个列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public IArrayList<TResult> ToListResult<TResult>()
        {
            return GetListResult<TResult>(dbCommand, dbTran);
        }

        /// <summary>
        /// 执行当前Sql命令并返回一个DbReader
        /// </summary>
        /// <returns></returns>
        public ISourceReader ToReader()
        {
            return dbProvider.ExecuteReader(dbCommand, dbTran);
        }

        /// <summary>
        /// 执行当前Sql命令并返回一个DataTable
        /// </summary>
        /// <returns></returns>
        public ISourceTable ToTable()
        {
            DataTable dt = dbProvider.ExecuteDataTable(dbCommand, dbTran);
            return new SourceTable(dt);
        }

        /// <summary>
        /// 执行当前Sql命令并返回一个DataSet
        /// </summary>
        /// <returns></returns>
        public DataSet ToDataSet()
        {
            return dbProvider.ExecuteDataSet(dbCommand, dbTran);
        }

        /// <summary>
        /// 执行当前Sql命令并返回一个值
        /// </summary>
        /// <returns></returns>
        public object ToScalar()
        {
            return dbProvider.ExecuteScalar(dbCommand, dbTran);
        }

        /// <summary>
        /// 执行当前
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public TResult ToScalar<TResult>()
        {
            object obj = this.ToScalar();
            if (obj == null) return default(TResult);
            return DataUtils.ConvertValue<TResult>(obj);
        }

        #region 私有方法

        private ISourceList<T> GetList<T>(DbCommand cmd, DbTrans dbTran)
            where T : Entity
        {
            try
            {
                using (ISourceReader reader = dbProvider.ExecuteReader(cmd, dbTran))
                {
                    ISourceList<T> list = new SourceList<T>();
                    FastCreateInstanceHandler creator = DataUtils.CreateHandler(typeof(T));

                    while (reader.Read())
                    {
                        T t = (T)creator();
                        t.SetAllValues(reader);
                        t.Attach();
                        list.Add(t);
                    }

                    reader.Close();

                    return list;
                }
            }
            catch
            {
                throw;
            }
        }

        private IArrayList<TResult> GetListResult<TResult>(DbCommand cmd, DbTrans dbTran)
        {
            try
            {
                using (ISourceReader reader = dbProvider.ExecuteReader(cmd, dbTran))
                {
                    IArrayList<TResult> list = new ArrayList<TResult>();

                    if (typeof(TResult) == typeof(object[]))
                    {
                        while (reader.Read())
                        {
                            List<object> objs = new List<object>();
                            for (int row = 0; row < reader.FieldCount; row++)
                            {
                                objs.Add(reader.GetValue(row));
                            }

                            TResult result = (TResult)(objs.ToArray() as object);
                            list.Add(result);
                        }
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetValue<TResult>(0));
                        }
                    }

                    reader.Close();

                    return list;
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}
