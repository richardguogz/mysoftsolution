using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MySoft.Data
{
    public class MemoryQuery<T> : IMemoryQuery<T>
    {
        private DataTable dt;
        private string filter;
        private string sort;

        #region 初始化MemoryQuery

        /// <summary>
        /// 传入一个DataTable
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageIndex"></param>
        internal MemoryQuery(DataTable dt)
        {
            this.dt = dt;
        }

        #endregion

        #region IMemoryQuery<T> 成员

        /// <summary>
        /// 进行条件操作
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public MemoryQuery<T> Where(WhereClip where)
        {
            if (!DataUtils.IsNullOrEmpty(where))
            {
                string strWhere = where.ToString();
                foreach (SQLParameter p in where.Parameters)
                {
                    strWhere = strWhere.Replace(p.Name, DataUtils.FormatValue(p.Value));
                }

                filter = DataUtils.RemoveTableAliasNamePrefixes(strWhere);
            }
            return this;
        }

        /// <summary>
        /// 进行排序操作
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public MemoryQuery<T> OrderBy(OrderByClip order)
        {
            if (!DataUtils.IsNullOrEmpty(order))
            {
                sort = DataUtils.RemoveTableAliasNamePrefixes(order.ToString());
            }
            return this;
        }

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public T ToSingle()
        {
            DataTable dt = GetDataTable(this, 1, 0);
            IList<T> list = new SourceTable(dt).ConvertTo<T>();
            if (list.Count > 0)
            {
                return list[0];
            }
            return default(T);
        }

        /// <summary>
        /// 返回IArrayList
        /// </summary>
        /// <returns></returns>
        public ISourceList<T> ToList()
        {
            return ToTable().ConvertTo<T>();
        }

        /// <summary>
        /// 返回IArrayList
        /// </summary>
        /// <returns></returns>
        public ISourceList<T> ToList(int topSize)
        {
            return ToTable(topSize).ConvertTo<T>();
        }

        /// <summary>
        /// 返回IArrayList
        /// </summary>
        /// <returns></returns>
        public ISourceList<T> ToList(int startIndex, int endIndex)
        {
            return ToTable(startIndex, endIndex).ConvertTo<T>();
        }

        /// <summary>
        /// 返回一个数据表
        /// </summary>
        /// <param name="pkValues"></param>
        /// <returns></returns>
        public ISourceTable ToTable()
        {
            DataTable dt = GetDataTable(this);
            return new SourceTable(dt);
        }

        /// <summary>
        /// 返回一个数据表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public ISourceTable ToTable(int topSize)
        {
            DataTable dt = GetDataTable(this, topSize, 0);
            return new SourceTable(dt);
        }

        /// <summary>
        /// 返回一个数据表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public ISourceTable ToTable(int startIndex, int endIndex)
        {
            if (startIndex <= 0) startIndex = 1;
            int topItem = endIndex - startIndex + 1;
            DataTable dt = GetDataTable(this, topItem, endIndex - topItem);
            return new SourceTable(dt);
        }

        #region 计算操作

        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Count(Field field)
        {
            try
            {
                if (filter == null)
                {
                    return dt.Rows.Count;
                }

                object obj = FindScalar(field.Count());
                return DataUtils.ConvertValue<int>(obj);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 返回平均值
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Avg(Field field)
        {
            return FindScalar(field.Avg());
        }

        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Max(Field field)
        {
            return FindScalar(field.Max());
        }

        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Min(Field field)
        {
            return FindScalar(field.Min());
        }

        /// <summary>
        /// 返回求和值
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Sum(Field field)
        {
            return FindScalar(field.Sum());
        }

        /// <summary>
        /// 返回平均值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public TResult Avg<TResult>(Field field)
        {
            object obj = Avg(field);
            return DataUtils.ConvertValue<TResult>(obj);
        }

        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public TResult Max<TResult>(Field field)
        {
            object obj = Max(field);
            return DataUtils.ConvertValue<TResult>(obj);
        }

        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public TResult Min<TResult>(Field field)
        {
            object obj = Min(field);
            return DataUtils.ConvertValue<TResult>(obj);
        }

        /// <summary>
        /// 返回求和值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public TResult Sum<TResult>(Field field)
        {
            object obj = Sum(field);
            return DataUtils.ConvertValue<TResult>(obj);
        }

        #endregion

        #region 返回Object列表

        /// <summary>
        ///  返回object列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public IArrayList<object> ToListResult(Field field)
        {
            return GetListResult<object>(this, field);
        }

        /// <summary>
        ///  返回object列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public IArrayList<object> ToListResult(Field field, int topSize)
        {
            return GetListResult<object>(this, field, topSize, 0);
        }

        /// <summary>
        /// 返回object列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public IArrayList<object> ToListResult(Field field, int startIndex, int endIndex)
        {
            if (startIndex <= 0) startIndex = 1;
            int topItem = endIndex - startIndex + 1;
            return GetListResult<object>(this, field, topItem, endIndex - topItem);
        }

        /// <summary>
        ///  返回object列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public IArrayList<TResult> ToListResult<TResult>(Field field)
        {
            return GetListResult<TResult>(this, field);
        }

        /// <summary>
        ///  返回object列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public IArrayList<TResult> ToListResult<TResult>(Field field, int topSize)
        {
            return GetListResult<TResult>(this, field, topSize, 0);
        }

        /// <summary>
        /// 返回object列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public IArrayList<TResult> ToListResult<TResult>(Field field, int startIndex, int endIndex)
        {
            if (startIndex <= 0) startIndex = 1;
            int topItem = endIndex - startIndex + 1;
            return GetListResult<TResult>(this, field, topItem, endIndex - topItem);
        }


        #endregion

        #region 私有方法

        /// <summary>
        /// 返回表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        internal DataTable GetDataTable(MemoryQuery<T> query)
        {
            try
            {
                DataTable q_dt = query.dt;
                string q_filter = query.filter;
                string q_sort = query.sort;

                DataTable table = new DataTable();
                table.TableName = typeof(T).Name;
                foreach (DataColumn column in q_dt.Columns)
                {
                    table.Columns.Add(column.ColumnName, column.DataType);
                }

                if (q_dt.Rows.Count == 0) return table;

                DataRow[] dtRows = q_dt.Select(q_filter, q_sort);

                foreach (DataRow row in dtRows)
                {
                    table.ImportRow(row);
                }

                return table;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 返回表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="itemCount"></param>
        /// <param name="skipCount"></param>
        /// <returns></returns>
        internal DataTable GetDataTable(MemoryQuery<T> query, int itemCount, int skipCount)
        {
            try
            {
                DataTable q_dt = query.dt;
                string q_filter = query.filter;
                string q_sort = query.sort;

                DataTable table = new DataTable();
                table.TableName = typeof(T).Name;
                foreach (DataColumn column in q_dt.Columns)
                {
                    table.Columns.Add(column.ColumnName, column.DataType);
                }

                if (q_dt.Rows.Count == 0) return table;

                DataRow[] dtRows = q_dt.Select(q_filter, q_sort);

                int index = 0;
                foreach (DataRow row in dtRows)
                {
                    if (index >= skipCount)
                    {
                        table.ImportRow(row);
                    }
                    index++;
                    if (table.Rows.Count == itemCount) break;
                }
                return table;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 返回列表
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        private IArrayList<TResult> GetListResult<TResult>(MemoryQuery<T> query, Field field)
        {
            try
            {
                DataTable q_dt = query.dt;
                string q_filter = query.filter;
                string q_sort = query.sort;

                if (q_dt.Rows.Count == 0) return new ArrayList<TResult>();

                DataRow[] dtRows = q_dt.Select(q_filter, q_sort);
                IArrayList<TResult> list = new ArrayList<TResult>();

                foreach (DataRow row in dtRows)
                {
                    list.Add(DataUtils.ConvertValue<TResult>(row[field.OriginalName]));
                }
                return list;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 返回列表
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        internal IArrayList<TResult> GetListResult<TResult>(MemoryQuery<T> query, Field field, int itemCount, int skipCount)
        {
            try
            {
                DataTable q_dt = query.dt;
                string q_filter = query.filter;
                string q_sort = query.sort;

                if (q_dt.Rows.Count == 0) return new ArrayList<TResult>();

                DataRow[] dtRows = q_dt.Select(q_filter, q_sort);
                IArrayList<TResult> list = new ArrayList<TResult>();

                int index = 0;
                foreach (DataRow row in dtRows)
                {
                    if (index >= skipCount)
                    {
                        list.Add(DataUtils.ConvertValue<TResult>(row[field.OriginalName]));
                    }
                    index++;
                    if (list.Count == itemCount) break;
                }
                return list;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 返回单行单列值
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        private object FindScalar(Field field)
        {
            try
            {
                if (dt.Rows.Count == 0) return null;

                string column = DataUtils.RemoveTableAliasNamePrefixes(field.Name);
                if (column.Contains("distinct"))
                {
                    DataView dv = new DataView(dt, filter, null, DataViewRowState.OriginalRows);
                    column = new System.Text.RegularExpressions.Regex(@"\S+distinct\(([\w]+)\S+").Replace(column, "$1");
                    return dv.ToTable(true, column).Rows.Count;
                }
                else
                {
                    return dt.Compute(column, filter);
                }
            }
            catch
            {
                return null;
            }
        }

        #endregion

        /// <summary>
        /// 返回分页MemoryQuery
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public MemoryPage<T> GetPage(Field field, int pageSize)
        {
            return new MemoryPage<T>(this, field, pageSize);
        }

        #region 返回分页信息

        /// <summary>
        /// 直接返回DataPage分页信息
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IDataPage<IList<T>> ToListPage(Field field, int pageSize, int pageIndex)
        {
            IDataPage<IList<T>> view = new DataPage<IList<T>>(pageSize);
            MemoryPage<T> page = GetPage(field, pageSize);
            view.CurrentPageIndex = pageIndex;
            view.RowCount = page.RowCount;
            view.DataSource = page.ToList(pageIndex);
            return view;
        }

        /// <summary>
        /// 直接返回DataPage分页信息
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IDataPage<DataTable> ToTablePage(Field field, int pageSize, int pageIndex)
        {
            IDataPage<DataTable> view = new DataPage<DataTable>(pageSize);
            MemoryPage<T> page = GetPage(field, pageSize);
            view.CurrentPageIndex = pageIndex;
            view.RowCount = page.RowCount;
            view.DataSource = page.ToTable(pageIndex) as SourceTable;
            return view;
        }

        #endregion

        #endregion
    }
}
