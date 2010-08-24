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
        public XmlNodeHelper(XmlDocument doc, string element)
        {
            this.doc = doc;
            this.element = element;
            this.node = doc.SelectSingleNode(element);
        }

        /// <summary>
        /// 获取一个节点
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public XmlNodeHelper GetNode(string element)
        {
            string el = "/" + string.Format("{0}/{1}", this.element, element);
            return new XmlNodeHelper(doc, el);
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
        /// 插入属性及值
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="values"></param>
        public XmlNodeHelper Insert(string[] attributes, string[] values)
        {
            return Insert(null, attributes, values);
        }

        /// <summary>
        /// 插入节点及值
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public XmlNodeHelper Insert(string element, string value)
        {
            return Insert(element, null, value);
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

        /// <summary>
        /// 插入节点、属性及值
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        public XmlNodeHelper Insert(string element, string attribute, string value)
        {
            try
            {
                if (string.IsNullOrEmpty(element))
                {
                    XmlElement xe = (XmlElement)node;
                    xe.SetAttribute(attribute, value);

                    return this;
                }
                else
                {
                    XmlElement xe = doc.CreateElement(element);
                    if (string.IsNullOrEmpty(attribute))
                        xe.InnerText = value;
                    else
                        xe.SetAttribute(attribute, value);
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
        /// <param name="node"></param>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        public XmlNodeHelper Update(string node, string attribute, string value)
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
                return node.Value;
            }
            set
            {
                node.Value = value;
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
        public XmlNodeHelper Create(string element)
        {
            try
            {
                MemoryStream ms = new MemoryStream();

                var xw = new XmlTextWriter(ms, Encoding.Default);
                xw.Formatting = Formatting.Indented;
                xw.WriteStartDocument();
                xw.WriteStartElement(element);
                xw.WriteEndElement();
                xw.WriteEndDocument();
                xw.Flush();

                ms.Position = 0;
                var xr = XmlReader.Create(ms);
                doc.Load(xr);

                ms.Close();
                xw.Close();

                content = doc.InnerXml;

                return new XmlNodeHelper(doc, element);
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
            if (content == null)
            {
                throw new MySoftException("xml文件不存在，请先创建！");
            }

            return new XmlNodeHelper(doc, element);
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

                            StreamWriter sw = new StreamWriter(fs);
                            sw.Write(doc.InnerXml);

                            doc.Save(sw);
                            sw.Close();
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
