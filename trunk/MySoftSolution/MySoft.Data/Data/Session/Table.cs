using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Table<T> : Table
    {
        /// <summary>
        /// 实例化一个表
        /// </summary>
        /// <param name="tableName"></param>
        public Table(string tableName)
            : base(tableName)
        {
            this.Prefix = EntityConfig.Instance.GetPrefix<T>();
        }
    }

    /// <summary>
    /// 数据库查询时用户传入的自定义信息表
    /// </summary>
    [Serializable]
    public class Table : ITable
    {
        private static readonly IDictionary<Type, Table> dictTable = new Dictionary<Type, Table>();

        /// <summary>
        /// 返回一个Field
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public Field this[string fieldName]
        {
            get
            {
                return new Field(fieldName).At(this);
            }
        }

        private string name;
        /// <summary>
        /// 表名称
        /// </summary>
        internal string Name
        {
            get
            {
                return string.Concat("{0}", OriginalName, "{1}");
            }
        }

        private string aliasName;
        /// <summary>
        /// 表别名
        /// </summary>
        internal string AliasName
        {
            get
            {
                return aliasName;
            }
            set
            {
                aliasName = value;
            }
        }

        private string prefix;
        /// <summary>
        /// 设置表前缀
        /// </summary>
        public string Prefix
        {
            set { prefix = value; }
        }

        private string suffix;
        /// <summary>
        /// 设置表后缀
        /// </summary>
        public string Suffix
        {
            set { suffix = value; }
        }

        /// <summary>
        /// 获取原始的表名
        /// </summary>
        public string OriginalName
        {
            get
            {
                return string.Format("{0}{1}{2}", prefix, name, suffix);
            }
        }

        /// <summary>
        /// 实例化一个表
        /// </summary>
        /// <param name="tableName"></param>
        public Table(string tableName)
        {
            this.name = tableName.Replace("{0}", "").Replace("{1}", "");
            this.prefix = EntityConfig.Instance.DefaultPrefix;
            this.suffix = null;
        }

        /// <summary>
        /// 返回一个DbTable实例
        /// </summary>
        /// <returns></returns>
        public static Table From<T>()
            where T : Entity
        {
            lock (dictTable)
            {
                if (dictTable.ContainsKey(typeof(T)))
                {
                    return dictTable[typeof(T)];
                }
                else
                {
                    Table table = DataUtils.CreateInstance<T>().GetTable();
                    dictTable.Add(typeof(T), table);

                    return table;
                }
            }
        }

        /// <summary>
        /// 返回一个DbTable实例
        /// </summary>
        /// <param name="suffix">后缀名称</param>
        /// <returns></returns>
        public static Table From<T>(string suffix)
            where T : Entity
        {
            Table table = From<T>();
            table.Suffix = suffix;

            return table;
        }

        /// <summary>
        /// 返回一个DbTable实例
        /// </summary>
        /// <param name="prefix">前缀名称</param>
        /// <param name="suffix">后缀名称</param>
        /// <returns></returns>
        public static Table From<T>(string prefix, string suffix)
            where T : Entity
        {
            Table table = From<T>();
            table.Prefix = prefix;
            table.Suffix = suffix;

            return table;
        }
    }
}
