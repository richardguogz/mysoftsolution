using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Text;
using System.IO;

namespace MySoft.Web.UI
{
    /// <summary>
    /// 提供异步回调处理的接口
    /// </summary>
    public interface IAjaxProcessEventHandler
    {
        void OnAjaxProcess(CallbackParams callbackParams);
    }
}