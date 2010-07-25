namespace KiShion.Web.Converter
{
    using System;
    using System.Runtime.InteropServices;

    public class ToString : IStringConverter
    {
        object IStringConverter.ConvertTo(string value, out bool succeeded)
        {
            succeeded = true;
            return value;
        }
    }
}

