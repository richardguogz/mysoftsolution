﻿namespace MySoft.Core.Converter
{
    using System;
    using System.Runtime.InteropServices;

    public class ToStringArray : IStringConverter
    {
        object IStringConverter.ConvertTo(string value, out bool succeeded)
        {
            succeeded = (value != null) & (value != string.Empty);
            if (!succeeded)
            {
                return null;
            }
            return value.Split(new char[] { ',' });
        }
    }
}

