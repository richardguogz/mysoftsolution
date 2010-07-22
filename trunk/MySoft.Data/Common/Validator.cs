using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Data
{
    /// <summary>
    /// 实体验证类
    /// </summary>
    public class Validator<T>
        where T : Entity
    {
        private T entity;

        /// <summary>
        /// 实例化验证类
        /// </summary>
        /// <param name="entity"></param>
        public Validator(T entity)
        {
            this.entity = entity;
            this.messages = new List<string>();
        }

        private IList<string> messages;
        /// <summary>
        /// 错误消息队列
        /// </summary>
        public IList<string> Messages
        {
            get { return messages; }
        }

        public Validator<T> Validate(Predicate<T> predicate, string message)
        {
            if (!predicate(this.entity))
            {
                this.messages.Add(message);
            }
            return this;
        }
    }
}
