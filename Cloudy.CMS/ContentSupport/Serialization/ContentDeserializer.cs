using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public class ContentDeserializer : IContentDeserializer
    {
        public IContent Deserialize(Document document, ContentTypeDescriptor contentType, string language)
        {
            var content = (IContent)Activator.CreateInstance(contentType.Type);

            foreach(var coreInterface in contentType.CoreInterfaces)
            {
                if (document.GlobalFacet.Interfaces.TryGetValue(coreInterface.Id, out var languageIndependentInterface))
                {
                    SetProperties(content, coreInterface.PropertyDefinitions, languageIndependentInterface.Properties);
                }
            }

            SetProperties(content, contentType.PropertyDefinitions, document.GlobalFacet.Properties);

            return content;
        }

        void SetProperties(IContent content, IEnumerable<PropertyDefinitionDescriptor> definitions, IDictionary<string, object> values)
        {
            foreach (var definition in definitions)
            {
                if (values.TryGetValue(definition.Name, out var value))
                {
                    definition.Setter(content, value);
                }
            }
        }
    }
}
