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
    /// 实体基类
    /// </summary>
    [Serializable]
    public abstract class Entity : IEntity
    {
        private List<Field> updatelist = new List<Field>();
        private List<Field> removeinsertlist = new List<Field>();
        private bool isUpdate = false;

        #region 公用方法

        #region 实体更新插入操作(对原有的字段)

        #region 改变更新状态

        /// <summary>
        /// 将实体置为修改状态
        /// </summary>
        public void Attach()
        {
            lock (this)
            {
                isUpdate = true;
            }
        }

        /// <summary>
        /// 将实体置为插入状态
        /// </summary>
        public void Detach()
        {
            lock (this)
            {
                isUpdate = false;
            }
        }

        #endregion

        #region 移除指定的列

        /// <summary>
        /// 将实体置为修改状态并移除指定的列
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
        /// 将实体置为插入状态并移除指定的列
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

        #region 指定移除以外的列

        /// <summary>
        /// 将实体置为修改状态并移除指定的列
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
        /// 将实体置为插入状态并移除指定的列
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

        #region 实体更新插入操作(对所有的字段)

        #region 改变更新状态

        /// <summary>
        /// 将实体置为修改状态(所有字段)
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
        /// 将实体置为插入状态(所有字段)
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

        #region 移除指定的列

        /// <summary>
        /// 将实体置为修改状态并移除指定的列(所有字段)
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
        /// 将实体置为插入状态并移除指定的列(所有字段)
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

        #region 指定移除以外的列

        /// <summary>
        /// 将实体置为修改状态并移除指定的列(所有字段)
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
        /// 将实体置为插入状态并移除指定的列(所有字段)
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
        /// 获取字段列表
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
        /// 获取全部中去除某些字段的列表
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

        #region 内部方法

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
        /// 获取系列的名称
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
        /// 获取字段列表
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
        /// 获取排序或分页字段
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
        /// 获取标识列
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
        /// 获取字段及值
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
                    throw new MySoftException("字段与值无法对应！"); ;
                }

                int index = 0;
                foreach (Field field in fields)
                {
                    FieldValue fv = new FieldValue(field, values[index]);

                    //判断是否为标识列
                    if ((IField)identityField != null)
                        if (identityField.Name == field.Name) fv.IsIdentity = true;

                    //判断是否为主键
                    if (pkFields.Contains(field)) fv.IsPrimaryKey = true;

                    if (isUpdate)
                    {
                        //如果是更新，则将更新的字段改变状态为true
                        if (updatelist.Contains(field)) fv.IsChanged = true;
                    }
                    else
                    {
                        //如果是插入，则将移除插入的字段改变状态为true
                        if (removeinsertlist.Contains(field)) fv.IsChanged = true;
                    }

                    fvlist.Add(fv);
                    index++;
                }

                return fvlist;
            }
        }

        #endregion

        #region 重载的方法

        /// <summary>
        /// 增加单个属性到修改列表
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
        /// 添加字段到修改列表
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
        /// 移除字段到修改列表
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
        /// 移除字段到修改列表
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

        #region 字段信息

        /// <summary>
        /// 返回标识列的名称（如Oracle中Sequence.nextval）
        /// </summary>
        /// <returns></returns>
        protected virtual string GetSequence()
        {
            return null;
        }

        /// <summary>
        /// 获取标识列
        /// </summary>
        /// <returns></returns>
        protected virtual Field GetIdentityField()
        {
            return null;
        }

        /// <summary>
        /// 获取主键列表
        /// </summary>
        /// <returns></returns>
        protected virtual Field[] GetPrimaryKeyFields()
        {
            return new Field[] { };
        }

        /// <summary>
        /// 获取字段列表
        /// </summary>
        /// <returns></returns>
        protected abstract Field[] GetFields();

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <returns></returns>
        protected abstract object[] GetValues();

        #endregion

        /// <summary>
        /// 获取只读属性
        /// </summary>
        /// <returns></returns>
        protected internal virtual bool GetReadOnly()
        {
            return false;
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <returns></returns>
        protected internal virtual Table GetTable()
        {
            return new Table("TempTable");
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="reader"></param>
        protected abstract void SetValues(IRowReader reader);

        /// <summary>
        /// 用于设置额外的值
        /// </summary>
        /// <param name="reader"></param>
        protected virtual void SetPropertyValues(IRowReader reader)
        { }

        /// <summary>
        /// 设置所有的值
        /// </summary>
        /// <param name="reader"></param>
        internal void SetAllValues(IRowReader reader)
        {
            lock (this)
            {
                //设置内部的值
                SetValues(reader);

                //设置外部的值
                SetPropertyValues(reader);
            }
        }

        #endregion
    }

    /// <summary>
    /// 字段类型枚举
    /// </summary>
    public enum FieldType
    {
        /// <summary>
        /// 所有字段
        /// </summary>
        All,
        /// <summary>
        /// 非主键列
        /// </summary>
        NotPrimaryKey,
        /// <summary>
        /// 非标识列
        /// </summary>
        NotIdentity,
        /// <summary>
        /// 主键字段
        /// </summary>
        PrimaryKey,
        /// <summary>
        /// 标识字段
        /// </summary>
        Identity
    }
}
