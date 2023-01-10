using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.PropertyDefinitionSupport
{
    public class PropertyDefinitionProvider : IPropertyDefinitionProvider
    {
        IDictionary<string, IEnumerable<PropertyDefinitionDescriptor>> Values { get; } = new Dictionary<string, IEnumerable<PropertyDefinitionDescriptor>>();

        public PropertyDefinitionProvider(IContentTypeProvider contentTypeProvider, IPropertyDefinitionCreator propertyDefinitionCreator)
        {
            foreach (var contentType in contentTypeProvider.GetAll())
            {
                var propertyDefinitions = new List<PropertyDefinitionDescriptor>();

                foreach (var property in contentType.Type.GetProperties())
                {
                    if (property.GetGetMethod() == null || property.GetSetMethod() == null)
                    {
                        continue;
                    }

                    propertyDefinitions.Add(propertyDefinitionCreator.Create(property));
                }

                Values[contentType.Name] = propertyDefinitions.AsReadOnly();
            }
        }

        public IEnumerable<PropertyDefinitionDescriptor> GetFor(string contentType)
        {
            return Values[contentType];
        }
    }
}
