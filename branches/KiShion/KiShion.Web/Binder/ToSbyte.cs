﻿namespace KiShion.Web.Converter
{
    using System;
    using System.Runtime.InteropServices;

    public class ToSbyte : IStringConverter
    {
        object IStringConverter.ConvertTo(string value, out bool succeeded)
        {
            sbyte num;
            succeeded = sbyte.TryParse(value, out num);
            return num;
        }
    }
}

