using Cloudy.CMS.ComponentSupport;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport
{
    public class ControlCreator : IControlCreator
    {
        IComponentProvider ComponentProvider { get; }

        public ControlCreator(IComponentProvider componentProvider)
        {
            ComponentProvider = componentProvider;
        }

        public IEnumerable<ControlDescriptor> Create()
        {
            var result = new List<ControlDescriptor>();

            foreach(var component in ComponentProvider.GetAll())
            {
                foreach (var type in component.Assembly.Types)
                {
                    var attribute = type.GetCustomAttribute<ControlAttribute>();

                    if(attribute == null)
                    {
                        continue;
                    }

                    if (attribute.ModulePath.StartsWith("/"))
                    {
                        throw new AbsoluteControlModulePathException(type, attribute.ModulePath);
                    }

                    result.Add(new ControlDescriptor(attribute.Id, attribute.ModulePath));
                }
            }

            return result.AsReadOnly();
        }
    }
}
