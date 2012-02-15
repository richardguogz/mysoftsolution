﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace MySoft.IoC.Http
{
    /// <summary>
    /// API文档类
    /// </summary>
    internal class APIDocument
    {
        private IDictionary<string, CallerInfo> callers;
        private int port;
        public APIDocument(IDictionary<string, CallerInfo> callers, int port)
        {
            this.callers = callers;
            this.port = port;
        }

        /// <summary>
        /// 生成文档
        /// </summary>
        /// <returns></returns>
        public string MakeDocument()
        {
            #region 读取资源

            Assembly assm = this.GetType().Assembly;
            Stream helpStream = assm.GetManifestResourceStream("MySoft.IoC.Http.Template.help.htm");
            Stream helpitemStream = assm.GetManifestResourceStream("MySoft.IoC.Http.Template.helpitem.htm");

            StreamReader helpReader = new StreamReader(helpStream);
            StreamReader helpitemReader = new StreamReader(helpitemStream);

            string html = helpReader.ReadToEnd(); helpReader.Close();
            string item = helpitemReader.ReadToEnd(); helpitemReader.Close();

            #endregion

            string uri = string.Format("http://{0}:{1}/", "127.0.0.1", port);
            html = html.Replace("${uri}", uri);

            StringBuilder sbUrl = new StringBuilder();
            foreach (var kv in callers)
            {
                sbUrl.Append(GetItemDocument(kv, item));
            }

            html = html.Replace("${body}", sbUrl.ToString());
            return html;
        }

        /// <summary>
        /// 获取Item文档
        /// </summary>
        /// <param name="kv"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private string GetItemDocument(KeyValuePair<string, CallerInfo> kv, string item)
        {
            var template = item;
            var plist = new List<string>();
            foreach (var p in kv.Value.Method.GetParameters())
            {
                plist.Add(string.Format("{0}=[{0}]", p.Name));
            }

            string uri = string.Empty;
            if (plist.Count == 0)
            {
                uri = string.Format("http://127.0.0.1:{0}/{1}", port, kv.Key);
            }
            else
            {
                uri = string.Format("http://127.0.0.1:{0}/{1}?{2}", port, kv.Key, string.Join("&", plist.ToArray()));
            }

            var url = string.Format("<a rel=\"operation\" target=\"_blank\" href=\"{0}\">{0}</a> 处的服务", uri);

            template = template.Replace("${method}", string.Format("<p title=\"分布式服务接口:\r\n{2}\"><b>{0}</b><br/>{1}</p>", kv.Key, kv.Value.Description, kv.Value.ServiceName));
            template = template.Replace("${parameter}", string.Join("&", plist.ToArray()));
            template = template.Replace("${uri}", url.ToString());
            template = template.Replace("[[", "{").Replace("]]", "}");

            return template;
        }
    }
}