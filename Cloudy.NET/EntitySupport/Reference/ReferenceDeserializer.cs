using Cloudy.NET.EntitySupport.PrimaryKey;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cloudy.NET.EntitySupport.Reference
{
    public record ReferenceDeserializer(IPrimaryKeyPropertyGetter PrimaryKeyPropertyGetter, ILogger<ReferenceDeserializer> Logger) : IReferenceDeserializer
    {
        public object[] Get(Type entityType, string reference, bool simpleKey)
        {
            var primaryKeyProperties = PrimaryKeyPropertyGetter.GetFor(entityType).ToList();

            if (simpleKey)
            {
                var primaryKeyProperty = primaryKeyProperties.First();

                if (primaryKeyProperty.PropertyType == typeof(string))
                {
                    return new object[] { reference };
                }
                if (primaryKeyProperty.PropertyType == typeof(Guid) || primaryKeyProperty.PropertyType == typeof(Guid?))
                {
                    return new object[] { Guid.Parse(reference) };
                }
                if (primaryKeyProperty.PropertyType == typeof(int) || primaryKeyProperty.PropertyType == typeof(int?))
                {
                    return new object[] { int.Parse(reference) };
                }
            }

            var result = new List<object>();

            JsonElement json;

            try
            {
                json = JsonDocument.Parse(reference).RootElement;
            }
            catch (JsonException exception)
            {
                Logger.LogError(exception, "Could not parse reference {Reference} as JSON", reference);
                return null;
            }

            if(json.ValueKind != JsonValueKind.Array)
            {
                throw new Exception($"Entity reference {reference} for type {entityType} could not be deserialized into JSON array");
            }

            if(primaryKeyProperties.Count != json.GetArrayLength())
            {
                throw new Exception($"Entity reference {reference} array length {json.GetArrayLength()} did not match primary key count {primaryKeyProperties.Count} on type {entityType}");
            }

            for (var i = 0; i < primaryKeyProperties.Count; i++)
            {
                var property = primaryKeyProperties[i];
                var element = json[i];

                result.Add(JsonSerializer.Deserialize(element, property.PropertyType));
            }

            return result.ToArray();
        }
    }
}
