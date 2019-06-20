using Poetry.ComponentSupport;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Poetry.UI.FormSupport.ControlSupport
{
    public class ControlCreator : IControlCreator
    {
        IComponentProvider ComponentProvider { get; }
        IBasePathProvider BasePathProvider { get; }

        public ControlCreator(IComponentProvider componentProvider, IBasePathProvider basePathProvider)
        {
            ComponentProvider = componentProvider;
            BasePathProvider = basePathProvider;
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

                    var path = $"/{BasePathProvider.BasePath}/{component.Id}/{attribute.ModulePath}";

                    result.Add(new ControlDescriptor(attribute.Id, path));
                }
            }

            return result.AsReadOnly();
        }
    }
}
