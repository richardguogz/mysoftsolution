using System;
using System.Collections.Generic;
using System.Web;
using MySoft.Data;
using MySoft.Core;
using MySoft.Web;

namespace LiveChat.Web
{
    public class AjaxBaseControl<T> : BaseControl
    {
        protected DataPage<IList<T>> dataPage;
        protected HtmlPager pager;
    }
}
