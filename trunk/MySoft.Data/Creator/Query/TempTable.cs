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
        /// 隐藏GetFields()方法
        /// </summary>
        /// <returns></returns>
        private new Field[] GetFields()
        {
            return new Field[] { };
        }

        /// <summary>
        /// 隐藏GetValues()方法
        /// </summary>
        /// <returns></returns>
        private new object[] GetValues()
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
