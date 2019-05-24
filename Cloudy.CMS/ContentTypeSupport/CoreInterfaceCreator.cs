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

        public CoreInterfaceCreator(IPropertyDefinitionCreator propertyDefinitionCreator)
        {
            PropertyDefinitionCreator = propertyDefinitionCreator;
        }

        public CoreInterfaceDescriptor Create(Type type)
        {
            var coreInterfaceAttribute = type.GetCustomAttribute<CoreInterfaceAttribute>();

            var propertyDefinitions = new List<PropertyDefinitionDescriptor>();

            foreach (var property in type.GetProperties())
            {
                propertyDefinitions.Add(PropertyDefinitionCreator.Create(property));
            }

            return new CoreInterfaceDescriptor(coreInterfaceAttribute.Id, propertyDefinitions);
        }
    }
}
