using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using MySoft.Logger;
using MySoft.Cache;

namespace MySoft.Data
{
    /// <summary>
    /// �Ự��������
    /// </summary>
    public partial class DbSession : IDbSession
    {
        /// <summary>
        /// Ĭ�ϻỰ����
        /// </summary>
        public static DbSession Default;
        private DbProvider dbProvider;
        private DbTrans dbTrans;

        #region ��ʼ��Session

        static DbSession()
        {
            if (Default == null)
            {
                try
                {
                    Default = new DbSession(DbProviderFactory.Default);
                }
                catch { }
            }
        }

        /// <summary>
        ///  ָ�����ƽ���ʵ����һ��Session�Ự
        /// </summary>
        /// <param name="connectName"></param>
        public DbSession(string connectName)
            : this(DbProviderFactory.CreateDbProvider(connectName))
        { }

        /// <summary>
        /// ָ������ʵ����һ��Session�Ự
        /// </summary>
        /// <param name="dbProvider"></param>
        public DbSession(DbProvider dbProvider)
        {
            try
            {
                InitSession(dbProvider);
            }
            catch
            {
                throw new DataException("��ʼ��DbSessionʧ�ܣ����������Ƿ���ȷ��");
            }
        }

        /// <summary>
        /// ����ָ�����ƽ���Session�ỰΪĬ�ϻỰ
        /// </summary>
        /// <param name="connectName"></param>
        public static void SetDefault(string connectName)
        {
            Default = new DbSession(connectName);
        }

        /// <summary>
        /// ����ָ������Session�ỰΪĬ�ϻỰ
        /// </summary>
        /// <param name="dbProvider"></param>
        public static void SetDefault(DbProvider dbProvider)
        {
            Default = new DbSession(dbProvider);
        }

        #endregion

        #region ʵ��IDbSession

        /// <summary>
        /// �����µ�����
        /// </summary>
        /// <param name="connectName"></param>
        public void SetProvider(string connectName)
        {
            SetProvider(DbProviderFactory.CreateDbProvider(connectName));
        }

        /// <summary>
        /// �����µ�����
        /// </summary>
        /// <param name="dbProvider"></param>
        public void SetProvider(DbProvider dbProvider)
        {
            InitSession(dbProvider);
        }

        /// <summary>
        /// ��ʼһ������
        /// </summary>
        /// <returns></returns>
        public DbTrans BeginTrans()
        {
            return new DbTrans(dbProvider, true);
        }

        /// <summary>
        /// ��ʼһ������
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        public DbTrans BeginTrans(IsolationLevel isolationLevel)
        {
            return new DbTrans(dbProvider, isolationLevel);
        }

        /// <summary>
        /// ����һ���ⲿ����
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public DbTrans SetTransaction(DbTransaction trans)
        {
            return new DbTrans(dbProvider, trans);
        }

        /// <summary>
        /// ��ʼһ���ⲿ����
        /// </summary>
        /// <returns></returns>
        public DbTransaction BeginTransaction()
        {
            return BeginTrans().Transaction;
        }

        /// <summary>
        /// ��ʼһ���ⲿ����
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        public DbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return BeginTrans(isolationLevel).Transaction;
        }

        /// <summary>
        /// ����һ���ⲿ����
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public DbTrans SetConnection(DbConnection connection)
        {
            return new DbTrans(dbProvider, connection);
        }

        /// <summary>
        /// ����һ���ⲿ����
        /// </summary>
        /// <returns></returns>
        public DbConnection CreateConnection()
        {
            return dbProvider.CreateConnection();
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public DbParameter CreateParameter()
        {
            return dbProvider.CreateParameter();
        }

        /// <summary>
        /// ����ConnectionString
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        protected virtual string Decrypt(string connectionString)
        {
            //��ӽ��ܵķ�ʽ
            return connectionString;
        }

        #region ע��Log

        /// <summary>
        /// ע��һ����־�¼�
        /// </summary>
        /// <param name="handler"></param>
        public void RegisterSqlLogger(LogEventHandler handler)
        {
            dbProvider.OnLog += handler;
        }

        /// <summary>
        /// ȡ��һ����־�¼�
        /// </summary>
        /// <param name="handler"></param>
        public void UnregisterSqlLogger(LogEventHandler handler)
        {
            dbProvider.OnLog -= handler;
        }

        /// <summary>
        /// ע��һ���쳣��־�¼�
        /// </summary>
        /// <param name="handler"></param>
        public void RegisterSqlExceptionLogger(ErrorLogEventHandler handler)
        {
            dbProvider.OnError += handler;
        }

        /// <summary>
        /// ȡ��һ���쳣��־�¼�
        /// </summary>
        /// <param name="handler"></param>
        public void UnregisterSqlExceptionLogger(ErrorLogEventHandler handler)
        {
            dbProvider.OnError -= handler;
        }

        /// <summary>
        /// ע��ִ������ǰ���¼�
        /// </summary>
        /// <param name="handler"></param>
        public void RegisterOnStartHandler(ExcutingEventHandler handler)
        {
            dbProvider.OnStart += handler;
        }

        /// <summary>
        /// ȡ��ִ������ǰ���¼�
        /// </summary>
        /// <param name="handler"></param>
        public void UnregisterOnStartHandler(ExcutingEventHandler handler)
        {
            dbProvider.OnStart -= handler;
        }

        /// <summary>
        /// ע��ִ���������¼�
        /// </summary>
        /// <param name="handler"></param>
        public void RegisterOnEndHandler(ExcutingEventHandler handler)
        {
            dbProvider.OnEnd += handler;
        }

        /// <summary>
        /// ȡ��ִ���������¼�
        /// </summary>
        /// <param name="handler"></param>
        public void UnregisterOnEndHandler(ExcutingEventHandler handler)
        {
            dbProvider.OnEnd -= handler;
        }

        #endregion

        #region ���ò���(ָ������)

        /// <summary>
        /// ��������ȡһ��ʵ��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pkValues"></param>
        /// <returns></returns>
        public T Single<T>(Table table, params object[] pkValues)
            where T : Entity
        {
            return dbTrans.Single<T>(table, pkValues);
        }

        /// <summary>
        /// ��������ȡһ��ʵ��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public T Single<T>(Table table, WhereClip where)
            where T : Entity
        {
            return dbTrans.Single<T>(table, where);
        }

        /// <summary>
        /// �Ƿ����ָ����ʵ�壬������ƥ��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Exists<T>(Table table, T entity)
            where T : Entity
        {
            return dbTrans.Exists<T>(table, entity);
        }

        /// <summary>
        /// �Ƿ����ָ�������ļ�¼
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pkValues"></param>
        /// <returns></returns>
        public bool Exists<T>(Table table, params object[] pkValues)
            where T : Entity
        {
            return dbTrans.Exists<T>(table, pkValues);
        }

        /// <summary>
        /// �Ƿ����ָ�������ļ�¼
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool Exists<T>(Table table, WhereClip where)
            where T : Entity
        {
            return dbTrans.Exists<T>(table, where);
        }

        /// <summary>
        /// ��������ȡ��¼����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Count<T>(Table table, WhereClip where)
            where T : Entity
        {
            return dbTrans.Count<T>(table, where);
        }

        /// <summary>
        /// ����������Sum����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Sum<T>(Table table, Field field, WhereClip where)
            where T : Entity
        {
            return dbTrans.Sum<T>(table, field, where);
        }

        /// <summary>
        /// ����������Avg����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Avg<T>(Table table, Field field, WhereClip where)
            where T : Entity
        {
            return dbTrans.Avg<T>(table, field, where);
        }

        /// <summary>
        /// ����������Max����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Max<T>(Table table, Field field, WhereClip where)
            where T : Entity
        {
            return dbTrans.Max<T>(table, field, where);
        }

        /// <summary>
        /// ����������Min����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Min<T>(Table table, Field field, WhereClip where)
            where T : Entity
        {
            return dbTrans.Min<T>(table, field, where);
        }

        #region ������Ӧ������

        /// <summary>
        /// ����������Sum����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public TResult Sum<T, TResult>(Table table, Field field, WhereClip where)
                    where T : Entity
        {
            return dbTrans.Sum<T, TResult>(table, field, where);
        }

        /// <summary>
        /// ����������Avg����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public TResult Avg<T, TResult>(Table table, Field field, WhereClip where)
            where T : Entity
        {
            return dbTrans.Avg<T, TResult>(table, field, where);
        }

        /// <summary>
        /// ����������Max����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public TResult Max<T, TResult>(Table table, Field field, WhereClip where)
            where T : Entity
        {
            return dbTrans.Max<T, TResult>(table, field, where);
        }

        /// <summary>
        /// ����������Min����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public TResult Min<T, TResult>(Table table, Field field, WhereClip where)
            where T : Entity
        {
            return dbTrans.Min<T, TResult>(table, field, where);
        }

        #endregion

        #endregion

        #region ���ò���

        /// <summary>
        /// ��������ȡһ��ʵ��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pkValues"></param>
        /// <returns></returns>
        public T Single<T>(params object[] pkValues)
            where T : Entity
        {
            return dbTrans.Single<T>(pkValues);
        }

        /// <summary>
        /// ��������ȡһ��ʵ��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public T Single<T>(WhereClip where)
            where T : Entity
        {
            return dbTrans.Single<T>(where);
        }

        /// <summary>
        /// �Ƿ����ָ����ʵ�壬������ƥ��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Exists<T>(T entity)
            where T : Entity
        {
            return dbTrans.Exists<T>(entity);
        }

        /// <summary>
        /// �Ƿ����ָ�������ļ�¼
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pkValues"></param>
        /// <returns></returns>
        public bool Exists<T>(params object[] pkValues)
            where T : Entity
        {
            return dbTrans.Exists<T>(pkValues);
        }

        /// <summary>
        /// �Ƿ����ָ�������ļ�¼
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool Exists<T>(WhereClip where)
            where T : Entity
        {
            return dbTrans.Exists<T>(where);
        }

        /// <summary>
        /// ��������ȡ��¼����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Count<T>(WhereClip where)
            where T : Entity
        {
            return dbTrans.Count<T>(where);
        }

        /// <summary>
        /// ����������Sum����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Sum<T>(Field field, WhereClip where)
            where T : Entity
        {
            return dbTrans.Sum<T>(field, where);
        }

        /// <summary>
        /// ����������Avg����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Avg<T>(Field field, WhereClip where)
            where T : Entity
        {
            return dbTrans.Avg<T>(field, where);
        }

        /// <summary>
        /// ����������Max����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Max<T>(Field field, WhereClip where)
            where T : Entity
        {
            return dbTrans.Max<T>(field, where);
        }

        /// <summary>
        /// ����������Min����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Min<T>(Field field, WhereClip where)
            where T : Entity
        {
            return dbTrans.Min<T>(field, where);
        }

        #region ������Ӧ������

        /// <summary>
        /// ����������Sum����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public TResult Sum<T, TResult>(Field field, WhereClip where)
            where T : Entity
        {
            return dbTrans.Sum<T, TResult>(field, where);
        }

        /// <summary>
        /// ����������Avg����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public TResult Avg<T, TResult>(Field field, WhereClip where)
            where T : Entity
        {
            return dbTrans.Avg<T, TResult>(field, where);
        }

        /// <summary>
        /// ����������Max����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public TResult Max<T, TResult>(Field field, WhereClip where)
            where T : Entity
        {
            return dbTrans.Max<T, TResult>(field, where);
        }

        /// <summary>
        /// ����������Min����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public TResult Min<T, TResult>(Field field, WhereClip where)
            where T : Entity
        {
            return dbTrans.Min<T, TResult>(field, where);
        }

        #endregion

        #endregion

        #region �������Ӳ���

        /// <summary>
        /// ����һ��From��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public FromSection<T> From<T>()
            where T : Entity
        {
            return dbTrans.From<T>();
        }

        /// <summary>
        /// ����һ��From��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public FromSection<T> From<T>(Table table)
            where T : Entity
        {
            return dbTrans.From<T>(table);
        }

        /// <summary>
        /// ����һ��From�ڣ�����ָ�������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aliasName"></param>
        /// <returns></returns>
        public FromSection<T> From<T>(string aliasName)
            where T : Entity
        {
            return dbTrans.From<T>(aliasName);
        }

        /// <summary>
        /// ����һ��From��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public FromSection<T> From<T>(TableRelation<T> relation)
            where T : Entity
        {
            return dbTrans.From<T>(relation);
        }

        /// <summary>
        /// ����һ��Sql��
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public SqlSection FromSql(string sql, params SQLParameter[] parameters)
        {
            return dbTrans.FromSql(sql, parameters);
        }

        /// <summary>
        /// ����һ��Proc��
        /// </summary>
        /// <param name="procName"></param>
        /// <returns></returns>
        public ProcSection FromProc(string procName, params SQLParameter[] parameters)
        {
            return dbTrans.FromProc(procName, parameters);
        }

        /// <summary>
        /// ����һ��Sql��
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public SqlSection FromSql(string sql, IDictionary<string, object> parameters)
        {
            return dbTrans.FromSql(sql, parameters);
        }

        /// <summary>
        /// ����һ��Proc��
        /// </summary>
        /// <param name="procName"></param>
        /// <returns></returns>
        public ProcSection FromProc(string procName, IDictionary<string, object> parameters)
        {
            return dbTrans.FromProc(procName, parameters);
        }

        #endregion

        #region ʹ�ô���������

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="creator"></param>
        /// <returns></returns>
        public int Excute(InsertCreator creator)
        {
            return dbTrans.Excute(creator);
        }

        /// <summary>
        ///  ��������
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="creator"></param>
        /// <param name="identityValue"></param>
        /// <returns></returns>
        public int Excute<TResult>(InsertCreator creator, out TResult identityValue)
        {
            return dbTrans.Excute(creator, out identityValue);
        }

        /// <summary>
        /// ������ɾ��ָ����¼
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="creator"></param>
        /// <returns></returns>
        public int Excute(DeleteCreator creator)
        {
            return dbTrans.Excute(creator);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="creator"></param>
        /// <returns></returns>
        public int Excute(UpdateCreator creator)
        {
            return dbTrans.Excute(creator);
        }

        /// <summary>
        ///  ����һ��Query��
        /// </summary>
        /// <param name="creator"></param>
        /// <returns></returns>
        public QuerySection From(QueryCreator creator)
        {
            return dbTrans.From(creator);
        }

        #endregion

        #region ��ɾ�Ĳ���

        /// <summary>
        /// ����һ��Batch
        /// </summary>
        /// <returns></returns>
        public DbBatch BeginBatch()
        {
            return dbTrans.BeginBatch();
        }

        /// <summary>
        /// ����һ��Batch
        /// </summary>
        /// <returns></returns>
        public DbBatch BeginBatch(int batchSize)
        {
            return dbTrans.BeginBatch(batchSize);
        }

        /// <summary>
        /// ����һ��ʵ��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Save<T>(Table table, T entity)
             where T : Entity
        {
            return dbTrans.Save<T>(table, entity);
        }

        /// <summary>
        ///  ����һ��ʵ��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public int Insert<T>(Table table, Field[] fields, object[] values)
            where T : Entity
        {
            return dbTrans.Insert<T>(table, fields, values);
        }

        /// <summary>
        ///  ����һ��ʵ��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public int Insert<T, TResult>(Table table, Field[] fields, object[] values, out TResult retVal)
            where T : Entity
        {
            return dbTrans.Insert<T, TResult>(table, fields, values, out retVal);
        }

        /// <summary>
        /// ɾ��һ��ʵ��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Delete<T>(Table table, T entity)
             where T : Entity
        {
            return dbTrans.Delete<T>(table, entity);
        }

        /// <summary>
        /// ɾ��ָ�������ļ�¼
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pkValues"></param>
        /// <returns></returns>
        public int Delete<T>(Table table, params object[] pkValues)
            where T : Entity
        {
            return dbTrans.Delete<T>(table, pkValues);
        }

        /// <summary>
        /// ɾ�����������ļ�¼
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Delete<T>(Table table, WhereClip where)
            where T : Entity
        {
            return dbTrans.Delete<T>(table, where);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertOrUpdate<T>(Table table, T entity)
            where T : Entity
        {
            return dbTrans.InsertOrUpdate<T>(table, entity);
        }

        /// <summary>
        /// ����ָ�������ļ�¼
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<T>(Table table, Field field, object value, WhereClip where)
            where T : Entity
        {
            return dbTrans.Update<T>(table, field, value, where);
        }

        /// <summary>
        /// ����ָ�������ļ�¼
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<T>(Table table, Field[] fields, object[] values, WhereClip where)
            where T : Entity
        {
            return dbTrans.Update<T>(table, fields, values, where);
        }

        #endregion

        #region ��ɾ�Ĳ���

        /// <summary>
        /// ����һ��ʵ��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Save<T>(T entity)
            where T : Entity
        {
            return dbTrans.Save(entity);
        }

        /// <summary>
        ///  ����һ��ʵ��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public int Insert<T>(Field[] fields, object[] values)
            where T : Entity
        {
            return dbTrans.Insert<T>(fields, values);
        }

        /// <summary>
        /// ����һ��ʵ��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="retVal"></param>
        /// <returns></returns>
        public int Insert<T, TResult>(Field[] fields, object[] values, out TResult retVal)
            where T : Entity
        {
            return dbTrans.Insert<T, TResult>(fields, values, out retVal);
        }

        /// <summary>
        /// ɾ��һ��ʵ��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Delete<T>(T entity)
             where T : Entity
        {
            return dbTrans.Delete<T>(entity);
        }

        /// <summary>
        /// ������ɾ��ָ����¼
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pkValues"></param>
        /// <returns></returns>
        public int Delete<T>(params object[] pkValues)
            where T : Entity
        {
            return dbTrans.Delete<T>(pkValues);
        }

        /// <summary>
        /// ������ɾ��ָ����¼
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Delete<T>(WhereClip where)
            where T : Entity
        {
            return dbTrans.Delete<T>(where);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertOrUpdate<T>(T entity)
            where T : Entity
        {
            return dbTrans.InsertOrUpdate<T>(entity);
        }

        /// <summary>
        /// ����������ָ����¼
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<T>(Field field, object value, WhereClip where)
            where T : Entity
        {
            return dbTrans.Update<T>(field, value, where);
        }

        /// <summary>
        /// ����������ָ����¼
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<T>(Field[] fields, object[] values, WhereClip where)
            where T : Entity
        {
            return dbTrans.Update<T>(fields, values, where);
        }

        #endregion

        #region ϵ�л�WhereClip

        /// <summary>
        /// ��������������SQL
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public string Serialization(WhereClip where)
        {
            string sql = dbProvider.Serialization(where.ToString());
            foreach (SQLParameter p in where.Parameters)
            {
                sql = sql.Replace(p.Name, DataHelper.FormatValue(p.Value));
            }
            return sql;
        }

        /// <summary>
        /// �������������SQL
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public string Serialization(OrderByClip order)
        {
            return dbProvider.Serialization(order.ToString());
        }

        #endregion

        #region ע�뻺��

        /// <summary>
        /// ע�뻺������
        /// </summary>
        /// <param name="cache"></param>
        public void InjectCacheDependent(ICacheDependent cache)
        {
            this.dbProvider.Cache = cache;
        }

        #endregion

        #region ˽�з���

        private void InitSession(DbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
            this.dbProvider.SetDecryptHandler(Decrypt);
            this.dbTrans = new DbTrans(dbProvider, false);
        }

        #endregion

        #region ֧��FieldValue��ʽ

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fvs"></param>
        /// <returns></returns>
        public int Insert<T>(FieldValue[] fvs)
            where T : Entity
        {
            return dbTrans.Insert<T>(fvs);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="fvs"></param>
        /// <param name="retVal"></param>
        /// <returns></returns>
        public int Insert<T, TResult>(FieldValue[] fvs, out TResult retVal)
            where T : Entity
        {
            return dbTrans.Insert<T, TResult>(fvs, out retVal);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fv"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<T>(FieldValue fv, WhereClip where)
            where T : Entity
        {
            return dbTrans.Update<T>(fv, where);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fvs"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<T>(FieldValue[] fvs, WhereClip where)
            where T : Entity
        {
            return dbTrans.Update<T>(fvs, where);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="fvs"></param>
        /// <returns></returns>
        public int Insert<T>(Table table, FieldValue[] fvs)
            where T : Entity
        {
            return dbTrans.Insert<T>(table, fvs);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="table"></param>
        /// <param name="fvs"></param>
        /// <param name="retVal"></param>
        /// <returns></returns>
        public int Insert<T, TResult>(Table table, FieldValue[] fvs, out TResult retVal)
            where T : Entity
        {
            return dbTrans.Insert<T, TResult>(table, fvs, out retVal);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="fv"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<T>(Table table, FieldValue fv, WhereClip where)
            where T : Entity
        {
            return dbTrans.Update<T>(table, fv, where);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="fvs"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<T>(Table table, FieldValue[] fvs, WhereClip where)
            where T : Entity
        {
            return dbTrans.Update<T>(table, fvs, where);
        }

        #endregion

        #region ����ʱ���ر�ʶ��

        /// <summary>
        /// ����ʵ��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="entity"></param>
        /// <param name="retVal"></param>
        /// <returns></returns>
        public int Insert<T, TResult>(T entity, out TResult retVal, params FieldValue[] fvs)
            where T : Entity
        {
            return dbTrans.Insert<T, TResult>(entity, out retVal, fvs);
        }

        /// <summary>
        /// ����ʵ��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="table"></param>
        /// <param name="entity"></param>
        /// <param name="retVal"></param>
        /// <returns></returns>
        public int Insert<T, TResult>(Table table, T entity, out TResult retVal, params FieldValue[] fvs)
            where T : Entity
        {
            return dbTrans.Insert<T, TResult>(table, entity, out retVal, fvs);
        }

        #endregion

        #endregion
    }
}
