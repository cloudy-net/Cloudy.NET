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
        IEnumerable<ScriptDescriptor> Scripts { get; }

        public ScriptProvider(IComposableProvider composableProvider)
        {
            Scripts = composableProvider.GetAll<IScriptCreator>().SelectMany(c => c.Create()).ToList().AsReadOnly();
        }

        public IEnumerable<ScriptDescriptor> GetAll()
        {
            return Scripts;
        }
    }
}
