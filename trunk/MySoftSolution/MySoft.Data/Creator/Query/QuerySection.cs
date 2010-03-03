using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MySoft.Data
{
    /// <summary>
    /// 查询器
    /// </summary>
    public class QuerySection : IUserQuery
    {
        private QuerySection<TempEntity> query;
        internal QuerySection(QuerySection<TempEntity> query)
        {
            this.query = query;
        }

        /// <summary>
        /// 设置分页字段
        /// </summary>
        /// <param name="pagingFieldName"></param>
        /// <returns></returns>
        public QuerySection SetPagingField(string pagingFieldName)
        {
            return SetPagingField(new Field(pagingFieldName));
        }

        /// <summary>
        /// 设置分页字段
        /// </summary>
        /// <param name="pagingField"></param>
        /// <returns></returns>
        public QuerySection SetPagingField(Field pagingField)
        {
            query.SetPagingField(pagingField);
            return this;
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PageSection GetPage(int pageSize)
        {
            return new PageSection(query, pageSize);
        }

        /// <summary>
        /// 返回一个查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public QuerySection<T> ToQuery<T>()
            where T : Entity
        {
            return query.CreateQuery<T>();
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
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public TResult ToScalar<TResult>()
        {
            return query.ToScalar<TResult>();
        }

        /// <summary>
        /// 返回记录数
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return query.Count();
        }

        /// <summary>
        /// 记录是否存在
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {
            return query.Count() > 0;
        }

        /// <summary>
        /// 返回T
        /// </summary>
        /// <returns></returns>
        public T ToSingle<T>()
             where T : Entity
        {
            return query.ToSingle<T>();
        }

        /// <summary>
        /// 返回IList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ISourceList<T> ToList<T>()
            where T : Entity
        {
            return query.ToList<T>();
        }

        /// <summary>
        /// 返回ISourceReader
        /// </summary>
        /// <returns></returns>
        public ISourceReader ToReader()
        {
            return query.ToReader();
        }

        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <returns></returns>
        public ISourceTable ToTable()
        {
            return query.ToTable();
        }

        /// <summary>
        /// 返回ISourceReader
        /// </summary>
        /// <returns></returns>
        public ISourceReader ToReader(int topSize)
        {
            return query.GetTop(topSize).ToReader();
        }

        /// <summary>
        /// 返回IList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ISourceList<T> ToList<T>(int topSize)
            where T : Entity
        {
            return query.GetTop(topSize).ToList<T>();
        }

        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <returns></returns>
        public ISourceTable ToTable(int topSize)
        {
            return query.GetTop(topSize).ToTable();
        }

        /// <summary>
        /// 返回IDataView
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public IDataPage<IList<T>> ToListPage<T>(int pageSize, int pageIndex)
             where T : Entity
        {
            IDataPage<IList<T>> view = new DataPage<IList<T>>(pageSize);
            PageSection page = GetPage(pageSize);
            view.CurrentPageIndex = pageIndex;
            view.RowCount = page.RowCount;
            view.DataSource = page.ToList<T>(pageIndex);
            return view;
        }

        /// <summary>
        /// 返回IDataView
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public IDataPage<DataTable> ToTablePage(int pageSize, int pageIndex)
        {
            IDataPage<DataTable> view = new DataPage<DataTable>(pageSize);
            PageSection page = GetPage(pageSize);
            view.CurrentPageIndex = pageIndex;
            view.RowCount = page.RowCount;
            view.DataSource = page.ToTable(pageIndex) as SourceTable;
            return view;
        }
    }
}
