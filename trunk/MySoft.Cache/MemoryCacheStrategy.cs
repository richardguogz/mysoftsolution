﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Collections;
using System.Text.RegularExpressions;

namespace MySoft.Cache
{
    /// <summary>
    /// 内存缓存管理类
    /// </summary>
    public class MemoryCacheStrategy : CacheStrategyBase, IMemoryCacheStrategy
    {
        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="objId"></param>
        public static void Remove(string objId)
        {
            lock (syncObject)
            {
                if (objId == null || objId.Length == 0)
                {
                    return;
                }

                webCache.Remove(objId);
            }
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void Clear()
        {
            lock (syncObject)
            {
                foreach (string objId in AllKeys)
                {
                    webCache.Remove(objId);
                }
            }
        }

        /// <summary>
        /// 获取所有Key值
        /// </summary>
        /// <returns></returns>
        public static IList<string> AllKeys
        {
            get
            {
                lock (syncObject)
                {
                    IDictionaryEnumerator cacheEnum = webCache.GetEnumerator();
                    IList<string> objIds = new List<string>();

                    while (cacheEnum.MoveNext())
                    {
                        objIds.Add(cacheEnum.Key.ToString());
                    }

                    return objIds;
                }
            }
        }

        /// <summary>
        /// 内存缓存单例
        /// </summary>
        public static readonly MemoryCacheStrategy Default = new MemoryCacheStrategy("default");

        private static volatile System.Web.Caching.Cache webCache = System.Web.HttpRuntime.Cache;
        private static readonly object syncObject = new object();
        private int _timeOut = 1440; // 默认缓存存活期为1440分钟(24小时)

        /// <summary>
        /// 实例化本地缓存
        /// </summary>
        /// <param name="regionName"></param>
        public MemoryCacheStrategy(string regionName) : base(regionName) { }

        /// <summary>
        /// 设置到期相对时间[单位：／分钟] 
        /// </summary>
        public int Timeout
        {
            set { _timeOut = value > 0 ? value : 6000; }
            get { return _timeOut > 0 ? _timeOut : 6000; }
        }

        /// <summary>
        /// 缓存对象
        /// </summary>
        public static System.Web.Caching.Cache CacheObject
        {
            get { return webCache; }
        }

        /// <summary>
        /// 缓存对象数
        /// </summary>
        public static int CacheCount
        {
            get { return AllKeys.Count; }
        }

        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        public void AddObject(string objId, object o)
        {
            if (objId == null || objId.Length == 0 || o == null)
            {
                return;
            }

            lock (syncObject)
            {
                CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

                if (Timeout == 6000)
                {
                    webCache.Insert(GetInputKey(objId), o, null, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
                }
                else
                {
                    webCache.Insert(GetInputKey(objId), o, null, DateTime.Now.AddMinutes(Timeout), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
                }
            }
        }

        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        public void AddObject(string objId, object o, TimeSpan expires)
        {
            AddObject(objId, o, DateTime.Now.Add(expires));
        }

        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        public void AddObject(string objId, object o, DateTime datetime)
        {
            if (objId == null || objId.Length == 0 || o == null)
            {
                return;
            }

            lock (syncObject)
            {
                CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

                webCache.Insert(GetInputKey(objId), o, null, datetime, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }

        /// <summary>
        /// 加入当前对象到缓存中,并对相关文件建立依赖
        /// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        /// <param name="files">监视的路径文件</param>
        public void AddObjectWithFileChange(string objId, object o, string[] files)
        {
            if (objId == null || objId.Length == 0 || o == null)
            {
                return;
            }

            lock (syncObject)
            {
                CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

                CacheDependency dep = new CacheDependency(files, DateTime.Now);

                webCache.Insert(GetInputKey(objId), o, dep, System.DateTime.Now.AddMinutes(Timeout), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }


        /// <summary>
        /// 加入当前对象到缓存中,并使用依赖键
        /// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        /// <param name="dependKey">依赖关联的键值</param>
        public void AddObjectWithDepend(string objId, object o, string[] dependKey)
        {
            if (objId == null || objId.Length == 0 || o == null)
            {
                return;
            }

            lock (syncObject)
            {
                CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

                CacheDependency dep = new CacheDependency(null, dependKey, DateTime.Now);

                webCache.Insert(GetInputKey(objId), o, dep, System.DateTime.Now.AddMinutes(Timeout), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }

        /// <summary>
        /// 加入当前对象到缓存中,并对相关文件建立依赖
        /// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        /// <param name="files">监视的路径文件</param>
        public void AddObjectWithFileChange(string objId, object o, TimeSpan expires, string[] files)
        {
            if (objId == null || objId.Length == 0 || o == null)
            {
                return;
            }

            lock (syncObject)
            {
                CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

                CacheDependency dep = new CacheDependency(files, DateTime.Now);

                webCache.Insert(GetInputKey(objId), o, dep, System.DateTime.Now.Add(expires), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }


        /// <summary>
        /// 加入当前对象到缓存中,并使用依赖键
        /// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        /// <param name="dependKey">依赖关联的键值</param>
        public void AddObjectWithDepend(string objId, object o, TimeSpan expires, string[] dependKey)
        {
            if (objId == null || objId.Length == 0 || o == null)
            {
                return;
            }

            lock (syncObject)
            {
                CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

                CacheDependency dep = new CacheDependency(null, dependKey, DateTime.Now);

                webCache.Insert(GetInputKey(objId), o, dep, System.DateTime.Now.Add(expires), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }

        /// <summary>
        /// 建立回调委托的一个实例
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="val"></param>
        /// <param name="reason"></param>
        public void onRemove(string objId, object val, CacheItemRemovedReason reason)
        {

            switch (reason)
            {
                case CacheItemRemovedReason.DependencyChanged:
                    break;
                case CacheItemRemovedReason.Expired:
                    break;
                case CacheItemRemovedReason.Removed:
                    break;
                case CacheItemRemovedReason.Underused:
                    break;
                default: break;
            }

            //如需要使用缓存日志,则需要使用下面代码
            //myLogVisitor.WriteLog(this,objId,val,reason);
        }

        /// <summary>
        /// 删除缓存对象
        /// </summary>
        /// <param name="objId">对象的关键字</param>
        public void RemoveObject(string objId)
        {
            if (objId == null || objId.Length == 0)
            {
                return;
            }

            lock (syncObject)
            {
                webCache.Remove(GetInputKey(objId));
            }
        }

        /// <summary>
        /// 返回一个指定的对象
        /// </summary>
        /// <param name="objId">对象的关键字</param>
        /// <returns>对象</returns>
        public object GetObject(string objId)
        {
            if (objId == null || objId.Length == 0)
            {
                return null;
            }

            lock (syncObject)
            {
                return webCache.Get(GetInputKey(objId));
            }
        }

        /// <summary>
        /// 返回指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        public T GetObject<T>(string objId)
        {
            if (objId == null || objId.Length == 0)
            {
                return default(T);
            }

            lock (syncObject)
            {
                return (T)GetObject(GetInputKey(objId));
            }
        }

        /// <summary>
        /// 返回指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        public object GetMatchObject(string regularExpression)
        {
            lock (syncObject)
            {
                IDictionary<string, object> values = GetMatchObjects(regularExpression);
                return values.Count > 0 ? values.First().Value : null;
            }
        }

        /// <summary>
        /// 返回指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        public T GetMatchObject<T>(string regularExpression)
        {
            lock (syncObject)
            {
                IDictionary<string, T> values = GetMatchObjects<T>(regularExpression);
                return values.Count > 0 ? values.First().Value : default(T);
            }
        }

        #region ICacheStrategy 成员

        /// <summary>
        /// 移除所有缓存对象
        /// </summary>
        public void RemoveAllObjects()
        {
            lock (syncObject)
            {
                IList<string> allKeys = GetAllKeys();
                RemoveObjects(allKeys);
            }
        }

        /// <summary>
        /// 获取所有的Key值
        /// </summary>
        /// <returns></returns>
        public IList<string> GetAllKeys()
        {
            lock (syncObject)
            {
                IDictionaryEnumerator cacheEnum = webCache.GetEnumerator();
                List<string> objIds = new List<string>();

                while (cacheEnum.MoveNext())
                {
                    objIds.Add(cacheEnum.Key.ToString());
                }

                objIds.RemoveAll(objId => !objId.StartsWith(prefix));
                return objIds.ConvertAll<string>(objId => GetOutputKey(objId));
            }
        }

        /// <summary>
        /// 获取缓存数
        /// </summary>
        /// <returns></returns>
        public int GetCacheCount()
        {
            lock (syncObject)
            {
                return GetAllKeys().Count;
            }
        }

        /// <summary>
        /// 获取所有对象
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, object> GetAllObjects()
        {
            lock (syncObject)
            {
                return GetObjects(GetAllKeys());
            }
        }

        /// <summary>
        /// 获取所有对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IDictionary<string, T> GetAllObjects<T>()
        {
            lock (syncObject)
            {
                return GetObjects<T>(GetAllKeys());
            }
        }

        /// <summary>
        /// 通过正则获取对应的Key列表
        /// </summary>
        /// <param name="regularExpression"></param>
        /// <returns></returns>
        public IList<string> GetKeys(string regularExpression)
        {
            lock (syncObject)
            {
                if (regularExpression == null || regularExpression.Length == 0)
                {
                    return new List<string>();
                }

                IList<string> objIds = new List<string>();
                Regex regex = new Regex(regularExpression, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

                foreach (var objId in GetAllKeys())
                {
                    if (regex.IsMatch(objId)) objIds.Add(objId);
                }

                return objIds;
            }
        }

        /// <summary>
        /// 添加多个对象到缓存
        /// </summary>
        /// <param name="data"></param>
        public void AddObjects(IDictionary<string, object> data)
        {
            lock (syncObject)
            {
                foreach (KeyValuePair<string, object> kv in data)
                {
                    AddObject(GetInputKey(kv.Key), kv.Value);
                }
            }
        }

        /// <summary>
        /// 添加多个对象到缓存
        /// </summary>
        /// <param name="data"></param>
        public void AddObjects<T>(IDictionary<string, T> data)
        {
            lock (syncObject)
            {
                foreach (KeyValuePair<string, T> kv in data)
                {
                    AddObject(GetInputKey(kv.Key), kv.Value);
                }
            }
        }

        /// <summary>
        /// 正则表达式方式移除对象
        /// </summary>
        /// <param name="regularExpression">匹配KEY正则表示式</param>
        public void RemoveMatchObjects(string regularExpression)
        {
            lock (syncObject)
            {
                var objIds = GetKeys(regularExpression);
                RemoveObjects(objIds);
            }
        }

        /// <summary>
        /// 移除多个对象
        /// </summary>
        /// <param name="objIds"></param>
        public void RemoveObjects(IList<string> objIds)
        {
            lock (syncObject)
            {
                foreach (string objId in objIds)
                {
                    RemoveObject(GetInputKey(objId));
                }
            }
        }

        /// <summary>
        /// 获取多个对象
        /// </summary>
        /// <param name="objIds"></param>
        /// <returns></returns>
        public IDictionary<string, object> GetObjects(IList<string> objIds)
        {
            lock (syncObject)
            {
                IDictionary<string, object> cacheData = new Dictionary<string, object>();
                foreach (string objId in objIds)
                {
                    var data = GetObject(GetInputKey(objId));
                    if (data != null)
                        cacheData.Add(objId, data);
                    else
                        cacheData.Add(objId, null);
                }

                return cacheData;
            }
        }

        /// <summary>
        /// 获取多个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objIds"></param>
        /// <returns></returns>
        public IDictionary<string, T> GetObjects<T>(IList<string> objIds)
        {
            lock (syncObject)
            {
                IDictionary<string, T> cacheData = new Dictionary<string, T>();
                foreach (string objId in objIds)
                {
                    var data = GetObject<T>(GetInputKey(objId));
                    if (data != null)
                        cacheData.Add(objId, data);
                    else
                        cacheData.Add(objId, default(T));
                }

                return cacheData;
            }
        }

        /// <summary>
        /// 返回指定正则表达式的对象
        /// </summary>
        /// <param name="regularExpression"></param>
        /// <returns></returns>
        public IDictionary<string, object> GetMatchObjects(string regularExpression)
        {
            lock (syncObject)
            {
                return GetObjects(GetKeys(regularExpression));
            }
        }

        /// <summary>
        /// 返回指定正则表达的对象
        /// </summary>
        /// <param name="regularExpression"></param>
        /// <returns></returns>
        public IDictionary<string, T> GetMatchObjects<T>(string regularExpression)
        {
            lock (syncObject)
            {
                return GetObjects<T>(GetKeys(regularExpression));
            }
        }

        #endregion
    }
}