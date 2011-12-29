using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;

namespace MySoft20
{
    /// <summary>
    /// ���������
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// DayFactor
        /// </summary>
        public static readonly int DayFactor = 17280;
        /// <summary>
        /// HourFactor
        /// </summary>
        public static readonly int HourFactor = 720;
        /// <summary>
        /// MinuteFactor
        /// </summary>
        public static readonly int MinuteFactor = 12;
        /// <summary>
        /// SecondFactor
        /// </summary>
        public static readonly double SecondFactor = 0.2;

        private static readonly System.Web.Caching.Cache _cache;

        private static int Factor = 1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheFactor"></param>
        public static void ReSetFactor(int cacheFactor)
        {
            Factor = cacheFactor;
        }

        /// <summary>
        /// ȷ����ǰHttpContextֻ��һ��Cacheʵ��
        /// </summary>
        static CacheHelper()
        {
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                _cache = context.Cache;
            }
            else
            {
                _cache = HttpRuntime.Cache;
            }
        }

        /// <summary>
        /// ���Cache
        /// </summary>
        public static void Clear()
        {
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();

            while (CacheEnum.MoveNext())
            {
                _cache.Remove(CacheEnum.Key.ToString());
            }
        }

        /// <summary>
        /// ����������ʽ��ģʽ�Ƴ�Cache
        /// </summary>
        /// <param name="pattern">ģʽ</param>
        public static void RemoveByPattern(string pattern)
        {
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
            while (CacheEnum.MoveNext())
            {
                if (regex.IsMatch(CacheEnum.Key.ToString()))
                    _cache.Remove(CacheEnum.Key.ToString());
            }
        }

        /// <summary>
        /// ���ݼ�ֵ�Ƴ�Cache
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            _cache.Remove(key);
        }

        /// <summary>
        /// �Ѷ�����ص�Cache
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="obj">����</param>
        public static void Insert(string key, object obj)
        {
            Insert(key, obj, null, 1);
        }

        /// <summary>
        /// �Ѷ�����ص�Cache,���ӻ���������Ϣ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="dep"></param>
        public static void Insert(string key, object obj, CacheDependency dep)
        {
            Insert(key, obj, dep, MinuteFactor * 3);
        }

        /// <summary>
        /// �Ѷ�����ص�Cache,���ӹ���ʱ����Ϣ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="seconds"></param>
        public static void Insert(string key, object obj, int seconds)
        {
            Insert(key, obj, null, seconds);
        }

        /// <summary>
        /// �Ѷ�����ص�Cache,���ӹ���ʱ����Ϣ�����ȼ�
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="seconds"></param>
        /// <param name="priority"></param>
        public static void Insert(string key, object obj, int seconds, CacheItemPriority priority)
        {
            Insert(key, obj, null, seconds, priority);
        }

        /// <summary>
        /// �Ѷ�����ص�Cache,���ӻ��������͹���ʱ��(����������)
        /// (Ĭ�����ȼ�ΪNormal)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="dep"></param>
        /// <param name="seconds"></param>
        public static void Insert(string key, object obj, CacheDependency dep, int seconds)
        {
            Insert(key, obj, dep, seconds, CacheItemPriority.Normal);
        }

        /// <summary>
        /// �Ѷ�����ص�Cache,���ӻ��������͹���ʱ��(����������)�����ȼ�
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="dep"></param>
        /// <param name="seconds"></param>
        /// <param name="priority"></param>
        public static void Insert(string key, object obj, CacheDependency dep, int seconds, CacheItemPriority priority)
        {
            if (obj != null)
            {
                _cache.Insert(key, obj, dep, DateTime.Now.AddSeconds(Factor * seconds), TimeSpan.Zero, priority, null);
            }

        }

        /// <summary>
        /// �Ѷ���ӵ����沢�������ȼ�
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="secondFactor"></param>
        public static void MicroInsert(string key, object obj, int secondFactor)
        {
            if (obj != null)
            {
                _cache.Insert(key, obj, null, DateTime.Now.AddSeconds(Factor * secondFactor), TimeSpan.Zero);
            }
        }

        /// <summary>
        /// �Ѷ���ӵ�����,���ѹ���ʱ����Ϊ���ֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public static void Max(string key, object obj)
        {
            Max(key, obj, null);
        }

        /// <summary>
        /// �Ѷ���ӵ�����,���ѹ���ʱ����Ϊ���ֵ,���ӻ���������Ϣ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="dep"></param>
        public static void Max(string key, object obj, CacheDependency dep)
        {
            if (obj != null)
            {
                _cache.Insert(key, obj, dep, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.AboveNormal, null);
            }
        }

        /// <summary>
        /// ����־��Ի���
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public static void Permanent(string key, object obj)
        {
            Permanent(key, obj, null);
        }

        /// <summary>
        /// ����־��Ի���,���ӻ�������
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="dep"></param>
        public static void Permanent(string key, object obj, CacheDependency dep)
        {
            if (obj != null)
            {
                _cache.Insert(key, obj, dep, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.NotRemovable, null);
            }
        }

        /// <summary>
        /// ���ݼ���ȡ������Ķ���
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            return _cache[key];
        }

        /// <summary>
        /// Return int of seconds * SecondFactor
        /// </summary>
        public static int SecondFactorCalculate(int seconds)
        {
            // Insert method below takes integer seconds, so we have to round any fractional values
            return Convert.ToInt32(Math.Round((double)seconds * SecondFactor));
        }
    }
}
