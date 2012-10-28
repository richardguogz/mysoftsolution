﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using MySoft.Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MySoft.RESTful.Utils
{
    /// <summary>
    /// 参数处理
    /// </summary>
    public class ParameterHelper
    {
        /// <summary>
        /// 参数解析
        /// </summary>
        /// <param name="paramters"></param>
        /// <param name="nvs"></param>
        /// <returns></returns>
        public static object[] Convert(ParameterInfo[] paramters, NameValueCollection nvget, NameValueCollection nvpost)
        {
            List<object> args = new List<object>();
            var obj = ConvertJObject(nvget, nvpost);

            foreach (ParameterInfo info in paramters)
            {
                var type = GetElementType(info.ParameterType);

                var property = obj.Properties().SingleOrDefault(p => string.Compare(p.Name, info.Name, true) == 0);
                if (property != null)
                {
                    try
                    {
                        //获取Json值
                        var jsonValue = CoreHelper.ConvertJsonValue(type, property.Value.ToString(Formatting.None));
                        args.Add(jsonValue);
                    }
                    catch (Exception ex)
                    {
                        throw new RESTfulException((int)HttpStatusCode.BadRequest, string.Format("Parameter [{0}] did not match type [{1}].",
                            info.Name, CoreHelper.GetTypeName(type)));
                    }
                }
                else
                {
                    throw new RESTfulException((int)HttpStatusCode.BadRequest, "Parameter [" + info.Name + "] is not found.");
                }
            }

            return args.ToArray();
        }

        private static Type GetElementType(Type type)
        {
            if (type.IsByRef) type = type.GetElementType();
            return type;
        }

        /// <summary>
        /// 转换成JObject
        /// </summary>
        /// <param name="nvget"></param>
        /// <param name="nvpost"></param>
        /// <returns></returns>
        private static JObject ConvertJObject(NameValueCollection nvget, NameValueCollection nvpost)
        {
            var obj = new JObject();
            if (nvget.Count > 0)
            {
                foreach (var key in nvget.AllKeys)
                {
                    obj[key] = UrlDecodeString(nvget[key]);
                }
            }

            if (nvpost.Count > 0)
            {
                foreach (var key in nvpost.AllKeys)
                {
                    try
                    {
                        obj[key] = JContainer.Parse(UrlDecodeString(nvpost[key]));
                    }
                    catch
                    {
                        obj[key] = UrlDecodeString(nvpost[key]);
                    }
                }
            }

            //转换成Json对象
            return obj;
        }

        /// <summary>
        /// 转换成NameValueCollection
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static NameValueCollection ConvertCollection(string data)
        {
            //处理成Form方式
            var values = HttpUtility.ParseQueryString(data, Encoding.UTF8);

            //为0表示为json方式
            if (values.Count == 0 || (values.Count == 1 && values.AllKeys[0] == null))
            {
                try
                {
                    //清除所的值
                    values.Clear();

                    //保持与Json兼容处理
                    var jobj = JObject.Parse(UrlDecodeString(data));
                    foreach (var kvp in jobj)
                    {
                        values[kvp.Key] = kvp.Value.ToString();
                    }
                }
                catch (Exception ex)
                {
                    //TODO 不做处理
                    SimpleLog.Instance.WriteLogForDir("DataConvert", ex);
                }
            }

            return values;
        }

        /// <summary>
        /// 反编码字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string UrlDecodeString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return HttpUtility.UrlDecode(value, Encoding.UTF8);
        }
    }
}