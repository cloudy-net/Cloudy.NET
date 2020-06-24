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
        IAssemblyProvider AssemblyProvider { get; }

        public CoreInterfaceCreator(IAssemblyProvider assemblyProvider, IPropertyDefinitionCreator propertyDefinitionCreator)
        {
            AssemblyProvider = assemblyProvider;
            PropertyDefinitionCreator = propertyDefinitionCreator;
        }

        public IEnumerable<CoreInterfaceDescriptor> Create()
        {
            var result = new List<CoreInterfaceDescriptor>();

            foreach (var type in AssemblyProvider.GetAll().SelectMany(c => c.Types))
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
