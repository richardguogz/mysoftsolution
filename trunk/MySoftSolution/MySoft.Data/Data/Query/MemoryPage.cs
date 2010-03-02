using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MySoft.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class MemoryPage<T> : IMemoryPage<T>
    {
        private MemoryQuery<T> query;
        private Field field;
        private int? rowCount;
        private int pageSize;
        internal MemoryPage(MemoryQuery<T> query, Field field, int pageSize)
        {
            this.pageSize = pageSize;
            this.query = query;
            this.field = field;
        }

        /// <summary>
        /// 返回页数
        /// </summary>
        public int PageCount
        {
            get
            {
                if (rowCount == null)
                {
                    rowCount = query.Count(field);
                }
                return Convert.ToInt32(Math.Ceiling(1.0 * rowCount.Value / pageSize));
            }
        }

        /// <summary>
        /// 返回记录数
        /// </summary>
        public int RowCount
        {
            get
            {
                if (rowCount == null)
                {
                    rowCount = query.Count(field);
                }
                return rowCount.Value;
            }
        }

        #region 返回object

        /// <summary>
        /// 返回一个Object列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public IArrayList<object> ToListResult(int pageIndex)
        {
            return query.GetListResult<object>(query, field, pageSize, pageSize * (pageIndex - 1));
        }

        /// <summary>
        /// 返回一个Object列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public IArrayList<TResult> ToListResult<TResult>(int pageIndex)
        {
            return query.GetListResult<TResult>(query, field, pageSize, pageSize * (pageIndex - 1));
        }

        #endregion

        /// <summary>
        /// 返回一个实体
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public T ToSingle(int pageIndex)
        {
            IList<T> list = ToList(pageIndex);
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
        /// 返回一个列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ISourceList<T> ToList(int pageIndex)
        {
            return ToTable(pageIndex).ConvertTo<T>();
        }

        /// <summary>
        /// 返回一个DataTable
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ISourceTable ToTable(int pageIndex)
        {
            DataTable dt = query.GetDataTable(query, pageSize, pageSize * (pageIndex - 1));
            return new SourceTable(dt);
        }
    }
}
