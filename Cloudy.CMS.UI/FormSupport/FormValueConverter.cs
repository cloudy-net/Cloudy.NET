using Cloudy.CMS.ContentTypeSupport;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FormSupport
{
    public class FormValueConverter : IFormValueConverter
    {
        public object Convert(string value, PropertyDefinitionDescriptor propertyDefinition)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null; // empty strings are converted to null
            }

            if(propertyDefinition.Type == typeof(string))
            {
                return value;
            }

            if(propertyDefinition.Type.IsGenericType && propertyDefinition.Type.GetGenericTypeDefinition() == typeof(Tuple<>))
            {
                var json = JsonDocument.Parse(value).RootElement;

                if(json.ValueKind != JsonValueKind.Array)
                {
                    return null;
                }

                var parameters = propertyDefinition.Type.GetGenericArguments();

                if (parameters.Length != json.GetArrayLength())
                {
                    return null;
                }

                var result = new List<object>();

                for (var i = 0; i < parameters.Length; i++)
                {
                    var parameter = parameters[i];
                    var element = json[i];

                    result.Add(JsonSerializer.Deserialize(element, parameter));
                }

                return Activator.CreateInstance(propertyDefinition.Type, result.ToArray());
            }

            return null;
        }
    }
}
