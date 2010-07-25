namespace KiShion.Web.Converter
{
    using System;
    using System.Runtime.InteropServices;

    public interface IStringConverter
    {
        object ConvertTo(string value, out bool succeeded);
    }
}

