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
        public Document Serialize(IContent content, ContentTypeDescriptor contentType)
        {
            var globalInterfaces = new List<DocumentInterface>();

            foreach (var coreInterface in contentType.CoreInterfaces)
            {
                globalInterfaces.Add(new DocumentInterface(coreInterface.Id, GetProperties(content, coreInterface.PropertyDefinitions)));
            }

            var global = new DocumentFacet(DocumentLanguageConstants.Global, globalInterfaces, GetProperties(content, contentType.PropertyDefinitions));

            return new Document(content.Id, global, Enumerable.Empty<DocumentFacet>().ToDictionary(f => f.Language, f => f));
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
