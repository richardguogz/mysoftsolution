using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Data
{
    /// <summary>
    /// 关系表，可以用来存储关联信息
    /// </summary>
    public class TableRelation<T>
        where T : Entity
    {
        private FromSection<T> section;
        internal FromSection<T> Section
        {
            get { return section; }
        }

        public TableRelation()
        {
            this.section = new FromSection<T>(Table.From<T>());
        }

        #region 不带别名

        /// <summary>
        /// 左关联
        /// </summary>
        /// <param name="table"></param>
        /// <param name="onWhere"></param>
        /// <returns></returns>
        public TableRelation<T> LeftJoin(Table table, WhereClip onWhere)
        {
            section.LeftJoin<TempTable>(table, onWhere);
            return this;
        }

        /// <summary>
        /// 右关联
        /// </summary>
        /// <param name="table"></param>
        /// <param name="onWhere"></param>
        /// <returns></returns>
        public TableRelation<T> RightJoin(Table table, WhereClip onWhere)
        {
            section.RightJoin<TempTable>(table, onWhere);
            return this;
        }

        /// <summary>
        /// 内关联
        /// </summary>
        /// <param name="table"></param>
        /// <param name="onWhere"></param>
        /// <returns></returns>
        public TableRelation<T> InnerJoin(Table table, WhereClip onWhere)
        {
            section.InnerJoin<TempTable>(table, onWhere);
            return this;
        }

        #endregion

        #region 不带别名

        /// <summary>
        /// 左关联
        /// </summary>
        /// <param name="table"></param>
        /// <param name="onWhere"></param>
        /// <returns></returns>
        public TableRelation<T> LeftJoin<TJoin>(WhereClip onWhere)
            where TJoin : Entity
        {
            section.LeftJoin<TJoin>(onWhere);
            return this;
        }

        /// <summary>
        /// 右关联
        /// </summary>
        /// <param name="table"></param>
        /// <param name="onWhere"></param>
        /// <returns></returns>
        public TableRelation<T> RightJoin<TJoin>(WhereClip onWhere)
            where TJoin : Entity
        {
            section.RightJoin<TJoin>(onWhere);
            return this;
        }

        /// <summary>
        /// 内关联
        /// </summary>
        /// <param name="table"></param>
        /// <param name="onWhere"></param>
        /// <returns></returns>
        public TableRelation<T> InnerJoin<TJoin>(WhereClip onWhere)
            where TJoin : Entity
        {
            section.InnerJoin<TJoin>(onWhere);
            return this;
        }

        #endregion

        #region 带别名

        /// <summary>
        /// 左关联
        /// </summary>
        /// <param name="table"></param>
        /// <param name="onWhere"></param>
        /// <returns></returns>
        public TableRelation<T> LeftJoin<TJoin>(string aliasName, WhereClip onWhere)
            where TJoin : Entity
        {
            section.LeftJoin<TJoin>(aliasName, onWhere);
            return this;
        }

        /// <summary>
        /// 右关联
        /// </summary>
        /// <param name="table"></param>
        /// <param name="onWhere"></param>
        /// <returns></returns>
        public TableRelation<T> RightJoin<TJoin>(string aliasName, WhereClip onWhere)
            where TJoin : Entity
        {
            section.RightJoin<TJoin>(aliasName, onWhere);
            return this;
        }

        /// <summary>
        /// 内关联
        /// </summary>
        /// <param name="table"></param>
        /// <param name="onWhere"></param>
        /// <returns></returns>
        public TableRelation<T> InnerJoin<TJoin>(string aliasName, WhereClip onWhere)
            where TJoin : Entity
        {
            section.InnerJoin<TJoin>(aliasName, onWhere);
            return this;
        }

        #endregion
    }
}
