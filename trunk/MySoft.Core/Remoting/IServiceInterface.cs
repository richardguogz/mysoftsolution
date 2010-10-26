using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Remoting
{
    /// <summary>
    /// interface used to mark service interfaces.
    /// </summary>
    public interface IServiceInterface
    {
    }

    /// <summary>
    /// Attribute used to mark service interfaces.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
    public class ServiceContractAttribute : Attribute
    {
    }
}
