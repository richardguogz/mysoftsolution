using System.Collections.Generic;

namespace MySoft.Data
{
    /// <summary>
    /// ʵ�����ӿ�
    /// </summary>
    public interface IEntityBase
    {
        /// <summary>
        /// ת������һ����
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        TEntity As<TEntity>();

        /// <summary>
        /// ����һ�����Ķ�����
        /// </summary>
        IRowReader ToRowReader();

        /// <summary>
        /// �����ֵ����
        /// </summary>
        /// <returns></returns>
        IDictionary<string, object> ToDictionary();

        /// <summary>
        /// ��¡һ������
        /// </summary>
        /// <returns></returns>
        EntityBase CloneObject();

        /// <summary>
        /// ��ȡ����״̬
        /// </summary>
        EntityState ObjectState { get; }

        /// <summary>
        /// ��ȡԭʼ����
        /// </summary>
        EntityBase OriginalObject { get; }

        /// <summary>
        /// ʹ��this��ȡֵ��Ϣ
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        object this[string propertyName] { get; set; }

        /// <summary>
        /// ʹ��this��ȡֵ��Ϣ
        /// </summary>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        object this[Field field] { get; set; }
    }
}

namespace MySoft.Data.Design
{
    /// <summary>
    /// ʵ��ӿ�
    /// </summary>
    public interface IEntity : IEntityBase
    {
        #region ״̬����

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
