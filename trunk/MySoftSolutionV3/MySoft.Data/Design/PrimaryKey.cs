using System;

namespace MySoft.Data.Design
{
    /// <summary>
    /// ��ʾ����
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class PrimaryKeyAttribute : Attribute
    { }
}
