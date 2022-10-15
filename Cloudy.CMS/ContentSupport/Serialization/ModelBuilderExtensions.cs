using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public static class ModelBuilderExtensions
    {
        static JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions { };
        public static void JsonConversion<ReturnType>(this PropertyBuilder<ReturnType> propertyBuilder)
        {
            propertyBuilder.HasConversion(
                value => value != null ? JsonSerializer.Serialize(value, JsonSerializerOptions) : null,
                value => value != null ? JsonSerializer.Deserialize<ReturnType>(value, JsonSerializerOptions) : default(ReturnType)
            );
        }
    }
}
