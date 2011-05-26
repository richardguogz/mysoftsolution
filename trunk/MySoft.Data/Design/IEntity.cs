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
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        TResult As<TResult>();

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
        EntityState GetObjectState();

        /// <summary>
        /// ��ȡԭʼ����
        /// </summary>
        EntityBase GetOriginalObject();

        /// <summary>
        /// ʹ��propertyName��ȡֵ��Ϣ
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        object GetValue(string propertyName);

        /// <summary>
        /// ʹ��propertyName��������Ϣ
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        void SetValue(string propertyName, object value);

        /// <summary>
        /// ʹ��field��ȡֵ��Ϣ
        /// </summary>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        object GetValue(Field field);

        /// <summary>
        /// ʹ��field��������Ϣ
        /// </summary>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        void SetValue(Field field, object value);

        /// <summary>
        /// ͨ�����Ի�ȡ�ֶ�
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        Field GetField(string propertyName);
    }
}

namespace MySoft.Data.Design
{
    /// <summary>
    /// ʵ��ӿ�
    /// </summary>
    public interface IEntity
    {
        #region ״̬����

        void Attach(params Field[] removeFields);
        void AttachSet(params Field[] setFields);
        void AttachAll(params Field[] removeFields);

        void Detach(params Field[] removeFields);
        void DetachSet(params Field[] setFields);
        void DetachAll(params Field[] removeFields);

        #endregion
    }
}
