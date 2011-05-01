using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace MySoft
{
    /// <summary>
    /// 响应返回
    /// </summary>
    [Serializable]
    public class ResponseResult
    {
        /// <summary>
        /// 响应的状态代码（跟http的状态码对应，默认为200）
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// 响应的代码（可用于自定义代码）
        /// </summary>
        public string ResponseCode { get; set; }

        /// <summary>
        /// 响应的消息
        /// </summary>
        public string ResponseMessage { get; set; }

        /// <summary>
        /// 响应的对应
        /// </summary>
        public object ResponseValue { get; set; }

        /// <summary>
        /// 实例化ResponseResult
        /// </summary>
        public ResponseResult()
        {
            this.StatusCode = HttpStatusCode.OK;
            this.ResponseMessage = "响应成功！";
        }

        /// <summary>
        ///  实例化ResponseResult
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public ResponseResult(HttpStatusCode code, string message)
        {
            this.StatusCode = code;
            this.ResponseMessage = message;
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
        /// 响应的对应
        /// </summary>
        public new T ResponseValue
        {
            get { return (T)base.ResponseValue; }
            set { base.ResponseValue = value; }
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
        public ResponseResult(HttpStatusCode code, string message)
            : base(code, message)
        { }
    }
}
