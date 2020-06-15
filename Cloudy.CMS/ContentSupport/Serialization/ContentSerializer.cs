using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContentTypeSupport;
using System.Collections;
using Microsoft.Extensions.Logging;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public class ContentSerializer : IContentSerializer
    {
        ILogger Logger { get; }
        IPropertyDefinitionProvider PropertyDefinitionProvider { get; }
        IContentTypeCoreInterfaceProvider ContentTypeCoreInterfaceProvider { get; }
        IPolymorphicSerializer PolymorphicSerializer { get; }

        public ContentSerializer(ILogger<ContentSerializer> logger, IPropertyDefinitionProvider propertyDefinitionProvider, IContentTypeCoreInterfaceProvider contentTypeCoreInterfaceProvider, IPolymorphicSerializer polymorphicSerializer)
        {
            Logger = logger;
            PropertyDefinitionProvider = propertyDefinitionProvider;
            ContentTypeCoreInterfaceProvider = contentTypeCoreInterfaceProvider;
            PolymorphicSerializer = polymorphicSerializer;
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
                object value = definition.Getter(content);

                if (definition.Type.IsGenericType && (definition.Type.GetGenericTypeDefinition() == typeof(IEnumerable<>) || definition.Type.GetGenericTypeDefinition() == typeof(List<>) || definition.Type.GetGenericTypeDefinition() == typeof(IList<>)) && definition.Type.GetGenericArguments().Single().IsInterface)
                {
                    var type = definition.Type.GetGenericArguments().Single();
                    var list = new JArray();

                    for (var i = 0; i < ((IList)value).Count; i++)
                    {
                        var element = ((IList)value)[i];
                        var result = PolymorphicSerializer.Serialize(element, type);

                        if (result == null)
                        {
                            Logger.LogInformation($"Skipping element {i} on ({definition.Type}) {definition.Name} with id {content.Id} because it was not polymorphically serializable");
                            continue;
                        }

                        list.Add(result);
                    }

                    value = list;
                }

                values[definition.Name] = value;
            }

            return values;
        }
    }
}
