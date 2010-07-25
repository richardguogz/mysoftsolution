namespace KiShion.Web.Converter
{
    using System;
    using System.Runtime.InteropServices;

    public class ToInt32 : IStringConverter
    {
        object IStringConverter.ConvertTo(string value, out bool succeeded)
        {
            int num;
            succeeded = int.TryParse(value, out num);
            return num;
        }
    }
}

