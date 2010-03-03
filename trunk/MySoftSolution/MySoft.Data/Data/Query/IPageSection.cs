using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace MySoft.Data
{
    interface IPageSection<T> : IMemoryPage<T>
    {
        ISourceReader ToReader(int pageIndex);
    }

    interface IMemoryPage<T>
    {
        int PageCount { get; }
        int RowCount { get; }

        T ToSingle(int pageIndex);

        IArrayList<object> ToListResult(int pageIndex);
        IArrayList<TResult> ToListResult<TResult>(int pageIndex);

        ISourceTable ToTable(int pageIndex);
        ISourceList<T> ToList(int pageIndex);
    }
}
