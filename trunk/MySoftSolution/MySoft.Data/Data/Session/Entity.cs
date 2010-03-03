using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Reflection;
using MySoft.Data.Design;

namespace MySoft.Data
{
    /// <summary>
    /// ʵ�����
    /// </summary>
    [Serializable]
    public abstract class Entity : IEntity
    {
        private List<Field> updatelist = new List<Field>();
        private List<Field> removeinsertlist = new List<Field>();
        private bool isUpdate = false;

        #region ���÷���

        #region ʵ����²������(��ԭ�е��ֶ�)

        #region �ı����״̬

        /// <summary>
        /// ��ʵ����Ϊ�޸�״̬
        /// </summary>
        public void Attach()
        {
            lock (this)
            {
                isUpdate = true;
            }
        }

        /// <summary>
        /// ��ʵ����Ϊ����״̬
        /// </summary>
        public void Detach()
        {
            lock (this)
            {
                isUpdate = false;
            }
        }

        #endregion

        #region �Ƴ�ָ������

        /// <summary>
        /// ��ʵ����Ϊ�޸�״̬���Ƴ�ָ������
        /// </summary>
        /// <param name="removeFields"></param>
        public void Attach(params Field[] removeFields)
        {
            lock (this)
            {
                isUpdate = true;
                RemoveFieldsToUpdate(removeFields);
            }
        }

        /// <summary>
        /// ��ʵ����Ϊ����״̬���Ƴ�ָ������
        /// </summary>
        /// <param name="removeFields"></param>
        public void Detach(params Field[] removeFields)
        {
            lock (this)
            {
                isUpdate = false;
                RemoveFieldsToInsert(removeFields);
            }
        }

        #endregion

        #region ָ���Ƴ��������

        /// <summary>
        /// ��ʵ����Ϊ�޸�״̬���Ƴ�ָ������
        /// </summary>
        /// <param name="removeField"></param>
        public void Attach(ExcludeField removeField)
        {
            lock (this)
            {
                isUpdate = true;
                List<Field> list = new List<Field>(this.GetFields());
                list.RemoveAll(f =>
                {
                    if (removeField.Fields.Contains(f)) return true;
                    return false;
                });
                RemoveFieldsToUpdate(list);
            }
        }

        /// <summary>
        /// ��ʵ����Ϊ����״̬���Ƴ�ָ������
        /// </summary>
        /// <param name="removeField"></param>
        public void Detach(ExcludeField removeField)
        {
            lock (this)
            {
                isUpdate = false;
                List<Field> list = new List<Field>(this.GetFields());
                list.RemoveAll(f =>
                {
                    if (removeField.Fields.Contains(f)) return true;
                    return false;
                });
                RemoveFieldsToInsert(list);
            }
        }

        #endregion

        #endregion

        #region ʵ����²������(�����е��ֶ�)

        #region �ı����״̬

        /// <summary>
        /// ��ʵ����Ϊ�޸�״̬(�����ֶ�)
        /// </summary>
        public void AttachAll()
        {
            lock (this)
            {
                AddFieldsToUpdate(this.GetFields());
                Attach();
            }
        }

        /// <summary>
        /// ��ʵ����Ϊ����״̬(�����ֶ�)
        /// </summary>
        public void DetachAll()
        {
            lock (this)
            {
                removeinsertlist.Clear();
                Detach();
            }
        }

        #endregion

        #region �Ƴ�ָ������

        /// <summary>
        /// ��ʵ����Ϊ�޸�״̬���Ƴ�ָ������(�����ֶ�)
        /// </summary>
        /// <param name="removeFields"></param>
        public void AttachAll(params Field[] removeFields)
        {
            lock (this)
            {
                AddFieldsToUpdate(this.GetFields());
                Attach(removeFields);
            }
        }

        /// <summary>
        /// ��ʵ����Ϊ����״̬���Ƴ�ָ������(�����ֶ�)
        /// </summary>
        /// <param name="removeFields"></param>
        public void DetachAll(params Field[] removeFields)
        {
            lock (this)
            {
                removeinsertlist.Clear();
                Detach(removeFields);
            }
        }

        #endregion

        #region ָ���Ƴ��������

        /// <summary>
        /// ��ʵ����Ϊ�޸�״̬���Ƴ�ָ������(�����ֶ�)
        /// </summary>
        /// <param name="removeField"></param>
        public void AttachAll(ExcludeField removeField)
        {
            lock (this)
            {
                AddFieldsToUpdate(this.GetFields());
                Attach(removeField);
            }
        }

        /// <summary>
        /// ��ʵ����Ϊ����״̬���Ƴ�ָ������(�����ֶ�)
        /// </summary>
        /// <param name="removeField"></param>
        public void DetachAll(ExcludeField removeField)
        {
            lock (this)
            {
                removeinsertlist.Clear();
                Detach(removeField);
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// ��ȡ�ֶ��б�
        /// </summary>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        public Field[] GetFields(FieldType fieldType)
        {
            Field[] fields = null;
            switch (fieldType)
            {
                case FieldType.All:
                    fields = this.GetFields();
                    break;
                case FieldType.Identity:
                    fields = this.GetIdentityField() == null ? null : new Field[] { this.GetIdentityField() };
                    break;
                case FieldType.PrimaryKey:
                    fields = this.GetPrimaryKeyFields();
                    break;
                case FieldType.NotIdentity:
                    fields = RemoveFields(this.GetIdentityField());
                    break;
                case FieldType.NotPrimaryKey:
                    fields = RemoveFields(this.GetPrimaryKeyFields());
                    break;
            }

            return fields;
        }

        /// <summary>
        /// ��ȡȫ����ȥ��ĳЩ�ֶε��б�
        /// </summary>
        /// <param name="removeFields"></param>
        /// <returns></returns>
        private Field[] RemoveFields(params Field[] removeFields)
        {
            if (removeFields == null || removeFields.Length == 0)
                return this.GetFields();

            List<Field> list = this.Fields as List<Field>;
            foreach (Field field in removeFields)
            {
                list.RemoveAll(p => p.Name == field.Name);
            }

            return list.ToArray();
        }

        #endregion

        #region �ڲ�����

        internal bool IsUpdate
        {
            get
            {
                lock (this)
                {
                    return isUpdate;
                }
            }
        }

        /// <summary>
        /// ��ȡϵ�е�����
        /// </summary>
        internal string SequenceName
        {
            get
            {
                lock (this)
                {
                    return this.GetSequence();
                }
            }
        }

        /// <summary>
        /// ��ȡ�ֶ��б�
        /// </summary>
        /// <returns></returns>
        internal IList<Field> Fields
        {
            get
            {
                lock (this)
                {
                    return new List<Field>(this.GetFields());
                }
            }
        }

        /// <summary>
        /// ��ȡ������ҳ�ֶ�
        /// </summary>
        /// <returns></returns>
        internal Field PagingField
        {
            get
            {
                lock (this)
                {
                    Field pagingField = this.GetIdentityField();

                    if ((IField)pagingField == null)
                    {
                        Field[] fields = this.GetPrimaryKeyFields();
                        if (fields.Length > 0) pagingField = fields[0];
                    }

                    return pagingField;
                }
            }
        }

        /// <summary>
        /// ��ȡ��ʶ��
        /// </summary>
        internal Field IdentityField
        {
            get
            {
                lock (this)
                {
                    return this.GetIdentityField();
                }
            }
        }

        /// <summary>
        /// ��ȡ�ֶμ�ֵ
        /// </summary>
        /// <returns></returns>
        internal List<FieldValue> GetFieldValues()
        {
            lock (this)
            {
                List<FieldValue> fvlist = new List<FieldValue>();

                Field identityField = this.GetIdentityField();
                List<Field> pkFields = new List<Field>(this.GetPrimaryKeyFields());

                Field[] fields = this.GetFields();
                object[] values = this.GetValues();

                if (fields.Length != values.Length)
                {
                    throw new MySoftException("�ֶ���ֵ�޷���Ӧ��"); ;
                }

                int index = 0;
                foreach (Field field in fields)
                {
                    FieldValue fv = new FieldValue(field, values[index]);

                    //�ж��Ƿ�Ϊ��ʶ��
                    if ((IField)identityField != null)
                        if (identityField.Name == field.Name) fv.IsIdentity = true;

                    //�ж��Ƿ�Ϊ����
                    if (pkFields.Contains(field)) fv.IsPrimaryKey = true;

                    if (isUpdate)
                    {
                        //����Ǹ��£��򽫸��µ��ֶθı�״̬Ϊtrue
                        if (updatelist.Contains(field)) fv.IsChanged = true;
                    }
                    else
                    {
                        //����ǲ��룬���Ƴ�������ֶθı�״̬Ϊtrue
                        if (removeinsertlist.Contains(field)) fv.IsChanged = true;
                    }

                    fvlist.Add(fv);
                    index++;
                }

                return fvlist;
            }
        }

        #endregion

        #region ���صķ���

        /// <summary>
        /// ���ӵ������Ե��޸��б�
        /// </summary>
        /// <param name="field"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected void OnPropertyValueChange(Field field, object oldValue, object newValue)
        {
            lock (this)
            {
                if (oldValue == null && newValue == null)
                {
                    AddFieldsToUpdate(new Field[] { field });
                }
                else if (oldValue != null)
                {
                    if (!oldValue.Equals(newValue))
                    {
                        AddFieldsToUpdate(new Field[] { field });
                    }
                }
                else if (newValue != null)
                {
                    if (!newValue.Equals(oldValue))
                    {
                        AddFieldsToUpdate(new Field[] { field });
                    }
                }
            }
        }

        /// <summary>
        /// ����ֶε��޸��б�
        /// </summary>
        /// <param name="fields"></param>
        private void AddFieldsToUpdate(IList<Field> fields)
        {
            if (fields == null || fields.Count == 0) return;

            foreach (Field field in fields)
            {
                if (!updatelist.Exists(p => p.Name == field.Name))
                {
                    updatelist.Add(field);
                }
            }
        }

        /// <summary>
        /// �Ƴ��ֶε��޸��б�
        /// </summary>
        /// <param name="fields"></param>
        private void RemoveFieldsToUpdate(IList<Field> fields)
        {
            if (fields == null || fields.Count == 0) return;

            foreach (Field field in fields)
            {
                updatelist.RemoveAll(p => p.Name == field.Name);
            }
        }

        /// <summary>
        /// �Ƴ��ֶε��޸��б�
        /// </summary>
        /// <param name="fields"></param>
        private void RemoveFieldsToInsert(IList<Field> fields)
        {
            if (fields == null || fields.Count == 0) return;

            foreach (Field field in fields)
            {
                if (!removeinsertlist.Exists(p => p.Name == field.Name))
                {
                    removeinsertlist.Add(field);
                }
            }
        }

        #region �ֶ���Ϣ

        /// <summary>
        /// ���ر�ʶ�е����ƣ���Oracle��Sequence.nextval��
        /// </summary>
        /// <returns></returns>
        protected virtual string GetSequence()
        {
            return null;
        }

        /// <summary>
        /// ��ȡ��ʶ��
        /// </summary>
        /// <returns></returns>
        protected virtual Field GetIdentityField()
        {
            return null;
        }

        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <returns></returns>
        protected virtual Field[] GetPrimaryKeyFields()
        {
            return new Field[] { };
        }

        /// <summary>
        /// ��ȡ�ֶ��б�
        /// </summary>
        /// <returns></returns>
        protected abstract Field[] GetFields();

        /// <summary>
        /// ��ȡ����ֵ
        /// </summary>
        /// <returns></returns>
        protected abstract object[] GetValues();

        #endregion

        /// <summary>
        /// ��ȡֻ������
        /// </summary>
        /// <returns></returns>
        protected internal virtual bool GetReadOnly()
        {
            return false;
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <returns></returns>
        protected internal virtual Table GetTable()
        {
            return new Table("TempTable");
        }

        /// <summary>
        /// ��������ֵ
        /// </summary>
        /// <param name="reader"></param>
        protected abstract void SetValues(IRowReader reader);

        /// <summary>
        /// �������ö����ֵ
        /// </summary>
        /// <param name="reader"></param>
        protected virtual void SetPropertyValues(IRowReader reader)
        { }

        /// <summary>
        /// �������е�ֵ
        /// </summary>
        /// <param name="reader"></param>
        internal void SetAllValues(IRowReader reader)
        {
            lock (this)
            {
                //�����ڲ���ֵ
                SetValues(reader);

                //�����ⲿ��ֵ
                SetPropertyValues(reader);
            }
        }

        #endregion
    }

    /// <summary>
    /// �ֶ�����ö��
    /// </summary>
    public enum FieldType
    {
        /// <summary>
        /// �����ֶ�
        /// </summary>
        All,
        /// <summary>
        /// ��������
        /// </summary>
        NotPrimaryKey,
        /// <summary>
        /// �Ǳ�ʶ��
        /// </summary>
        NotIdentity,
        /// <summary>
        /// �����ֶ�
        /// </summary>
        PrimaryKey,
        /// <summary>
        /// ��ʶ�ֶ�
        /// </summary>
        Identity
    }
}
