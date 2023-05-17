using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace System.Text.Json
{
    public static class JsonSerializerOptionExtensions
    {
        public static JsonSerializerOptions CloudyDefault(this JsonSerializerOptions options)
        {
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

            return options;
        }
    }
}
