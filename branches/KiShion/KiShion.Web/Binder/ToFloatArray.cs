namespace KiShion.Web.Converter
{
    using System;

    public class ToFloatArray : ToArray
    {
        private static Type mValueType = typeof(float);

        protected override Type ValueType
        {
            get
            {
                return mValueType;
            }
        }
    }
}

