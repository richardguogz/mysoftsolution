﻿using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using MySoft.RESTful.Auth;

namespace MySoft.RESTful.Utils
{
    /// <summary>
    /// 参数处理
    /// </summary>
    public class ParameterHelper
    {
        /// <summary>
        /// 解析参数
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static JObject Resolve(NameValueCollection parameters)
        {
            JObject obj = new JObject();
            foreach (var key in parameters.AllKeys)
            {
                obj.Add(key, parameters[key]);
            }
            return obj;
        }

        /// <summary>
        /// 参数解析
        /// </summary>
        /// <param name="paramters"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object[] Convert(ParameterInfo[] paramters, JObject obj)
        {
            try
            {
                List<object> args = new List<object>();
                foreach (ParameterInfo info in paramters)
                {
                    var property = obj.Properties().SingleOrDefault(p => string.Compare(p.Name, info.Name, true) == 0);
                    if (property != null)
                    {
                        string value = property.Value.ToString(Newtonsoft.Json.Formatting.None);
                        object jsonValue = null;

                        if (!(string.IsNullOrEmpty(value) || value == "{}"))
                        {
                            var type = GetPrimitiveType(info.ParameterType);
                            if (type.IsArray)
                            {
                                value = string.Format("[{0}]", value.Replace(",", "\",\""));
                            }
                            else if (type.IsGenericType)
                            {
                                var t = type.GetGenericTypeDefinition();
                                if (typeof(IList<>).IsAssignableFrom(t))
                                    value = string.Format("[{0}]", value.Replace(",", "\",\""));
                            }

                            if (value.Contains("new Date"))
                                jsonValue = SerializationManager.DeserializeJson(type, value, new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
                            else
                                jsonValue = SerializationManager.DeserializeJson(type, value);
                        }

                        if (jsonValue == null)
                            jsonValue = CoreHelper.GetTypeDefaultValue(GetPrimitiveType(info.ParameterType));

                        args.Add(jsonValue);
                    }
                    else
                    {
                        //没有的参数使用默认值
                        var jsonValue = CoreHelper.GetTypeDefaultValue(GetPrimitiveType(info.ParameterType));
                        args.Add(jsonValue);
                    }
                }

                return args.ToArray();
            }
            catch (NullReferenceException ex)
            {
                throw new RESTfulException(ex.Message) { Code = RESTfulCode.BUSINESS_METHOD_PARAMS_TYPE_NOT_MATCH };
            }
            catch
            {
                throw new RESTfulException("Parameter type did not match!") { Code = RESTfulCode.BUSINESS_METHOD_PARAMS_TYPE_NOT_MATCH };
            }
        }

        private static Type GetPrimitiveType(Type type)
        {
            if (type.IsByRef) type = type.GetElementType();
            return type;
        }
    }
}
