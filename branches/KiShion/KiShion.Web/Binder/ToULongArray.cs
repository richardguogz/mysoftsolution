namespace KiShion.Web.Converter
{
    using System;

    public class ToULongArray : ToArray
    {
        private static Type mValueType = typeof(ulong);

        protected override Type ValueType
        {
            get
            {
                return mValueType;
            }
        }
    }
}

