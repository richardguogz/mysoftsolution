using System;
using System.Data;
using System.Reflection;

namespace Newtonsoft.Json.Converters
{
    public class DataSetConverter : JsonConverter
    {
        /// <summary>   
        /// Writes the JSON representation of the object.   
        /// </summary>   
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>   
        /// <param name="value">The value.</param>   
        public override void WriteJson(JsonWriter writer, object dataset)
        {
            DataSet dataSet = dataset as DataSet;
            DataTableConverter converter = new DataTableConverter();
            writer.WriteStartObject();
            writer.WritePropertyName("Tables");
            writer.WriteStartArray();
            BindingFlags bf = BindingFlags.Public | BindingFlags.Static;
            foreach (DataTable table in dataSet.Tables)
            {
                converter.WriteJson(writer, table);
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
        /// <summary>   
        /// Determines whether this instance can convert the specified value type.   
        /// </summary>   
        /// <param name="valueType">Type of the value.</param>   
        /// <returns>   
        ///     <c>true</c> if this instance can convert the specified value type; otherwise, <c>false</c>.   
        /// </returns>   
        public override bool CanConvert(Type valueType)
        {
            return typeof(DataSet).IsAssignableFrom(valueType);
        }
        /// <summary>   
        /// Reads the JSON representation of the object.   
        /// </summary>   
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>   
        /// <param name="objectType">Type of the object.</param>   
        /// <returns>The object value.</returns>   
        public override object ReadJson(JsonReader reader, Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}
