namespace KiShion.Web.Converter
{
    using System;
    using System.Runtime.InteropServices;

    public class ToFloat : IStringConverter
    {
        object IStringConverter.ConvertTo(string value, out bool succeeded)
        {
            float num;
            succeeded = float.TryParse(value, out num);
            return num;
        }
    }
}

