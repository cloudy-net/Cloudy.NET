using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public static class ModelBuilderExtensions
    {
        static JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions { };
        public static void JsonConversion<ReturnType>(this PropertyBuilder<ReturnType> propertyBuilder)
        {
            propertyBuilder.HasConversion(v => JsonSerializer.Serialize(v, JsonSerializerOptions), v => JsonSerializer.Deserialize<ReturnType>(v, JsonSerializerOptions));
        }
    }
}
