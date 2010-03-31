using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Data
{
    /// <summary>
    /// SQL命令的参数
    /// </summary>
    [Serializable]
    public class SQLParameter
    {
        /// <summary>
        /// 参数名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 初始化OrmParameter
        /// </summary>
        /// <param name="pName"></param>
        public SQLParameter(string pName)
        {
            this.Name = pName;
        }

        /// <summary>
        /// 初始化OrmParameter
        /// </summary>
        /// <param name="pName"></param>
        /// <param name="pValue"></param>
        public SQLParameter(string pName, object pValue)
            : this(pName)
        {
            this.Value = pValue;
        }
    }
}
