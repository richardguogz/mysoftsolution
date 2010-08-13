﻿using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Core.Converter;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace MySoft.Core
{
    /// <summary>
    /// 常用方法
    /// </summary>
    public static class CoreUtils
    {
        #region 对象克隆

        /// <summary>
        /// 克隆一个对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object CloneObject(object obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                return formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// 克隆一个Object对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static T CloneObject<T>(T entity)
            where T : class
        {
            if (entity == null) return default(T);
            Type type = entity.GetType();
            object t = GetFastInstanceCreator(type)();
            var fiels = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            var list = new List<FieldInfo>(fiels);
            foreach (PropertyInfo p in type.GetProperties())
            {
                if (!CanUseType(p.PropertyType)) continue; //shallow only
                object value = GetPropertyValue(entity, p.Name);
                if (value == null) continue;

                //对属性赋值
                FieldInfo field = list.Find(f => f.Name == "_" + p.Name);
                if (field != null) field.SetValue(t, value);
            }

            return (T)t;
        }

        private static bool CanUseType(Type propertyType)
        {
            //only strings and value types
            if (propertyType.IsArray) return false;
            if (!propertyType.IsValueType && propertyType != typeof(string)) return false;
            return true;
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
            return (T)GetFastInstanceCreator(typeof(T))();
        }

        /// <summary>
        /// 创建一个委托
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static FastCreateInstanceHandler GetFastInstanceCreator(Type type)
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
        /// <param name="method"></param>
        /// <returns></returns>
        public static FastInvokeHandler GetFastMethodInvoke(MethodInfo method)
        {
            FastInvokeHandler invoke = DynamicCalls.GetMethodInvoker(method);
            return invoke;
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

        #region 值转换

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

            if (value is T)
            {
                return (T)value;
            }
            else
            {
                object obj = ConvertValue(typeof(T), value);
                if (obj == null)
                {
                    return default(T);
                }
                return (T)obj;
            }
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

                //如果当前值是从类型Type分配
                if (type.IsAssignableFrom(valueType))
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

        #endregion

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

        #region 转换和系列化相关

        /// <summary>
        /// 将value转换成对应的类型值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(string value, T defaultValue)
        {
            bool isNullable = false;
            Type conversionType = typeof(T);
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                conversionType = Nullable.GetUnderlyingType(conversionType);
                isNullable = true;
            }

            bool success;
            if (ConverterFactory.Converters.ContainsKey(conversionType))
            {
                //如果转换的值为空并且对象可为空时返回默认值
                if (string.IsNullOrEmpty(value) && isNullable) return defaultValue;

                object obj = ConverterFactory.Converters[conversionType].ConvertTo(value, out success);
                if (success) return (T)obj;
            }

            return defaultValue;
        }

        /// <summary>
        /// 将对象系列化成字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            if (obj == null)
            {
                return "null";
            }

            Type type = obj.GetType();
            if (type.IsEnum)
            {
                return JavaScriptConvert.ToString((Enum)obj);
            }
            else if (type == typeof(string))
            {
                return JavaScriptConvert.ToString(obj.ToString());
            }
            else if (type == typeof(DateTime))
            {
                return JavaScriptConvert.ToString((DateTime)obj);
            }
            else if (type == typeof(bool))
            {
                return JavaScriptConvert.ToString((bool)obj);
            }
            else if (type.IsValueType)
            {
                return obj.ToString();
            }
            return JavaScriptConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 将字符串反系列化成对象
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="JSONString"></param>
        /// <returns></returns>
        public static object Deserialize(string jsonString, Type type)
        {
            if (type.IsArray && jsonString != null && !jsonString.StartsWith("["))
            {
                jsonString = "[" + jsonString + "]";
            }
            return JavaScriptConvert.DeserializeObject(jsonString, type);
        }

        /// <summary>
        /// 将字符串反系列化成对象
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="JSONString"></param>
        /// <returns></returns>
        public static TObject Deserialize<TObject>(string jsonString)
        {
            return (TObject)Deserialize(jsonString, typeof(TObject));
        }

        #endregion

        #region 常用方法

        /// <summary>
        /// Makes a unique key.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="prefix">The prefix.</param>
        /// <returns></returns>
        public static string MakeUniqueKey(int length, string prefix)
        {
            if (prefix != null)
            {
                //如果传入的前缀长度大于总长度，则抛出错误
                if (prefix.Length >= length)
                {
                    throw new ArgumentException("错误的前缀，传入的前缀长度大于总长度！");
                }
            }

            int prefixLength = prefix == null ? 0 : prefix.Length;

            string chars = "1234567890abcdefghijklmnopqrstuvwxyz";

            StringBuilder sb = new StringBuilder();
            if (prefixLength > 0) sb.Append(prefix);

            int dupCount = 0;
            int preIndex = 0;

            Random rnd = new Random(Guid.NewGuid().GetHashCode());
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
                rnd = new Random(Guid.NewGuid().GetHashCode());
                return MakeUniqueKey(length, prefix);
            }

            return sb.ToString();
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

            bool isNullable = false;
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                conversionType = Nullable.GetUnderlyingType(conversionType);
                isNullable = true;
            }

            //进行字符串类型转换
            if (value.GetType() == typeof(string))
            {
                string data = value.ToString();

                //如果转换的值为空并且对象可为空时返回null
                if (string.IsNullOrEmpty(data) && isNullable) return null;

                bool success;
                value = ConverterFactory.Converters[conversionType].ConvertTo(data, out success);
                if (success)
                    return value;
                else
                    throw new MySoftException(string.Format("【{0}】转换成数据类型【{1}】出错！", value, conversionType.Name));
            }
            else
                return Convert.ChangeType(value, conversionType);
        }

        /// <summary>
        /// 获取指定长度的字符串，按字节长度
        /// </summary>
        /// <param name="p_SrcString"></param>
        /// <param name="p_Length"></param>
        /// <param name="p_TailString"></param>
        /// <returns></returns>
        internal static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            string text = p_SrcString;
            if (p_Length < 0)
            {
                return text;
            }
            byte[] sourceArray = Encoding.Default.GetBytes(p_SrcString);
            if (sourceArray.Length <= p_Length)
            {
                return text;
            }
            int length = p_Length;
            int[] numArray = new int[p_Length];
            byte[] destinationArray = null;
            int num2 = 0;
            for (int i = 0; i < p_Length; i++)
            {
                if (sourceArray[i] > 0x7f)
                {
                    num2++;
                    if (num2 == 3)
                    {
                        num2 = 1;
                    }
                }
                else
                {
                    num2 = 0;
                }
                numArray[i] = num2;
            }
            if ((sourceArray[p_Length - 1] > 0x7f) && (numArray[p_Length - 1] == 1))
            {
                length = p_Length + 1;
            }
            destinationArray = new byte[length];
            Array.Copy(sourceArray, destinationArray, length);
            return (Encoding.Default.GetString(destinationArray) + p_TailString);
        }

        #endregion
    }
}
