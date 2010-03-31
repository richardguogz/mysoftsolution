using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MySoft.Data
{
    /// <summary>
    /// Top对应的Query查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TopSection<T> : ITopSection<T>
        where T : Entity
    {
        private QuerySection<T> query;
        private int topSize;
        internal TopSection(QuerySection<T> query, int topSize)
        {
            this.query = query;
            this.topSize = topSize;
        }

        internal string QueryString
        {
            get
            {
                return query.QueryString;
            }
        }

        internal SQLParameter[] Parameters
        {
            get
            {
                return query.Parameters;
            }
        }

        #region SubQuery

        /// <summary>
        /// 返回一个子查询
        /// </summary>
        /// <returns></returns>
        public QuerySection<T> SubQuery()
        {
            return query.SubQuery();
        }

        /// <summary>
        /// 返回一个子查询
        /// </summary>
        /// <param name="aliasName"></param>
        /// <returns></returns>
        public QuerySection<T> SubQuery(string aliasName)
        {
            return query.SubQuery(aliasName);
        }

        /// <summary>
        /// 返回一个子查询
        /// </summary>
        /// <typeparam name="TSub"></typeparam>
        /// <returns></returns>
        public QuerySection<TSub> SubQuery<TSub>()
            where TSub : Entity
        {
            return query.SubQuery<TSub>();
        }

        /// <summary>
        /// 返回一个子查询
        /// </summary>
        /// <typeparam name="TSub"></typeparam>
        /// <param name="aliasName"></param>
        /// <returns></returns>
        public QuerySection<TSub> SubQuery<TSub>(string aliasName)
            where TSub : Entity
        {
            return query.SubQuery<TSub>(aliasName);
        }

        #endregion

        #region ITopSection<T> 成员

        /// <summary>
        /// 返回Object列表
        /// </summary>
        /// <returns></returns>
        public IArrayList<object> ToListResult()
        {
            return query.ToListResult(1, topSize);
        }

        /// <summary>
        /// 返回Object列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public IArrayList<TResult> ToListResult<TResult>()
        {
            return query.ToListResult<TResult>(1, topSize);
        }

        /// <summary>
        /// 返回T类型列表
        /// </summary>
        /// <returns></returns>
        public ISourceList<T> ToList()
        {
            return query.ToList(1, topSize);
        }

        /// <summary>
        /// 返回TEntity类型列表
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public ISourceList<TEntity> ToList<TEntity>()
            where TEntity : Entity
        {
            return query.ToList<TEntity>(1, topSize);
        }

        /// <summary>
        /// 返回一个DataTable
        /// </summary>
        /// <returns></returns>
        public ISourceTable ToTable()
        {
            return query.ToTable(1, topSize);
        }

        /// <summary>
        /// 返回一个DataReader
        /// </summary>
        /// <returns></returns>
        public ISourceReader ToReader()
        {
            return query.ToReader(1, topSize);
        }

        /// <summary>
        /// 返回首行首列值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public TResult ToScalar<TResult>()
        {
            return query.ToScalar<TResult>();
        }

        /// <summary>
        /// 返回首行首列值
        /// </summary>
        /// <returns></returns>
        public object ToScalar()
        {
            return query.ToScalar();
        }

        /// <summary>
        /// 返回首行首列值
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return query.Count();
        }

        #endregion
    }
}
