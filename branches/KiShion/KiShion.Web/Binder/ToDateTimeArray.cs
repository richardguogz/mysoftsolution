namespace KiShion.Web.Converter
{
    using System;

    public class ToDateTimeArray : ToArray
    {
        private static Type mValueType = typeof(DateTime);

        protected override Type ValueType
        {
            get
            {
                return mValueType;
            }
        }
    }
}

