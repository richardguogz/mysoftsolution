using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace MySoft.Data
{
    interface ISqlSection
    {
        int Execute();
        T ToSingle<T>() where T : Entity;

        ISourceList<T> ToList<T>() where T : Entity;
        IArrayList<TResult> ToListResult<TResult>();

        ISourceReader ToReader();
        ISourceTable ToTable();
        DataSet ToDataSet();

        TResult ToScalar<TResult>();
        object ToScalar();
    }

    interface IProcSection : ISqlSection
    {
        int Execute(out IDictionary<string, object> outValues);
        T ToSingle<T>(out IDictionary<string, object> outValues) where T : Entity;

        ISourceList<T> ToList<T>(out IDictionary<string, object> outValues) where T : Entity;
        IArrayList<TResult> ToListResult<TResult>(out IDictionary<string, object> outValues);

        ISourceReader ToReader(out IDictionary<string, object> outValues);
        ISourceTable ToTable(out IDictionary<string, object> outValues);
        DataSet ToDataSet(out IDictionary<string, object> outValues);

        TResult ToScalar<TResult>(out IDictionary<string, object> outValues);
        object ToScalar(out IDictionary<string, object> outValues);
    }
}
