using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Text;
using System.IO;

namespace MySoft.Web.UI
{
    /// <summary>
    /// 提供异步初始化处理的接口
    /// </summary>
    public interface IAjaxInitEventHandler
    {
        void OnAjaxInit();
    }
}