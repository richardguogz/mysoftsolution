using System.Text;
using System.Web;
using System.Web.SessionState;

namespace MySoft.Web
{
    /// <summary>
    /// �첽����Handler
    /// </summary>
    public class AjaxPageHandler : IHttpHandler, IRequiresSessionState
    {
        // ժҪ:
        //     ��ȡһ��ֵ����ֵָʾ���������Ƿ����ʹ�� System.Web.IHttpHandler ʵ����
        //
        // ���ؽ��:
        //     ��� System.Web.IHttpHandler ʵ�����ٴ�ʹ�ã���Ϊ true������Ϊ false��
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        // ժҪ:
        //     ͨ��ʵ�� System.Web.IHttpHandler �ӿڵ��Զ��� HttpHandler ���� HTTP Web ����Ĵ�����
        //
        // ����:
        //   context:
        //     System.Web.HttpContext �������ṩ������Ϊ HTTP �����ṩ������ڲ������������� Request��Response��Session
        //     �� Server�������á�
        public void ProcessRequest(HttpContext context)
        {
            string ajaxKey = "AjaxProcess", url = string.Empty, space = string.Empty;
            string[] split = context.Request.Url.Query.Remove(0, 1).Split(';');

            url = CoreHelper.Decrypt(split[0], ajaxKey);
            url = (url.IndexOf('/') >= 0 ? url : "/" + url);
            if (split.Length > 1) space = CoreHelper.Decrypt(split[1], ajaxKey);
            if (string.IsNullOrEmpty(space)) space = "AjaxMethods";

            StringBuilder sb = new StringBuilder();
            sb.Append("var ajaxRequestInfo = { \r\n");
            sb.Append("\t\turl : '" + url + "',\r\n");
            sb.Append("\t\tkey : '" + WebHelper.MD5Encrypt(ajaxKey) + "'\r\n");
            sb.Append("\t};\r\n\r\n");
            sb.Append(string.Format("var {0} = Ajax.registerPage(this);", space));

            //д��javascript����
            context.Response.ContentType = "application/x-javascript";

            //��javascript����������ļ�
            context.Response.Clear();
            context.Response.Write(sb.ToString());
            context.Response.Flush();
            context.Response.End();
        }
    }
}