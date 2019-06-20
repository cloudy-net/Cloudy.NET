using Poetry.ComponentSupport;
using Poetry.ComposableSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Poetry.UI.ScriptSupport
{
    public class ScriptCreator : IScriptCreator
    {
        IComponentProvider ComponentProvider { get; }

        public ScriptCreator(IComponentProvider componentProvider)
        {
            ComponentProvider = componentProvider;
        }

        public IEnumerable<ScriptDescriptor> Create()
        {
            var result = new List<ScriptDescriptor>();

            foreach (var component in ComponentProvider.GetAll())
            {
                foreach (var attribute in component.Type.GetCustomAttributes<ScriptAttribute>())
                {
                    if (attribute.Path.StartsWith("/"))
                    {
                        throw new AbsoluteScriptPathException(component.Type, attribute.Path);
                    }

                    result.Add(new ScriptDescriptor(component.Id, attribute.Path));
                }
            }

            return result.AsReadOnly();
        }
    }
}
