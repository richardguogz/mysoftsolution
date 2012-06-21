using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data.Design;

namespace MySoft.Data
{
    /// <summary>
    /// ����ʵ���ڵ�Field����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Field<T> : Field
        where T : Entity
    {
        public Field(string fieldName)
            : this(fieldName, fieldName)
        { }

        public Field(string propertyName, string fieldName)
            : base(propertyName, null, fieldName, null)
        {
            this.tableName = Table.GetTable<T>().OriginalName;

            Field field = EntityConfig.Instance.GetMappingField<T>(propertyName, fieldName);
            this.fieldName = field.OriginalName;
        }
    }

    /// <summary>
    /// ����ʵ���ڵ�Field����
    /// </summary>
    [Serializable]
    public class Field : IField
    {
        /// <summary>
        /// �ֶ�*
        /// </summary>
        public static readonly AllField All = new AllField();

        protected string propertyName;
        protected string tableName;
        protected string fieldName;
        protected string aliasName;

        #region ����FieldValue

        /// <summary>
        /// ����һ��FieldValue
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public FieldValue Set(object value)
        {
            return new FieldValue(this, value);
        }

        /// <summary>
        /// ����һ��FieldValue
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public FieldValue Set(DbValue value)
        {
            return new FieldValue(this, value);
        }

        /// <summary>
        /// ����һ��FieldValue
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public FieldValue Set(Field field)
        {
            return new FieldValue(this, field);
        }

        #endregion

        #region ���������ֶ�

        /// <summary>
        /// ����һ�������ֶ�
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public static Field Create(string fieldName, QueryCreator creator)
        {
            return new CustomField(fieldName, creator);
        }

        /// <summary>
        /// ����һ�������ֶ�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static Field Create<T>(string fieldName, QuerySection<T> query)
            where T : Entity
        {
            return new CustomField<T>(fieldName, query);
        }

        /// <summary>
        /// ����һ�������ֶ�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="relation"></param>
        /// <returns></returns>
        public static Field Create<T>(string fieldName, TableRelation<T> relation)
            where T : Entity
        {
            return new CustomField<T>(fieldName, relation);
        }

        #endregion

        #region �����ֶ�

        internal string FullName
        {
            get
            {
                if (aliasName == null)
                {
                    return this.Name;
                }
                return string.Format("{0} as __[{1}]__", this.Name, aliasName);
            }
        }

        /// <summary>
        /// ��ȡ��ʵ�ֶ���
        /// </summary>
        internal virtual string Name
        {
            get
            {
                if (tableName == null)
                {
                    return FieldName;
                }
                return TableName + "." + FieldName;
            }
        }

        /// <summary>
        /// ��ȡԭʼ���ֶ���
        /// </summary>
        public string OriginalName
        {
            get
            {
                if (aliasName != null)
                {
                    return aliasName;
                }

                if (fieldName.Contains("__$") || fieldName.Contains("$__"))
                {
                    return fieldName.Replace("__$", "").Replace("$__", "");
                }
                else
                {
                    return fieldName;
                }
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public string PropertyName
        {
            get
            {
                return propertyName;
            }
        }

        #region ˽�г�Ա

        private string TableName
        {
            get
            {
                if (tableName == null || tableName.Contains("__[") || tableName.Contains("]__"))
                {
                    return tableName;
                }

                return "__[" + tableName + "]__";
            }
        }

        private string FieldName
        {
            get
            {
                if (fieldName.Contains("__$") || fieldName.Contains("$__"))
                {
                    return fieldName.Replace("__$", "").Replace("$__", "");
                }
                else if (fieldName == "*" || fieldName.Contains("'") || fieldName.Contains("(") || fieldName.Contains(")") || fieldName.Contains("__[") || fieldName.Contains("]__"))
                {
                    return fieldName;
                }

                return "__[" + fieldName + "]__";
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// ʵ����Field
        /// </summary>
        /// <param name="fieldName"></param>
        public Field(string fieldName)
        {
            this.fieldName = fieldName;
            this.propertyName = this.OriginalName;
        }

        /// <summary>
        /// ʵ����Field
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        internal Field(string tableName, string fieldName)
            : this(fieldName)
        {
            this.tableName = string.IsNullOrEmpty(tableName) ? null : tableName;
        }

        /// <summary>
        /// ʵ����Field
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="aliasName"></param>
        internal Field(string propertyName, string tableName, string fieldName, string aliasName)
            : this(tableName, fieldName)
        {
            this.propertyName = propertyName;
            this.aliasName = string.IsNullOrEmpty(aliasName) ? null : aliasName;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return this.FieldName == (obj as Field).FieldName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region �������

        public OrderByClip Asc
        {
            get
            {
                return new OrderByClip(this.Name + " asc ");
            }
        }

        public OrderByClip Desc
        {
            get
            {
                return new OrderByClip(this.Name + " desc ");
            }
        }

        public GroupByClip Group
        {
            get
            {
                return new GroupByClip(this.Name);
            }
        }

        #endregion

        #region ��������

        #region ����������

        public static WhereClip operator ==(Field leftField, Field rightField)
        {
            if ((IField)leftField == null)
            {
                return null;
            }
            if ((IField)rightField == null)
            {
                return null;
            }
            return new WhereClip(leftField.Name + " = " + rightField.Name);
        }

        public static WhereClip operator !=(Field leftField, Field rightField)
        {
            if ((IField)leftField == null)
            {
                return null;
            }
            if ((IField)rightField == null)
            {
                return null;
            }
            return new WhereClip(leftField.Name + " <> " + rightField.Name);
        }

        public static WhereClip operator >(Field leftField, Field rightField)
        {
            if ((IField)leftField == null)
            {
                return null;
            }
            if ((IField)rightField == null)
            {
                return null;
            }
            return new WhereClip(leftField.Name + " > " + rightField.Name);
        }

        public static WhereClip operator >=(Field leftField, Field rightField)
        {
            if ((IField)leftField == null)
            {
                return null;
            }
            if ((IField)rightField == null)
            {
                return null;
            }
            return new WhereClip(leftField.Name + " >= " + rightField.Name);
        }

        public static WhereClip operator <(Field leftField, Field rightField)
        {
            if ((IField)leftField == null)
            {
                return null;
            }
            if ((IField)rightField == null)
            {
                return null;
            }
            return new WhereClip(leftField.Name + " < " + rightField.Name);
        }

        public static WhereClip operator <=(Field leftField, Field rightField)
        {
            if ((IField)leftField == null)
            {
                return null;
            }
            if ((IField)rightField == null)
            {
                return null;
            }
            return new WhereClip(leftField.Name + " <= " + rightField.Name);
        }

        public static WhereClip operator ==(Field field, object value)
        {
            if ((IField)field == null)
            {
                return null;
            }
            return CreateWhereClip(field, "=", value);
        }

        public static WhereClip operator !=(Field field, object value)
        {
            if ((IField)field == null)
            {
                return null;
            }
            return CreateWhereClip(field, "<>", value);
        }

        public static WhereClip operator >(Field field, object value)
        {
            if ((IField)field == null)
            {
                return null;
            }
            return CreateWhereClip(field, ">", value);
        }

        public static WhereClip operator >=(Field field, object value)
        {
            if ((IField)field == null)
            {
                return null;
            }
            return CreateWhereClip(field, ">=", value);
        }

        public static WhereClip operator <(Field field, object value)
        {
            if ((IField)field == null)
            {
                return null;
            }
            return CreateWhereClip(field, "<", value);
        }

        public static WhereClip operator <=(Field field, object value)
        {
            if ((IField)field == null)
            {
                return null;
            }
            return CreateWhereClip(field, "<=", value);
        }

        #endregion

        #region �����ֶ�

        #region ����������

        public static Field operator +(Field leftField, Field rightField)
        {
            if ((IField)leftField == null)
            {
                return null;
            }
            if ((IField)rightField == null)
            {
                return null;
            }
            return new Field("(" + leftField.Name + " + " + rightField.Name + ")").As(leftField.OriginalName);
        }

        public static Field operator -(Field leftField, Field rightField)
        {
            if ((IField)leftField == null)
            {
                return null;
            }
            if ((IField)rightField == null)
            {
                return null;
            }
            return new Field("(" + leftField.Name + " - " + rightField.Name + ")").As(leftField.OriginalName);
        }

        public static Field operator *(Field leftField, Field rightField)
        {
            if ((IField)leftField == null)
            {
                return null;
            }
            if ((IField)rightField == null)
            {
                return null;
            }
            return new Field("(" + leftField.Name + " * " + rightField.Name + ")").As(leftField.OriginalName);
        }

        public static Field operator /(Field leftField, Field rightField)
        {
            if ((IField)leftField == null)
            {
                return null;
            }
            if ((IField)rightField == null)
            {
                return null;
            }
            return new Field("(" + leftField.Name + " / " + rightField.Name + ")").As(leftField.OriginalName);
        }

        public static Field operator %(Field leftField, Field rightField)
        {
            if ((IField)leftField == null)
            {
                return null;
            }
            if ((IField)rightField == null)
            {
                return null;
            }
            return new Field("(" + leftField.Name + " % " + rightField.Name + ")").As(leftField.OriginalName);
        }

        public static Field operator +(Field field, object value)
        {
            if ((IField)field == null)
            {
                return null;
            }
            return new Field(field.Name + " + " + DataHelper.FormatValue(value)).As(field.OriginalName);
        }

        public static Field operator -(Field field, object value)
        {
            if ((IField)field == null)
            {
                return null;
            }
            return new Field(field.Name + " - " + DataHelper.FormatValue(value)).As(field.OriginalName);
        }

        public static Field operator *(Field field, object value)
        {
            if ((IField)field == null)
            {
                return null;
            }
            return new Field(field.Name + " * " + DataHelper.FormatValue(value)).As(field.OriginalName);
        }

        public static Field operator /(Field field, object value)
        {
            if ((IField)field == null)
            {
                return null;
            }
            return new Field(field.Name + " / " + DataHelper.FormatValue(value)).As(field.OriginalName);
        }

        public static Field operator %(Field field, object value)
        {
            if ((IField)field == null)
            {
                return null;
            }
            return new Field(field.Name + " % " + DataHelper.FormatValue(value)).As(field.OriginalName);
        }

        public static Field operator +(object value, Field field)
        {
            if ((IField)field == null)
            {
                return null;
            }
            return new Field(DataHelper.FormatValue(value) + " + " + field.Name).As(field.OriginalName);
        }

        public static Field operator -(object value, Field field)
        {
            if ((IField)field == null)
            {
                return null;
            }
            return new Field(DataHelper.FormatValue(value) + " - " + field.Name).As(field.OriginalName);
        }

        public static Field operator *(object value, Field field)
        {
            if ((IField)field == null)
            {
                return null;
            }
            return new Field(DataHelper.FormatValue(value) + " * " + field.Name).As(field.OriginalName);
        }

        public static Field operator /(object value, Field field)
        {
            if ((IField)field == null)
            {
                return null;
            }
            return new Field(DataHelper.FormatValue(value) + " / " + field.Name).As(field.OriginalName);
        }

        public static Field operator %(object value, Field field)
        {
            if ((IField)field == null)
            {
                return null;
            }
            return new Field(DataHelper.FormatValue(value) + " % " + field.Name).As(field.OriginalName);
        }

        #endregion

        #region �ֶβ���

        /// <summary>
        /// ���ֶν���Distinct����
        /// </summary>
        /// <returns></returns>
        public Field Distinct()
        {
            return new Field("distinct(" + this.Name + ")");
        }

        /// <summary>
        /// ���ֶν���Count����
        /// </summary>
        /// <returns></returns>
        public Field Count()
        {
            return new Field("count(" + this.Name + ")").As(this.OriginalName);
        }

        /// <summary>
        /// ���ֶν���Sum����
        /// </summary>
        /// <returns></returns>
        public Field Sum()
        {
            return new Field("sum(" + this.Name + ")").As(this.OriginalName);
        }

        /// <summary>
        /// ���ֶν���Avg����
        /// </summary>
        /// <returns></returns>
        public Field Avg()
        {
            return new Field("avg(" + this.Name + ")").As(this.OriginalName);
        }

        /// <summary>
        /// ���ֶν���Max����
        /// </summary>
        /// <returns></returns>
        public Field Max()
        {
            return new Field("max(" + this.Name + ")").As(this.OriginalName);
        }

        /// <summary>
        /// ���ֶν���Min����
        /// </summary>
        /// <returns></returns>
        public Field Min()
        {
            return new Field("min(" + this.Name + ")").As(this.OriginalName);
        }

        /// <summary>
        /// �����ֶεı���
        /// </summary>
        /// <param name="aliasName"></param>
        /// <returns></returns>
        public Field As(string aliasName)
        {
            return new Field(this.propertyName, this.tableName, this.fieldName, aliasName);
        }

        /// <summary>
        /// �����ֶ����ڱ�
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public Field At(string tableName)
        {
            if (fieldName.Contains("(") || fieldName.Contains(")"))
                return this;
            else
                return new Field(this.propertyName, tableName, this.fieldName, aliasName);
        }

        /// <summary>
        /// �����ֶ����ڱ�
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public Field At(Table table)
        {
            if (table == null) return this;

            //�жϱ���
            if (table.Alias != null)
                return At(table.Alias);
            else
                return At(table.Name);
        }

        #region ��������

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="function"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static Field Func(string function, params Field[] fields)
        {
            if (fields != null && fields.Length > 0)
            {
                List<string> list = new List<string>();
                foreach (var field in fields)
                {
                    list.Add(field.Name);
                }

                return new Field(string.Format(function, list.ToArray()));
            }
            else
                return new Field(function);
        }

        #endregion

        #endregion

        #endregion

        /// <summary>
        /// ����һ��ֵΪnull������
        /// </summary>
        /// <returns></returns>
        public WhereClip IsNull()
        {
            return this == (object)null;
        }

        #region Like��ѯ

        /// <summary>
        /// ָ��value����ģ����ѯ
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WhereClip Contains(string value)
        {
            return Like("%" + value + "%");
        }

        /// <summary>
        /// ָ��value����Like��ѯ
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WhereClip Like(string value)
        {
            return CreateWhereClip(this, "like", value);
        }

        /// <summary>
        /// ָ��value����Like��ѯ
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WhereClip StartsWith(string value)
        {
            return Like(value + "%");
        }

        /// <summary>
        /// ָ��value����Like��ѯ
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WhereClip EndsWith(string value)
        {
            return Like("%" + value);
        }

        #endregion

        /// <summary>
        /// ����Between����
        /// </summary>
        /// <param name="leftValue"></param>
        /// <param name="rightValue"></param>
        /// <returns></returns>
        public WhereClip Between(object leftValue, object rightValue)
        {
            string p0Name = CoreHelper.MakeUniqueKey(100, "$");
            SQLParameter p0 = new SQLParameter(p0Name);
            p0.Value = leftValue;

            string p1Name = CoreHelper.MakeUniqueKey(100, "$");
            SQLParameter p1 = new SQLParameter(p1Name);
            p1.Value = rightValue;

            string where = string.Format("{0} between {1} and {2}", this.Name, p0Name, p1Name);

            return new WhereClip(where, p0, p1);
            //return this >= leftValue && this <= rightValue;
        }

        /// <summary>
        /// ����In�Ӳ�ѯ����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public WhereClip In<T>(Field field)
            where T : Entity
        {
            return In<T>(null, field);
        }

        /// <summary>
        /// ����In�Ӳ�ѯ����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public WhereClip In<T>(Table table, Field field)
            where T : Entity
        {
            return In<T>(table, field, WhereClip.None);
        }

        /// <summary>
        /// ����In�Ӳ�ѯ����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public WhereClip In<T>(Field field, WhereClip where)
            where T : Entity
        {
            return In<T>(null, field, where);
        }

        /// <summary>
        /// ����In�Ӳ�ѯ����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public WhereClip In<T>(Table table, Field field, WhereClip where)
            where T : Entity
        {
            return In<T>(new FromSection<T>(table, null).Select(field).Where(where));
        }

        /// <summary>
        /// ����In����,queryΪһ���Ӳ�ѯ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public WhereClip In<T>(QuerySection<T> query)
            where T : Entity
        {
            return new WhereClip(this.Name + " in (" + query.QueryString + ") ", query.Parameters);
        }

        /// <summary>
        /// ����In����,relationΪһ��������ѯ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="relation"></param>
        /// <returns></returns>
        public WhereClip In<T>(TableRelation<T> relation)
            where T : Entity
        {
            QuerySection<T> q = relation.GetFromSection().Query;
            return new WhereClip(this.Name + " in (" + q.QueryString + ") ", q.Parameters);
        }

        /// <summary>
        /// ����In�Ӳ�ѯ����
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public WhereClip In(params object[] values)
        {
            values = DataHelper.CheckAndReturnValues(values);

            //���ֵֻ��һ����ʱ��ֱ��ʹ����ȴ���
            if (values.Length == 1)
            {
                return this == values[0];
            }
            else
            {
                List<SQLParameter> plist = new List<SQLParameter>();
                StringBuilder sb = new StringBuilder();
                foreach (object value in values)
                {
                    string pName = CoreHelper.MakeUniqueKey(100, "$");
                    SQLParameter p = new SQLParameter(pName);
                    p.Value = value;

                    sb.Append(pName);
                    sb.Append(",");

                    plist.Add(p);
                }

                string where = sb.Remove(sb.Length - 1, 1).ToString().Trim();

                return new WhereClip(this.Name + " in (" + where + ") ", plist.ToArray());
            }
        }

        /// <summary>
        /// ����In�Ӳ�ѯ����
        /// </summary>
        /// <param name="creator"></param>
        /// <returns></returns>
        public WhereClip In(QueryCreator creator)
        {
            QuerySection<ViewEntity> query = GetQuery(creator);
            return In<ViewEntity>(query);
        }

        #endregion

        #region ˽�з���

        internal QuerySection<ViewEntity> GetQuery(QueryCreator creator)
        {
            if (creator.Table == null)
            {
                throw new DataException("�ô���������ʱ������Ϊnull��");
            }

            FromSection<ViewEntity> f = new FromSection<ViewEntity>(creator.Table, null);
            if (creator.IsRelation)
            {
                foreach (TableJoin join in creator.Relations.Values)
                {
                    if (join.Type == JoinType.LeftJoin)
                        f.LeftJoin<ViewEntity>(join.Table, join.Where);
                    else if (join.Type == JoinType.RightJoin)
                        f.RightJoin<ViewEntity>(join.Table, join.Where);
                    else
                        f.InnerJoin<ViewEntity>(join.Table, join.Where);
                }
            }

            QuerySection<ViewEntity> query = f.Select(creator.Fields).Where(creator.Where)
                    .OrderBy(creator.OrderBy);
            return query;
        }


        /// <summary>
        /// ����һ��������ʽ��WhereClip
        /// </summary>
        /// <param name="field"></param>
        /// <param name="join"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static WhereClip CreateWhereClip(Field field, string join, object value)
        {
            if (value == null)
            {
                if (join == "=")
                    return new WhereClip(field.Name + " is null");
                else if (join == "<>")
                    return new WhereClip(field.Name + " is not null");
                else
                    throw new DataException("��ֵΪnullʱֻ��Ӧ����=��<>������");
            }

            string pName = CoreHelper.MakeUniqueKey(100, "$p_");
            SQLParameter p = new SQLParameter(pName);
            p.Value = value;

            string where = string.Format("{0} {1} {2}", field.Name, join, pName);

            return new WhereClip(where, p);
        }

        #endregion
    }
}