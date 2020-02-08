using Poetry.ComponentSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport.PropertyMappingSupport;
using Cloudy.CMS.Core;
using Cloudy.CMS.Core.ContentSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentTypeSupport
{
    public class ContentTypeCreator : IContentTypeCreator
    {
        IPropertyDefinitionCreator PropertyDefinitionCreator { get; }
        ICoreInterfaceProvider CoreInterfaceProvider { get; }
        IPropertyMappingProvider PropertyMappingRepository { get; }
        IComponentProvider ComponentProvider { get; }

        public ContentTypeCreator(IPropertyDefinitionCreator propertyDefinitionCreator, ICoreInterfaceProvider coreInterfaceProvider, IPropertyMappingProvider propertyMappingRepository, IComponentProvider componentProvider)
        {
            PropertyDefinitionCreator = propertyDefinitionCreator;
            CoreInterfaceProvider = coreInterfaceProvider;
            PropertyMappingRepository = propertyMappingRepository;
            ComponentProvider = componentProvider;
        }

        public IEnumerable<ContentTypeDescriptor> Create()
        {
            var types = ComponentProvider
                    .GetAll()
                    .SelectMany(a => a.Assembly.Types)
                    .Where(a => typeof(IContent).IsAssignableFrom(a));

            var result = new List<ContentTypeDescriptor>();

            foreach (var type in types)
            {
                var contentTypeAttribute = type.GetTypeInfo().GetCustomAttribute<ContentTypeAttribute>();

                if (contentTypeAttribute == null)
                {
                    continue;
                }

                var container = type.GetTypeInfo().GetCustomAttribute<ContainerAttribute>()?.Id ?? ContainerConstants.Content;

                var propertyDefinitions = new List<PropertyDefinitionDescriptor>();

                foreach (var property in type.GetProperties())
                {
                    var mapping = PropertyMappingRepository.Get(property);

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

                    propertyDefinitions.Add(PropertyDefinitionCreator.Create(property));
                }

                var coreInterfaces = type.GetInterfaces()
                    .Select(i => CoreInterfaceProvider.GetFor(i))
                    .Where(i => i != null);

                result.Add(new ContentTypeDescriptor(contentTypeAttribute.Id, type, container, propertyDefinitions, coreInterfaces));
            }

            return result;
        }
    }
}
