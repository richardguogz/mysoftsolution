#region usings

using System;
using System.Runtime.Serialization;
using System.Web;
using MySoft.Core;

#endregion

namespace MySoft.Web.UI
{
    /// <summary>
    /// AjaxCallback²ÎÊý
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