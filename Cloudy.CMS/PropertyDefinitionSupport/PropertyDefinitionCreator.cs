using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cloudy.CMS.PropertyDefinitionSupport
{
    public class PropertyDefinitionCreator : IPropertyDefinitionCreator
    {
        public PropertyDefinitionDescriptor Create(PropertyInfo property)
        {
            var type = property.PropertyType;
            var nullable = false;

            var nullableType = Nullable.GetUnderlyingType(type);

            if (nullableType != null)
            {
                type = nullableType;
                nullable = true;
            }

            var list = false;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>))
            {
                type = type.GetGenericArguments().Single();
                list = true;
            }

            var block = type != typeof(string) && (type.IsClass || type.IsInterface);

            return new PropertyDefinitionDescriptor(
                property.Name,
                type,
                property.GetValue,
                property.SetValue,
                property.GetCustomAttributes(),
                nullable,
                list,
                type.IsEnum,
                block
            );
        }
    }
}
