using Cloudy.CMS.ComponentSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentTypeSupport
{
    public class CoreInterfaceCreator : ICoreInterfaceCreator
    {
        IPropertyDefinitionCreator PropertyDefinitionCreator { get; }
        IComponentProvider ComponentProvider { get; }

        public CoreInterfaceCreator(IComponentProvider componentProvider, IPropertyDefinitionCreator propertyDefinitionCreator)
        {
            ComponentProvider = componentProvider;
            PropertyDefinitionCreator = propertyDefinitionCreator;
        }

        public IEnumerable<CoreInterfaceDescriptor> Create()
        {
            var result = new List<CoreInterfaceDescriptor>();

            foreach (var type in ComponentProvider.GetAll().SelectMany(c => c.Assembly.Types))
            {
                if (!type.IsInterface)
                {
                    continue;
                }

                var coreInterfaceAttribute = type.GetCustomAttribute<CoreInterfaceAttribute>();

                if (coreInterfaceAttribute == null)
                {
                    continue;
                }

                var propertyDefinitions = new List<PropertyDefinitionDescriptor>();

                foreach (var property in type.GetProperties())
                {
                    propertyDefinitions.Add(PropertyDefinitionCreator.Create(property));
                }

                result.Add(new CoreInterfaceDescriptor(coreInterfaceAttribute.Id, type, propertyDefinitions));
            }

            return result.AsReadOnly();
        }
    }
}
