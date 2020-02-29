using Cloudy.CMS.ComponentSupport;
using Cloudy.CMS.ComposableSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cloudy.CMS.UI.ScriptSupport
{
    public class ScriptCreator : IScriptCreator
    {
        IComponentTypeProvider ComponentTypeProvider { get; }

        public ScriptCreator(IComponentTypeProvider componentTypeProvider)
        {
            ComponentTypeProvider = componentTypeProvider;
        }

        public IEnumerable<ScriptDescriptor> Create()
        {
            var result = new List<ScriptDescriptor>();

            foreach (var type in ComponentTypeProvider.GetAll())
            {
                var componentAttribute = type.GetCustomAttribute<ComponentAttribute>();

                foreach (var scriptAttribute in type.GetCustomAttributes<ScriptAttribute>())
                {
                    if (scriptAttribute.Path.StartsWith("/"))
                    {
                        throw new AbsoluteScriptPathException(type, scriptAttribute.Path);
                    }

                    result.Add(new ScriptDescriptor(componentAttribute.Id, scriptAttribute.Path));
                }
            }

            return result.AsReadOnly();
        }
    }
}
