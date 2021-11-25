using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public class JsonValueConverter<T> : ValueConverter<T, string> where T: class
    {
        static JsonSerializerOptions JsonSerializerOptions { get; } = new JsonSerializerOptions
        {
            //Converters = { ContentJsonConverter.UglyInstance }
        };

        public JsonValueConverter() : base(
            v => JsonSerializer.Serialize(v, JsonSerializerOptions),
            v => (T)JsonSerializer.Deserialize(v, typeof(T), JsonSerializerOptions)
        ) { }
    }
}
