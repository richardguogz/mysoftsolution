using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Data
{
    /// <summary>
    /// 可以输出Array的接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IArrayList<T> : IList<T>
    {
        /// <summary>
        /// 获取当前索引的对象
        /// </summary>
        /// <returns></returns>
        new T this[int index] { get; set; }

        /// <summary>
        /// 返回数组
        /// </summary>
        /// <returns></returns>
        T[] ToArray();
    }

    /// <summary>
    /// 数据源接口
    /// </summary>
    public interface ISourceList<T> : IListConvert<T>, IArrayList<T>
    {
        /// <summary>
        /// 转换成SourceTable
        /// </summary>
        /// <returns></returns>
        ISourceTable ToTable();

        /// <summary>
        /// 循环item
        /// </summary>
        /// <param name="action"></param>
        void ForEach(Action<T> action);

        /// <summary>
        /// 排序，可以使用内部的SortComparer类来实现多列排序
        /// </summary>
        /// <param name="comparer"></param>
        void Sort(IComparer<T> comparer);

        /// <summary>
        /// 查找符合条件的对象
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        ISourceList<T> FindAll(Predicate<T> match);

        #region 字典操作

        /// <summary>
        /// 返回字典
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="groupName"></param>
        /// <returns></returns>
        IDictionary<TResult, IList<T>> ToGroupList<TResult>(string groupName);

        /// <summary>
        /// 返回字典
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="groupField"></param>
        /// <returns></returns>
        IDictionary<TResult, IList<T>> ToGroupList<TResult>(Field groupField);

        #endregion
    }
}
