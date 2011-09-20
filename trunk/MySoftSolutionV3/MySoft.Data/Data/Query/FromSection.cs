using System.Collections;
using System.Collections.Generic;
using System.Data;
using MySoft.Data.Design;

namespace MySoft.Data
{
    public class FromSection<T> : IQuerySection<T>
        where T : Entity
    {
        private Entity fromEntity;
        private Table fromTable;
        private QuerySection<T> query;
        internal QuerySection<T> Query
        {
            get { return query; }
        }
        private List<Entity> entityList = new List<Entity>();
        internal List<Entity> EntityList
        {
            get { return entityList; }
            set { entityList = value; }
        }

        #region ��ʼ��FromSection

        internal FromSection()
        {
            this.fromEntity = CoreHelper.CreateInstance<T>();
            this.fromTable = this.fromEntity.GetTable();
        }

        internal FromSection(DbProvider dbProvider, DbTrans dbTran, Table table)
            : this()
        {
            this.entityList.Add(this.fromEntity);
            //�Ӵ���ı���Ϣ�л�ȡ����
            this.tableName = table == null ? fromTable.Name : table.Name;
            fromTable.As(this.tableName);
            Field pagingField = fromEntity.PagingField;
            this.query = new QuerySection<T>(this, dbProvider, dbTran, pagingField);
        }

        internal FromSection(DbProvider dbProvider, DbTrans dbTran, string aliasName)
            : this()
        {
            fromTable.As(aliasName);
            this.entityList.Add(this.fromEntity);
            this.tableName = fromTable.Name;
            Field pagingField = fromEntity.PagingField;
            if (aliasName != null)
            {
                if ((IField)pagingField != null)
                {
                    pagingField = pagingField.At(aliasName);
                }

                this.tableName += " __[" + aliasName + "]__ ";
            }

            this.query = new QuerySection<T>(this, dbProvider, dbTran, pagingField);
        }

        internal FromSection(string tableName, string relation, IList<Entity> list)
        {
            this.tableName = tableName;
            this.relation = relation;
            this.entityList.AddRange(list);
            this.query = new QuerySection<T>(this);
        }

        internal FromSection(string tableName, string aliasName)
            : this()
        {
            fromTable.As(aliasName);
            this.entityList.Add(this.fromEntity);
            this.tableName = tableName;
            if (aliasName != null)
            {
                this.tableName += " __[" + aliasName + "]__ ";
            }
            this.query = new QuerySection<T>(this);
        }

        internal FromSection(Table table)
            : this()
        {
            this.tableName = table == null ? fromTable.Name : table.Name;
            fromTable.As(this.tableName);
            this.query = new QuerySection<T>(this);
        }

        #endregion

        #region ʵ��IQuerySection

        #region ʵ��IDataQuery

        /// <summary>
        /// ����һ��ʵ��
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public TResult ToSingle<TResult>()
            where TResult : class
        {
            return query.ToSingle<TResult>();
        }

        #endregion

        #region �����Ӳ�ѯ

        /// <summary>
        /// ����һ���Ӳ�ѯ
        /// </summary>
        /// <returns></returns>
        public QuerySection<T> SubQuery()
        {
            return query.SubQuery();
        }

        /// <summary>
        /// ����һ���Ӳ�ѯ
        /// </summary>
        /// <returns></returns>
        public QuerySection<T> SubQuery(string aliasName)
        {
            return query.SubQuery(aliasName);
        }

        /// <summary>
        /// ����һ���Ӳ�ѯ
        /// </summary>
        /// <returns></returns>
        public QuerySection<TSub> SubQuery<TSub>()
            where TSub : Entity
        {
            return query.SubQuery<TSub>();
        }

        /// <summary>
        /// ����һ�����������Ӳ�ѯ
        /// </summary>
        /// <param name="aliasName"></param>
        /// <returns></returns>
        public QuerySection<TSub> SubQuery<TSub>(string aliasName)
            where TSub : Entity
        {
            return query.SubQuery<TSub>(aliasName);
        }

        #endregion

        //���ò�ѯ��
        internal void SetQuery(QuerySection<T> query)
        {
            this.query = query;
        }

        #region ����������

        /// <summary>
        /// ����GroupBy����
        /// </summary>
        /// <param name="groupBy"></param>
        /// <returns></returns>
        public QuerySection<T> GroupBy(GroupByClip groupBy)
        {
            return query.GroupBy(groupBy);
        }

        /// <summary>
        /// ����OrderBy����
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public QuerySection<T> OrderBy(OrderByClip orderBy)
        {
            return query.OrderBy(orderBy);
        }

        /// <summary>
        /// ѡ���������
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public QuerySection<T> Select(params Field[] fields)
        {
            return query.Select(fields);
        }

        /// <summary>
        /// ѡ���ų�������У������ж�ʱ�ų�ĳ���е������
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public QuerySection<T> Select(ExcludeField field)
        {
            return query.Select(field);
        }

        /// <summary>
        /// ע�뵱ǰ��ѯ������
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public QuerySection<T> Where(WhereClip where)
        {
            return query.Where(where);
        }

        /// <summary>
        /// ����Union����
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public QuerySection<T> Union(QuerySection<T> uquery)
        {
            return query.Union(uquery);
        }

        /// <summary>
        /// ����Union����
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public QuerySection<T> UnionAll(QuerySection<T> uquery)
        {
            return query.UnionAll(uquery);
        }

        /// <summary>
        /// ����Having����
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public QuerySection<T> Having(WhereClip where)
        {
            return query.Having(where);
        }

        /// <summary>
        /// ���÷�ҳ�ֶ�
        /// </summary>
        /// <param name="pagingField"></param>
        /// <returns></returns>
        public QuerySection<T> SetPagingField(Field pagingField)
        {
            return query.SetPagingField(pagingField);
        }

        /// <summary>
        /// ѡ��ǰn��
        /// </summary>
        /// <param name="topSize"></param>
        /// <returns></returns>
        public TopSection<T> GetTop(int topSize)
        {
            return query.GetTop(topSize);
        }

        /// <summary>
        /// ����Distinct����
        /// </summary>
        /// <returns></returns>
        public QuerySection<T> Distinct()
        {
            return query.Distinct();
        }

        #endregion

        #region ��������

        /// <summary>
        /// ����һ����ҳ������Page��
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PageSection<T> GetPage(int pageSize)
        {
            return query.GetPage(pageSize);
        }

        /// <summary>
        /// ����һ��ʵ��
        /// </summary>
        /// <returns></returns>
        public T ToSingle()
        {
            return query.ToSingle();
        }

        #region ����object

        /// <summary>
        /// ����һ��Object�б�
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public ArrayList<object> ToListResult(int startIndex, int endIndex)
        {
            return query.ToListResult(startIndex, endIndex);
        }

        /// <summary>
        /// ����һ��Object�б�
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public ArrayList<object> ToListResult()
        {
            return query.ToListResult();
        }

        /// <summary>
        /// ����һ��Object�б�
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public ArrayList<TResult> ToListResult<TResult>(int startIndex, int endIndex)
        {
            return query.ToListResult<TResult>(startIndex, endIndex);
        }

        /// <summary>
        /// ����һ��Object�б�
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public ArrayList<TResult> ToListResult<TResult>()
        {
            return query.ToListResult<TResult>();
        }

        #endregion

        /// <summary>
        /// ����һ��DbReader
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public SourceReader ToReader(int startIndex, int endIndex)
        {
            return query.ToReader(startIndex, endIndex);
        }

        /// <summary>
        /// ����һ��DbReader
        /// </summary>
        /// <returns></returns>
        public SourceReader ToReader()
        {
            return query.ToReader();
        }

        #region ���ݲ�ѯ

        /// <summary>
        /// ����IArrayList
        /// </summary>
        /// <returns></returns>
        public SourceList<T> ToList()
        {
            return query.ToList();
        }

        /// <summary>
        /// ����IArrayList
        /// </summary>
        /// <returns></returns>
        public SourceList<T> ToList(int startIndex, int endIndex)
        {
            return query.ToList(startIndex, endIndex);
        }

        /// <summary>
        /// ����IArrayList
        /// </summary>
        /// <returns></returns>
        public SourceList<TResult> ToList<TResult>()
            where TResult : class
        {
            return query.ToList<TResult>();
        }

        /// <summary>
        /// ����IArrayList
        /// </summary>
        /// <returns></returns>
        public SourceList<TResult> ToList<TResult>(int startIndex, int endIndex)
            where TResult : class
        {
            return query.ToList<TResult>(startIndex, endIndex);
        }

        #endregion

        /// <summary>
        /// ����һ��DataTable
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public SourceTable ToTable(int startIndex, int endIndex)
        {
            return query.ToTable(startIndex, endIndex);
        }

        /// <summary>
        /// ����һ��DataTable
        /// </summary>
        /// <returns></returns>
        public SourceTable ToTable()
        {
            return query.ToTable();
        }

        /// <summary>
        /// ����һ��ֵ
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public TResult ToScalar<TResult>()
        {
            return query.ToScalar<TResult>();
        }

        /// <summary>
        /// ����һ��ֵ
        /// </summary>
        /// <returns></returns>
        public object ToScalar()
        {
            return query.ToScalar();
        }

        /// <summary>
        /// ���ص�ǰ��ѯ��¼��
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return query.Count();
        }

        /// <summary>
        /// ��ȡ��ҳ��
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public int GetPageCount(int pageSize)
        {
            return query.GetPageCount(pageSize);
        }

        #endregion

        #region ���ط�ҳ��Ϣ

        public DataPage<IList<T>> ToListPage(int pageSize, int pageIndex)
        {
            return query.ToListPage(pageSize, pageIndex);
        }

        /// <summary>
        /// ����ָ������Դ�ķ�ҳ��Ϣ
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public DataPage<DataTable> ToTablePage(int pageSize, int pageIndex)
        {
            return query.ToTablePage(pageSize, pageIndex);
        }

        #endregion

        #endregion

        #region ���Ӳ�ѯ

        #region ������

        /// <summary>
        /// �����Ӳ�ѯ
        /// </summary>
        /// <typeparam name="TJoin"></typeparam>
        /// <param name="onWhere"></param>
        /// <returns></returns>
        public FromSection<T> InnerJoin<TJoin>(WhereClip onWhere)
            where TJoin : Entity
        {
            return InnerJoin<TJoin>((Table)null, onWhere);
        }

        /// <summary>
        /// �����Ӳ�ѯ
        /// </summary>
        /// <typeparam name="TJoin"></typeparam>
        /// <param name="table"></param>
        /// <param name="onWhere"></param>
        /// <returns></returns>
        public FromSection<T> InnerJoin<TJoin>(Table table, WhereClip onWhere)
            where TJoin : Entity
        {
            return Join<TJoin>(table, onWhere, JoinType.InnerJoin);
        }

        /// <summary>
        /// �����Ӳ�ѯ
        /// </summary>
        /// <typeparam name="TJoin"></typeparam>
        /// <param name="aliasName"></param>
        /// <param name="onWhere"></param>
        /// <returns></returns>
        public FromSection<T> InnerJoin<TJoin>(string aliasName, WhereClip onWhere)
            where TJoin : Entity
        {
            return Join<TJoin>(aliasName, onWhere, JoinType.InnerJoin);
        }

        /// <summary>
        /// �����Ӳ�ѯ
        /// </summary>
        /// <typeparam name="TJoin"></typeparam>
        /// <param name="relation"></param>
        /// <param name="onWhere"></param>
        /// <returns></returns>
        public FromSection<T> InnerJoin<TJoin>(TableRelation<TJoin> relation, WhereClip onWhere)
            where TJoin : Entity
        {
            return InnerJoin(relation, null, onWhere);
        }

        /// <summary>
        /// �����Ӳ�ѯ
        /// </summary>
        /// <typeparam name="TJoin"></typeparam>
        /// <param name="relation"></param>
        /// <param name="aliasName"></param>
        /// <param name="onWhere"></param>
        /// <returns></returns>
        public FromSection<T> InnerJoin<TJoin>(TableRelation<TJoin> relation, string aliasName, WhereClip onWhere)
            where TJoin : Entity
        {
            return Join<TJoin>(relation, aliasName, onWhere, JoinType.InnerJoin);
        }

        #endregion

        #region ������

        /// <summary>
        /// �����Ӳ�ѯ
        /// </summary>
        /// <typeparam name="TJoin"></typeparam>
        /// <param name="onWhere"></param>
        /// <returns></returns>
        public FromSection<T> LeftJoin<TJoin>(WhereClip onWhere)
            where TJoin : Entity
        {
            return LeftJoin<TJoin>((Table)null, onWhere);
        }

        /// <summary>
        /// �����Ӳ�ѯ
        /// </summary>
        /// <typeparam name="TJoin"></typeparam>
        /// <param name="table"></param>
        /// <param name="onWhere"></param>
        /// <returns></returns>
        public FromSection<T> LeftJoin<TJoin>(Table table, WhereClip onWhere)
            where TJoin : Entity
        {
            return Join<TJoin>(table, onWhere, JoinType.LeftJoin);
        }

        /// <summary>
        /// �����Ӳ�ѯ
        /// </summary>
        /// <typeparam name="TJoin"></typeparam>
        /// <param name="aliasName"></param>
        /// <param name="onWhere"></param>
        /// <returns></returns>
        public FromSection<T> LeftJoin<TJoin>(string aliasName, WhereClip onWhere)
            where TJoin : Entity
        {
            return Join<TJoin>(aliasName, onWhere, JoinType.LeftJoin);
        }

        /// <summary>
        /// �����Ӳ�ѯ
        /// </summary>
        /// <typeparam name="TJoin"></typeparam>
        /// <param name="relation"></param>
        /// <param name="onWhere"></param>
        /// <returns></returns>
        public FromSection<T> LeftJoin<TJoin>(TableRelation<TJoin> relation, WhereClip onWhere)
            where TJoin : Entity
        {
            return LeftJoin(relation, null, onWhere);
        }

        /// <summary>
        /// �����Ӳ�ѯ
        /// </summary>
        /// <typeparam name="TJoin"></typeparam>
        /// <param name="relation"></param>
        /// <param name="aliasName"></param>
        /// <param name="onWhere"></param>
        /// <returns></returns>
        public FromSection<T> LeftJoin<TJoin>(TableRelation<TJoin> relation, string aliasName, WhereClip onWhere)
            where TJoin : Entity
        {
            return Join<TJoin>(relation, aliasName, onWhere, JoinType.LeftJoin);
        }

        #endregion

        #region ������

        /// <summary>
        /// �����Ӳ�ѯ
        /// </summary>
        /// <typeparam name="TJoin"></typeparam>
        /// <param name="onWhere"></param>
        /// <returns></returns>
        public FromSection<T> RightJoin<TJoin>(WhereClip onWhere)
            where TJoin : Entity
        {
            return RightJoin<TJoin>((Table)null, onWhere);
        }

        /// <summary>
        /// �����Ӳ�ѯ
        /// </summary>
        /// <typeparam name="TJoin"></typeparam>
        /// <param name="table"></param>
        /// <param name="onWhere"></param>
        /// <returns></returns>
        public FromSection<T> RightJoin<TJoin>(Table table, WhereClip onWhere)
            where TJoin : Entity
        {
            return Join<TJoin>(table, onWhere, JoinType.RightJoin);
        }

        /// <summary>
        /// �����Ӳ�ѯ
        /// </summary>
        /// <typeparam name="TJoin"></typeparam>
        /// <param name="aliasName"></param>
        /// <param name="onWhere"></param>
        /// <returns></returns>
        public FromSection<T> RightJoin<TJoin>(string aliasName, WhereClip onWhere)
            where TJoin : Entity
        {
            return Join<TJoin>(aliasName, onWhere, JoinType.RightJoin);
        }

        /// <summary>
        /// �����Ӳ�ѯ
        /// </summary>
        /// <typeparam name="TJoin"></typeparam>
        /// <param name="relation"></param>
        /// <param name="onWhere"></param>
        /// <returns></returns>
        public FromSection<T> RightJoin<TJoin>(TableRelation<TJoin> relation, WhereClip onWhere)
            where TJoin : Entity
        {
            return RightJoin(relation, null, onWhere);
        }

        /// <summary>
        /// �����Ӳ�ѯ
        /// </summary>
        /// <typeparam name="TJoin"></typeparam>
        /// <param name="relation"></param>
        /// <param name="aliasName"></param>
        /// <param name="onWhere"></param>
        /// <returns></returns>
        public FromSection<T> RightJoin<TJoin>(TableRelation<TJoin> relation, string aliasName, WhereClip onWhere)
            where TJoin : Entity
        {
            return Join<TJoin>(relation, aliasName, onWhere, JoinType.RightJoin);
        }

        #endregion

        #region ˽�з���

        private FromSection<T> Join<TJoin>(TableRelation<TJoin> relation, string aliasName, WhereClip onWhere, JoinType joinType)
            where TJoin : Entity
        {
            TJoin entity = CoreHelper.CreateInstance<TJoin>();
            //entity.GetTable().As(aliasName);
            this.entityList.AddRange(relation.Section.entityList);

            if ((IField)query.PagingField == null)
            {
                //��ʶ�к���������,����ID���б�����
                query.PagingField = entity.PagingField;
            }

            FromSection<TJoin> from = new FromSection<TJoin>(null);

            string tableName = entity.GetTable().Name;
            if (aliasName != null) tableName = aliasName;

            from.tableName = "(" + relation.Section.query.QueryString + ") " + tableName;
            from.query.Parameters = relation.Section.query.Parameters;
            string strJoin = string.Empty;
            if (onWhere != null)
            {
                strJoin = " ON " + onWhere.ToString();
            }

            string join = GetJoinEnumString(joinType);

            if (this.relation != null)
            {
                this.tableName = " __[[ " + this.tableName;
                this.relation += " ]]__ ";
            }
            this.relation += join + from.TableName + strJoin;

            return this;
        }

        private FromSection<T> Join<TJoin>(Table table, WhereClip onWhere, JoinType joinType)
            where TJoin : Entity
        {
            TJoin entity = CoreHelper.CreateInstance<TJoin>();
            this.entityList.Add(entity);

            if ((IField)query.PagingField == null)
            {
                //��ʶ�к���������,����ID���б�����
                query.PagingField = entity.PagingField;
            }

            FromSection<TJoin> from = new FromSection<TJoin>(table);
            string strJoin = string.Empty;
            if (onWhere != null)
            {
                strJoin = " ON " + onWhere.ToString();
            }

            string join = GetJoinEnumString(joinType);

            if (this.relation != null)
            {
                this.tableName = " __[[ " + this.tableName;
                this.relation += " ]]__ ";
            }
            this.relation += join + from.TableName + strJoin;

            return this;
        }

        private FromSection<T> Join<TJoin>(string aliasName, WhereClip onWhere, JoinType joinType)
            where TJoin : Entity
        {

            TJoin entity = CoreHelper.CreateInstance<TJoin>();
            entity.GetTable().As(aliasName);
            this.entityList.Add(entity);

            if ((IField)query.PagingField == null)
            {
                //��ʶ�к���������,����ID���б�����
                query.PagingField = entity.PagingField;
            }

            FromSection<TJoin> from = new FromSection<TJoin>(null);
            if (aliasName != null)
            {
                if ((IField)query.PagingField != null)
                    query.PagingField = query.PagingField.At(aliasName);

                from.tableName += " __[" + aliasName + "]__ ";
            }
            string strJoin = string.Empty;
            if (onWhere != null)
            {
                strJoin = " on " + onWhere.ToString();
            }

            string join = GetJoinEnumString(joinType);

            if (this.relation != null)
            {
                this.tableName = " __[[ " + this.tableName;
                this.relation += " ]]__ ";
            }
            this.relation += join + from.TableName + strJoin;

            return this;
        }

        #endregion

        #endregion

        private string GetJoinEnumString(JoinType joinType)
        {
            switch (joinType)
            {
                case JoinType.LeftJoin:
                    return " LEFT OUTER JOIN ";
                case JoinType.RightJoin:
                    return " RIGHT OUTER JOIN ";
                case JoinType.InnerJoin:
                    return " INNER JOIN ";
                default:
                    return " INNER JOIN ";
            }
        }

        #region ���з���

        internal Field[] GetSelectFields()
        {
            IDictionary<string, Field> dictfields = new Dictionary<string, Field>();
            List<Field> fieldlist = new List<Field>();

            foreach (Entity entity in this.entityList)
            {
                Table table = entity.GetTable();
                Field[] fields = entity.GetFields();
                if (fields == null || fields.Length == 0)
                {
                    throw new DataException("û���κα�ѡ�е��ֶ��б���");
                }
                else
                {
                    foreach (Field field in fields)
                    {
                        if (!dictfields.ContainsKey(field.OriginalName))
                        {
                            if (table.AliasName != null)
                            {
                                dictfields.Add(field.OriginalName, field.At(table.AliasName));
                            }
                            else
                            {
                                dictfields.Add(field.OriginalName, field);
                            }
                        }
                    }
                }
            }

            //�����ֵ䣬��Fieldȡ��
            foreach (KeyValuePair<string, Field> kv in dictfields)
            {
                fieldlist.Add(kv.Value);
            }

            return fieldlist.ToArray();

        }

        #endregion

        #region �ڲ�����

        private string tableName;
        internal string TableName
        {
            get
            {
                return tableName;
            }
            set
            {
                tableName = value;
            }
        }

        private string relation;
        internal string Relation
        {
            get
            {
                return relation;
            }
            set
            {
                relation = value;
            }
        }

        #endregion
    }
}