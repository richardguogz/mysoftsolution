using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace MySoft.Data
{
    /// <summary>
    /// 数据源接口
    /// </summary>
    public interface ISourceTable : IListConvert<IRowReader>, IListSource
    {
        #region 常用方法

        /// <summary>
        /// 获取数据行数
        /// </summary>
        int RowCount { get; }

        #endregion
    }
}