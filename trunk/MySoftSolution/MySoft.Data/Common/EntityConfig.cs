using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Xml.Serialization;

namespace MySoft.Data
{
    /// <summary>
    /// 前缀设置
    /// </summary>
    [Serializable]
    public class PrefixSetting
    {
        [XmlAttribute("Name")]
        public string Name
        {
            get;
            set;
        }

        [XmlAttribute("Value")]
        public string Value
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 映像配置类
    /// </summary>
    [Serializable]
    public class EntityConfig
    {
        public static EntityConfig Instance = new EntityConfig();
        private IDictionary<string, string> _dictPrefix;
        private string _defaultPrefix;
        public string DefaultPrefix
        {
            get { return _defaultPrefix; }
        }

        private EntityConfig()
        {
            LoadConfig();
        }

        private void LoadConfig()
        {
            _dictPrefix = new Dictionary<string, string>();
            string xmlPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\EntityConfig.xml";
            if (File.Exists(xmlPath))
            {
                XmlTextReader reader = new XmlTextReader(xmlPath);
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(PrefixSetting[]));

                    PrefixSetting[] objs;
                    objs = serializer.Deserialize(reader) as PrefixSetting[];
                    List<PrefixSetting> list = new List<PrefixSetting>(objs);

                    list.ForEach(nvc =>
                    {
                        if (nvc.Name == "Default")
                            _defaultPrefix = nvc.Value;
                        else
                            _dictPrefix.Add(nvc.Name, nvc.Value);
                    });
                }
                catch { }
                finally
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// 刷新配置
        /// </summary>
        public void RefreshConfig()
        {
            LoadConfig();
        }

        /// <summary>
        /// 获取表前缀
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string GetPrefix<T>()
        {
            string key = typeof(T).Namespace;
            if (_dictPrefix.ContainsKey(key))
                return _dictPrefix[key];
            else
                return _defaultPrefix;
        }
    }
}
