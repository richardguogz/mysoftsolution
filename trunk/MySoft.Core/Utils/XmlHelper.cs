using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace MySoft.Core
{
    /// <summary>
    /// XmlHelper 的摘要说明
    /// </summary>
    public class XmlHelper : IDisposable
    {
        XmlDocument doc = new XmlDocument();
        private string path;
        private string oldContent;

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
                    oldContent = doc.InnerXml;
                }
                catch { }
            }
        }

        /// <summary>
        /// 创建xml文件
        /// </summary>
        /// <param name="root">根目录名称</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Create(path, "Node")
         ************************************************/
        public void Create(string element)
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

                oldContent = doc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new MySoftException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时返回该属性值，否则返回串联值</param>
        /// <returns>string</returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Read(path, "/Node", "")
         * XmlHelper.Read(path, "/Node/Element[@Attribute='Name']", "Attribute")
         ************************************************/
        public string Read(string node)
        {
            return Read(node, null);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时返回该属性值，否则返回串联值</param>
        /// <returns>string</returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Read(path, "/Node", "")
         * XmlHelper.Read(path, "/Node/Element[@Attribute='Name']", "Attribute")
         ************************************************/
        public string Read(string node, string attribute)
        {
            if (oldContent == null)
            {
                throw new MySoftException("xml文件不存在，请先创建！");
            }

            string value = "";
            try
            {
                XmlNode xn = doc.SelectSingleNode(node);
                value = (string.IsNullOrEmpty(attribute) ? xn.InnerText : xn.Attributes[attribute].Value);
            }
            catch (Exception ex)
            {
                throw new MySoftException(ex.Message, ex);
            }
            return value;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="element">元素名，非空时插入新元素，否则在该元素中插入属性</param>
        /// <param name="attribute">属性名，非空时插入该元素属性值，否则插入元素值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Insert(path, "/Node", "Element", "", "Value")
         * XmlHelper.Insert(path, "/Node", "Element", "Attribute", "Value")
         * XmlHelper.Insert(path, "/Node", "", "Attribute", "Value")
         ************************************************/
        public void Insert(string node, string[] attributes, string[] values)
        {
            Insert(node, null, attributes, values);
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="element">元素名，非空时插入新元素，否则在该元素中插入属性</param>
        /// <param name="attribute">属性名，非空时插入该元素属性值，否则插入元素值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Insert(path, "/Node", "Element", "", "Value")
         * XmlHelper.Insert(path, "/Node", "Element", "Attribute", "Value")
         * XmlHelper.Insert(path, "/Node", "", "Attribute", "Value")
         ************************************************/
        public void Insert(string node, string element, string[] attributes, string[] values)
        {
            if (oldContent == null)
            {
                throw new MySoftException("xml文件不存在，请先创建！");
            }

            try
            {
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe;
                bool isRoot = true;
                if (string.IsNullOrEmpty(element))
                    xe = (XmlElement)xn;
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

                if (!isRoot) xn.AppendChild(xe);
            }
            catch (Exception ex)
            {
                throw new MySoftException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="element">元素名，非空时插入新元素，否则在该元素中插入属性</param>
        /// <param name="attribute">属性名，非空时插入该元素属性值，否则插入元素值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Insert(path, "/Node", "Element", "", "Value")
         * XmlHelper.Insert(path, "/Node", "Element", "Attribute", "Value")
         * XmlHelper.Insert(path, "/Node", "", "Attribute", "Value")
         ************************************************/
        public void Insert(string node, string element, string value)
        {
            Insert(node, element, null, value);
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="element">元素名，非空时插入新元素，否则在该元素中插入属性</param>
        /// <param name="attribute">属性名，非空时插入该元素属性值，否则插入元素值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Insert(path, "/Node", "Element", "", "Value")
         * XmlHelper.Insert(path, "/Node", "Element", "Attribute", "Value")
         * XmlHelper.Insert(path, "/Node", "", "Attribute", "Value")
         ************************************************/
        public void Insert(string node, string element, string attribute, string value)
        {
            if (oldContent == null)
            {
                throw new MySoftException("xml文件不存在，请先创建！");
            }

            try
            {
                XmlNode xn = doc.SelectSingleNode(node);
                if (string.IsNullOrEmpty(element))
                {
                    if (!string.IsNullOrEmpty(attribute))
                    {
                        XmlElement xe = (XmlElement)xn;
                        xe.SetAttribute(attribute, value);
                    }
                }
                else
                {
                    XmlElement xe = doc.CreateElement(element);
                    if (string.IsNullOrEmpty(attribute))
                        xe.InnerText = value;
                    else
                        xe.SetAttribute(attribute, value);
                    xn.AppendChild(xe);
                }
            }
            catch (Exception ex)
            {
                throw new MySoftException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时修改该节点属性值，否则修改节点值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Insert(path, "/Node", "", "Value")
         * XmlHelper.Insert(path, "/Node", "Attribute", "Value")
         ************************************************/
        public void Update(string node, string value)
        {
            Update(node, null, value);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时修改该节点属性值，否则修改节点值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Insert(path, "/Node", "", "Value")
         * XmlHelper.Insert(path, "/Node", "Attribute", "Value")
         ************************************************/
        public void Update(string node, string attribute, string value)
        {
            Update(node, new string[] { attribute }, new string[] { value });
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时修改该节点属性值，否则修改节点值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Insert(path, "/Node", "", "Value")
         * XmlHelper.Insert(path, "/Node", "Attribute", "Value")
         ************************************************/
        public void Update(string node, string[] attributes, string[] values)
        {
            if (oldContent == null)
            {
                throw new MySoftException("xml文件不存在，请先创建！");
            }

            try
            {
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                int index = 0;
                foreach (string attribute in attributes)
                {
                    xe.SetAttribute(attribute, values[index]);
                    index++;
                }
            }
            catch (Exception ex)
            {
                throw new MySoftException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时删除该节点属性值，否则删除节点值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Delete(path, "/Node", "")
         * XmlHelper.Delete(path, "/Node", "Attribute")
         ************************************************/
        public void Delete(string node)
        {
            Delete(node, null);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时删除该节点属性值，否则删除节点值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Delete(path, "/Node", "")
         * XmlHelper.Delete(path, "/Node", "Attribute")
         ************************************************/
        public void Delete(string node, string attribute)
        {
            if (oldContent == null)
            {
                throw new MySoftException("xml文件不存在，请先创建！");
            }

            try
            {
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (string.IsNullOrEmpty(attribute))
                    xn.ParentNode.RemoveChild(xn);
                else
                    xe.RemoveAttribute(attribute);
            }
            catch (Exception ex)
            {
                throw new MySoftException(ex.Message, ex);
            }
        }

        #region IDisposable 成员

        /// <summary>
        /// 保存更新
        /// </summary>
        public void Save()
        {
            try
            {
                //有更改时才保存
                if (oldContent != doc.InnerXml)
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
