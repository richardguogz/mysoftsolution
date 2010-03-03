using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MySoft.Data.Design
{
    /// <summary>
    /// 
    /// </summary>
    public interface IField
    {
        string Name { get; }
        string OriginalName { get; }

        #region 条件操作

        WhereClip Like(string value);
        WhereClip Contains(string value);
        WhereClip Between(object leftValue, object rightValue);
        WhereClip In<T>(Field field) where T : Entity;
        WhereClip In<T>(Table table, Field field) where T : Entity;
        WhereClip In<T>(Field field, WhereClip where) where T : Entity;
        WhereClip In<T>(Table table, Field field, WhereClip where) where T : Entity;
        WhereClip In<T>(QuerySection<T> query) where T : Entity;
        WhereClip In<T>(TopSection<T> top) where T : Entity;

        WhereClip In(params object[] values);
        WhereClip In(QueryCreator creator);

        #endregion

        #region 字段操作

        Field Distinct();
        Field Count();
        Field Sum();
        Field Avg();
        Field Trim();
        Field Len();
        Field Max();
        Field Min();

        Field Left(int length);
        Field Right(int length);
        Field Substring(int startIndex, int endIndex);
        Field IndexOf(string subString);
        Field As(string aliasName);
        Field At(string tableName);
        Field At(Table table);

        Field Year { get; }
        Field Month { get; }
        Field Day { get; }

        #endregion
    }
}
