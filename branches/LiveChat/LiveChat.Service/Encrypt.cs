using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace LiveChat.Service
{
    /// <summary>
    /// 加密类
    /// </summary>
    public static class Encrypt
    {
        /// <summary>
        /// 对字符串进行MD5加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string MD5(string text)
        {
            //MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            //byte[] bytes = Encoding.UTF8.GetBytes(text);
            //byte[] buffer2 = provider.ComputeHash(bytes);
            //provider.Clear();
            //string str = "";
            //int index = 0;
            //int length = buffer2.Length;
            //while (index < length)
            //{
            //    str = str + buffer2[index].ToString("x").PadLeft(2, '0');
            //    index++;
            //}
            //return str;

            return text;
        }
    }
}
