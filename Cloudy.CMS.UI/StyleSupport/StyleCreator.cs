using Cloudy.CMS.ComponentSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cloudy.CMS.UI.StyleSupport
{
    public class StyleCreator : IStyleCreator
    {
        IComponentTypeProvider ComponentTypeProvider { get; }

        public StyleCreator(IComponentTypeProvider componentTypeProvider)
        {
            ComponentTypeProvider = componentTypeProvider;
        }

        public IEnumerable<StyleDescriptor> Create()
        {
            var result = new List<StyleDescriptor>();

            foreach (var type in ComponentTypeProvider.GetAll())
            {
                var componentAttribute = type.GetCustomAttribute<ComponentAttribute>();

                foreach (var scriptAttribute in type.GetCustomAttributes<StyleAttribute>())
                {
                    result.Add(new StyleDescriptor(scriptAttribute.Path));
                }
            }

            return result.AsReadOnly();
        }
    }
}
