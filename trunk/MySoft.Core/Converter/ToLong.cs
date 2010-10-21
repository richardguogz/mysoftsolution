﻿namespace MySoft.Converter
{
    using System;
    using System.Runtime.InteropServices;

    public class ToLong : IStringConverter
    {
        object IStringConverter.ConvertTo(string value, out bool succeeded)
        {
            long num;
            succeeded = long.TryParse(value, out num);
            return num;
        }
    }
}

