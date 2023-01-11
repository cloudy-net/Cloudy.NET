using Cloudy.CMS.AssemblySupport;
using Cloudy.CMS.EntityTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.PropertyDefinitionSupport
{
    public class PropertyDefinitionProvider : IPropertyDefinitionProvider
    {
        IDictionary<string, IEnumerable<PropertyDefinitionDescriptor>> Values { get; } = new Dictionary<string, IEnumerable<PropertyDefinitionDescriptor>>();

        public PropertyDefinitionProvider(IEntityTypeProvider entityTypeProvider, IPropertyDefinitionCreator propertyDefinitionCreator, IAssemblyProvider assemblyProvider)
        {
            foreach (var entityType in entityTypeProvider.GetAll())
            {
                var propertyDefinitions = new List<PropertyDefinitionDescriptor>();

                foreach (var property in entityType.Type.GetProperties())
                {
                    if (property.GetGetMethod() == null || property.GetSetMethod() == null)
                    {
                        continue;
                    }

                    propertyDefinitions.Add(propertyDefinitionCreator.Create(property));
                }

                Values[entityType.Name] = propertyDefinitions.AsReadOnly();
            }
        }

        public IEnumerable<PropertyDefinitionDescriptor> GetFor(string entityType)
        {
            return Values[entityType];
        }
    }
}
