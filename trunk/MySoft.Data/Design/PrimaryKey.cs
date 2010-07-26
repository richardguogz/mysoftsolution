using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Data.Design
{
    /// <summary>
    /// ±íÊ¾Ö÷¼ü
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class PrimaryKeyAttribute : Attribute
    { }
}
