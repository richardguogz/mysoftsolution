﻿using System;
using System.Collections.Generic;
using MySoft.IoC.Messages;

namespace MySoft.IoC
{
    /// <summary>
    /// 状态服务信息
    /// </summary>
    [ServiceContract(CallbackType = typeof(IStatusListener))]
    public interface IStatusService
    {
        /// <summary>
        /// 获取所有应用客户端
        /// </summary>
        /// <returns></returns>
        [OperationContract(CacheTime = 5)]
        IList<AppClient> GetAppClients();

        /// <summary>
        /// 订阅服务
        /// </summary>
        void Subscribe(params string[] subscribeTypes);

        /// <summary>
        /// 订阅服务
        /// </summary>
        /// <param name="options">订阅选项</param>
        void Subscribe(SubscribeOptions options, params string[] subscribeTypes);

        /// <summary>
        /// 退订服务
        /// </summary>
        void Unsubscribe();

        /// <summary>
        /// 获取订阅的类型
        /// </summary>
        /// <returns></returns>
        [OperationContract(CacheTime = 5)]
        IList<string> GetSubscribeTypes();

        /// <summary>
        /// 订阅发布类型
        /// </summary>
        /// <param name="subscribeType"></param>
        void SubscribeType(string subscribeType);

        /// <summary>
        /// 退订发布类型
        /// </summary>
        /// <param name="subscribeType"></param>
        void UnsubscribeType(string subscribeType);

        /// <summary>
        /// 获取订阅的应用
        /// </summary>
        /// <returns></returns>
        [OperationContract(CacheTime = 5)]
        IList<string> GetSubscribeApps();

        /// <summary>
        /// 订阅发布应用
        /// </summary>
        /// <param name="appName"></param>
        void SubscribeApp(string appName);

        /// <summary>
        /// 退订发布应用
        /// </summary>
        /// <param name="appName"></param>
        void UnsubscribeApp(string appName);

        /// <summary>
        /// 是否存在服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        [OperationContract(CacheTime = 60)]
        bool ContainsService(string serviceName);

        /// <summary>
        /// 获取服务信息列表
        /// </summary>
        /// <returns></returns>
        [OperationContract(CacheTime = 60)]
        IList<ServiceInfo> GetServiceList();

        /// <summary>
        /// 获取服务状态信息（包括SummaryStatus，HighestStatus，TimeStatus）
        /// </summary>
        /// <returns></returns>
        [OperationContract(CacheTime = 5)]
        ServerStatus GetServerStatus();

        /// <summary>
        /// 清除服务器状态
        /// </summary>
        void ClearServerStatus();

        /// <summary>
        /// 获取时段的服务状态信息
        /// </summary>
        /// <returns></returns>
        [OperationContract(CacheTime = 5)]
        IList<TimeStatus> GetTimeStatusList();

        /// <summary>
        /// 获取所有的客户端信息
        /// </summary>
        /// <returns></returns>
        [OperationContract(CacheTime = 5)]
        IList<ClientInfo> GetClientList();

        /// <summary>
        /// 刷新接口
        /// </summary>
        /// <returns></returns>
        void RefreshWebAPI();

        /// <summary>
        /// 获取服务节点
        /// </summary>
        /// <param name="nodeKey"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        [OperationContract(CacheTime = 5)]
        IList<ServerNode> GetServerNodes(string nodeKey, string serviceName);
    }
}