using System;

namespace MySoft.Data.Design
{
    /// <summary>
    /// ��ʾֻ����һ��������ͼ
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public sealed class ReadOnlyAttribute : Attribute
    { }
}
