using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using MySoft.Data.Design;
using MySoft.Logger;
using MySoft.Cache;
using System.Diagnostics;

namespace MySoft.Data
{
    public abstract class DbProvider : IDbProvider
    {
        private DbHelper dbHelper;
        private char leftToken;
        private char rightToken;
        private char paramPrefixToken;
        private IExcutingLog logger;

        /// <summary>
        /// ��־����
        /// </summary>
        internal IExcutingLog Logger
        {
            set
            {
                logger = value;
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        internal ICacheDependent Cache { get; set; }

        /// <summary>
        /// ��ʱʱ��
        /// </summary>
        internal int Timeout { get; set; }

        protected DbProvider(string connectionString, System.Data.Common.DbProviderFactory dbFactory, char leftToken, char rightToken, char paramPrefixToken)
        {
            this.leftToken = leftToken;
            this.rightToken = rightToken;
            this.paramPrefixToken = paramPrefixToken;
            this.dbHelper = new DbHelper(connectionString, dbFactory);
            this.Timeout = -1;
        }

        internal void SetDecryptHandler(DecryptEventHandler handler)
        {
            this.dbHelper.SetDecryptHandler(handler);
        }

        #region ����DbCommand����

        private string FormatParameter(string parameterName)
        {
            if (parameterName.IndexOf(paramPrefixToken) == 0)
                return parameterName;
            if (parameterName.IndexOf('$') == 0)
                return paramPrefixToken + parameterName.TrimStart('$');
            return paramPrefixToken + parameterName;
        }

        /// <summary>
        /// ��������Ӳ���
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameters"></param>
        public void AddParameter(DbCommand cmd, DbParameter[] parameters)
        {
            foreach (DbParameter p in parameters)
            {
                if (p.Value == null)
                    p.Value = DBNull.Value;
                else if (p.Value is Enum)
                {
                    //��ö�ٽ������⴦��
                    p.Value = Convert.ToInt32(p.Value);
                }

                cmd.Parameters.Add(p);
            }
        }

        /// <summary>
        /// ��������Ӳ���
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public void AddParameter(DbCommand cmd, SQLParameter[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
                return;

            List<DbParameter> list = new List<DbParameter>();
            foreach (SQLParameter p in parameters)
            {
                DbParameter dbParameter = CreateParameter(p.Name, p.Value);
                dbParameter.Direction = p.Direction;

                list.Add(dbParameter);
            }

            AddParameter(cmd, list.ToArray());
        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        public void AddInputParameter(DbCommand cmd, string parameterName, DbType dbType, int size, object value)
        {
            dbHelper.AddInputParameter(cmd, parameterName, dbType, size, value);
        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        public void AddInputParameter(DbCommand cmd, string parameterName, DbType dbType, object value)
        {
            dbHelper.AddInputParameter(cmd, parameterName, dbType, value);
        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        public void AddOutputParameter(DbCommand cmd, string parameterName, DbType dbType, int size)
        {
            dbHelper.AddOutputParameter(cmd, parameterName, dbType, size);
        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        public void AddOutputParameter(DbCommand cmd, string parameterName, DbType dbType)
        {
            dbHelper.AddOutputParameter(cmd, parameterName, dbType);
        }

        /// <summary>
        /// ��������������
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        public void AddInputOutputParameter(DbCommand cmd, string parameterName, DbType dbType, int size, object value)
        {
            dbHelper.AddInputOutputParameter(cmd, parameterName, dbType, size, value);
        }

        /// <summary>
        /// ��������������
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        public void AddInputOutputParameter(DbCommand cmd, string parameterName, DbType dbType, object value)
        {
            dbHelper.AddInputOutputParameter(cmd, parameterName, dbType, value);
        }

        /// <summary>
        /// ��ӷ��ز���
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        public void AddReturnValueParameter(DbCommand cmd, string parameterName, DbType dbType, int size)
        {
            dbHelper.AddReturnValueParameter(cmd, parameterName, dbType, size);
        }

        /// <summary>
        /// ��ӷ��ز���
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        public void AddReturnValueParameter(DbCommand cmd, string parameterName, DbType dbType)
        {
            dbHelper.AddReturnValueParameter(cmd, parameterName, dbType);
        }

        public DbParameter GetParameter(DbCommand cmd, string parameterName)
        {
            return dbHelper.GetParameter(cmd, parameterName);
        }

        #endregion

        #region ִ��SQL���

        internal int ExecuteNonQuery(DbCommand cmd, DbTrans trans)
        {
            //����DbCommand;
            PrepareCommand(cmd);

            //ִ������ǰ���¼�
            if (!StartExcuteCommand(cmd))
            {
                throw new DataException("��ǰ���ݿ���������� ==> " + GetLog(cmd));
            }

            int retVal = -1;
            Stopwatch watch = Stopwatch.StartNew();
            try
            {
                if (trans.Connection == null && trans.Transaction == null)
                {
                    retVal = dbHelper.ExecuteNonQuery(cmd);
                }
                else
                {
                    retVal = dbHelper.ExecuteNonQuery(cmd, trans);
                }
            }
            catch (Exception ex)
            {
                WriteErrorCommand(ex, cmd);

                throw ex;
            }
            finally
            {
                watch.Stop();

                //ִ���������¼�
                EndExcuteCommand(cmd, retVal, (int)watch.ElapsedMilliseconds);
            }

            return retVal;
        }

        internal SourceReader ExecuteReader(DbCommand cmd, DbTrans trans)
        {
            //����DbCommand;
            PrepareCommand(cmd);

            //ִ������ǰ���¼�
            if (!StartExcuteCommand(cmd))
            {
                throw new DataException("��ǰ���ݿ���������� ==> " + GetLog(cmd));
            }

            SourceReader retVal = null;
            Stopwatch watch = Stopwatch.StartNew();
            try
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
                retVal = new SourceReader(reader);
            }
            catch (Exception ex)
            {
                WriteErrorCommand(ex, cmd);

                throw ex;
            }
            finally
            {
                watch.Stop();

                //ִ���������¼�
                EndExcuteCommand(cmd, retVal, (int)watch.ElapsedMilliseconds);
            }

            return retVal;
        }

        internal DataSet ExecuteDataSet(DbCommand cmd, DbTrans trans)
        {
            //����DbCommand;
            PrepareCommand(cmd);

            //ִ������ǰ���¼�
            if (!StartExcuteCommand(cmd))
                return null;

            DataSet retVal = null;
            Stopwatch watch = Stopwatch.StartNew();
            try
            {
                if (trans.Connection == null && trans.Transaction == null)
                {
                    retVal = dbHelper.ExecuteDataSet(cmd);
                }
                else
                {
                    retVal = dbHelper.ExecuteDataSet(cmd, trans);
                }
            }
            catch (Exception ex)
            {
                WriteErrorCommand(ex, cmd);

                throw ex;
            }
            finally
            {
                watch.Stop();

                //ִ���������¼�
                EndExcuteCommand(cmd, retVal, (int)watch.ElapsedMilliseconds);
            }

            return retVal;
        }

        internal DataTable ExecuteDataTable(DbCommand cmd, DbTrans trans)
        {
            //����DbCommand;
            PrepareCommand(cmd);

            //ִ������ǰ���¼�
            if (!StartExcuteCommand(cmd))
            {
                throw new DataException("��ǰ���ݿ���������� ==> " + GetLog(cmd));
            }

            DataTable retVal = null;
            Stopwatch watch = Stopwatch.StartNew();
            try
            {
                if (trans.Connection == null && trans.Transaction == null)
                {
                    retVal = dbHelper.ExecuteDataTable(cmd);
                }
                else
                {
                    retVal = dbHelper.ExecuteDataTable(cmd, trans);
                }
            }
            catch (Exception ex)
            {
                WriteErrorCommand(ex, cmd);

                throw ex;
            }
            finally
            {
                watch.Stop();

                //ִ���������¼�
                EndExcuteCommand(cmd, retVal, (int)watch.ElapsedMilliseconds);
            }

            return retVal;
        }

        internal object ExecuteScalar(DbCommand cmd, DbTrans trans)
        {
            //����DbCommand;
            PrepareCommand(cmd);

            //ִ������ǰ���¼�
            if (!StartExcuteCommand(cmd))
            {
                throw new DataException("��ǰ���ݿ���������� ==> " + GetLog(cmd));
            }

            object retVal = null;
            Stopwatch watch = Stopwatch.StartNew();
            try
            {
                if (trans.Connection == null && trans.Transaction == null)
                {
                    retVal = dbHelper.ExecuteScalar(cmd);
                }
                else
                {
                    retVal = dbHelper.ExecuteScalar(cmd, trans);
                }
            }
            catch (Exception ex)
            {
                WriteErrorCommand(ex, cmd);

                throw ex;
            }
            finally
            {
                watch.Stop();

                //ִ���������¼�
                EndExcuteCommand(cmd, retVal, (int)watch.ElapsedMilliseconds);
            }

            return retVal;
        }

        /// <summary>
        /// ����DbCommand����
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private void PrepareCommand(DbCommand cmd)
        {
            //����command����
            cmd.CommandText = FormatCommandText(cmd.CommandText);
            if (Timeout > 0) //�����ʱʱ�����ô���0����ʹ��ָ���ĳ�ʱʱ��
            {
                cmd.CommandTimeout = Timeout;
            }

            int index = 0;
            foreach (DbParameter p in cmd.Parameters)
            {
                string oldName = p.ParameterName;

                //���sql��ʽ���Ž��в�������
                if (cmd.CommandType == CommandType.Text)
                {
                    if (oldName.Length >= 100)
                        p.ParameterName = FormatParameter("p" + index++);
                    else
                        p.ParameterName = FormatParameter(oldName);

                    cmd.CommandText = cmd.CommandText.Replace(oldName, p.ParameterName);
                }
                else
                {
                    p.ParameterName = FormatParameter(oldName);
                }
            }

            //��������
            PrepareParameter(cmd);
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

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public DbParameter CreateParameter()
        {
            return dbHelper.CreateParameter();
        }

        #endregion

        #region ������ɾ�ķ���

        #region Insert

        internal int Insert<T>(Table table, List<FieldValue> fvlist, DbTrans trans, Field identityfield, string autoIncrementName, bool isOutValue, out object retVal)
            where T : Entity
        {
            retVal = null;
            T entity = CoreHelper.CreateInstance<T>();

            int returnValue = 0;
            DbCommand cmd = CreateInsert<T>(table, fvlist, identityfield, autoIncrementName);
            string tableName = table == null ? entity.GetTable().Name : table.Name;

            if ((IField)identityfield != null)
            {
                //Access��ȡ���ļ�¼��
                if (AccessProvider)
                {
                    returnValue = ExecuteNonQuery(cmd, trans);
                    if (isOutValue)
                    {
                        cmd = CreateSqlCommand(string.Format(AutoIncrementValue, identityfield.Name, tableName));
                        retVal = ExecuteScalar(cmd, trans);
                    }
                }
                else
                {
                    if (AllowAutoIncrement)
                    {
                        returnValue = ExecuteNonQuery(cmd, trans);

                        if (isOutValue)
                        {
                            if (!string.IsNullOrEmpty(autoIncrementName))
                            {
                                cmd = CreateSqlCommand(string.Format(AutoIncrementValue, autoIncrementName));
                                retVal = ExecuteScalar(cmd, trans);
                            }
                        }
                    }
                    else
                    {
                        if (isOutValue)
                        {
                            cmd.CommandText += ";" + string.Format(AutoIncrementValue, identityfield.Name, tableName);
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
            T entity = CoreHelper.CreateInstance<T>();

            if (entity.GetReadOnly())
            {
                throw new DataException("ֻ��ʵ��" + typeof(T).Name + "ֻ�����ڲ�ѯ��");
            }

            //�Ƴ�����
            if (this.Cache != null) Cache.RemoveCache(typeof(T));

            string tableName = table == null ? entity.GetTable().Name : table.Name;

            List<SQLParameter> plist = new List<SQLParameter>();
            StringBuilder sbsql = new StringBuilder();
            StringBuilder sbparam = new StringBuilder();

            if (AllowAutoIncrement)
            {
                //�����ʶ�кͱ�ʶ���ƶ���Ϊnull
                if ((IField)identityfield != null && !string.IsNullOrEmpty(autoIncrementName))
                {
                    string identityName = GetAutoIncrement(autoIncrementName);
                    bool exist = false;
                    fvlist.ForEach(fv =>
                    {
                        if (fv.IsIdentity)
                        {
                            fv.Value = new DbValue(identityName);
                            fv.IsIdentity = false;
                            exist = true;
                        }
                    });

                    if (!exist)
                    {
                        object value = new DbValue(identityName);
                        FieldValue fv = new FieldValue(identityfield, value);
                        fvlist.Insert(0, fv);
                    }
                }
            }

            sbsql.Append("INSERT INTO " + tableName + "(");
            sbparam.Append(" VALUES (");

            fvlist.ForEach(fv =>
            {
                if (fv.IsIdentity)
                    return;

                sbsql.Append(fv.Field.At((string)null).Name);
                if (CheckValue(fv.Value))
                {
                    sbparam.Append(DataHelper.FormatValue(fv.Value));
                }
                else
                {
                    SQLParameter p = null;
                    if (CoreHelper.CheckStructType(fv.Value))
                        p = CreateOrmParameter(DataHelper.FormatValue(fv.Value));
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
            T entity = CoreHelper.CreateInstance<T>();

            if (entity.GetReadOnly())
            {
                throw new DataException("ֻ��ʵ��" + typeof(T).Name + "ֻ�����ڲ�ѯ��");
            }

            if ((object)where == null)
            {
                throw new DataException("ɾ����������Ϊnull��");
            }

            //�Ƴ�����
            if (this.Cache != null) Cache.RemoveCache(typeof(T));

            StringBuilder sb = new StringBuilder();
            string tableName = table == null ? entity.GetTable().Name : table.Name;
            sb.Append("DELETE FROM " + tableName);

            if (!DataHelper.IsNullOrEmpty(where))
            {
                sb.Append(" WHERE " + where.ToString());
            }

            return CreateSqlCommand(sb.ToString(), where.Parameters);
        }

        #endregion

        #region Update

        internal int Update<T>(Table table, List<FieldValue> fvlist, WhereClip where, DbTrans trans)
            where T : Entity
        {
            //���û�����ø��µ��ֶΣ��׳��쳣
            if (fvlist.FindAll(fv => { return fv.IsChanged; }).Count == 0)
            {
                //throw new DataException("���������쳣��û����Ҫ���µ����ݣ�");
                return -1; //-1��ʾû����Ҫ���µ���
            }

            DbCommand cmd = CreateUpdate<T>(table, fvlist, where);
            return ExecuteNonQuery(cmd, trans);
        }

        internal DbCommand CreateUpdate<T>(Table table, List<FieldValue> fvlist, WhereClip where)
            where T : Entity
        {
            T entity = CoreHelper.CreateInstance<T>();

            if (entity.GetReadOnly())
            {
                throw new DataException("ֻ��ʵ��" + typeof(T).Name + "ֻ�����ڲ�ѯ��");
            }

            if ((object)where == null)
            {
                throw new DataException("������������Ϊnull��");
            }

            //�Ƴ�����
            if (this.Cache != null) Cache.RemoveCache(typeof(T));

            string tableName = table == null ? entity.GetTable().Name : table.Name;

            List<SQLParameter> plist = new List<SQLParameter>();
            StringBuilder sb = new StringBuilder();

            sb.Append("UPDATE " + tableName + " SET ");

            fvlist.ForEach(fv =>
            {
                if (fv.IsPrimaryKey || fv.IsIdentity)
                    return;

                if (fv.IsChanged)
                {
                    if (CheckValue(fv.Value))
                    {
                        sb.Append(fv.Field.At((string)null).Name + " = " + DataHelper.FormatValue(fv.Value));
                    }
                    else
                    {
                        SQLParameter p = null;
                        if (CoreHelper.CheckStructType(fv.Value))
                            p = CreateOrmParameter(DataHelper.FormatValue(fv.Value));
                        else
                            p = CreateOrmParameter(fv.Value);

                        sb.Append(fv.Field.At((string)null).Name + " = " + p.Name);
                        plist.Add(p);
                    }

                    sb.Append(",");
                }
            });

            sb.Remove(sb.Length - 1, 1);

            if (!DataHelper.IsNullOrEmpty(where))
            {
                sb.Append(" WHERE " + where.ToString());
                plist.AddRange(where.Parameters);
            }

            return CreateSqlCommand(sb.ToString(), plist.ToArray());
        }

        #endregion

        /// <summary>
        /// �������������SQL
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        internal string FormatCommandText(string cmdText)
        {
            return DataHelper.FormatSQL(cmdText, leftToken, rightToken, AccessProvider);
        }

        internal DbCommand CreateSqlCommand(string cmdText)
        {
            return dbHelper.CreateSqlStringCommand(cmdText);
        }

        /// <summary>
        /// ����SQL����
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        internal DbCommand CreateSqlCommand(string cmdText, SQLParameter[] parameters)
        {
            DbCommand cmd = dbHelper.CreateSqlStringCommand(cmdText);
            AddParameter(cmd, parameters);

            return cmd;
        }

        /// <summary>
        /// �����洢��������
        /// </summary>
        /// <param name="procName"></param>
        /// <returns></returns>
        internal DbCommand CreateProcCommand(string procName)
        {
            return dbHelper.CreateStoredProcCommand(procName);
        }

        #endregion

        #region ILogable Members

        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="command">The command.</param>
        private bool StartExcuteCommand(DbCommand command)
        {
            if (logger != null)
            {
                try
                {
                    //���sql��־
                    logger.WriteLog(GetLog(command), LogType.Information);

                    string cmdText = command.CommandText;
                    List<SQLParameter> parameters = new List<SQLParameter>();
                    foreach (DbParameter p in command.Parameters)
                    {
                        parameters.Add(new SQLParameter(p));
                    }
                    return logger.StartExcute(cmdText, parameters.ToArray());
                }
                catch { }
            }

            return true;
        }

        /// <summary>
        /// ����ʱִ�еĲ���
        /// </summary>
        /// <param name="command"></param>
        private void EndExcuteCommand(DbCommand command, object result, int elapsedTime)
        {
            if (logger != null)
            {
                try
                {
                    string cmdText = command.CommandText;
                    List<SQLParameter> parameters = new List<SQLParameter>();
                    foreach (DbParameter p in command.Parameters)
                    {
                        parameters.Add(new SQLParameter(p));
                    }
                    logger.EndExcute(cmdText, parameters.ToArray(), result, elapsedTime);
                }
                catch { }
            }
        }

        /// <summary>
        /// Writes the exception log.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="command"></param>
        private void WriteErrorCommand(Exception ex, DbCommand command)
        {
            if (logger != null)
            {
                try
                {
                    //���������־
                    logger.WriteLog(GetLog(command) + " ==> Error: " + ex.Message, LogType.Error);

                    var exception = new DataException(GetLog(command), ex);
                    logger.WriteError(exception);
                }
                catch { }
            }
        }

        /// <summary>
        /// ��ȡ�������־
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected string GetLog(DbCommand command)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format("{0}\t{1}\t\r\n", command.CommandType, command.CommandText));
            if (command.Parameters != null && command.Parameters.Count > 0)
            {
                sb.Append("Parameters:\r\n");
                foreach (DbParameter p in command.Parameters)
                {
                    if (p.Size > 0)
                        sb.Append(string.Format("{0}[{1}][{2}({3})] = {4}\r\n", p.ParameterName, p.DbType, GetParameterType(p), p.Size, p.Value));
                    else
                        sb.Append(string.Format("{0}[{1}][{2}] = {3}\r\n", p.ParameterName, p.DbType, GetParameterType(p), p.Value));
                }
            }
            sb.Append("\r\n");

            return sb.ToString();
        }

        #endregion

        #region ����д�ķ���

        /// <summary>
        /// ��ȡIdentityName
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual string GetAutoIncrement(string name)
        {
            return name;
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
                    throw new DataException("SqlServer2000��Access��ʹ��SetPagingField�趨��ҳ������");
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
        protected virtual bool AllowAutoIncrement
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
        protected abstract string AutoIncrementValue { get; }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected abstract object GetParameterType(DbParameter parameter);

        /// <summary>
        /// ����DbParameter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        protected abstract DbParameter CreateParameter(string parameterName, object val);

        /// <summary>
        /// ����DbCommand����
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        protected abstract void PrepareParameter(DbCommand cmd);

        #endregion

        /// <summary>
        /// ����OrmParameter
        /// </summary>
        /// <returns></returns>
        private SQLParameter CreateOrmParameter(object value)
        {
            string pName = CoreHelper.MakeUniqueKey(100, "$");
            return new SQLParameter(pName, value);
        }

        /// <summary>
        /// ����Ƿ�Ϊ�Ƿ�ֵ
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool CheckValue(object value)
        {
            if (value == null || value == DBNull.Value || value is Field || value is DbValue)
            {
                return true;
            }
            return false;
        }
    }
}
