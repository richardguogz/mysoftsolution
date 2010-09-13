using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Core.Remoting
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
    public class ServiceContractAttribute : Attribute
    {
    }
}
