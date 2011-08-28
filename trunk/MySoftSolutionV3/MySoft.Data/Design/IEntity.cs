using System.Collections.Generic;

namespace MySoft.Data
{
    /// <summary>
    /// ʵ�������Ϣ
    /// </summary>
    public interface IEntityInfo
    {
        /// <summary>
        /// ����Ϣ
        /// </summary>
        Table Table { get; }

        /// <summary>
        /// �ֶ���Ϣ
        /// </summary>
        Field[] Fields { get; }

        /// <summary>
        /// �ֶμ�ֵ��Ϣ
        /// </summary>
        FieldValue[] FieldValues { get; }

        /// <summary>
        /// �����ֶ�
        /// </summary>
        Field[] UpdateFields { get; }

        /// <summary>
        /// �����ֶμ�ֵ��Ϣ
        /// </summary>
        FieldValue[] UpdateFieldValues { get; }

        /// <summary>
        /// �Ƿ��޸�
        /// </summary>
        bool IsUpdate { get; }

        /// <summary>
        /// �Ƿ�ֻ�� (ֻ��ʱΪ��ͼ���Զ���ʵ��)
        /// </summary>
        bool IsReadOnly { get; }
    }

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

        /// <summary>
        /// ��Ϊ�޸�״̬���Ƴ��ֶ�
        /// </summary>
        /// <param name="removeFields"></param>
        void Attach(params Field[] removeFields);

        /// <summary>
        /// ��Ϊ�޸�״̬�������ֶ�
        /// </summary>
        /// <param name="setFields"></param>
        void AttachSet(params Field[] setFields);

        /// <summary>
        /// ��Ϊ�޸�״̬���Ƴ��ֶ�
        /// </summary>
        /// <param name="removeFields"></param>
        void AttachAll(params Field[] removeFields);

        /// <summary>
        /// ��Ϊ����״̬���Ƴ��ֶ�
        /// </summary>
        /// <param name="removeFields"></param>
        void Detach(params Field[] removeFields);

        /// <summary>
        /// ��Ϊ����״̬�������ֶ�
        /// </summary>
        /// <param name="setFields"></param>
        void DetachSet(params Field[] setFields);

        /// <summary>
        /// ��Ϊ����״̬���Ƴ��ֶ�
        /// </summary>
        /// <param name="removeFields"></param>
        void DetachAll(params Field[] removeFields);

        #endregion
    }
}
