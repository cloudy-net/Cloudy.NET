using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Cloudy.CMS.PropertyDefinitionSupport
{
    public class PropertyDefinitionCreator : IPropertyDefinitionCreator
    {
        public PropertyDefinitionDescriptor Create(PropertyInfo property)
        {
            var type = property.PropertyType;

            var attributes = new List<Attribute>(property.GetCustomAttributes(true).OfType<Attribute>());

            attributes.AddRange(GetInterfaceAttributes(property));

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

            var block = type != typeof(string) && (type.IsClass || type.IsInterface) && !type.IsAssignableTo(typeof(ITuple));

            return new PropertyDefinitionDescriptor(
                property.Name,
                type,
                property.GetValue,
                property.SetValue,
                attributes,
                nullable,
                list,
                type.IsEnum,
                block
            );
        }

        IEnumerable<Attribute> GetInterfaceAttributes(PropertyInfo property)
        {
            var result = new List<Attribute>();

            foreach (var @interface in property.DeclaringType.GetInterfaces())
            {
                var interfaceProperty = @interface.GetProperty(property.Name);

                if (interfaceProperty != null)
                {
                    foreach(var attribute in interfaceProperty.GetCustomAttributes(true).OfType<Attribute>())
                    {
                        result.Add(attribute);
                    }
                }
            }

            return result;
        }
    }
}
