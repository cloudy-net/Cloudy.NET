using Cloudy.CMS.ComponentSupport;
using Cloudy.CMS.UI.ScriptSupport;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport
{
    public class ControlScriptCreator : IScriptCreator
    {
        IComponentProvider ComponentProvider { get; }

        public ControlScriptCreator(IComponentProvider componentProvider)
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
                    var controlAttribute = type.GetCustomAttribute<ControlAttribute>();

                    if (controlAttribute == null)
                    {
                        continue;
                    }

                    result.Add(new ScriptDescriptor(component.Id, controlAttribute.ModulePath));
                }
            }

            return result.AsReadOnly();
        }
    }
}
