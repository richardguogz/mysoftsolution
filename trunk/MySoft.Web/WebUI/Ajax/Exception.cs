#region usings

using System;
using System.Runtime.Serialization;
using System.Web;

#endregion

namespace MySoft.Web.UI
{
    /// <summary>
    /// Ajax调用异常类
    /// </summary>
    [Serializable]
    public class AjaxException : HttpException
    {
        public AjaxException() : base("Ajax Service Error.") { }
        public AjaxException(string ErrorMessage) : base(ErrorMessage) { }
        protected AjaxException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public AjaxException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// AjaxMethod参数
    /// </summary>
    [Serializable]
    internal class AjaxMethodParam
    {
        public bool Success { get; set; }
        public object Message { get; set; }

        public AjaxMethodParam(object message)
        {
            this.Message = message;
            this.Success = true;
        }
    }

    /// <summary>
    /// AjaxControl参数
    /// </summary>
    [Serializable]
    internal class AjaxControlParam
    {
        public bool Success { get; set; }
        public string Content { get; set; }

        public AjaxControlParam(string content)
        {
            this.Content = content;
            this.Success = true;
        }
    }
}