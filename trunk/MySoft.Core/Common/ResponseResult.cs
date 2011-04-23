using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft
{
    /// <summary>
    /// 响应返回
    /// </summary>
    [Serializable]
    public class ResponseResult
    {
        /// <summary>
        /// 响应的代码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 响应的消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 响应的数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 实例化ResponseResult
        /// </summary>
        public ResponseResult()
        {
            this.Code = 0;
            this.Message = "响应成功！";
        }

        /// <summary>
        ///  实例化ResponseResult
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public ResponseResult(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }
    }

    /// <summary>
    /// 响应返回
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ResponseResult<T> : ResponseResult
    {
        /// <summary>
        /// 响应的数据
        /// </summary>
        public new T Data
        {
            get { return (T)base.Data; }
            set { base.Data = value; }
        }

        /// <summary>
        /// 实体化DataResult
        /// </summary>
        public ResponseResult()
            : base()
        { }

        /// <summary>
        ///  实例化ResponseResult
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public ResponseResult(int code, string message)
            : base(code, message)
        { }
    }
}
