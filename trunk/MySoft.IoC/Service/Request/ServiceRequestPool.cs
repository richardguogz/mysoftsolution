﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySoft.Net.Client;

namespace MySoft.IoC
{
    /// <summary>
    /// 服务请求池
    /// </summary>
    internal sealed class ServiceRequestPool<T>
    {
        /// <summary>
        /// ServiceRequest栈
        /// </summary>
        private Stack<ServiceRequest<T>> pool;

        /// <summary>
        /// 初始化ServiceRequest池
        /// </summary>
        /// <param name="capacity">最大可能使用的ServiceRequest对象.</param>
        internal ServiceRequestPool(Int32 capacity)
        {
            this.pool = new Stack<ServiceRequest<T>>(capacity);
        }

        /// <summary>
        /// 返回ServiceRequest池中的 数量
        /// </summary>
        internal Int32 Count
        {
            get { return this.pool.Count; }
        }

        /// <summary>
        /// 弹出一个ServiceRequest
        /// </summary>
        /// <returns>ServiceRequest removed from the pool.</returns>
        internal ServiceRequest<T> Pop()
        {
            if (this.Count > 0)
            {
                lock (this.pool)
                {
                    return this.pool.Pop();
                }
            }

            return null;
        }

        /// <summary>
        /// 添加一个 ServiceRequest
        /// </summary>
        /// <param name="item">ServiceRequest instance to add to the pool.</param>
        internal void Push(ServiceRequest<T> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Items added to a ServiceRequestPool cannot be null");
            }
            lock (this.pool)
            {
                this.pool.Push(item);
            }
        }
    }
}