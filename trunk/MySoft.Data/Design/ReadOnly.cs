using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Data.Design
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public sealed class ReadOnlyAttribute : Attribute
    { }
}
