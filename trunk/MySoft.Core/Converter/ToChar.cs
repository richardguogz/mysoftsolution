namespace MySoft.Converter
{
    using System;
    using System.Runtime.InteropServices;

    public class ToChar : IStringConverter
    {
        public object ConvertTo(string value, out bool succeeded)
        {
            char ch;
            succeeded = char.TryParse(value, out ch);
            return ch;
        }
    }
}

