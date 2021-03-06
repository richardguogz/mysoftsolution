﻿using System;

namespace MySoft.RESTful.SDK
{
    /// <summary>
    /// Token参数
    /// </summary>
    public class Token
    {
        /// <summary>
        /// TokenID
        /// </summary>
        public string TokenId { get; set; }

        /// <summary>
        /// 参数集合
        /// </summary>
        public ApiParameterCollection Parameters { get; set; }

        /// <summary>
        /// 实例化Token
        /// </summary>
        public Token()
        {
            this.TokenId = Guid.NewGuid().ToString();
            this.Parameters = new ApiParameterCollection();
        }

        /// <summary>
        /// 实例化Token
        /// </summary>
        /// <param name="tokenId"></param>
        public Token(string tokenId)
        {
            this.TokenId = tokenId;
            this.Parameters = new ApiParameterCollection();
        }

        /// <summary>
        /// 解析一个参数
        /// </summary>
        /// <param name="urlParameter"></param>
        public static Token Parse(string urlParameter)
        {
            Token token = new Token();
            var items = urlParameter.Split('&');
            foreach (var item in items)
            {
                var vitem = item.Split('=');
                token.AddParameter(vitem[0], vitem[1]);
            }

            return token;
        }

        /// <summary>
        /// 查找指定名称的对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ApiParameter Find(string name)
        {
            return this.Parameters.Find(p => p.Name == name);
        }

        /// <summary>
        /// 添加一个参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddParameter(string name, object value)
        {
            this.Parameters.Add(name, value);
        }

        /// <summary>
        /// 添加一组参数
        /// </summary>
        /// <param name="names"></param>
        /// <param name="values"></param>
        public void AddParameter(string[] names, object[] values)
        {
            for (int index = 0; index < names.Length; index++)
            {
                AddParameter(names[index], values[index]);
            }
        }

        /// <summary>
        /// 添加一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public void AddParameter<T>(T item)
            where T : class
        {
            //添加对象参数
            foreach (var p in CoreHelper.GetPropertiesFromType(item.GetType()))
            {
                AddParameter(p.Name, CoreHelper.GetPropertyValue(item, p));
            }
        }
    }
}
