using System;
using System.Collections.Generic;
using System.Threading;

namespace MySoft
{
    /// <summary>
    /// 缓存操作
    /// </summary>
    public static class CacheHelper
    {
        private static IDictionary<Type, object> mItemCaches = new Dictionary<Type, object>();

        static CacheHelper()
        {
            Timer timer = new Timer(new TimerCallback(CacheHelper.DoClear), null, 3600000, 3600000);
        }

        /// <summary>
        /// 获取指定key的缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            return GetTypeCache<T>()[key];
        }

        /// <summary>
        /// 移除指定key的缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public static void Remove<T>(string key)
        {
            GetTypeCache<T>().Remove(key);
        }

        /// <summary>
        /// 移除所有缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void RemoveAll<T>()
        {
            GetTypeCache<T>().RemoveAll();
        }

        /// <summary>
        /// 设置缓存信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="item"></param>
        public static void Set<T>(string key, T item)
        {
            GetTypeCache<T>()[key] = item;
        }

        private static void DoClear(object state)
        {
            lock (typeof(CacheHelper))
            {
                foreach (Type type in mItemCaches.Keys)
                {
                    ((IDisposable)mItemCaches[type]).Dispose();
                }
            }
        }

        private static TypeCache<T> GetTypeCache<T>()
        {
            Type key = typeof(T);
            if (mItemCaches.ContainsKey(key))
            {
                return (TypeCache<T>)mItemCaches[key];
            }
            lock (typeof(CacheHelper))
            {
                if (mItemCaches.ContainsKey(key))
                {
                    return (TypeCache<T>)mItemCaches[key];
                }
                TypeCache<T> cache = new TypeCache<T>();
                mItemCaches.Add(key, cache);
                return cache;
            }
        }

        private class CacheItem<T>
        {
            private DateTime mLastTime;
            private T mSource;

            public CacheItem(T value)
            {
                this.Source = value;
            }

            public T Source
            {
                get
                {
                    this.mLastTime = DateTime.Now;
                    return this.mSource;
                }
                set
                {
                    this.mLastTime = DateTime.Now;
                    this.mSource = value;
                }
            }

            public bool TimeOut
            {
                get
                {
                    TimeSpan span = (TimeSpan)(DateTime.Now - this.mLastTime);
                    return (span.Hours > 1);
                }
            }
        }

        private class TypeCache<T> : IDisposable
        {
            private IDictionary<string, CacheHelper.CacheItem<T>> mCache;

            public TypeCache()
            {
                this.mCache = new Dictionary<string, CacheHelper.CacheItem<T>>();
            }

            public void Dispose()
            {
                lock (((CacheHelper.TypeCache<T>)this))
                {
                    string[] array = new string[this.mCache.Keys.Count];
                    this.mCache.Keys.CopyTo(array, 0);
                    foreach (string str in array)
                    {
                        if (this.mCache[str].TimeOut)
                        {
                            this.mCache.Remove(str);
                        }
                    }
                }
            }

            public void Remove(string key)
            {
                lock (((CacheHelper.TypeCache<T>)this))
                {
                    this.mCache.Remove(key);
                }
            }

            public void RemoveAll()
            {
                lock (((CacheHelper.TypeCache<T>)this))
                {
                    this.mCache.Clear();
                }
            }

            public T this[string key]
            {
                get
                {
                    lock (((CacheHelper.TypeCache<T>)this))
                    {
                        if (this.mCache.ContainsKey(key))
                        {
                            return this.mCache[key].Source;
                        }
                        return default(T);
                    }
                }
                set
                {
                    lock (((CacheHelper.TypeCache<T>)this))
                    {
                        if (this.mCache.ContainsKey(key))
                        {
                            this.mCache[key].Source = value;
                        }
                        else
                        {
                            this.mCache.Add(key, new CacheHelper.CacheItem<T>(value));
                        }
                    }
                }
            }
        }
    }
}
