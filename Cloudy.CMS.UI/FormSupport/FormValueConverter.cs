using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.UI.FormSupport.FieldSupport;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
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

            if (propertyDefinition.Type == typeof(string))
            {
                return value;
            }

            if (propertyDefinition.Type == typeof(Guid))
            {
                return Guid.Parse(value);
            }

            if (propertyDefinition.Type == typeof(int))
            {
                return int.Parse(value);
            }

            if (propertyDefinition.Type == typeof(double))
            {
                return double.Parse(value);
            }

            if (propertyDefinition.Type == typeof(DateTime))
            {
                return DateTime.Parse(value);
            }

            if (propertyDefinition.Type == typeof(DateTimeOffset))
            {
                return DateTimeOffset.Parse(value);
            }

            if (propertyDefinition.Type == typeof(TimeSpan))
            {
                return TimeSpan.Parse(value);
            }

            if (propertyDefinition.Type == typeof(DateOnly))
            {
                return DateOnly.Parse(value);
            }

            if (propertyDefinition.Type == typeof(TimeOnly))
            {
                return TimeOnly.Parse(value);
            }

            if (propertyDefinition.Enum)
            {
                return Enum.Parse(propertyDefinition.Type, value);
            }

            if (propertyDefinition.Type.IsAssignableTo(typeof(ITuple)))
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
