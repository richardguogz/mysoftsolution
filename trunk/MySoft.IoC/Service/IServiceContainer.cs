using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Core;
using Castle.MicroKernel;
using MySoft.Logger;
using MySoft.Cache;
using System.Reflection;

namespace MySoft.IoC
{
    /// <summary>
    /// The service container interface.
    /// </summary>
    public interface IServiceContainer : IDisposable, ILogable, IErrorLogable
    {
        /// <summary>
        /// Gets the kernel.
        /// </summary>
        /// <value>The kernel.</value>
        IKernel Kernel { get; }
        /// <summary>
        /// Registers the component.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="classType">Type of the class.</param>
        void RegisterComponent(string key, Type classType);
        /// <summary>
        /// Registers the component.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="classType">Type of the class.</param>
        void RegisterComponent(string key, Type serviceType, Type classType);
        /// <summary>
        /// Registers the components.
        /// </summary>
        /// <param name="serviceKeyTypes">The service key types.</param>
        void RegisterComponents(IDictionary serviceKeyTypes);
        /// <summary>
        /// Releases the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        void Release(object obj);
        /// <summary>
        /// Gets the <see cref="System.Object"/> with the specified key.
        /// </summary>
        /// <value></value>
        object this[string Key] { get; }
        /// <summary>
        /// Gets the <see cref="System.Object"/> with the specified service type.
        /// </summary>
        /// <value></value>
        object this[Type serviceType] { get; }
        /// <summary>
        /// Calls the service.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns>The response msg.</returns>
        ResponseMessage CallService(RequestMessage msg);
        /// <summary>
        /// Calls the service.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns>The response msg.</returns>
        ResponseMessage CallService(Type serviceType, MethodInfo method, RequestMessage msg);
        /// <summary>
        /// Gets the service nodes.
        /// </summary>
        /// <returns>The service nodes.</returns>
        ServiceNodeInfo[] GetServiceNodes();
        /// <summary>
        /// Gets the depender service nodes.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The service nodes.</returns>
        ServiceNodeInfo[] GetDependerServiceNodes(string key);
        /// <summary>
        /// Gets the dependent service nodes.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The service nodes.</returns>
        ServiceNodeInfo[] GetDependentServiceNodes(string key);
        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="log">The log info.</param>
        void WriteLog(string log, LogType type);
        /// <summary>
        /// Writes the exception.
        /// </summary>
        /// <param name="exception">The exception info.</param>
        void WriteError(Exception exception);
        /// <summary>
        /// ���÷������
        /// </summary>
        IServiceProxy Proxy { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        ICacheDependent Cache { get; set; }
    }
}
