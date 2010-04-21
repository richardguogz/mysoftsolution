using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Configuration;
using MySoft.Data.Mapping;
using System.Xml.Serialization;

namespace MySoft.Data
{
    /// <summary>
    /// 映像配置类
    /// </summary>
    [Serializable]
    public class EntityConfig
    {
        public static EntityConfig Instance = new EntityConfig();
        private IDictionary<TableSetting, List<TableMapping>> _dictTableMapping;
        private EntityConfig()
        {
            LoadConfig();
        }

        private void LoadConfig()
        {
            _dictTableMapping = new Dictionary<TableSetting, List<TableMapping>>();
            string configPath = ConfigurationManager.AppSettings["EntityConfigPath"];
            if (string.IsNullOrEmpty(configPath))
            {
                configPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\EntityConfig.xml";
            }
            else
            {
                //如果是~则表示当前目录
                if (configPath.Contains("~/") || configPath.Contains("~\\"))
                {
                    configPath = configPath.Replace("/", "\\").Replace("~\\", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\");
                }
            }

            if (File.Exists(configPath))
            {
                XmlTextReader reader = new XmlTextReader(configPath);
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(TableSetting[]));

                    TableSetting[] objs;
                    objs = serializer.Deserialize(reader) as TableSetting[];
                    List<TableSetting> list = new List<TableSetting>(objs);

                    list.ForEach(p =>
                    {
                        List<TableMapping> _mappingList = new List<TableMapping>();
                        if (p.Mappings != null && p.Mappings.Length > 0)
                        {
                            _mappingList.AddRange(p.Mappings);
                        }
                        _dictTableMapping[p] = _mappingList;
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
        public void Refresh()
        {
            LoadConfig();
        }

        /// <summary>
        /// 获取映射的表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public Table GetMappingTable<T>(string tableName)
            where T : class
        {
            //通过Namespace与ClassName来获取映射的表名
            string Namespace = typeof(T).Namespace;
            string ClassName = typeof(T).Name;

            Table table = new Table(tableName);
            var list = new List<TableSetting>(_dictTableMapping.Keys);
            if (list.Exists(p => p.Namespace == Namespace))
            {
                TableSetting setting = list.Find(p => p.Namespace == Namespace);
                table.Prefix = setting.Prefix;
                table.Suffix = setting.Suffix;

                //查询mapping的表名
                if (_dictTableMapping[setting].Exists(p => p.ClassName == ClassName))
                {
                    TableMapping mapping = _dictTableMapping[setting].Find(p => p.ClassName == ClassName);
                    table.TableName = mapping.MappingName;

                    if (!mapping.UsePrefix) table.Prefix = null;
                    if (!mapping.UseSuffix) table.Suffix = null;
                }
            }

            return table;
        }

        /// <summary>
        /// 获取映射的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public Field GetMappingField<T>(string propertyName, string fieldName)
            where T : class
        {
            //通过Namespace与ClassName来获取映射的表名
            string Namespace = typeof(T).Namespace;
            string ClassName = typeof(T).Name;

            Field field = new Field(fieldName);
            var list = new List<TableSetting>(_dictTableMapping.Keys);
            if (list.Exists(p => p.Namespace == Namespace))
            {
                TableSetting setting = list.Find(p => p.Namespace == Namespace);

                if (_dictTableMapping[setting].Exists(p => p.ClassName == ClassName))
                {
                    TableMapping mapping = _dictTableMapping[setting].Find(p => p.ClassName == ClassName);

                    if (mapping.Mappings != null && mapping.Mappings.Length > 0)
                    {
                        //查找mapping的字段名
                        foreach (FieldMapping f in mapping.Mappings)
                        {
                            if (f.PropertyName == propertyName)
                            {
                                field = new Field(f.MappingName);
                                break;
                            }
                        }
                    }
                }
            }

            return field;
        }
    }
}
