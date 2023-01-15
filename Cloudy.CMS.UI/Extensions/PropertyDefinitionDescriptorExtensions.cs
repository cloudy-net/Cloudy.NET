using System;
using System.Collections.Generic;
using System.Linq;

namespace Cloudy.CMS.UI.Extensions
{
    public static class PropertyDefinitionDescriptorExtensions
    {
        public static T GetAttribute<T>(this IEnumerable<Attribute> attributes)
            where T : class
        {
            return attributes
                .Where(x => typeof(T).IsAssignableFrom(x.GetType()))
                .OfType<T>()
                .FirstOrDefault();
        }

        public static bool AnyAttribute<T>(this IEnumerable<Attribute> attributes)
        {
            return attributes.Any(x => typeof(T).IsAssignableFrom(x.GetType()));
        }
    }
}
