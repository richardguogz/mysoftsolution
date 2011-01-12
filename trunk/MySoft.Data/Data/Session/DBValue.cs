using System;

namespace MySoft.Data
{
    /// <summary>
    /// 数据库值
    /// </summary>
    [Serializable]
    public class DBValue
    {
        /// <summary>
        /// 系统时间
        /// </summary>
        public static DBValue DateTime
        {
            get
            {
                return new DBValue("getdate()");
            }
        }

        /// <summary>
        /// 返回默认值
        /// </summary>
        public static DBValue Default
        {
            get
            {
                return new DBValue("$$$___$$$___$$$");
            }
        }

        private string dbvalue;
        public DBValue(string dbvalue)
        {
            this.dbvalue = dbvalue;
        }

        internal string Value
        {
            get { return dbvalue; }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DBValue)) return false;
            return string.Compare(this.Value, (obj as DBValue).Value, true) == 0;
        }
    }
}
