﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Newtonsoft.Json.Converters
{
    public class DataTableConverter : JsonConverter
    {
        /// <summary>   
        /// Writes the JSON representation of the object.   
        /// </summary>   
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>   
        /// <param name="value">The value.</param>   
        public override void WriteJson(JsonWriter writer, object dataTable)
        {
            DataTable table = dataTable as DataTable;
            DataRowConverter converter = new DataRowConverter();
            writer.WriteStartObject();
            writer.WritePropertyName("Rows");
            writer.WriteStartArray();
            foreach (DataRow row in table.Rows)
            {
                converter.WriteJson(writer, row);
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
            return typeof(DataTable).IsAssignableFrom(valueType);
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
