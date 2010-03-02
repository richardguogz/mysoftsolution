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
        /// ����ҳ��
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
        /// ���ؼ�¼��
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

        #region ����object

        /// <summary>
        /// ����һ��Object�б�
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public IArrayList<object> ToListResult(int pageIndex)
        {
            return query.GetListResult<object>(query, field, pageSize, pageSize * (pageIndex - 1));
        }

        /// <summary>
        /// ����һ��Object�б�
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
        /// ����һ��ʵ��
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
        /// ����һ���б�
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ISourceList<T> ToList(int pageIndex)
        {
            return ToTable(pageIndex).ConvertTo<T>();
        }

        /// <summary>
        /// ����һ��DataTable
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
