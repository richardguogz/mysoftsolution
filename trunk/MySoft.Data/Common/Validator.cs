﻿using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data.Design;

namespace MySoft.Data
{
    /// <summary>
    /// 实体验证类
    /// </summary>
    public class Validator<T>
        where T : Entity
    {
        private T entity;
        private List<Field> vlist;

        /// <summary>
        /// 实例化验证类
        /// </summary>
        /// <param name="entity"></param>
        public Validator(T entity)
        {
            this.entity = entity;
            this.messages = new List<string>();

            //获取需要处理的字段列表
            if (entity.As<IEntityBase>().State == EntityState.Insert)
            {
                this.vlist = entity.GetFieldValues().ConvertAll<Field>(fv => !fv.IsChanged ? fv.Field : null);
            }
            else
            {
                this.vlist = entity.GetFieldValues().ConvertAll<Field>(fv => fv.IsChanged ? fv.Field : null);
            }
            this.vlist.RemoveAll(p => (IField)p == null);
        }

        private IList<string> messages;
        /// <summary>
        /// 错误消息队列
        /// </summary>
        public IList<string> Messages
        {
            get { return messages; }
        }

        /// <summary>
        /// 验证实体属性的有效性并返回错误列表(验证所有的列)
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Validator<T> Check(Predicate<T> predicate, string message)
        {
            if (predicate(this.entity))
            {
                this.messages.Add(message);
            }
            return this;
        }

        /// <summary>
        /// 验证实体属性的有效性并返回错误列表(只验证需要插入或更新的列)
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="field"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Validator<T> Check(Predicate<T> predicate, Field field, string message)
        {
            if (this.vlist.Exists(p => p.Name == field.Name))
            {
                if (predicate(this.entity))
                {
                    this.messages.Add(string.Format("{0}|{1}", message, field.PropertyName));
                }
            }
            return this;
        }
    }
}
