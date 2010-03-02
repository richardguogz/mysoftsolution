using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

namespace MySoft.Data
{
    /// <summary>
    /// 读数据接口
    /// </summary>
    public interface ISourceReader : IListConvert<IRowReader>, IRowReader, IDisposable
    {
        #region 常用方法

        /// <summary>
        /// 获取字段数
        /// </summary>
        int FieldCount { get; }

        /// <summary>
        /// 关闭DbReader
        /// </summary>
        void Close();

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        bool Read();

        #endregion
    }
}
