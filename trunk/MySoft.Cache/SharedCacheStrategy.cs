using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web.Caching;
using SharedCache.WinServiceCommon.Provider.Cache;
using System.Text.RegularExpressions;
using SharedCache.WinServiceCommon.Formatters;

namespace MySoft.Cache
{
    /// <summary>
    /// 分布式缓存管理类
    /// </summary>
    public class SharedCacheStrategy : CacheStrategyBase, ISharedCacheStrategy
    {
        private ICacheStrategy localCache;
        private TimeSpan localTimeSpan;

        /// <summary>
        /// 设置本地缓存超时时间
        /// </summary>
        /// <param name="timeout">超时时间，单位：秒</param>
        public void SetLocalCacheTimeout(int timeout)
        {
            if (timeout > 0)
            {
                this.localCache = CacheFactory.CreateCache("Local_" + base.regionName, CacheType.Local);
                this.localCache.Timeout = timeout;
                this.localTimeSpan = TimeSpan.FromSeconds(timeout);
            }
            else
                this.localCache = null;
        }

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

                dataCache.Remove(objId);
            }
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void Clear()
        {
            lock (syncObject)
            {
                dataCache.Clear();
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
                    return dataCache.GetAllKeys();
                }
            }
        }

        /// <summary>
        /// 分布式缓存单例
        /// </summary>
        public static readonly SharedCacheStrategy Default = new SharedCacheStrategy("defaultCache");

        private static volatile IndexusProviderBase dataCache = IndexusDistributionCache.SharedCache;
        private static readonly object syncObject = new object();

        /// <summary>
        /// 实例化分布式缓存
        /// </summary>
        /// <param name="regionName"></param>
        public SharedCacheStrategy(string regionName) : base(regionName) { }

        /// <summary>
        /// 缓存对象
        /// </summary>
        public static IndexusProviderBase CacheObject
        {
            get { return dataCache; }
        }

        /// <summary>
        /// 缓存对象数
        /// </summary>
        public static int CacheCount
        {
            get { return AllKeys.Count; }
        }

        /// <summary>
        /// 添加指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        public void AddObject(string objId, object o)
        {
            if (objId == null || objId.Length == 0 || o == null)
            {
                return;
            }

            lock (syncObject)
            {
                if (Timeout <= 0)
                {
                    dataCache.Add(GetInputKey(objId), o);
                }
                else
                {
                    dataCache.Add(GetInputKey(objId), o, DateTime.Now.AddSeconds(Timeout));
                }

                //处理本地缓存
                if (localCache != null) localCache.AddObject(objId, o, localTimeSpan);
            }
        }

        /// <summary>
        /// 添加指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        public void AddObject(string objId, object o, TimeSpan expires)
        {
            AddObject(objId, o, DateTime.Now.Add(expires));
        }

        /// <summary>
        /// 添加指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        public void AddObject(string objId, object o, DateTime datetime)
        {
            if (objId == null || objId.Length == 0 || o == null)
            {
                return;
            }

            lock (syncObject)
            {
                if (Timeout > 0)
                {
                    dataCache.Add(GetInputKey(objId), o, datetime);
                }
                else
                {
                    dataCache.Add(GetInputKey(objId), o);
                }

                //处理本地缓存
                if (localCache != null) localCache.AddObject(objId, o, localTimeSpan);
            }
        }

        /// <summary>
        /// 移除指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        public void RemoveObject(string objId)
        {
            if (objId == null || objId.Length == 0)
            {
                return;
            }

            lock (syncObject)
            {
                dataCache.Remove(GetInputKey(objId));

                //处理本地缓存
                if (localCache != null) localCache.RemoveObject(objId);
            }
        }

        /// <summary>
        /// 返回指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        public object GetObject(string objId)
        {
            if (objId == null || objId.Length == 0)
            {
                return null;
            }

            lock (syncObject)
            {
                object returnObject = null;

                //处理本地缓存
                if (localCache != null)
                {
                    returnObject = localCache.GetObject(objId);
                    if (returnObject != null) return returnObject;
                }

                returnObject = dataCache.Get(GetInputKey(objId));

                //添加到本地缓存
                if (returnObject != null && localCache != null)
                {
                    localCache.AddObject(objId, returnObject, localTimeSpan);
                }

                return returnObject;
            }
        }

        /// <summary>
        /// 返回指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        public T GetObject<T>(string objId)
        {
            return (T)GetObject(objId);
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
                RemoveObjects(GetAllKeys());
            }
        }

        /// <summary>
        /// 获取所有Key值
        /// </summary>
        /// <returns></returns>
        public IList<string> GetAllKeys()
        {
            lock (syncObject)
            {
                var objIds = dataCache.GetAllKeys();

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
            if (regularExpression == null || regularExpression.Length == 0)
            {
                return new List<string>();
            }

            lock (syncObject)
            {
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
        /// 添加多个对象
        /// </summary>
        /// <param name="data"></param>
        public void AddObjects(IDictionary<string, object> data)
        {
            lock (syncObject)
            {
                IDictionary<string, byte[]> cacheData = new Dictionary<string, byte[]>();
                foreach (KeyValuePair<string, object> kv in data)
                {
                    if (kv.Value != null)
                        cacheData.Add(GetInputKey(kv.Key), Serialization.BinarySerialize(kv.Value));
                }

                dataCache.MultiAdd(cacheData);
            }
        }

        /// <summary>
        /// 添加多个对象
        /// </summary>
        /// <param name="data"></param>
        public void AddObjects<T>(IDictionary<string, T> data)
        {
            lock (syncObject)
            {
                IDictionary<string, byte[]> cacheData = new Dictionary<string, byte[]>();
                foreach (KeyValuePair<string, T> kv in data)
                {
                    if (kv.Value != null)
                        cacheData.Add(GetInputKey(kv.Key), Serialization.BinarySerialize(kv.Value));
                }

                dataCache.MultiAdd(cacheData);
            }
        }

        /// <summary>
        /// 正则表达式方式移除对象
        /// </summary>
        /// <param name="regularExpression">匹配KEY正则表示式</param>
        public void RemoveMatchObjects(string regularExpression)
        {
            if (regularExpression == null || regularExpression.Length == 0)
            {
                return;
            }

            lock (syncObject)
            {
                dataCache.RegexRemove(prefix + regularExpression);
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
                var objIdList = new List<string>(objIds);
                objIdList = objIdList.ConvertAll<string>(objId => GetInputKey(objId));
                objIdList = (from item in objIdList select item).Distinct().ToList();

                dataCache.MultiDelete(objIdList);
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
                var objIdList = new List<string>(objIds);
                objIdList = objIdList.ConvertAll<string>(objId => GetInputKey(objId));
                objIdList = (from item in objIdList select item).Distinct().ToList();

                IDictionary<string, object> cacheData = new Dictionary<string, object>();

                var data = dataCache.MultiGet(objIdList);
                if (data != null)
                {
                    foreach (KeyValuePair<string, byte[]> kv in data)
                    {
                        if (kv.Value != null)
                            cacheData.Add(GetOutputKey(kv.Key), Serialization.BinaryDeSerialize<object>(kv.Value));
                        else
                            cacheData.Add(GetOutputKey(kv.Key), null);
                    }
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
                var objIdList = new List<string>(objIds);
                objIdList = objIdList.ConvertAll<string>(objId => GetInputKey(objId));
                objIdList = (from item in objIdList select item).Distinct().ToList();

                IDictionary<string, T> cacheData = new Dictionary<string, T>();

                var data = dataCache.MultiGet(objIdList);
                if (data != null)
                {
                    foreach (KeyValuePair<string, byte[]> kv in data)
                    {
                        if (kv.Value != null)
                            cacheData.Add(GetOutputKey(kv.Key), Serialization.BinaryDeSerialize<T>(kv.Value));
                        else
                            cacheData.Add(GetOutputKey(kv.Key), default(T));
                    }
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
