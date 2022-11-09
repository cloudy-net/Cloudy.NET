using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Cloudy.CMS.PropertyDefinitionSupport.PropertyMappingSupport
{
    public class PropertyMappingProvider : IPropertyMappingProvider
    {
        IDictionary<string, PropertyMapping> Mappings { get; } = new Dictionary<string, PropertyMapping>();

        IPropertyMappingCreator PropertyMappingCreator { get; }

        public PropertyMappingProvider(IPropertyMappingCreator propertyMappingCreator)
        {
            PropertyMappingCreator = propertyMappingCreator;
        }

        public PropertyMapping Get(PropertyInfo property)
        {
            var key = $"{property.DeclaringType.FullName}.{property.Name}";

            if (!Mappings.ContainsKey(key))
            {
                Mappings[key] = PropertyMappingCreator.Create(property);
            }

            return Mappings[key];
        }
    }
}
