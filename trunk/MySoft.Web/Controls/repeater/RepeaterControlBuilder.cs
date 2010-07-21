using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace MySoft.Web.Controls
{
    public class RepeaterControlBuilder : ControlBuilder
    {
        public override void Init(TemplateParser parser, ControlBuilder parentBuilder, Type type, string tagName, string id, System.Collections.IDictionary attribs)
        {
            string dataItemTypeName = attribs["DataItemType"] as string;
            Type dataItemType = typeof(object);
            if (!string.IsNullOrEmpty(dataItemTypeName))
            {
                dataItemType = BuildManager.GetType(dataItemTypeName, true);
            }
            Type repeaterFakeType = new RepeaterFakeType(dataItemType);

            base.Init(parser, parentBuilder, repeaterFakeType, tagName, id, attribs);
        }
    }
}