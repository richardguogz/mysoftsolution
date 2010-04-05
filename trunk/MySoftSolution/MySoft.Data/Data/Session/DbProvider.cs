using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using MySoft.Data.Design;

namespace MySoft.Data
{
    public abstract class DbProvider : IDbProvider
    {
        private DbHelper dbHelper;
        private char leftToken;
        private char rightToken;
        private char paramPrefixToken;

        protected DbProvider(string connectionString, DbProviderFactory dbFactory, char leftToken, char rightToken, char paramPrefixToken)
        {
            this.connectName = "Default";
            this.leftToken = leftToken;
            this.rightToken = rightToken;
            this.paramPrefixToken = paramPrefixToken;
            this.dbHelper = new DbHelper(connectionString, dbFactory);
        }

        internal void SetEventHandler(DecryptEventHandler decryptEvent)
        {
            this.dbHelper.SetDecryptHandler(decryptEvent);
        }

        #region ����Cache��Ϣ
        private string connectName;
        internal string ConnectName
        {
            get { return connectName; }
            set { connectName = value; }
        }

        private DataCache dataCache;
        internal DataCache DataCache
        {
            get { return dataCache; }
            set { dataCache = value; }
        }

        private CacheConfigurationSection cacheConfigSection = null;
        internal CacheConfigurationSection CacheConfigSection
        {
            get { return cacheConfigSection; }
            set { cacheConfigSection = value; }
        }

        private IDictionary<string, CacheConfigInfo> cacheConfigMap = new Dictionary<string, CacheConfigInfo>();
        internal IDictionary<string, CacheConfigInfo> CacheConfigMap
        {
            get { return cacheConfigMap; }
            set { cacheConfigMap = value; }
        }

        internal void RemoveCache(string entityName)
        {
            //�����и��½��Ƴ�����
            //ͬʱ��Ҫ�Ƴ��й�����ϵ�еı���
            lock (cacheConfigMap)
            {
                if (cacheConfigMap.ContainsKey(entityName))
                {
                    dataCache.RemoveCache(cacheConfigMap[entityName]);
                }
            }
        }

        internal void RemoveAllCache()
        {
            dataCache.RemoveAllCache();
        }

        #endregion

        #region ����DbCommand����

        internal string FormatParameter(string parameterName)
        {
            if (parameterName.IndexOf(paramPrefixToken) == 0) return parameterName;
            if (parameterName.IndexOf('$') == 0) return paramPrefixToken + parameterName.TrimStart('$');
            return paramPrefixToken + parameterName;
        }

        public void AddParameter(DbCommand cmd, DbParameter parameter)
        {
            parameter.ParameterName = FormatParameter(parameter.ParameterName);
            cmd.Parameters.Add(parameter);
        }

        public void AddInputParameter(DbCommand cmd, string parameterName, DbType dbType, int size, object value)
        {
            parameterName = FormatParameter(parameterName);
            dbHelper.AddInputParameter(cmd, parameterName, dbType, size, value);
        }

        public void AddInputParameter(DbCommand cmd, string parameterName, DbType dbType, object value)
        {
            parameterName = FormatParameter(parameterName);
            dbHelper.AddInputParameter(cmd, parameterName, dbType, value);
        }

        public void AddOutputParameter(DbCommand cmd, string parameterName, DbType dbType, int size)
        {
            parameterName = FormatParameter(parameterName);
            dbHelper.AddOutputParameter(cmd, parameterName, dbType, size);
        }

        public void AddInputOutputParameter(DbCommand cmd, string parameterName, DbType dbType, object value, int size)
        {
            parameterName = FormatParameter(parameterName);
            dbHelper.AddInputOutputParameter(cmd, parameterName, dbType, value, size);
        }

        public void AddReturnValueParameter(DbCommand cmd, string parameterName, DbType dbType)
        {
            parameterName = FormatParameter(parameterName);
            dbHelper.AddReturnValueParameter(cmd, parameterName, dbType);
        }

        public DbParameter GetParameter(DbCommand cmd, string parameterName)
        {
            parameterName = FormatParameter(parameterName);
            return dbHelper.GetParameter(cmd, parameterName);
        }

        #endregion

        #region ִ��SQL���

        public int ExecuteNonQuery(DbCommand cmd, DbTrans trans)
        {
            //����DbCommand;
            PrepareCommand(cmd);

            //д��־
            WriteLog(cmd);

            if (OnNoExecuteLog != null)
            {
                OnNoExecuteLog(GetLog(cmd));
                return int.MinValue;
            }
            else
            {
                if (trans.Connection == null && trans.Transaction == null)
                {
                    return dbHelper.ExecuteNonQuery(cmd);
                }
                return dbHelper.ExecuteNonQuery(cmd, trans);
            }
        }

        public ISourceReader ExecuteReader(DbCommand cmd, DbTrans trans)
        {
            //����DbCommand;
            PrepareCommand(cmd);

            //д��־
            WriteLog(cmd);

            if (OnNoExecuteLog != null)
            {
                OnNoExecuteLog(GetLog(cmd));
                return null;
            }
            else
            {
                IDataReader reader;
                if (trans.Connection == null && trans.Transaction == null)
                {
                    reader = dbHelper.ExecuteReader(cmd);
                }
                else
                {
                    reader = dbHelper.ExecuteReader(cmd, trans);
                }
                return new SourceReader(reader);
            }
        }

        public DataSet ExecuteDataSet(DbCommand cmd, DbTrans trans)
        {
            //����DbCommand;
            PrepareCommand(cmd);

            //д��־
            WriteLog(cmd);

            if (OnNoExecuteLog != null)
            {
                OnNoExecuteLog(GetLog(cmd));
                return null;
            }
            else
            {
                if (trans.Connection == null && trans.Transaction == null)
                {
                    return dbHelper.ExecuteDataSet(cmd);
                }
                return dbHelper.ExecuteDataSet(cmd, trans);
            }
        }

        public DataTable ExecuteDataTable(DbCommand cmd, DbTrans trans)
        {
            //����DbCommand;
            PrepareCommand(cmd);

            //д��־
            WriteLog(cmd);

            if (OnNoExecuteLog != null)
            {
                OnNoExecuteLog(GetLog(cmd));
                return null;
            }
            else
            {
                if (trans.Connection == null && trans.Transaction == null)
                {
                    return dbHelper.ExecuteDataTable(cmd);
                }
                return dbHelper.ExecuteDataTable(cmd, trans);
            }
        }

        public object ExecuteScalar(DbCommand cmd, DbTrans trans)
        {
            //����DbCommand;
            PrepareCommand(cmd);

            //д��־
            WriteLog(cmd);

            if (OnNoExecuteLog != null)
            {
                OnNoExecuteLog(GetLog(cmd));
                return null;
            }
            else
            {
                if (trans.Connection == null && trans.Transaction == null)
                {
                    return dbHelper.ExecuteScalar(cmd);
                }
                return dbHelper.ExecuteScalar(cmd, trans);
            }
        }

        /// <summary>
        /// ��ȡCommandText
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public string GetCommandText(DbCommand cmd)
        {
            //����DbCommand;
            PrepareCommand(cmd);

            //����������־
            return GetLog(cmd);
        }

        #endregion

        #region ����DbConnection��DbParameter

        /// <summary>
        /// ����DbConnection
        /// </summary>
        /// <returns></returns>
        public DbConnection CreateConnection()
        {
            return dbHelper.CreateConnection();
        }

        #endregion

        #region ������ɾ�ķ���

        #region Insert

        internal int Insert<T>(Table table, List<FieldValue> fvlist, DbTrans trans, Field identityfield, string autoIncrementName, bool isOutValue, out object retVal)
            where T : Entity
        {
            retVal = null;
            T entity = DataUtils.CreateInstance<T>();

            int returnValue = 0;
            DbCommand cmd = CreateInsert<T>(table, fvlist, identityfield, autoIncrementName);
            string tableName = table == null ? entity.GetTable().Name : table.Name;

            if ((IField)identityfield != null)
            {
                //Access��ȡ���ļ�¼��
                if (AccessProvider)
                {
                    returnValue = ExecuteNonQuery(cmd, trans);
                    cmd = CreateSqlCommand(string.Format(RowAutoID, identityfield.Name, tableName));
                    if (isOutValue) retVal = ExecuteScalar(cmd, trans);
                }
                else
                {
                    if (UseAutoIncrement)
                    {
                        returnValue = ExecuteNonQuery(cmd, trans);

                        if (isOutValue)
                        {
                            if (!string.IsNullOrEmpty(autoIncrementName))
                            {
                                cmd = CreateSqlCommand(string.Format(RowAutoID, autoIncrementName));
                                retVal = ExecuteScalar(cmd, trans);
                            }
                        }
                    }
                    else
                    {
                        if (isOutValue)
                        {
                            cmd.CommandText += ";" + string.Format(RowAutoID, identityfield.Name, tableName);
                            retVal = ExecuteScalar(cmd, trans);
                            returnValue = 1;
                        }
                        else
                        {
                            returnValue = ExecuteNonQuery(cmd, trans);
                        }
                    }
                }
            }
            else
            {
                returnValue = ExecuteNonQuery(cmd, trans);
            }

            return returnValue;
        }

        internal DbCommand CreateInsert<T>(Table table, List<FieldValue> fvlist, Field identityfield, string autoIncrementName)
            where T : Entity
        {
            T entity = DataUtils.CreateInstance<T>();

            if (entity.GetReadOnly())
            {
                throw new MySoftException("ֻ��ʵ��" + typeof(T).Name + "ֻ�����ڲ�ѯ��");
            }

            //�Ƴ�����
            RemoveCache(typeof(T).Name);

            string tableName = table == null ? entity.GetTable().Name : table.Name;

            List<SQLParameter> plist = new List<SQLParameter>();
            StringBuilder sbsql = new StringBuilder();
            StringBuilder sbparam = new StringBuilder();

            if (UseAutoIncrement)
            {
                //�����ʶ�кͱ�ʶ���ƶ���Ϊnull
                if ((IField)identityfield != null && !string.IsNullOrEmpty(autoIncrementName))
                {
                    string identityName = FormatIdentityName(autoIncrementName);
                    bool exist = false;
                    fvlist.ForEach(fv =>
                    {
                        if (fv.IsIdentity)
                        {
                            fv.Value = new SysField(identityName);
                            fv.IsIdentity = false;
                            exist = true;
                        }
                    });

                    if (!exist)
                    {
                        Field value = new SysField(identityName);
                        FieldValue fv = new FieldValue(identityfield, value);
                        fvlist.Insert(0, fv);
                    }
                }
            }

            sbsql.Append("insert into " + tableName + "(");
            sbparam.Append(" values (");

            fvlist.ForEach(fv =>
            {
                if (fv.IsIdentity) return;

                sbsql.Append(fv.Field.At((string)null).Name);
                if (CheckNullOrField(fv.Value))
                {
                    sbparam.Append(DataUtils.FormatValue(fv.Value));
                }
                else
                {
                    SQLParameter p = null;
                    if (CheckStruct(fv.Value))
                        p = CreateOrmParameter(DataUtils.FormatValue(fv.Value));
                    else
                        p = CreateOrmParameter(fv.Value);

                    sbparam.Append(p.Name);
                    plist.Add(p);
                }

                sbsql.Append(",");
                sbparam.Append(",");
            });

            sbsql.Remove(sbsql.Length - 1, 1).Append(")");
            sbparam.Remove(sbparam.Length - 1, 1).Append(")");

            string cmdText = string.Format("{0}{1}", sbsql, sbparam);

            return CreateSqlCommand(cmdText, plist.ToArray());
        }

        #endregion

        #region Delete

        internal int Delete<T>(Table table, WhereClip where, DbTrans trans)
            where T : Entity
        {
            DbCommand cmd = CreateDelete<T>(table, where);
            return ExecuteNonQuery(cmd, trans);
        }

        internal DbCommand CreateDelete<T>(Table table, WhereClip where)
            where T : Entity
        {
            T entity = DataUtils.CreateInstance<T>();

            if (entity.GetReadOnly())
            {
                throw new MySoftException("ֻ��ʵ��" + typeof(T).Name + "ֻ�����ڲ�ѯ��");
            }

            if ((object)where == null)
            {
                throw new MySoftException("ɾ����������Ϊnull��");
            }

            //�Ƴ�����
            RemoveCache(typeof(T).Name);

            StringBuilder sb = new StringBuilder();
            string tableName = table == null ? entity.GetTable().Name : table.Name;
            sb.Append("delete from " + tableName);

            if (!DataUtils.IsNullOrEmpty(where))
            {
                sb.Append(" where " + where.ToString());
            }

            return CreateSqlCommand(sb.ToString(), where.Parameters);
        }

        #endregion

        #region Update

        internal int Update<T>(Table table, List<FieldValue> fvlist, WhereClip where, DbTrans trans)
            where T : Entity
        {
            //���û�����ø��µ��ֶΣ�����-1
            if (fvlist.FindAll(fv => { return fv.IsChanged; }).Count == 0) return -1;

            DbCommand cmd = CreateUpdate<T>(table, fvlist, where);
            return ExecuteNonQuery(cmd, trans);
        }

        internal DbCommand CreateUpdate<T>(Table table, List<FieldValue> fvlist, WhereClip where)
            where T : Entity
        {
            T entity = DataUtils.CreateInstance<T>();

            if (entity.GetReadOnly())
            {
                throw new MySoftException("ֻ��ʵ��" + typeof(T).Name + "ֻ�����ڲ�ѯ��");
            }

            if ((object)where == null)
            {
                throw new MySoftException("������������Ϊnull��");
            }

            //�Ƴ�����
            RemoveCache(typeof(T).Name);

            string tableName = table == null ? entity.GetTable().Name : table.Name;

            List<SQLParameter> plist = new List<SQLParameter>();
            StringBuilder sb = new StringBuilder();

            sb.Append("update " + tableName + " set ");

            fvlist.ForEach(fv =>
            {
                if (fv.IsPrimaryKey || fv.IsIdentity) return;

                if (fv.IsChanged)
                {
                    if (CheckNullOrField(fv.Value))
                    {
                        sb.Append(fv.Field.At((string)null).Name + " = " + DataUtils.FormatValue(fv.Value));
                    }
                    else
                    {
                        SQLParameter p = null;
                        if (CheckStruct(fv.Value))
                            p = CreateOrmParameter(DataUtils.FormatValue(fv.Value));
                        else
                            p = CreateOrmParameter(fv.Value);

                        sb.Append(fv.Field.At((string)null).Name + " = " + p.Name);
                        plist.Add(p);
                    }

                    sb.Append(",");
                }
            });

            sb.Remove(sb.Length - 1, 1);

            if (!DataUtils.IsNullOrEmpty(where))
            {
                sb.Append(" where " + where.ToString());
                plist.AddRange(where.Parameters);
            }

            return CreateSqlCommand(sb.ToString(), plist.ToArray());
        }

        #endregion

        /// <summary>
        /// �������������SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        internal protected string Serialization(string sql)
        {
            return DataUtils.FormatSQL(sql, leftToken, rightToken, AccessProvider);
        }

        /// <summary>
        /// ����SQL����
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        internal DbCommand CreateSqlCommand(string cmdText, params SQLParameter[] plist)
        {
            DbCommand cmd = dbHelper.CreateSqlStringCommand(cmdText);
            AddOrmParameter(cmd, plist);
            return cmd;
        }

        /// <summary>
        /// �����洢��������
        /// </summary>
        /// <param name="procName"></param>
        /// <returns></returns>
        internal DbCommand CreateProcCommand(string procName, params SQLParameter[] plist)
        {
            DbCommand cmd = dbHelper.CreateStoredProcCommand(procName);
            AddOrmParameter(cmd, plist);
            return cmd;
        }

        /// <summary>
        /// ��ʽ������
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        internal DbCommand FormatCommand(DbCommand cmd)
        {
            return PrepareCommand(cmd);
        }

        #endregion

        #region ILogable Members

        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="command">The command.</param>
        private void WriteLog(DbCommand command)
        {
            if (OnLog != null)
            {
                OnLog(GetLog(command));
            }
        }

        /// <summary>
        /// ��ȡ�������־
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected virtual string GetLog(DbCommand command)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format("{0}\t{1}\t\r\n", command.CommandType, command.CommandText));
            if (command.Parameters != null && command.Parameters.Count > 0)
            {
                sb.Append("Parameters:\r\n");
                foreach (DbParameter p in command.Parameters)
                {
                    if (p.Size > 0)
                        sb.Append(string.Format("{0}[{2}({3})] = {1}\r\n", p.ParameterName, p.Value, p.DbType, p.Size));
                    else
                        sb.Append(string.Format("{0}[{2}] = {1}\r\n", p.ParameterName, p.Value, p.DbType));
                }
            }
            sb.Append("\r\n");

            return sb.ToString();
        }

        /// <summary>
        /// OnLog event.
        /// </summary>
        public event LogHandler OnLog;

        /// <summary>
        /// OnLog event.
        /// </summary>
        public event LogHandler OnNoExecuteLog;

        #endregion

        #region ����д�ķ���

        /// <summary>
        /// ��ʽ��IdentityName
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual string FormatIdentityName(string name)
        {
            return name;
        }

        /// <summary>
        /// ����DbCommand����
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        protected virtual DbCommand PrepareCommand(DbCommand cmd)
        {
            cmd.CommandText = Serialization(cmd.CommandText);
            foreach (DbParameter p in cmd.Parameters)
            {
                p.ParameterName = FormatParameter(p.ParameterName);
            }
            return cmd;
        }

        /// <summary>
        /// ������ҳ��ѯ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="itemCount"></param>
        /// <param name="skipCount"></param>
        /// <returns></returns>
        protected internal virtual QuerySection<T> CreatePageQuery<T>(QuerySection<T> query, int itemCount, int skipCount)
            where T : Entity
        {
            if (skipCount == 0)
            {
                ((IPaging)query).Prefix("top " + itemCount);
                return query;
            }
            else
            {
                ((IPaging)query).Prefix("top " + itemCount);

                Field pagingField = query.PagingField;

                if ((IField)pagingField == null)
                {
                    throw new MySoftException("Sql2000��Access��ʹ��SetPagingField�趨��ҳ������");
                }

                QuerySection<T> jquery = query.CreateQuery<T>();
                ((IPaging)jquery).Prefix("top " + skipCount);
                jquery.Select(pagingField);

                //��������ϲ�ѯ������Ҫ��ֵ����QueryString
                if (query.UnionQuery)
                {
                    jquery.QueryString = query.QueryString;
                }

                query.PageWhere = !pagingField.In(jquery);

                return query;
            }
        }

        #endregion

        #region ���񷽷�

        /// <summary>
        /// �Ƿ�ΪAccess����
        /// </summary>
        protected virtual bool AccessProvider
        {
            get { return false; }
        }

        /// <summary>
        /// �Ƿ�ʹ��������
        /// </summary>
        protected virtual bool UseAutoIncrement
        {
            get { return false; }
        }

        /// <summary>
        /// �Ƿ�֧��������
        /// </summary>
        protected internal abstract bool SupportBatch { get; }

        /// <summary>
        /// �����Զ�ID��sql���
        /// </summary>
        protected abstract string RowAutoID { get; }

        /// <summary>
        /// ����DbParameter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        protected abstract DbParameter CreateParameter(string parameterName, object val);

        #endregion

        /// <summary>
        /// ����OrmParameter
        /// </summary>
        /// <returns></returns>
        private SQLParameter CreateOrmParameter(object value)
        {
            string pName = DataUtils.MakeUniqueKey(30, "p");
            return new SQLParameter(pName, value);
        }

        /// <summary>
        /// ��������Ӳ���
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="plist"></param>
        /// <returns></returns>
        private void AddOrmParameter(DbCommand cmd, IList<SQLParameter> plist)
        {
            if (plist == null || plist.Count == 0) return;

            foreach (SQLParameter p in plist)
            {
                string pName = FormatParameter(p.Name);
                object pValue = p.Value;

                //��ö�ٽ������⴦��
                if (pValue.GetType().IsEnum) pValue = Convert.ToInt32(pValue);

                DbParameter dbParameter = CreateParameter(pName, pValue);
                cmd.Parameters.Add(dbParameter);

                if (!cmd.CommandText.Contains(pName))
                {
                    cmd.CommandText = cmd.CommandText.Replace(p.Name, pName);
                }
            }
        }

        /// <summary>
        /// ����Ƿ�Ϊ�Ƿ�ֵ
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool CheckNullOrField(object value)
        {
            if (value == null || value == DBNull.Value || value is Field)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// ����Ƿ�Ϊ�ṹ����
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool CheckStruct(object value)
        {
            //������Ϊ�ṹʱ����ϵ�л�
            Type type = value.GetType();
            if (type.IsValueType && !type.IsEnum && !type.IsPrimitive && !type.IsSerializable)
            {
                return true;
            }
            return false;
        }
    }
}
