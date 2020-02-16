using Cloudy.CMS.ContentTypeSupport.PropertyMappingSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentTypeSupport
{
    public class PropertyDefinitionProvider : IPropertyDefinitionProvider
    {
        IDictionary<string, IEnumerable<PropertyDefinitionDescriptor>> Values { get; } = new Dictionary<string, IEnumerable<PropertyDefinitionDescriptor>>();

        public PropertyDefinitionProvider(IContentTypeProvider contentTypeProvider, IPropertyMappingProvider propertyMappingProvider, IPropertyDefinitionCreator propertyDefinitionCreator)
        {
            foreach (var contentType in contentTypeProvider.GetAll())
            {
                var propertyDefinitions = new List<PropertyDefinitionDescriptor>();

                foreach (var property in contentType.Type.GetProperties())
                {
                    var mapping = propertyMappingProvider.Get(property);

                    if (mapping.PropertyMappingType == PropertyMappingType.Ignored)
                    {
                        continue;
                    }

                    if (mapping.PropertyMappingType == PropertyMappingType.CoreInterface)
                    {
                        continue;
                    }

                    if (mapping.PropertyMappingType == PropertyMappingType.Incomplete)
                    {
                        continue;
                    }

                    propertyDefinitions.Add(propertyDefinitionCreator.Create(property));
                }

                Values[contentType.Id] = propertyDefinitions.AsReadOnly();
            }
        }

        public IEnumerable<PropertyDefinitionDescriptor> GetFor(string contentTypeId)
        {
            return Values[contentTypeId];
        }
    }
}
