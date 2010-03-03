using System;
using System.Collections.Generic;
using System.Text;

namespace MySoft.Tools.EntityDesign
{
    public class EntityDesignConfiguration
    {
        public bool EnabledPropertyValueChange = true;
        public string CompileMode = "Debug";
        public string InputDllName = string.Empty;
        public string OutputNamespace = string.Empty;
        public string OutputLanguage = "C#";
        public string OutputCodeFileEncoding = string.Empty;
        public string EntityCodePath = string.Empty;
    }
}
