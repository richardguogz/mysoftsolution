using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using MySoft.Web.Configuration;

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
            string staticFile = null, fileExtension = null;
            bool htmlExists = false;

            //检测是否为Ajax调用
            bool AjaxProcess = WebHelper.GetRequestParam<bool>(context.Request, "X-Ajax-Process", false);
            if (!AjaxProcess)
            {
                var config = StaticPageConfiguration.GetConfig();

                //判断config配置信息
                if (config != null && config.Enabled)
                {
                    // iterate through the rules
                    foreach (StaticPageRule rule in config.Rules)
                    {
                        // get the pattern to look for, and Resolve the Url (convert ~ into the appropriate directory)
                        string lookFor = "^" + RewriterUtils.ResolveUrl(context.Request.ApplicationPath, rule.LookFor) + "$";

                        // Create a regex (note that IgnoreCase is set...)
                        Regex re = new Regex(lookFor, RegexOptions.IgnoreCase);

                        if (re.IsMatch(url))
                        {
                            // match found - do any replacement needed

                            string staticUrl = RewriterUtils.ResolveUrl(context.Request.ApplicationPath, re.Replace(url, rule.WriteTo));
                            staticFile = context.Server.MapPath(staticUrl);

                            try
                            {
                                //需要生成静态页面
                                if (!File.Exists(staticFile))  //静态页面不存在
                                {
                                    context.Response.Filter = new ResponseFilter(context.Response.Filter, staticFile, rule.ValidateString);
                                    break;
                                }
                                else
                                {
                                    //静态页面存在
                                    FileInfo file = new FileInfo(staticFile);
                                    htmlExists = file.Exists;
                                    fileExtension = file.Extension;

                                    //按秒检测页面重新生成
                                    int span = (int)DateTime.Now.Subtract(file.LastWriteTime).TotalSeconds;
                                    if (rule.Timeout > 0 && span >= rule.Timeout) //静态页面过期
                                    {
                                        context.Response.Filter = new ResponseFilter(context.Response.Filter, staticFile, rule.ValidateString);
                                        break;
                                    }
                                    else
                                    {
                                        //判断是否为xml格式
                                        if (fileExtension.ToLower().Contains("xml"))
                                        {
                                            context.Response.ContentType = "text/xml";
                                        }

                                        context.Response.WriteFile(staticFile);
                                        return;
                                    }
                                }
                            }
                            catch (IOException ex)
                            {
                                string logFile = RewriterUtils.ResolveUrl(context.Request.ApplicationPath, string.Format("/StaticLog/ERROR_{0}.log", DateTime.Today.ToString("yyyyMMdd")));
                                logFile = context.Server.MapPath(logFile);
                                string logText = string.Format("{0} => {3}\r\n请求路径：{1}\r\n生成路径：{2}", DateTime.Now.ToString("HH:mm:ss"), context.Request.Url, staticFile, ex.Message);
                                logText += "\r\n\r\n=======================================================================================================================================================================\r\n\r\n";
                                if (!Directory.Exists(Path.GetDirectoryName(logFile)))
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(logFile));
                                }
                                File.AppendAllText(logFile, logText);
                            }
                        }
                    }
                }
            }

            //进行错误处理，如果出错，则转到原有的静态页面
            try
            {
                string sendToUrlLessQString;
                RewriterUtils.RewriteUrl(context, sendToUrl, out sendToUrlLessQString, out filePath);
                IHttpHandler handler = PageParser.GetCompiledPageInstance(sendToUrlLessQString, filePath, context);
                handler.ProcessRequest(context);
            }
            catch (HttpException ex)
            {
                if (htmlExists && !string.IsNullOrEmpty(staticFile))
                {
                    //判断是否为xml格式
                    if (fileExtension.ToLower().Contains("xml"))
                    {
                        context.Response.ContentType = "text/xml";
                    }

                    context.Response.WriteFile(staticFile);
                    return;
                }
                else
                    throw ex;
            }
        }
    }
}
