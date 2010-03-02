﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace MySoft.Web.UI
{
    /// <summary>
    /// Callback参数值
    /// </summary>
    public class CallbackParam
    {
        private string keyValue;
        public string Value
        {
            get
            {
                return keyValue;
            }
        }

        internal CallbackParam(string value)
        {
            this.keyValue = value;
        }

        /// <summary>
        /// 将value转换成对应的类型值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T To<T>()
        {
            return To<T>(default(T));
        }

        /// <summary>
        /// 将value转换成对应的类型值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defvalue"></param>
        /// <returns></returns>
        public T To<T>(T defvalue)
        {
            return WebUtils.ConvertTo<T>(this.keyValue, defvalue);
        }

        public override string ToString()
        {
            return this.keyValue;
        }
    }

    /// <summary>
    /// 返回Callback参数字典
    /// </summary>
    public class CallbackParams
    {
        private Dictionary<string, CallbackParam> dictValues;
        internal CallbackParams()
        {
            this.dictValues = new Dictionary<string, CallbackParam>();
        }

        /// <summary>
        /// 重载字典
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public CallbackParam this[string key]
        {
            get
            {
                if (dictValues.ContainsKey(key))
                    return dictValues[key];

                return null;
            }
            set
            {
                dictValues[key] = value;
            }
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, string value)
        {
            if (Exists(key))
            {
                throw new Exception("已经存在此Key的值");
            }
            dictValues.Add(key, new CallbackParam(value));
        }

        /// <summary>
        /// 判断是否存在key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            return dictValues.ContainsKey(key);
        }

        /// <summary>
        /// 获取键及值
        /// </summary>
        public IList<KeyValuePair<string, CallbackParam>> KeyValues
        {
            get
            {
                var list = new List<KeyValuePair<string, CallbackParam>>();
                foreach (KeyValuePair<string, CallbackParam> pair in dictValues)
                {
                    list.Add(pair);
                }

                return list;
            }
        }
    }
}
