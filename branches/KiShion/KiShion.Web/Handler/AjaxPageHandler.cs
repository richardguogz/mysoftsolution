using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using KiShion.Web.UI;

namespace KiShion.Web
{
    /// <summary>
    /// �첽����Handler
    /// </summary>
    public class AjaxPageHandler : IHttpHandler
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
        //     ͨ��ʵ�� System.Web.IHttpHandler �ӿڵ��Զ��� HttpHandler ���� HTTP Web ����Ĵ���
        //
        // ����:
        //   context:
        //     System.Web.HttpContext �������ṩ������Ϊ HTTP �����ṩ������ڲ������������� Request��Response��Session
        //     �� Server�������á�
        public void ProcessRequest(HttpContext context)
        {
            string ajaxKey = "AjaxProcess", url = string.Empty, space = string.Empty;
            string[] split = context.Request.Url.Query.Remove(0, 1).Split(';');

            url = WebUtils.Decrypt(split[0], ajaxKey);
            url = (url.IndexOf('/') >= 0 ? url : "/" + url);
            if (split.Length > 1) space = WebUtils.Decrypt(split[1], ajaxKey);
            if (string.IsNullOrEmpty(space)) space = "AjaxMethods";

            StringBuilder sb = new StringBuilder();
            sb.Append("var AjaxInfo = { \r\n");
            sb.Append("\t\turl : '" + url + "',\r\n");
            sb.Append("\t\tkey : '" + WebUtils.MD5Encrypt(ajaxKey) + "'\r\n");
            sb.Append("\t};\r\n\r\n");
            sb.Append(string.Format("var {0} = Ajax.RegisterPage(this);", space));

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
