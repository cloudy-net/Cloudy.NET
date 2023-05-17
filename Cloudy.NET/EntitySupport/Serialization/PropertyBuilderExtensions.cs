using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Cloudy.NET.EntitySupport.Serialization
{
    public static class PropertyBuilderExtensions
    {
        static JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions { };
        public static void SerializeIntoJson<ReturnType>(this PropertyBuilder<ReturnType> propertyBuilder)
        {
            propertyBuilder.HasConversion(
                value => value != null ? JsonSerializer.Serialize(value, JsonSerializerOptions) : null,
                value => value != null ? JsonSerializer.Deserialize<ReturnType>(value, JsonSerializerOptions) : default
            );
        }

        public static void JsonBlockConversion<ReturnType>(this PropertyBuilder<ReturnType> propertyBuilder) where ReturnType : class
        {
            propertyBuilder.HasConversion(new JsonValueConverter<ReturnType>());
        }
    }
}
