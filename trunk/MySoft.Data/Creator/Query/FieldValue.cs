using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Data
{
    /// <summary>
    /// 字段及值
    /// </summary>
    [Serializable]
    public class FieldValue
    {
        private Field field;
        /// <summary>
        /// 字段
        /// </summary>
        public Field Field
        {
            get { return field; }
        }

        private object fvalue;
        /// <summary>
        /// 值
        /// </summary>
        public object Value
        {
            get { return fvalue; }
            internal set { fvalue = value; }
        }

        private bool isIdentity;
        /// <summary>
        /// 是否标识列
        /// </summary>
        internal bool IsIdentity
        {
            get { return isIdentity; }
            set { isIdentity = value; }
        }

        private bool isPrimaryKey;
        /// <summary>
        /// 是否主键
        /// </summary>
        internal bool IsPrimaryKey
        {
            get { return isPrimaryKey; }
            set { isPrimaryKey = value; }
        }

        private bool isChanged;
        /// <summary>
        /// 是否更改
        /// </summary>
        internal bool IsChanged
        {
            get { return isChanged; }
            set { isChanged = value; }
        }

        /// <summary>
        /// 实例化FieldValue
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        public FieldValue(Field field, object value)
        {
            this.field = field;
            this.fvalue = value;
        }
    }
}
