using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentTypeSupport
{
    public class PropertyDefinitionCreator : IPropertyDefinitionCreator
    {
        public PropertyDefinitionDescriptor Create(PropertyInfo property)
        {
            var type = property.PropertyType;
            var nullable = false;

            var nullableType = Nullable.GetUnderlyingType(type);

            if(nullableType != null)
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

            return new PropertyDefinitionDescriptor(
                property.Name,
                type,
                (instance) => property.GetValue(instance),
                (instance, value) => property.SetValue(instance, value),
                property.GetCustomAttributes(),
                nullable,
                list,
                type.IsEnum
            );
        }
    }
}
