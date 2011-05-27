using System;
using System.Collections.Generic;

namespace MySoft.Data
{

    /// <summary>
    /// 所有字段（特殊字段）
    /// </summary>
    [Serializable]
    public class AllField<T> : AllField
        where T : Entity
    {
        /// <summary>
        /// All实例化
        /// </summary>
        public AllField()
            : base()
        {
            this.tableName = Table.GetTable<T>().OriginalName;
        }

        /// <summary>
        /// 通过属性返回字段
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public override Field this[string propertyName]
        {
            get
            {
                return CoreHelper.CreateInstance<T>().As<IEntityBase>().GetField(propertyName);
            }
        }

        /// <summary>
        /// 通过属性返回字段
        /// </summary>
        /// <param name="propertyNames"></param>
        /// <returns></returns>
        public override Field[] Get(params string[] propertyNames)
        {
            var entity = CoreHelper.CreateInstance<T>().As<IEntityBase>();
            List<Field> list = new List<Field>();
            foreach (var propertyName in propertyNames)
            {
                var field = entity.GetField(propertyName);
                if (field != null) list.Add(field);
            }
            return list.ToArray();
        }
    }

    /// <summary>
    /// 所有字段（特殊字段）
    /// </summary>
    [Serializable]
    public class AllField : Field
    {
        /// <summary>
        /// All实例化
        /// </summary>
        public AllField() : base("All", null, "*") { }

        /// <summary>
        /// 选择被排除的列
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public ExcludeField Remove(params Field[] fields)
        {
            return new ExcludeField(fields);
        }

        /// <summary>
        /// 从实体中获取属性转换成Field
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public virtual Field this[string propertyName]
        {
            get
            {
                //throw new Exception("请通过Entity中的All字段进行操作！");
                return new Field(propertyName);
            }
        }

        /// <summary>
        /// 通过属性返回字段
        /// </summary>
        /// <param name="propertyNames"></param>
        /// <returns></returns>
        public virtual Field[] Get(params string[] propertyNames)
        {
            List<Field> list = new List<Field>();
            foreach (var propertyName in propertyNames)
            {
                var field = this[propertyName];
                if (field != null) list.Add(field);
            }
            return list.ToArray();
        }
    }

    /// <summary>
    /// 用于被排除的Field
    /// </summary>
    [Serializable]
    public class ExcludeField
    {
        private Field[] fields;
        internal List<Field> Fields
        {
            get
            {
                if (fields == null || fields.Length == 0)
                    return new List<Field>();

                return new List<Field>(fields);
            }
        }

        /// <summary>
        /// 实例化被排序的Field
        /// </summary>
        /// <param name="fields"></param>
        internal ExcludeField(Field[] fields)
        {
            this.fields = fields;
        }
    }
}
