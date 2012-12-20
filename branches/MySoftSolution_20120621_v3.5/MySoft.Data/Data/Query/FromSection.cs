using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySoft.Data.Design;

namespace MySoft.Data
{
    /// <summary>
    /// Form�����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FromSection<T> : IQuerySection<T>
        where T : Entity
    {
        private QuerySection<T> query;
        /// <summary>
        /// ��ǰ��ѯ����
        /// </summary>
        internal QuerySection<T> Query
        {
            get { return query; }
            set { query = value; }
        }

        private List<TableEntity> entities = new List<TableEntity>();
        /// <summary>
        /// ��ǰʵ���б�
        /// </summary>
        internal List<TableEntity> TableEntities
        {
            get { return entities; }
            set { entities = value; }
        }

        #region ��ʼ��FromSection

        internal FromSection(DbProvider dbProvider, DbTrans dbTran, Table table, string aliasName)
        {
            InitForm(table, aliasName);

            this.query = new QuerySection<T>(this, dbProvider, dbTran);
        }

        internal FromSection(Table table, string aliasName)
        {
            InitForm(table, aliasName);

            this.query = new QuerySection<T>(this);
        }

        internal void InitForm(Table table, string aliasName)
        {
            var entity = CoreHelper.CreateInstance<T>();
            table = table ?? entity.GetTable();

            table.As(aliasName);
            var tableEntity = new TableEntity { Table = table, Entity = entity };
            this.entities.Add(tableEntity);

            SetTableName(table);
        }

        internal void SetTableName(Table table)
        {
            this.tableName = table.FullName;
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

        #region ͨ��Field��������

        /// <summary>
        /// ����GroupBy����
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public QuerySection<T> GroupBy(Field[] fields)
        {
            return query.GroupBy(fields);
        }

        /// <summary>
        /// ����OrderBy����
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public QuerySection<T> OrderBy(Field[] fields, bool desc)
        {
            return query.OrderBy(fields, desc);
        }

        #endregion

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
        /// <param name="filter"></param>
        /// <returns></returns>
        public QuerySection<T> Select(IFieldFilter filter)
        {
            return query.Select(filter);
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
        public QuerySection<T> GetTop(int topSize)
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
        /// ����һ����ҳ�����Page��
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
        /// ����һ��DataSet
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public DataSet ToDataSet(int startIndex, int endIndex)
        {
            return query.ToDataSet(startIndex, endIndex);
        }

        /// <summary>
        /// ����һ��DataSet
        /// </summary>
        /// <returns></returns>
        public DataSet ToDataSet()
        {
            return query.ToDataSet();
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

        /// <summary>
        /// ����ָ������Դ�ķ�ҳ��Ϣ
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public DataPage<DataSet> ToDataSetPage(int pageSize, int pageIndex)
        {
            return query.ToDataSetPage(pageSize, pageIndex);
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
            return Join<TJoin>(table, null, onWhere, JoinType.InnerJoin);
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
            return Join<TJoin>((Table)null, aliasName, onWhere, JoinType.InnerJoin);
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
            return Join<TJoin>(table, null, onWhere, JoinType.LeftJoin);
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
            return Join<TJoin>((Table)null, aliasName, onWhere, JoinType.LeftJoin);
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
            return Join<TJoin>(table, null, onWhere, JoinType.RightJoin);
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
            return Join<TJoin>((Table)null, aliasName, onWhere, JoinType.RightJoin);
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
            //��TableRelation�Ķ�����ӵ���ǰ��
            this.entities.AddRange(relation.GetFromSection().TableEntities);

            TJoin entity = CoreHelper.CreateInstance<TJoin>();
            var table = entity.GetTable().As(aliasName);

            if ((IField)query.PagingField == null)
            {
                //��ʶ�к���������,����ID���б�����
                query.SetPagingField(entity.PagingField);
            }

            string tableName = entity.GetTable().Name;
            if (aliasName != null) tableName = aliasName;

            //����tableRelation��ϵ
            string joinString = "(" + relation.GetFromSection().Query.QueryString + ") " + tableName;
            this.query.Parameters = relation.GetFromSection().Query.Parameters;

            string strJoin = string.Empty;
            if (onWhere != null)
            {
                strJoin = " ON " + onWhere.ToString();
            }

            //��ȡ������ʽ
            string join = GetJoinEnumString(joinType);

            if (this.relation != null)
            {
                this.tableName = " __[[ " + this.tableName;
                this.relation += " ]]__ ";
            }
            this.relation += join + joinString + strJoin;

            return this;
        }

        private FromSection<T> Join<TJoin>(Table table, string aliasName, WhereClip onWhere, JoinType joinType)
            where TJoin : Entity
        {
            TJoin entity = CoreHelper.CreateInstance<TJoin>();
            table = table ?? entity.GetTable();
            table.As(aliasName);

            //����һ��TableEntity
            var tableEntity = new TableEntity
            {
                Table = table,
                Entity = entity
            };

            this.entities.Add(tableEntity);

            if ((IField)query.PagingField == null)
            {
                //��ʶ�к���������,����ID���б�����
                query.SetPagingField(entity.PagingField);
            }

            string strJoin = string.Empty;
            if (onWhere != null)
            {
                strJoin = " ON " + onWhere.ToString();
            }

            //��ȡ������ʽ
            string join = GetJoinEnumString(joinType);

            if (this.relation != null)
            {
                this.tableName = " __[[ " + this.tableName;
                this.relation += " ]]__ ";
            }
            this.relation += join + table.FullName + strJoin;

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

        internal Field GetPagingField()
        {
            foreach (TableEntity entity in this.entities)
            {
                var field = entity.Entity.PagingField;
                if ((IField)field != null)
                {
                    return field.At(entity.Table);
                }
            }

            return null;
        }

        internal Field[] GetSelectFields()
        {
            var dictfields = new Dictionary<string, Field>();
            foreach (TableEntity entity in this.entities)
            {
                Table table = entity.Table;
                Field[] fields = entity.Entity.GetFields();
                if (fields == null || fields.Length == 0)
                {
                    throw new DataException("û���κα�ѡ�е��ֶ��б�");
                }
                else
                {
                    foreach (Field field in fields)
                    {
                        //ȥ���ظ����ֶ�
                        if (!dictfields.ContainsKey(field.OriginalName))
                        {
                            dictfields[field.OriginalName] = field.At(table);
                        }
                    }
                }
            }

            //����ѡ�е��ֶ�
            return dictfields.Select(p => p.Value).ToArray();
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
