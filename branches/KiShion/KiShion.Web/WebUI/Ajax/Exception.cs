#region usings

using System;
using System.Runtime.Serialization;
using System.Web;

#endregion

namespace KiShion.Web.UI
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
    /// AjaxCallback参数
    /// </summary>
    [Serializable]
    internal class AjaxCallbackParam
    {
        public bool Success { get; set; }
        public object Message { get; set; }

        public AjaxCallbackParam(object message)
        {
            this.Message = message;
            this.Success = true;
        }
    }
}