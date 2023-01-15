using Cloudy.CMS.PropertyDefinitionSupport;
using System.Linq;

namespace Cloudy.CMS.UI.Extensions
{
    public static class PropertyDefinitionDescriptorExtensions
    {
        public static T GetAttribute<T>(this PropertyDefinitionDescriptor propertyDefinitionDescriptor)
            where T : class
        {
            return propertyDefinitionDescriptor?
                .Attributes
                .Where(x => typeof(T).IsAssignableFrom(x.GetType()))
                .OfType<T>()
                .FirstOrDefault();
        }

        public static bool AnyAttribute<T>(this PropertyDefinitionDescriptor propertyDefinitionDescriptor)
        {
            return propertyDefinitionDescriptor?
                .Attributes
                .Any(x => typeof(T).IsAssignableFrom(x.GetType())) ?? false;
        }
    }
}
