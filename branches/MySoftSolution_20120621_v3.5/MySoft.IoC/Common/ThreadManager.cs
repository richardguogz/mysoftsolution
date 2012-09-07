﻿using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using MySoft.IoC.Messages;

namespace MySoft.IoC
{
    /// <summary>
    /// 线程管理
    /// </summary>
    internal static class ThreadManager
    {
        //实例化队列
        private static Hashtable hashtable = Hashtable.Synchronized(new Hashtable());

        static ThreadManager()
        {
            //刷新30分钟未更新的数据
            ThreadPool.QueueUserWorkItem(state =>
            {
                while (true)
                {
                    //1分钟检测一次
                    Thread.Sleep(TimeSpan.FromMinutes(1));

                    try
                    {
                        var keys = hashtable.Keys.Cast<string>().ToList();

                        foreach (var key in keys)
                        {
                            if (hashtable.ContainsKey(key))
                            {
                                var item = hashtable[key] as WorkerItem;

                                var timeSpan = TimeSpan.FromSeconds(ServiceConfig.DEFAULT_CACHE_TIMEOUT);

                                //如果超过指定时间未刷新数据，则自动刷新
                                if (DateTime.Now.Subtract(item.UpdateTime) > timeSpan)
                                {
                                    RemoveWorker(key);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //TODO
                    }
                }
            });
        }

        /// <summary>
        /// 移除工作项
        /// </summary>
        /// <param name="key"></param>
        private static void RemoveWorker(string key)
        {
            if (hashtable.ContainsKey(key))
            {
                hashtable.Remove(key);
            }
        }

        /// <summary>
        /// 添加工作项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        public static void AddWorker(string key, WorkerItem item)
        {
            if (!hashtable.ContainsKey(key))
            {
                //将当前线程放入队列中
                hashtable[key] = item;

                Console.WriteLine("[{0}] => Worker item count: {1}.", DateTime.Now, hashtable.Count);
            }
        }

        /// <summary>
        /// 刷新工作项
        /// </summary>
        /// <param name="key"></param>
        public static void RefreshWorker(string key)
        {
            if (hashtable.ContainsKey(key))
            {
                var item = hashtable[key] as WorkerItem;

                lock (item)
                {
                    if (item.IsRunning) return;
                    item.IsRunning = true;

                    ThreadPool.QueueUserWorkItem(RefreshData, new ArrayList { key, item });
                }
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="state"></param>
        private static void RefreshData(object state)
        {
            var arr = state as ArrayList;
            var callKey = arr[0] as string;
            var worker = arr[1] as WorkerItem;

            var watch = Stopwatch.StartNew();

            try
            {
                var resMsg = GetResponse(worker.Service, worker.Context, worker.Request);

                watch.Stop();

                //不为null而且未出错
                if (resMsg != null && !resMsg.IsError)
                {
                    resMsg.ElapsedTime = watch.ElapsedMilliseconds;

                    var elapsedTime = worker.ElapsedTime;
                    var count = worker.TotalCount;

                    //记录数大于指定的记录数，而且时间大于指定的时间，则重新缓存
                    if (resMsg.Count > count && resMsg.ElapsedTime > elapsedTime.TotalMilliseconds)
                    {
                        CacheHelper.Insert(callKey, resMsg, ServiceConfig.DEFAULT_CACHE_TIMEOUT);
                    }
                    else
                    {
                        CacheHelper.Remove(callKey);

                        //如果耗时小于设置的值，则移除
                        RemoveWorker(callKey);
                    }
                }
            }
            catch (Exception ex)
            {
                watch.Stop();
                //no exception
            }
            finally
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));

                worker.UpdateTime = DateTime.Now;
                worker.IsRunning = false;
            }
        }

        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="service"></param>
        /// <param name="context"></param>
        /// <param name="reqMsg"></param>
        /// <returns></returns>
        public static ResponseMessage GetResponse(IService service, OperationContext context, RequestMessage reqMsg)
        {
            //定义响应的消息
            ResponseMessage resMsg = null;

            try
            {
                OperationContext.Current = context;

                //响应结果，清理资源
                resMsg = service.CallService(reqMsg);
            }
            catch (Exception ex)
            {
                //处理异常
                resMsg = IoCHelper.GetResponse(reqMsg, ex);
            }
            finally
            {
                OperationContext.Current = null;
            }

            return resMsg;
        }
    }

    /// <summary>
    /// Worker item
    /// </summary>
    internal class WorkerItem
    {
        /// <summary>
        /// Service
        /// </summary>
        public IService Service { get; set; }

        /// <summary>
        /// Request
        /// </summary>
        public RequestMessage Request { get; set; }

        /// <summary>
        /// Context
        /// </summary>
        public OperationContext Context { get; set; }

        /// <summary>
        /// 是否在运行
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 耗时时间
        /// </summary>
        public TimeSpan ElapsedTime { get; set; }

        /// <summary>
        /// 记录数
        /// </summary>
        public int TotalCount { get; set; }

        public WorkerItem()
        {
            this.ElapsedTime = TimeSpan.Zero;
            this.UpdateTime = DateTime.Now;
        }
    }
}
