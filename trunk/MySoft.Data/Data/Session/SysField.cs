﻿using System;

namespace MySoft.Data
{
    interface ISysField
    {
        /// <summary>
        /// 设置驱动
        /// </summary>
        /// <param name="dbProvider"></param>
        /// <param name="dbTran"></param>
        void SetProvider(DbProvider dbProvider, DbTrans dbTran);
    }

    /// <summary>
    /// 系统字段
    /// </summary>
    [Serializable]
    internal class SysField : Field, ISysField
    {
        private QueryCreator creator;
        private string qString;
        public SysField(string fieldName, QueryCreator creator)
            : base(fieldName)
        {
            this.creator = creator;
        }

        /// <summary>
        /// 处理qString;
        /// </summary>
        /// <param name="dbProvider"></param>
        /// <param name="dbTran"></param>
        public void SetProvider(DbProvider dbProvider, DbTrans dbTran)
        {
            if (creator != null)
            {
                var query = GetQuery(creator);
                query.SetDbProvider(dbProvider, dbTran);
                qString = query.GetTop(1).QueryString;
            }
        }

        /// <summary>
        /// 重载名称
        /// </summary>
        internal override string Name
        {
            get
            {
                if (string.IsNullOrEmpty(qString))
                {
                    throw new DataException("需要设置DbProvider及DbTrans才能处理SysField！");
                }
                return string.Format("({1}) as {0}", base.Name, qString);
            }
        }
    }

    /// <summary>
    /// 系统字段
    /// </summary>
    [Serializable]
    internal class SysField<T> : Field, ISysField
        where T : Entity
    {
        private TableRelation<T> relation;
        private string qString;
        public SysField(string fieldName, QuerySection<T> query)
            : base(fieldName)
        {
            this.qString = query.GetTop(1).QueryString;
        }

        public SysField(string fieldName, TableRelation<T> relation)
            : base(fieldName)
        {
            this.relation = relation;
        }

        /// <summary>
        /// 处理qString;
        /// </summary>
        /// <param name="dbProvider"></param>
        /// <param name="dbTran"></param>
        public void SetProvider(DbProvider dbProvider, DbTrans dbTran)
        {
            if (string.IsNullOrEmpty(qString) && relation != null)
            {
                var query = relation.Section.Query;
                query.SetDbProvider(dbProvider, dbTran);
                qString = query.GetTop(1).QueryString;
            }
        }

        /// <summary>
        /// 重载名称
        /// </summary>
        internal override string Name
        {
            get
            {
                if (string.IsNullOrEmpty(qString))
                {
                    throw new DataException("需要设置DbProvider及DbTrans才能处理SysField！");
                }
                return string.Format("({1}) as {0}", base.Name, qString);
            }
        }
    }
}
