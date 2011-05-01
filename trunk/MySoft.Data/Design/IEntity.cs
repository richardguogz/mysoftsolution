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
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        TEntity As<TEntity>();

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
        EntityState ObjectState { get; }

        /// <summary>
        /// 获取原始对象
        /// </summary>
        EntityBase OriginalObject { get; }

        /// <summary>
        /// 使用this获取值信息
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        object this[string propertyName] { get; set; }

        /// <summary>
        /// 使用this获取值信息
        /// </summary>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        object this[Field field] { get; set; }
    }
}

namespace MySoft.Data.Design
{
    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IEntity : IEntityBase
    {
        #region 状态操作

        void Attach();
        void Attach(params Field[] removeFields);

        void Detach();
        void Detach(params Field[] removeFields);

        void AttachAll();
        void AttachAll(params Field[] removeFields);

        void DetachAll();
        void DetachAll(params Field[] removeFields);

        void AttachSet(params Field[] setFields);
        void DetachSet(params Field[] setFields);

        #endregion
    }
}
