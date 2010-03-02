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
        /// 获取第一个实体
        /// </summary>
        /// <returns></returns>
        T First { get; }

        /// <summary>
        /// 获取最后一个实体
        /// </summary>
        T Last { get; }

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
        /// 返回MemoryFrom
        /// </summary>
        /// <returns></returns>
        MemoryFrom<T> ToMemory();

        /// <summary>
        /// 添加一个实体
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        new ISourceList<T> Add(T item);

        /// <summary>
        /// 添加一个实体
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        ISourceList<T> AddRange(IEnumerable<T> collection);

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
