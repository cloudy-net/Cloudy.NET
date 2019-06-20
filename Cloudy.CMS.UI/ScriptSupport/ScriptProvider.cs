using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Poetry.ComponentSupport;
using Poetry.ComposableSupport;

namespace Poetry.UI.ScriptSupport
{
    public class ScriptProvider : IScriptProvider
    {
        IDictionary<string, IEnumerable<ScriptDescriptor>> Scripts { get; }

        public ScriptProvider(IComposableProvider composableProvider)
        {
            Scripts = composableProvider.GetAll<IScriptCreator>().SelectMany(c => c.Create()).GroupBy(s => s.ComponentId).ToDictionary(c => c.Key, c => (IEnumerable<ScriptDescriptor>)c.ToList().AsReadOnly());
        }

        public IEnumerable<ScriptDescriptor> GetAllFor(ComponentDescriptor component)
        {
            if (!Scripts.ContainsKey(component.Id))
            {
                return Enumerable.Empty<ScriptDescriptor>();
            }

            return Scripts[component.Id];
        }
    }
}
