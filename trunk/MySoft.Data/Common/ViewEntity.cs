using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Data
{
    /// <summary>
    /// 只用于显示的实体
    /// </summary>
    [Serializable]
    public class ViewEntity : Entity
    {
        /// <summary>
        /// 重载GetFields()方法
        /// </summary>
        /// <returns></returns>
        internal protected override Field[] GetFields()
        {
            return new Field[] { };
        }

        /// <summary>
        /// 重载GetValues()方法
        /// </summary>
        /// <returns></returns>
        protected override object[] GetValues()
        {
            return new object[] { };
        }

        protected override void SetValues(IRowReader reader)
        { }

        /// <summary>
        /// 隐藏SetPropertyValues()方法
        /// </summary>
        /// <param name="reader"></param>
        private new void SetPropertyValues(IRowReader reader)
        {
            base.SetPropertyValues(reader);
        }
    }
}
