using Cloudy.CMS.ComponentSupport;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.TypeControlMappingSupport
{
    public class TypeControlMappingCreator : ITypeControlMappingCreator
    {
        IComponentProvider ComponentProvider { get; }

        public TypeControlMappingCreator(IComponentProvider componentProvider)
        {
            ComponentProvider = componentProvider;
        }

        public IEnumerable<TypeControlMapping> Create()
        {
            var result = new List<TypeControlMapping>();

            foreach (var component in ComponentProvider.GetAll())
            {
                foreach (var type in component.Assembly.Types)
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
