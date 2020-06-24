using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Collections;
using Microsoft.Extensions.Logging;
using Cloudy.CMS.ContentSupport.RuntimeSupport;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public class ContentDeserializer : IContentDeserializer
    {
        ILogger Logger { get; }
        IPropertyDefinitionProvider PropertyDefinitionProvider { get; }
        IContentTypeCoreInterfaceProvider ContentTypeCoreInterfaceProvider { get; }
        IPolymorphicDeserializer PolymorphicDeserializer { get; }
        IContentInstanceCreator ContentInstanceCreator { get; }

        public ContentDeserializer(ILogger<ContentDeserializer> logger, IPropertyDefinitionProvider propertyDefinitionProvider, IContentTypeCoreInterfaceProvider contentTypeCoreInterfaceProvider, IPolymorphicDeserializer polymorphicDeserializer, IContentInstanceCreator contentInstanceCreator)
        {
            Logger = logger;
            PropertyDefinitionProvider = propertyDefinitionProvider;
            ContentTypeCoreInterfaceProvider = contentTypeCoreInterfaceProvider;
            PolymorphicDeserializer = polymorphicDeserializer;
            ContentInstanceCreator = contentInstanceCreator;
        }

        public IContent Deserialize(Document document, ContentTypeDescriptor contentType, string language)
        {
            var content = ContentInstanceCreator.Create(contentType);

            foreach (var coreInterface in ContentTypeCoreInterfaceProvider.GetFor(contentType.Id))
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
                if (!values.TryGetValue(definition.Name, out var value))
                {
                    continue;
                }

                if (value is long && definition.Type == typeof(int))
                {
                    value = (int)(long)value;
                }
                if (value is JArray)
                {
                    if (definition.Type.IsGenericType && (definition.Type.GetGenericTypeDefinition() == typeof(IEnumerable<>) || definition.Type.GetGenericTypeDefinition() == typeof(List<>) || definition.Type.GetGenericTypeDefinition() == typeof(IList<>)) && definition.Type.GetGenericArguments().Single().IsInterface)
                    {
                        var type = definition.Type.GetGenericArguments().Single();
                        var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));

                        foreach (var element in (JArray)value)
                        {
                            if (element.Type != JTokenType.Object)
                            {
                                Logger.LogInformation($"Skipping array element `{element.ToString(Newtonsoft.Json.Formatting.None)}` on ({definition.Type}) {definition.Name} because it was not deserializable into a {type}");
                                continue;
                            }

                            var result = PolymorphicDeserializer.Deserialize((JObject)element, type);

                            if(result == null)
                            {
                                Logger.LogInformation($"Skipping array element `{element.ToString(Newtonsoft.Json.Formatting.None)}` on ({definition.Type}) {definition.Name} because it was not polymorphically deserializable");
                                continue;
                            }

                            list.Add(result);
                        }

                        value = list;
                    }
                    else
                    {
                        value = ((JArray)value).ToObject(definition.Type);
                    }
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
