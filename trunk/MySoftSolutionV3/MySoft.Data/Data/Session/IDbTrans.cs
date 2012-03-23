using System.Collections.Generic;

namespace MySoft.Data
{
    interface IDbTrans : IDbProcess
    {
        #region Process����

        DbBatch BeginBatch();
        DbBatch BeginBatch(int batchSize);

        #endregion

        #region ���ò���

        bool Exists<T>(T entity) where T : Entity;
        bool Exists<T>(params object[] pkValues) where T : Entity;
        bool Exists<T>(WhereClip where) where T : Entity;
        T Single<T>(params object[] pkValues) where T : Entity;
        T Single<T>(WhereClip where) where T : Entity;
        int Count<T>(WhereClip where) where T : Entity;
        object Avg<T>(Field field, WhereClip where) where T : Entity;
        object Max<T>(Field field, WhereClip where) where T : Entity;
        object Min<T>(Field field, WhereClip where) where T : Entity;
        object Sum<T>(Field field, WhereClip where) where T : Entity;
        TResult Avg<T, TResult>(Field field, WhereClip where) where T : Entity;
        TResult Max<T, TResult>(Field field, WhereClip where) where T : Entity;
        TResult Min<T, TResult>(Field field, WhereClip where) where T : Entity;
        TResult Sum<T, TResult>(Field field, WhereClip where) where T : Entity;

        #endregion

        #region ���ò���(�ֱ���)

        bool Exists<T>(Table table, T entity) where T : Entity;
        bool Exists<T>(Table table, params object[] pkValues) where T : Entity;
        bool Exists<T>(Table table, WhereClip where) where T : Entity;
        T Single<T>(Table table, params object[] pkValues) where T : Entity;
        T Single<T>(Table table, WhereClip where) where T : Entity;
        int Count<T>(Table table, WhereClip where) where T : Entity;
        object Avg<T>(Table table, Field field, WhereClip where) where T : Entity;
        object Max<T>(Table table, Field field, WhereClip where) where T : Entity;
        object Min<T>(Table table, Field field, WhereClip where) where T : Entity;
        object Sum<T>(Table table, Field field, WhereClip where) where T : Entity;
        TResult Avg<T, TResult>(Table table, Field field, WhereClip where) where T : Entity;
        TResult Max<T, TResult>(Table table, Field field, WhereClip where) where T : Entity;
        TResult Min<T, TResult>(Table table, Field field, WhereClip where) where T : Entity;
        TResult Sum<T, TResult>(Table table, Field field, WhereClip where) where T : Entity;

        #endregion

        #region SQL����

        FromSection<T> From<T>() where T : Entity;
        FromSection<T> From<T>(Table table) where T : Entity;
        FromSection<T> From<T>(string aliasName) where T : Entity;
        QuerySection<T> From<T>(TableRelation<T> relation) where T : Entity;

        SqlSection FromSql(string sql, params SQLParameter[] parameters);
        ProcSection FromProc(string procName, params SQLParameter[] parameters);

        SqlSection FromSql(string sql, IDictionary<string, object> parameters);
        ProcSection FromProc(string procName, IDictionary<string, object> parameters);

        #endregion

        #region ����������

        int Execute<TResult>(InsertCreator creator, out TResult identityValue);
        int Execute(InsertCreator creator);
        int Execute(DeleteCreator creator);
        int Execute(UpdateCreator creator);

        QuerySection From(QueryCreator creator);

        #endregion

        #region ����������

        int Delete<T>(WhereClip where) where T : Entity;
        int Insert<T>(Field[] fields, object[] values) where T : Entity;
        int Insert<T, TResult>(Field[] fields, object[] values, out TResult retVal) where T : Entity;
        int Update<T>(Field field, object value, WhereClip where) where T : Entity;
        int Update<T>(Field[] fields, object[] values, WhereClip where) where T : Entity;

        int Delete<T>(Table table, WhereClip where) where T : Entity;
        int Insert<T>(Table table, Field[] fields, object[] values) where T : Entity;
        int Insert<T, TResult>(Table table, Field[] fields, object[] values, out TResult retVal) where T : Entity;
        int Update<T>(Table table, Field field, object value, WhereClip where) where T : Entity;
        int Update<T>(Table table, Field[] fields, object[] values, WhereClip where) where T : Entity;

        #endregion

        #region ֧��FieldValue��ʽ

        int Insert<T>(FieldValue[] fvs) where T : Entity;
        int Insert<T, TResult>(FieldValue[] fvs, out TResult retVal) where T : Entity;
        int Update<T>(FieldValue fv, WhereClip where) where T : Entity;
        int Update<T>(FieldValue[] fvs, WhereClip where) where T : Entity;

        int Insert<T>(Table table, FieldValue[] fvs) where T : Entity;
        int Insert<T, TResult>(Table table, FieldValue[] fvs, out TResult retVal) where T : Entity;
        int Update<T>(Table table, FieldValue fv, WhereClip where) where T : Entity;
        int Update<T>(Table table, FieldValue[] fvs, WhereClip where) where T : Entity;

        #endregion

        #region ����ʱ���ر�ʶ��

        int Insert<T, TResult>(T entity, out TResult retVal, params FieldValue[] fvs) where T : Entity;
        int Insert<T, TResult>(Table table, T entity, out TResult retVal, params FieldValue[] fvs) where T : Entity;

        #endregion
    }
}
