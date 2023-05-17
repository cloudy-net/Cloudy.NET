using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Cloudy.NET.EntitySupport.Serialization
{
    public class JsonValueConverter<T> : ValueConverter<T, string> where T : class
    {
        static Lazy<JsonSerializerOptions> JsonSerializerOptions { get; } = new Lazy<JsonSerializerOptions>(() =>
        {
            var options = new JsonSerializerOptions();
            EmbeddedBlockJsonConverterProvider.UglyInstance.GetAll().ToList().ForEach(c => options.Converters.Add(c));
            return options;
        });

        public JsonValueConverter() : base(
            v => JsonSerializer.Serialize(v, JsonSerializerOptions.Value),
            v => (T)JsonSerializer.Deserialize(v, typeof(T), JsonSerializerOptions.Value)
        )
        { }
    }
}
