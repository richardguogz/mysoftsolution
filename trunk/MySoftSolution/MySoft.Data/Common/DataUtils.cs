using System;
using System.IO;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;

namespace MySoft.Data
{
    /// <summary>
    /// 数据服务类
    /// </summary>
    public static class DataUtils
    {
        #region 外部方法

        /// <summary>
        /// 将xml反系列化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string xml)
        {
            return (T)SerializationManager.Deserialize(typeof(T), xml);
        }

        /// <summary>
        /// 将对象系列化成xml
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            return SerializationManager.Serialize(obj);
        }

        /// <summary>
        /// 克隆一个对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Clone<T>(T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// 从对象obj中获取值传给当前实体,TOutput必须为class或接口
        /// TInput可以为class、IRowReader、NameValueCollection
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TOutput ConvertType<TInput, TOutput>(TInput obj)
        {
            if (obj == null) return default(TOutput);

            if (obj is TOutput)
            {
                return (TOutput)(obj as object);
            }
            else
            {
                try
                {
                    TOutput t = CreateInstance<TOutput>();
                    foreach (PropertyInfo p in typeof(TOutput).GetProperties())
                    {
                        object value = null;
                        if (obj is IRowReader)
                        {
                            IRowReader reader = obj as IRowReader;
                            if (reader.IsDBNull(p.Name)) continue;
                            value = reader[p.Name];
                        }
                        else if (obj is NameValueCollection)
                        {
                            NameValueCollection reader = obj as NameValueCollection;
                            if (reader[p.Name] == null) continue;
                            value = reader[p.Name];
                        }
                        else
                        {
                            value = GetPropertyValue(obj, p.Name);
                        }
                        if (value == null) continue;
                        SetPropertyValue(t, p, value);
                    }
                    return t;
                }
                catch (Exception ex)
                {
                    throw new MySoftException("传入的TInput与TOutput无法进行转换！", ex);
                }
            }
        }

        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ConvertValue<T>(object value)
        {
            if (value == DBNull.Value || value == null)
                return default(T);

            object obj = ConvertValue(typeof(T), value);
            if (obj == null)
            {
                return default(T);
            }
            return (T)obj;
        }

        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ConvertValue(Type type, object value)
        {
            if (value == DBNull.Value || value == null)
                return null;

            if (CheckStruct(type))
            {
                //如果字段为结构，则进行系列化操作
                return SerializationManager.Deserialize(type, value.ToString());
            }
            else
            {
                Type valueType = value.GetType();
                if (type == valueType)
                {
                    return value;
                }
                else if (type == typeof(object))
                {
                    return value;
                }
                else
                {
                    if (type.IsEnum)
                    {
                        try
                        {
                            return Enum.Parse(type, value.ToString(), true);
                        }
                        catch
                        {
                            return Enum.ToObject(type, value);
                        }
                    }
                    else
                    {
                        return ChangeType(value, type);
                    }
                }
            }
        }

        #region 属性操作

        /// <summary>
        /// 获取自定义属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pi"></param>
        /// <returns></returns>
        public static T GetPropertyAttribute<T>(PropertyInfo pi)
        {
            object[] attrs = pi.GetCustomAttributes(typeof(T), false);
            if (attrs != null && attrs.Length > 0)
            {
                return (T)attrs[0];
            }
            return default(T);
        }

        /// <summary>
        /// 获取自定义属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T GetTypeAttribute<T>(Type type)
        {
            object[] attrs = type.GetCustomAttributes(typeof(T), false);
            if (attrs != null && attrs.Length > 0)
            {
                return (T)attrs[0];
            }
            return default(T);
        }

        #endregion

        /// <summary>
        /// 判断WhereClip是否为null或空
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(WhereClip where)
        {
            if ((object)where == null || string.IsNullOrEmpty(where.ToString()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断OrderByClip是否为null或空
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(OrderByClip order)
        {
            if ((object)order == null || string.IsNullOrEmpty(order.ToString()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断GroupByClip是否为null或空
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(GroupByClip group)
        {
            if ((object)group == null || string.IsNullOrEmpty(group.ToString()))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region DynamicCalls

        /// <summary>
        /// 快速创建一个T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateInstance<T>()
        {
            return (T)CreateHandler(typeof(T))();
        }

        /// <summary>
        /// 创建一个委托
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static FastCreateInstanceHandler CreateHandler(Type type)
        {
            if (type.IsInterface)
            {
                throw new MySoftException("可实例化的对象类型不能是接口！");
            }
            FastCreateInstanceHandler creator = DynamicCalls.GetInstanceCreator(type);
            return creator;
        }

        /// <summary>
        /// 快速调用方法
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object FastMethodInvoke(object obj, MethodInfo method, params object[] parameters)
        {
            FastInvokeHandler invoke = DynamicCalls.GetMethodInvoker(method);
            return invoke(obj, parameters);
        }

        #endregion

        #region 属性赋值及取值

        /// <summary>
        /// 快速设置属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(object obj, PropertyInfo property, object value)
        {
            if (!property.CanWrite) return;
            try
            {
                FastPropertySetHandler setter = DynamicCalls.GetPropertySetter(property);
                value = ConvertValue(property.PropertyType, value);
                setter(obj, value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 快速设置属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(object obj, string propertyName, object value)
        {
            PropertyInfo property = obj.GetType().GetProperty(propertyName);
            if (property != null)
            {
                SetPropertyValue(obj, property, value);
            }
        }

        /// <summary>
        /// 快速获取属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static object GetPropertyValue(object obj, PropertyInfo property)
        {
            if (!property.CanRead) return null;
            FastPropertyGetHandler getter = DynamicCalls.GetPropertyGetter(property);
            return getter(obj);
        }

        /// <summary>
        /// 快速获取属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetPropertyValue(object obj, string propertyName)
        {
            PropertyInfo property = obj.GetType().GetProperty(propertyName);
            if (property != null)
            {
                return GetPropertyValue(obj, property);
            }
            return null;
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 格式化数据为数据库通用格式
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        internal static string FormatValue(object val)
        {
            if (val == null || val == DBNull.Value)
            {
                return "null";
            }

            Type type = val.GetType();

            if (type == typeof(Guid))
            {
                return string.Format("'{0}'", val);
            }
            else if (type == typeof(DateTime))
            {
                return string.Format("'{0}'", ((DateTime)val).ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else if (type == typeof(bool))
            {
                return ((bool)val) ? "1" : "0";
            }
            else if (val is Field)
            {
                return ((Field)val).Name;
            }
            else if (type.IsEnum)
            {
                return Convert.ToInt32(val).ToString();
            }
            else if (type.IsValueType)
            {
                if (CheckStruct(type))
                {
                    //如果属性是值类型，则进行系列化存储
                    return SerializationManager.Serialize(val);
                }

                return val.ToString();
            }
            else
            {
                string sqlvalue = val.ToString();

                // 去除不合法的字符，防止SQL注入式攻击
                //sqlvalue = sqlvalue.Replace("'", "''");
                //sqlvalue = sqlvalue.Replace("--", "");
                //sqlvalue = sqlvalue.Replace(";", "");

                return string.Format("N'{0}'", sqlvalue);
            }
        }

        internal static string FormatSQL(string sql, char leftToken, char rightToken, bool isAccess)
        {
            if (sql == null) return string.Empty;

            if (isAccess)
                sql = string.Format(sql, leftToken, rightToken, '(', ')');
            else
                sql = string.Format(sql, leftToken, rightToken, ' ', ' ');

            return sql.Trim().Replace(" . ", ".")
                            .Replace(" , ", ",")
                            .Replace(" ( ", " (")
                            .Replace(" ) ", ") ")
                            .Replace("   ", " ")
                            .Replace("  ", " ");
        }

        internal static object[] CheckAndReturnValues(object[] values)
        {
            //如果值为null，则返回不等条件
            if (values == null)
            {
                throw new MySoftException("传入的数据不能为null！");
            }

            //如果长度为0，则返回不等条件
            if (values.Length == 0)
            {
                throw new MySoftException("传入的数据个数不能为0！");
            }

            //如果传的类型不是object,则强制转换
            if (values.Length == 1 && values[0].GetType().IsArray)
            {
                try
                {
                    values = ArrayList.Adapter((Array)values[0]).ToArray();
                }
                catch
                {
                    throw new MySoftException("传入的数据不能正确被解析！");
                }
            }

            return values;
        }

        internal static WhereClip GetPkWhere<T>(Table table, object[] pkValues)
            where T : Entity
        {
            WhereClip where = null;
            List<FieldValue> list = CreateInstance<T>().GetFieldValues();
            pkValues = CheckAndReturnValues(pkValues);

            list.ForEach(fv =>
            {
                int index = 0;
                if (fv.IsPrimaryKey)
                {
                    where &= fv.Field.At(table) == pkValues[index];
                    index++;
                }
            });

            return where;
        }

        internal static WhereClip GetPkWhere<T>(Table table, T entity)
            where T : Entity
        {
            WhereClip where = null;
            List<FieldValue> list = entity.GetFieldValues();

            list.ForEach(fv =>
            {
                if (fv.IsPrimaryKey)
                {
                    where &= fv.Field.At(table) == fv.Value;
                }
            });

            return where;
        }

        /// <summary>
        /// 创建一个FieldValue列表
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        internal static List<FieldValue> CreateFieldValue(Field[] fields, object[] values, bool isInsert)
        {
            if (fields == null || values == null)
            {
                throw new MySoftException("字段及值不能为null！");
            }

            if (fields.Length != values.Length)
            {
                throw new MySoftException("字段及值长度不一致！");
            }

            int index = 0;
            List<FieldValue> fvlist = new List<FieldValue>();
            foreach (Field field in fields)
            {
                FieldValue fv = new FieldValue(field, values[index]);

                if (isInsert && values[index] is Field)
                {
                    fv.IsIdentity = true;
                }
                else if (!isInsert)
                {
                    fv.IsChanged = true;
                }

                fvlist.Add(fv);

                index++;
            }

            return fvlist;
        }

        private static Random rnd = new Random();

        /// <summary>
        /// Makes a unique key.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="prefix">The prefix.</param>
        /// <returns></returns>
        internal static string MakeUniqueKey(int length, string prefix)
        {
            int prefixLength = prefix == null ? 0 : prefix.Length;

            string chars = "1234567890abcdefghijklmnopqrstuvwxyz";

            StringBuilder sb = new StringBuilder();
            if (prefixLength > 0)
            {
                sb.Append(prefix);
                sb.Append("_");
                prefixLength++;
            }

            int dupCount = 0;
            int preIndex = 0;
            for (int i = 0; i < length - prefixLength; ++i)
            {
                int index = rnd.Next(0, 35);
                if (index == preIndex)
                {
                    ++dupCount;
                }
                sb.Append(chars[index]);
                preIndex = index;
            }
            if (dupCount >= length - prefixLength - 2)
            {
                rnd = new Random();
                return MakeUniqueKey(length, prefix);
            }

            return sb.ToString();
        }

        internal static string RemoveTableAliasNamePrefixes(string sql)
        {
            if (string.IsNullOrEmpty(sql)) return sql;

            sql = sql.Replace("N'", "'");

            //匹配{0}Table{1}.{0}Field{1}的情况
            Regex r = new Regex(@"\{0\}([\w_]+)\{1\}.\{0\}([\w_]+)\{1\}");
            if (r.IsMatch(sql))
            {
                return r.Replace(sql, "$2");
            }
            else
            {
                r = new Regex(@"\{0\}([\w_]+)\{1\}.");
                return r.Replace(sql, "$1");
            }
        }

        internal static int GetEndIndexOfMethod(string cmdText, int startIndexOfCharIndex)
        {
            int foundEnd = -1;
            int endIndexOfCharIndex = 0;
            for (int i = startIndexOfCharIndex; i < cmdText.Length; ++i)
            {
                if (cmdText[i] == '(')
                {
                    --foundEnd;
                }
                else if (cmdText[i] == ')')
                {
                    ++foundEnd;
                }

                if (foundEnd == 0)
                {
                    endIndexOfCharIndex = i;
                    break;
                }
            }
            return endIndexOfCharIndex;
        }

        internal static string[] SplitTwoParamsOfMethodBody(string bodyText)
        {
            int colonIndex = 0;
            int foundEnd = 0;
            for (int i = 1; i < bodyText.Length - 1; i++)
            {
                if (bodyText[i] == '(')
                {
                    --foundEnd;
                }
                else if (bodyText[i] == ')')
                {
                    ++foundEnd;
                }

                if (bodyText[i] == ',' && foundEnd == 0)
                {
                    colonIndex = i;
                    break;
                }
            }

            return new string[] { bodyText.Substring(0, colonIndex), bodyText.Substring(colonIndex + 1) };
        }

        /// <summary>
        /// 检测是否为结构数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool CheckStruct(Type type)
        {
            //当属性为结构时进行系列化
            if (type.IsValueType && !type.IsEnum && !type.IsPrimitive && !type.IsSerializable)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 转换类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        private static object ChangeType(object value, Type conversionType)
        {
            if (value == null) return null;
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                conversionType = Nullable.GetUnderlyingType(conversionType);
            }

            if (value.GetType() == typeof(string))
            {
                bool success;
                value = ConverterFactory.Converters[conversionType].ConvertTo(value.ToString(), out success);
                if (success)
                    return value;
                else
                    throw new MySoftException(string.Format("【{0}】转换成数据类型【{1}】出错！", value, conversionType.Name));
            }
            else
                return Convert.ChangeType(value, conversionType);
        }

        #endregion
    }
}