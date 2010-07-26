using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Data.Design
{
    /// <summary>
    /// 表示只读，一般用于视图
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public sealed class ReadOnlyAttribute : Attribute
    { }
}
