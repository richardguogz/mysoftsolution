using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.SessionState;
using System.IO;

namespace KiShion.Web
{
    public class PageHandlerFactory : IHttpHandlerFactory, IRequiresSessionState
    {
        // ժҪ:
        //     ����ʵ�� System.Web.IHttpHandler �ӿڵ����ʵ����
        //
        // ����:
        //   pathTranslated:
        //     ��������Դ�� System.Web.HttpRequest.PhysicalApplicationPath��
        //
        //   url:
        //     ��������Դ�� System.Web.HttpRequest.RawUrl��
        //
        //   context:
        //     System.Web.HttpContext ���ʵ�������ṩ������Ϊ HTTP �����ṩ������ڲ������������� Request��Response��Session
        //     �� Server�������á�
        //
        //   requestType:
        //     �ͻ���ʹ�õ� HTTP ���ݴ��䷽����GET �� POST����
        //
        // ���ؽ��:
        //     ����������µ� System.Web.IHttpHandler ����
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            string sendToUrl = context.Request.Url.PathAndQuery;
            string filePath = pathTranslated;
            string sendToUrlLessQString;
            PageUtils.RewriteUrl(context, sendToUrl, out sendToUrlLessQString, out filePath);
            return PageParser.GetCompiledPageInstance(sendToUrlLessQString, filePath, context);
        }

        //
        // ժҪ:
        //     ʹ���������������еĴ������ʵ����
        //
        // ����:
        //   handler:
        //     Ҫ���õ� System.Web.IHttpHandler ����
        public void ReleaseHandler(IHttpHandler handler)
        { }
    }
}
