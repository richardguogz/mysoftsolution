using System;
using System.Collections.Generic;
using System.Text;

namespace KiShion.Data.Design
{
    /// <summary>
    /// ��ʾֻ����һ��������ͼ
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public sealed class ReadOnlyAttribute : Attribute
    { }
}
