using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Net;
using System.Web.SessionState;
using System.Text.RegularExpressions;
using MySoft.Web.UI;
using MySoft.Web.Config;

namespace MySoft.Web
{
    public class StaticPageHandler : IHttpHandler, IRequiresSessionState
    {
        // 摘要:
        //     获取一个值，该值指示其他请求是否可以使用 System.Web.IHttpHandler 实例。
        //
        // 返回结果:
        //     如果 System.Web.IHttpHandler 实例可再次使用，则为 true；否则为 false。
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        // 摘要:
        //     通过实现 System.Web.IHttpHandler 接口的自定义 HttpHandler 启用 HTTP Web 请求的处理。
        //
        // 参数:
        //   context:
        //     System.Web.HttpContext 对象，它提供对用于为 HTTP 请求提供服务的内部服务器对象（如 Request、Response、Session
        //     和 Server）的引用。
        public void ProcessRequest(HttpContext context)
        {
            string url = context.Request.Url.PathAndQuery;
            string sendToUrl = url;
            string filePath = context.Request.PhysicalApplicationPath;
            string htmlFile = null;
            bool htmlExists = false;

            //检测是否为Ajax调用
            bool AjaxProcess = WebHelper.GetRequestParam<bool>(context.Request, "X-Ajax-Process", false);
            if (!AjaxProcess)
            {
                // get the configuration rules
                StaticPageRuleCollection rules = StaticPageConfiguration.GetConfig().Rules;

                // iterate through each rule...
                for (int i = 0; i < rules.Count; i++)
                {
                    // get the pattern to look for, and Resolve the Url (convert ~ into the appropriate directory)
                    string lookFor = "^" + PageUtils.ResolveUrl(context.Request.ApplicationPath, rules[i].LookFor) + "$";

                    // Create a regex (note that IgnoreCase is set...)
                    Regex re = new Regex(lookFor, RegexOptions.IgnoreCase);

                    if (re.IsMatch(url))
                    {
                        // match found - do any replacement needed

                        string htmlUrl = PageUtils.ResolveUrl(context.Request.ApplicationPath, re.Replace(url, rules[i].SendTo));
                        htmlFile = context.Server.MapPath(htmlUrl);

                        try
                        {
                            //需要生成静态页面
                            if (!File.Exists(htmlFile))  //静态页面不存在
                            {
                                context.Response.Filter = new ResponseFilter(context.Response.Filter, htmlFile, rules[i].ValidateString);
                                break;
                            }
                            else
                            {
                                //静态页面存在
                                FileInfo file = new FileInfo(htmlFile);
                                htmlExists = file.Exists;

                                //按秒检测页面重新生成
                                int span = (int)DateTime.Now.Subtract(file.LastWriteTime).TotalSeconds;
                                int timeSpan = rules[i].TimeSpan;

                                if (timeSpan > 0 && span >= timeSpan) //静态页面过期
                                {
                                    context.Response.Filter = new ResponseFilter(context.Response.Filter, htmlFile, rules[i].ValidateString);
                                    break;
                                }
                                else
                                {
                                    context.Response.WriteFile(htmlFile);
                                    return;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //当文件正在写还没关闭时，此时读文件会出错
                            //所以当发生此错误时直接跳过

                            try
                            {
                                string logFile = PageUtils.ResolveUrl(context.Request.ApplicationPath, string.Format("/StaticLog/ERROR_{0}.log", DateTime.Today.ToString("yyyyMMdd")));
                                logFile = context.Server.MapPath(logFile);
                                string logText = string.Format("{0} => 请求路径：{1}\r\n生成路径：{2}\r\n{3}", url, htmlFile, DateTime.Now.ToString("HH:mm:ss"), ex.Message);
                                logText += "\r\n\r\n=======================================================================================================================================================================\r\n\r\n";
                                if (!Directory.Exists(Path.GetDirectoryName(logFile)))
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(logFile));
                                }
                                File.AppendAllText(logFile, logText);
                            }
                            catch { }
                        }
                    }
                }
            }

            string sendToUrlLessQString;
            PageUtils.RewriteUrl(context, sendToUrl, out sendToUrlLessQString, out filePath);

            IHttpHandler handler = PageParser.GetCompiledPageInstance(sendToUrlLessQString, filePath, context);

            try
            {
                handler.ProcessRequest(context);
            }
            catch (HttpException ex)
            {
                if (htmlExists && !string.IsNullOrEmpty(htmlFile))
                {
                    context.Response.WriteFile(htmlFile);
                    return;
                }
                else
                    throw ex;
            }
        }
    }
}
