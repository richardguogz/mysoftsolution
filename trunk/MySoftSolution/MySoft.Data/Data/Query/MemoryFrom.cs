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
    public class MemoryFrom<T> : IMemoryFrom<T>
    {
        private MemoryQuery<T> query;

        #region 初始化MemoryQuery

        /// <summary>
        /// 传入一个DataTable
        /// </summary>
        /// <param name="dt"></param>
        public MemoryFrom(DataTable dt)
        {
            this.query = new MemoryQuery<T>(dt);
        }

        /// <summary>
        /// 传入一个List
        /// </summary>
        /// <param name="list"></param>
        public MemoryFrom(IList<T> list)
        {
            DataTable dt = new SourceList<T>(list).ToTable() as DataTable;
            this.query = new MemoryQuery<T>(dt);
        }

        #endregion

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public T Single(WhereClip where)
        {
            return query.Where(where).ToSingle();
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool Exists(WhereClip where)
        {
            return Single(where) != null;
        }

        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Count(Field field, WhereClip where)
        {
            return query.Where(where).Count(field);
        }

        #region IMemoryQuery<T> 成员

        /// <summary>
        /// 进行条件操作
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public MemoryQuery<T> Where(WhereClip where)
        {
            return query.Where(where);
        }

        /// <summary>
        /// 进行排序操作
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public MemoryQuery<T> OrderBy(OrderByClip order)
        {
            return query.OrderBy(order);
        }

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public T ToSingle()
        {
            return query.ToSingle();
        }

        /// <summary>
        /// 返回IArrayList
        /// </summary>
        /// <returns></returns>
        public ISourceList<T> ToList()
        {
            return query.ToList();
        }

        /// <summary>
        /// 返回IArrayList
        /// </summary>
        /// <returns></returns>
        public ISourceList<T> ToList(int topSize)
        {
            return query.ToList(topSize);
        }

        /// <summary>
        /// 返回IArrayList
        /// </summary>
        /// <returns></returns>
        public ISourceList<T> ToList(int startIndex, int endIndex)
        {
            return query.ToList(startIndex, endIndex);
        }

        /// <summary>
        /// 获取一个数据表
        /// </summary>
        /// <param name="pkValues"></param>
        /// <returns></returns>
        public ISourceTable ToTable()
        {
            return query.ToTable();
        }

        /// <summary>
        /// 获取一个数据表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public ISourceTable ToTable(int topSize)
        {
            return query.ToTable(topSize);
        }

        /// <summary>
        /// 获取一个数据表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public ISourceTable ToTable(int startIndex, int endIndex)
        {
            return query.ToTable(startIndex, endIndex);
        }

        #region 计算操作

        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Count(Field field)
        {
            return query.Count(field);
        }

        /// <summary>
        /// 返回平均值
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Avg(Field field)
        {
            return query.Avg(field);
        }

        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Max(Field field)
        {
            return query.Max(field);
        }

        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Min(Field field)
        {
            return query.Min(field);
        }

        /// <summary>
        /// 返回求和值
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Sum(Field field)
        {
            return query.Sum(field);
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
            return query.Avg<TResult>(field);
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
            return query.Max<TResult>(field);
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
            return query.Min<TResult>(field);
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
            return query.Sum<TResult>(field);
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
            return query.ToListResult(field);
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
            return query.ToListResult(field, topSize);
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
            return query.ToListResult(field, startIndex, endIndex);
        }

        /// <summary>
        ///  返回object列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public IArrayList<TResult> ToListResult<TResult>(Field field)
        {
            return query.ToListResult<TResult>(field);
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
            return query.ToListResult<TResult>(field, topSize);
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
            return query.ToListResult<TResult>(field, startIndex, endIndex);
        }


        /// <summary>
        /// 返回分页MemoryQuery
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public MemoryPage<T> GetPage(Field field, int pageSize)
        {
            return query.GetPage(field, pageSize);
        }

        #endregion

        #region 返回分页信息

        /// <summary>
        /// 直接返回DataPage分页信息
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IDataPage<IList<T>> ToListPage(Field field, int pageSize, int pageIndex)
        {
            return query.ToListPage(field, pageSize, pageIndex);
        }

        /// <summary>
        /// 直接返回DataPage分页信息
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IDataPage<DataTable> ToTablePage(Field field, int pageSize, int pageIndex)
        {
            return query.ToTablePage(field, pageSize, pageIndex);
        }

        #endregion

        #endregion
    }
}
