using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Web;
using System.Net;

namespace MySoft.RESTful
{
    /// <summary>
    /// 系列化工厂
    /// </summary>
    public sealed class SerializerFactory
    {
        public static ISerializer Create(ParameterFormat format)
        {
            switch (format)
            {
                case ParameterFormat.Xml:
                    return new XmlSerializer();
                case ParameterFormat.Json:
                default:
                    return new JsonSerializer();
            }
        }
    }
}

