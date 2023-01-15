using Cloudy.CMS.PropertyDefinitionSupport;
using Cloudy.CMS.UI.FieldSupport.Select;
using System;
using System.Linq;

namespace Cloudy.CMS.UI.Extensions
{
    public static class PropertyDefinitionDescriptorExtensions
    {
        public static Type GetSelectAttributeType(this PropertyDefinitionDescriptor propertyDefinitionDescriptor)
        {
            return propertyDefinitionDescriptor?
                .Attributes
                .Select(x => x.GetType())
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(SelectAttribute<>))
                .FirstOrDefault()?.GetGenericArguments().FirstOrDefault()?.UnderlyingSystemType;
        }

        public static bool AnySelectAttribute(this PropertyDefinitionDescriptor propertyDefinitionDescriptor)
        {
            return propertyDefinitionDescriptor?
                .Attributes
                .Select(x => x.GetType())
                .Any(
                    t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(SelectAttribute<>)
            ) ?? false;

        }
    }
}
