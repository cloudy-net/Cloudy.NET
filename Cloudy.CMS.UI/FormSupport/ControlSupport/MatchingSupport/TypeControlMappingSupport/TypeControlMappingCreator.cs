using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.TypeControlMappingSupport
{
    public class TypeControlMappingCreator : ITypeControlMappingCreator
    {
        IAssemblyProvider AssemblyProvider { get; }

        public TypeControlMappingCreator(IAssemblyProvider assemblyProvider)
        {
            AssemblyProvider = assemblyProvider;
        }

        public IEnumerable<TypeControlMapping> Create()
        {
            var result = new List<TypeControlMapping>();

            foreach (var assembly in AssemblyProvider.GetAll())
            {
                foreach (var type in assembly.Types)
                {
                    var controlAttribute = type.GetCustomAttribute<ControlAttribute>();

                    if (controlAttribute == null)
                    {
                        continue;
                    }

                    foreach (var mapControlToTypeAttribute in type.GetCustomAttributes<MapControlToTypeAttribute>())
                    {
                        result.Add(new TypeControlMapping(mapControlToTypeAttribute.Type, controlAttribute.Id));
                    }
                }
            }

            return result.AsReadOnly();
        }
    }
}
