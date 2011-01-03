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

        private string dbvalue;
        public DBValue(string dbvalue)
        {
            this.dbvalue = dbvalue;
        }

        internal string Value
        {
            get { return dbvalue; }
        }
    }
}
