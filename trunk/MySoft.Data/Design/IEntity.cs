using System.Collections.Generic;

namespace MySoft.Data
{
    /// <summary>
    /// 实体基类接口
    /// </summary>
    public interface IEntityBase
    {
        /// <summary>
        /// 转换成另一对象
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        TResult As<TResult>();

        /// <summary>
        /// 返回一个行阅读对象
        /// </summary>
        IRowReader ToRowReader();

        /// <summary>
        /// 返回字典对象
        /// </summary>
        /// <returns></returns>
        IDictionary<string, object> ToDictionary();

        /// <summary>
        /// 克隆一个对象
        /// </summary>
        /// <returns></returns>
        EntityBase CloneObject();

        /// <summary>
        /// 获取对象状态
        /// </summary>
        EntityState GetObjectState();

        /// <summary>
        /// 获取原始对象
        /// </summary>
        EntityBase GetOriginalObject();

        /// <summary>
        /// 使用propertyName获取值信息
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        object GetValue(string propertyName);

        /// <summary>
        /// 使用propertyName获设置信息
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        void SetValue(string propertyName, object value);

        /// <summary>
        /// 使用field获取值信息
        /// </summary>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        object GetValue(Field field);

        /// <summary>
        /// 使用field获设置信息
        /// </summary>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        void SetValue(Field field, object value);

        /// <summary>
        /// 通过属性获取字段
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        Field GetField(string propertyName);
    }
}

namespace MySoft.Data.Design
{
    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IEntity
    {
        #region 状态操作

        void Attach(params Field[] removeFields);
        void AttachSet(params Field[] setFields);
        void AttachAll(params Field[] removeFields);

        void Detach(params Field[] removeFields);
        void DetachSet(params Field[] setFields);
        void DetachAll(params Field[] removeFields);

        #endregion
    }
}
