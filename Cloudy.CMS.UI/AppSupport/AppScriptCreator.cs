using Cloudy.CMS.ComponentSupport;
using Cloudy.CMS.UI.ScriptSupport;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.UI.AppSupport
{
    public class AppScriptCreator : IScriptCreator
    {
        IComponentProvider ComponentProvider { get; }

        public AppScriptCreator(IComponentProvider componentProvider)
        {
            ComponentProvider = componentProvider;
        }

        public IEnumerable<ScriptDescriptor> Create()
        {
            var result = new List<ScriptDescriptor>();

            foreach (var component in ComponentProvider.GetAll())
            {
                foreach (var type in component.Assembly.Types)
                {
                    var attribute = type.GetCustomAttribute<AppAttribute>();

                    if (attribute == null)
                    {
                        continue;
                    }

                    var scriptAttributes = type.GetCustomAttributes<ScriptAttribute>();

                    foreach(var scriptAttribute in scriptAttributes)
                    {
                        if (scriptAttribute.Path.StartsWith("/"))
                        {
                            throw new AbsoluteScriptPathException(type, scriptAttribute.Path);
                        }

                        result.Add(new ScriptDescriptor(component.Id, scriptAttribute.Path));
                    }
                }
            }

            return result.AsReadOnly();
        }
    }
}
