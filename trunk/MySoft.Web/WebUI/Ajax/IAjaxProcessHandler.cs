using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Text;
using System.IO;

namespace MySoft.Web.UI
{
    /// <summary>
    /// �ṩ�첽�ص�����Ľӿ�
    /// </summary>
    public interface IAjaxProcessHandler
    {
        void OnAjaxProcess(CallbackParams callbackParams);
    }
}