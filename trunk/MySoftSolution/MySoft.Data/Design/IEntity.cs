using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace MySoft.Data.Design
{
    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IEntity
    {
        #region 状态操作

        void Attach();
        void Attach(params Field[] removeFields);
        void Attach(ExcludeField removeField);

        void Detach();
        void Detach(params Field[] removeFields);
        void Detach(ExcludeField removeField);

        void AttachAll();
        void AttachAll(params Field[] removeFields);
        void AttachAll(ExcludeField removeField);

        void DetachAll();
        void DetachAll(params Field[] removeFields);
        void DetachAll(ExcludeField removeField);

        #endregion

        /// <summary>
        /// 获取字段列表
        /// </summary>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        Field[] GetFields(FieldType fieldType);
    }
}
