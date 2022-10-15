using Cloudy.CMS.ContentSupport.RepositorySupport.PrimaryKey;
using Microsoft.AspNetCore.Routing.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cloudy.CMS.EntitySupport.Reference
{
    public record ReferenceDeserializer(IPrimaryKeyPropertyGetter PrimaryKeyPropertyGetter) : IReferenceDeserializer
    {
        public object[] Get(Type contentType, string reference)
        {
            var primaryKeyProperties = PrimaryKeyPropertyGetter.GetFor(contentType).ToList();

            var result = new List<object>();

            var json = JsonDocument.Parse(reference).RootElement;

            if(json.ValueKind != JsonValueKind.Array)
            {
                throw new Exception($"Entity reference {reference} for type {contentType} could not be deserialized into JSON array");
            }

            if(primaryKeyProperties.Count != json.GetArrayLength())
            {
                throw new Exception($"Entity reference {reference} array length {json.GetArrayLength()} did not match primary key count {primaryKeyProperties.Count} on type {contentType}");
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
