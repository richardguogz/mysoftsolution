using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace MySoft.Core
{
    /// <summary>
    /// XmlNodeHelper 的摘要说明
    /// </summary>
    public class XmlNodeHelper
    {
        private XmlDocument doc;
        private string element;
        private XmlNode node;

        /// <summary>
        /// 实例化 XmlNodeHelper
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="element"></param>
        internal XmlNodeHelper(XmlDocument doc, string element)
        {
            this.doc = doc;
            this.element = element;

            if (string.IsNullOrEmpty(element))
                this.node = (XmlNode)doc.DocumentElement;
            else
                this.node = doc.SelectSingleNode(element);
        }

        /// <summary>
        /// 实例化 XmlNodeHelper
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="node"></param>
        private XmlNodeHelper(XmlDocument doc, XmlNode node)
        {
            this.doc = doc;
            this.element = node.Name;
            this.node = node;
        }

        /// <summary>
        /// 获取一个节点
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public XmlNodeHelper GetNode(string element)
        {
            string el = null;
            if (string.IsNullOrEmpty(this.element))
                el = "/" + element.TrimStart('/');
            else
                el = string.Format("/{0}/{1}", this.element.TrimStart('/'), element.TrimStart('/'));

            return new XmlNodeHelper(doc, el);
        }

        /// <summary>
        /// 获取节点列表
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public XmlNodeHelper[] GetNodes(string element)
        {
            var list = new List<XmlNodeHelper>();
            foreach (XmlNode nd in node.ChildNodes)
            {
                if (nd.Name == element)
                {
                    var helper = new XmlNodeHelper(doc, nd);
                    list.Add(helper);
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public string GetAttribute(string attribute)
        {
            try
            {
                return node.Attributes[attribute].Value;
            }
            catch (Exception ex)
            {
                throw new MySoftException(ex.Message, ex);
            }
        }

        #region 插入节点

        /// <summary>
        /// 插入节点及值
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public XmlNodeHelper Insert(string attribute, string value)
        {
            return Insert(null, attribute, value);
        }

        /// <summary>
        /// 插入属性及值
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="values"></param>
        public XmlNodeHelper Insert(string[] attributes, string[] values)
        {
            return Insert(null, attributes, values);
        }

        /// <summary>
        /// 插入节点、属性及值
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        public XmlNodeHelper Insert(string element, string attribute, string value)
        {
            return Insert(element, new string[] { attribute }, new string[] { value });
        }

        /// <summary>
        /// 插入节点、属性及值
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        /// <param name="values"></param>
        public XmlNodeHelper Insert(string element, string[] attributes, string[] values)
        {
            try
            {
                XmlElement xe;
                bool isRoot = true;
                if (string.IsNullOrEmpty(element))
                    xe = (XmlElement)node;
                else
                {
                    isRoot = false;
                    xe = doc.CreateElement(element);
                }

                int index = 0;
                foreach (string attribute in attributes)
                {
                    xe.SetAttribute(attribute, values[index]);
                    index++;
                }

                if (isRoot)
                {
                    return this;
                }
                else
                {
                    node.AppendChild(xe);
                    return GetNode(element);
                }
            }
            catch (Exception ex)
            {
                throw new MySoftException(ex.Message, ex);
            }
        }

        #endregion

        #region 更新节点

        /// <summary>
        /// 更新属性值
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        public XmlNodeHelper Update(string attribute, string value)
        {
            return Update(new string[] { attribute }, new string[] { value });
        }

        /// <summary>
        /// 更新属性值
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="values"></param>
        public XmlNodeHelper Update(string[] attributes, string[] values)
        {
            try
            {
                XmlElement xe = (XmlElement)node;
                int index = 0;
                foreach (string attribute in attributes)
                {
                    xe.SetAttribute(attribute, values[index]);
                    index++;
                }

                return this;
            }
            catch (Exception ex)
            {
                throw new MySoftException(ex.Message, ex);
            }
        }

        #endregion

        #region 删除节点

        /// <summary>
        /// 删除节点
        /// </summary>
        public XmlNodeHelper Delete()
        {
            return Delete(null);
        }

        /// <summary>
        /// 删除属性
        /// </summary>
        /// <param name="attribute"></param>
        public XmlNodeHelper Delete(string attribute)
        {
            try
            {
                XmlElement xe = (XmlElement)node;
                if (string.IsNullOrEmpty(attribute))
                    node.ParentNode.RemoveChild(node);
                else
                    xe.RemoveAttribute(attribute);

                return this;
            }
            catch (Exception ex)
            {
                throw new MySoftException(ex.Message, ex);
            }
        }

        #endregion

        /// <summary>
        /// 获取节点值
        /// </summary>
        public string Text
        {
            get
            {
                return node.InnerText;
            }
            set
            {
                node.InnerText = value;
            }
        }
    }

    /// <summary>
    /// XmlHelper 的摘要说明
    /// </summary>
    public class XmlHelper : IDisposable
    {
        XmlDocument doc = new XmlDocument();
        private string path;
        private string content;

        /// <summary>
        /// 实例化XmlHelper
        /// </summary>
        /// <param name="path"></param>
        public XmlHelper(string path)
        {
            this.path = path;

            if (File.Exists(path))
            {
                try
                {
                    doc.Load(path);
                    content = doc.InnerXml;
                }
                catch { }
            }
        }

        /// <summary>
        /// 创建element根节点
        /// </summary>
        /// <param name="element"></param>
        public XmlNodeHelper Insert(string element)
        {
            return Insert(element, (string[])null, (string[])null);
        }

        /// <summary>
        /// 创建element根节点
        /// </summary>
        /// <param name="element"></param>
        public XmlNodeHelper Insert(string element, string attribute, string value)
        {
            return Insert(element, new string[] { attribute }, new string[] { value });
        }

        /// <summary>
        /// 创建element根节点
        /// </summary>
        /// <param name="element"></param>
        public XmlNodeHelper Insert(string element, string[] attributes, string[] values)
        {
            try
            {
                MemoryStream ms = new MemoryStream();

                var xw = new XmlTextWriter(ms, Encoding.Default);
                xw.Formatting = Formatting.Indented;
                xw.WriteStartDocument();
                xw.WriteStartElement(element);

                if (attributes != null)
                {
                    int index = 0;
                    foreach (string attribute in attributes)
                    {
                        xw.WriteAttributeString(attribute, values[index]);
                    }
                }

                xw.WriteEndElement();
                xw.WriteEndDocument();
                xw.Flush();

                ms.Position = 0;
                var xr = XmlReader.Create(ms);
                doc.Load(xr);
                xr.Close();

                ms.Close();
                xw.Close();

                return GetNode(element);
            }
            catch (Exception ex)
            {
                throw new MySoftException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取一个节点
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public XmlNodeHelper GetNode(string element)
        {
            if (doc.ChildNodes.Count == 0)
            {
                throw new MySoftException("xml文件不存在，请先创建！");
            }

            string[] elements = element.Split(new char[] { '.', '/', '|' });
            if (elements.Length == 1)
            {
                return new XmlNodeHelper(doc, elements[0]);
            }

            XmlNodeHelper node = null;
            foreach (string el in elements)
            {
                if (node == null)
                    node = new XmlNodeHelper(doc, el);
                else
                    node = node.GetNode(el);
            }

            return node;
        }

        #region 保存节点

        /// <summary>
        /// 保存更新
        /// </summary>
        public void Save()
        {
            try
            {
                //有更改时才保存
                if (content != doc.InnerXml)
                {
                    if (!string.IsNullOrEmpty(doc.InnerXml))
                    {
                        if (!File.Exists(path))
                        {
                            //是否存在指定文件路径的目录
                            if (!Directory.Exists(Path.GetDirectoryName(path)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(path));
                            }

                            var fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
                            fs.SetLength(0);

                            using (var sw = new StreamWriter(fs))
                            {
                                sw.Write(doc.InnerXml);
                                sw.Flush();
                            }
                        }
                        else
                        {
                            doc.Save(path);
                        }
                    }
                }
            }
            catch { };
        }

        #endregion

        #region IDisposable 成员

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Save();
        }

        #endregion
    }
}
