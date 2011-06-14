﻿using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MySoft.RESTful
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
        /// 解析参数
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static JObject Resolve(string parameters, ParameterFormat format)
        {
            switch (format)
            {
                case ParameterFormat.Json:
                    return JObject.Parse(parameters);
                case ParameterFormat.Xml:
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(parameters);
                    return JObject.Parse(JsonConvert.SerializeXmlNode(doc));
                default:
                    return new JObject();
            }
        }

        public static object[] Convert(ParameterInfo[] paramters, JObject obj)
        {
            try
            {
                List<object> args = new List<object>();
                foreach (ParameterInfo info in paramters)
                {
                    var property = obj.Properties().SingleOrDefault(p => string.Compare(p.Name, info.Name, true) == 0);
                    if (property == null)
                        throw new NullReferenceException(info.Name + " is not found in parameters!");
                    string value = property.Value.ToString(Newtonsoft.Json.Formatting.None);
                    var jsonValue = SerializationManager.DeserializeJson(info.ParameterType, value);
                    args.Add(jsonValue);
                }

                return args.ToArray();
            }
            catch
            {
                throw new RESTfulException("Parameter type did not match!") { Code = RESTfulCode.BUSINESS_METHOD_PARAMS_TYPE_NOT_MATCH };
            }
        }
    }
}
