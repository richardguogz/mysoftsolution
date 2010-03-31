using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MySoft.Data
{
    interface IUserQuery
    {
        QuerySection SetPagingField(string pagingFieldName);
        QuerySection SetPagingField(Field pagingField);

        PageSection GetPage(int pageSize);
        QuerySection<T> ToQuery<T>() where T : Entity;

        object ToScalar();
        TResult ToScalar<TResult>();
        int Count();
        bool Exists();

        T ToSingle<T>() where T : Entity;

        ISourceReader ToReader();
        ISourceReader ToReader(int topSize);

        ISourceList<T> ToList<T>() where T : Entity;
        ISourceList<T> ToList<T>(int topSize) where T : Entity;

        ISourceTable ToTable();
        ISourceTable ToTable(int topSize);

        IDataPage<IList<T>> ToListPage<T>(int pageSize, int pageIndex) where T : Entity;
        IDataPage<DataTable> ToTablePage(int pageSize, int pageIndex);
    }

    interface IPageQuery
    {
        int PageCount { get; }
        int RowCount { get; }

        ISourceReader ToReader(int pageIndex);
        ISourceList<T> ToList<T>(int pageIndex) where T : Entity;
        ISourceTable ToTable(int pageIndex);
    }
}
