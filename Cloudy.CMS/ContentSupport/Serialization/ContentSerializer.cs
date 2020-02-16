using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContentTypeSupport;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public class ContentSerializer : IContentSerializer
    {
        IPropertyDefinitionProvider PropertyDefinitionProvider { get; }
        IContentTypeCoreInterfaceProvider ContentTypeCoreInterfaceProvider { get; }

        public ContentSerializer(IPropertyDefinitionProvider propertyDefinitionProvider, IContentTypeCoreInterfaceProvider contentTypeCoreInterfaceProvider)
        {
            PropertyDefinitionProvider = propertyDefinitionProvider;
            ContentTypeCoreInterfaceProvider = contentTypeCoreInterfaceProvider;
        }

        public Document Serialize(IContent content, ContentTypeDescriptor contentType)
        {
            var globalInterfaces = new List<DocumentInterface>();

            foreach (var coreInterface in ContentTypeCoreInterfaceProvider.GetFor(contentType.Id))
            {
                globalInterfaces.Add(DocumentInterface.CreateFrom(coreInterface.Id, GetProperties(content, coreInterface.PropertyDefinitions)));
            }

            var global = DocumentFacet.CreateFrom(DocumentLanguageConstants.Global, globalInterfaces, GetProperties(content, PropertyDefinitionProvider.GetFor(contentType.Id)));

            return Document.CreateFrom(content.Id, global, Enumerable.Empty<DocumentFacet>().ToDictionary(f => f.Language, f => f));
        }

        IDictionary<string, object> GetProperties(IContent content, IEnumerable<PropertyDefinitionDescriptor> definitions)
        {
            var values = new Dictionary<string, object>();

            foreach(var definition in definitions)
            {
                values[definition.Name] = definition.Getter(content);
            }

            return values;
        }
    }
}
