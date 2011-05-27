﻿using System;

namespace MySoft.Data
{
    /// <summary>
    /// 系统字段
    /// </summary>
    [Serializable]
    internal class SysField : Field
    {
        public SysField(string fieldName, QueryCreator creator)
            : base(fieldName)
        {
            string qString = GetQuery(creator).QueryString;
            base.fieldName = string.Format("{0} = ({1})", base.Name, qString);
        }

        /// <summary>
        /// 重载名称
        /// </summary>
        internal override string Name
        {
            get
            {
                return base.OriginalName;
            }
        }
    }

    /// <summary>
    /// 系统字段
    /// </summary>
    [Serializable]
    internal class SysField<T> : Field
        where T : Entity
    {
        public SysField(string fieldName, QuerySection<T> query)
            : base(fieldName)
        {
            string qString = query.QueryString;
            base.fieldName = string.Format("{0} = ({1})", base.Name, qString);
        }

        public SysField(string fieldName, TopSection<T> top)
            : base(fieldName)
        {
            string qString = top.QueryString;
            base.fieldName = string.Format("{0} = ({1})", base.Name, qString);
        }

        public SysField(string fieldName, TableRelation<T> relation)
            : base(fieldName)
        {
            string qString = relation.Section.Query.QueryString;
            base.fieldName = string.Format("{0} = ({1})", base.Name, qString);
        }

        /// <summary>
        /// 重载名称
        /// </summary>
        internal override string Name
        {
            get
            {
                return base.OriginalName;
            }
        }
    }
}
