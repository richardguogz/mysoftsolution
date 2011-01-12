using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft
{
    /// <summary>
    /// 数据返回
    /// </summary>
    public class DataResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 实体化DataResult
        /// </summary>
        public DataResult()
        {
            this.IsSuccess = true;
            this.Message = "返回成功！";
        }
    }

    /// <summary>
    /// 数据返回
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class DataResult<T> : DataResult
    {
        /// <summary>
        /// 返回值
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// 实体化DataResult
        /// </summary>
        public DataResult()
            : base()
        { }

        /// <summary>
        /// 实体化DataResult
        /// </summary>
        /// <param name="value"></param>
        public DataResult(T value)
            : base()
        {
            this.Value = value;
        }
    }
}
