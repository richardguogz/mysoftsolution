﻿using System;
using System.Collections;
using System.IO;
using MySoft.Logger;

namespace MySoft.IoC
{
    /// <summary>
    /// 文件缓存帮助类
    /// </summary>
    internal static class FileCacheHelper
    {
        /// <summary>
        /// 从文件读取对象
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static CacheItem GetCache(string filePath)
        {
            try
            {
                //从文件读取对象
                if (File.Exists(filePath))
                {
                    var buffer = File.ReadAllBytes(filePath);
                    return SerializationManager.DeserializeBin<CacheItem>(buffer);
                }
            }
            catch (IOException ex) { }
            catch (Exception ex)
            {
                SimpleLog.Instance.WriteLogForDir("CacheHelper", ex);
            }

            return null;
        }

        /// <summary>
        /// （本方法仅适应于本地缓存）
        /// 从缓存中获取数据，如获取失败，返回从指定的方法中获取
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeout"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static ResponseItem Get(CacheKey key, TimeSpan timeout, Func<ResponseItem> func)
        {
            return Get(key, timeout, state => func(), null);
        }

        /// <summary>
        /// （本方法仅适应于本地缓存）
        /// 从缓存中获取数据，如获取失败，返回从指定的方法中获取
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="timeout"></param>
        /// <param name="func"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static ResponseItem Get(CacheKey cacheKey, TimeSpan timeout, Func<object, ResponseItem> func, object state)
        {
            //定义缓存项
            ResponseItem cacheItem = null;

            lock (GetSyncRoot(cacheKey))
            {
                var cacheObj = GetCacheItem(cacheKey, timeout);

                if (cacheObj == null)
                {
                    //获取数据缓存
                    cacheItem = func(state);

                    if (cacheItem != null && cacheItem.Buffer != null)
                    {
                        //插入缓存
                        InsertCacheItem(cacheKey, cacheItem, timeout);
                    }
                }
                else
                {
                    //判断是否过期
                    if (cacheObj.ExpiredTime < DateTime.Now)
                    {
                        //更新缓存项
                        UpdateCacheItem(cacheKey, func, state, timeout);
                    }

                    //获取数据缓存
                    cacheItem = new ResponseItem { Buffer = cacheObj.Value, Count = cacheObj.Count };
                }
            }

            //返回缓存的对象
            return cacheItem;
        }

        /// <summary>
        /// 更新缓存项
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="func"></param>
        /// <param name="state"></param>
        /// <param name="timeout"></param>
        private static void UpdateCacheItem(CacheKey cacheKey, Func<object, ResponseItem> func, object state, TimeSpan timeout)
        {
            //开始异步调用
            func.BeginInvoke(state, ar =>
            {
                try
                {
                    var arr = ar.AsyncState as ArrayList;
                    var _key = arr[0] as CacheKey;
                    var _func = arr[1] as Func<object, ResponseItem>;
                    var _timeout = (TimeSpan)arr[2];

                    //获取数据缓存
                    var cacheItem = _func.EndInvoke(ar);

                    if (cacheItem != null && cacheItem.Buffer != null)
                    {
                        //插入缓存
                        InsertCacheItem(_key, cacheItem, _timeout);
                    }
                }
                finally
                {
                    //关闭句柄
                    ar.AsyncWaitHandle.Close();
                }
            }, new ArrayList { cacheKey, func, timeout });
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        private static CacheItem GetCacheItem(CacheKey cacheKey, TimeSpan timeout)
        {
            var path = GetFilePath(cacheKey);

            //从文件获取缓存
            return GetCache(path);
        }

        /// <summary>
        /// 插入缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="item"></param>
        /// <param name="timeout"></param>
        private static void InsertCacheItem(CacheKey cacheKey, ResponseItem item, TimeSpan timeout)
        {
            //序列化成二进制
            var cacheObj = new CacheItem
            {
                ExpiredTime = DateTime.Now.Add(timeout),
                Count = item.Count,
                Value = item.Buffer
            };

            try
            {
                var path = GetFilePath(cacheKey);

                //写入文件
                var buffer = SerializationManager.SerializeBin(cacheObj);

                lock (GetSyncRoot(cacheKey))
                {
                    string dirPath = Path.GetDirectoryName(path);
                    if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
                    File.WriteAllBytes(path, buffer);
                }
            }
            catch (IOException ex) { }
            catch (Exception ex)
            {
                SimpleLog.Instance.WriteLogForDir("CacheHelper", ex);
            }
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        private static string GetFilePath(CacheKey cacheKey)
        {
            return CoreHelper.GetFullPath(string.Format("ServiceCache\\{0}\\{1}\\{2}.dat",
                                            cacheKey.ServiceName, cacheKey.MethodName, cacheKey.UniqueId));
        }

        //读文件同步
        private static readonly Hashtable hashtable = new Hashtable();

        /// <summary>
        /// 获取同步Root
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        private static object GetSyncRoot(CacheKey cacheKey)
        {
            //var key = string.Format("{0}${1}", cacheKey.ServiceName, cacheKey.MethodName);
            var key = cacheKey.UniqueId;

            lock (hashtable.SyncRoot)
            {
                if (!hashtable.ContainsKey(key))
                {
                    hashtable[key] = new object();
                }

                return hashtable[key];
            }
        }
    }
}