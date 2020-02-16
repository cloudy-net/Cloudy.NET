using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public class ContentDeserializer : IContentDeserializer
    {
        IPropertyDefinitionProvider PropertyDefinitionProvider { get; }
        IContentTypeCoreInterfaceProvider ContentTypeCoreInterfaceProvider { get; }

        public ContentDeserializer(IPropertyDefinitionProvider propertyDefinitionProvider, IContentTypeCoreInterfaceProvider contentTypeCoreInterfaceProvider)
        {
            PropertyDefinitionProvider = propertyDefinitionProvider;
            ContentTypeCoreInterfaceProvider = contentTypeCoreInterfaceProvider;
        }

        public IContent Deserialize(Document document, ContentTypeDescriptor contentType, string language)
        {
            var content = (IContent)Activator.CreateInstance(contentType.Type);

            foreach(var coreInterface in ContentTypeCoreInterfaceProvider.GetFor(contentType.Id))
            {
                if (document.GlobalFacet.Interfaces.TryGetValue(coreInterface.Id, out var languageIndependentInterface))
                {
                    SetProperties(content, coreInterface.PropertyDefinitions, languageIndependentInterface.Properties);
                }
            }

            SetProperties(content, PropertyDefinitionProvider.GetFor(contentType.Id), document.GlobalFacet.Properties);

            return content;
        }

        void SetProperties(IContent content, IEnumerable<PropertyDefinitionDescriptor> definitions, IDictionary<string, object> values)
        {
            foreach (var definition in definitions)
            {
                if (values.TryGetValue(definition.Name, out var value))
                {
                    if (value is long && definition.Type == typeof(int))
                    {
                        value = (int)(long)value;
                    }
                    if (value is JArray)
                    {
                        value = ((JArray)value).ToObject(definition.Type);
                    }
                    if (value is JObject)
                    {
                        value = ((JObject)value).ToObject(definition.Type);
                    }

                    definition.Setter(content, value);
                }
            }
        }
    }
}
