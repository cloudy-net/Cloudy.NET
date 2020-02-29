using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cloudy.CMS.ComponentSupport;
using Cloudy.CMS.ComposableSupport;

namespace Cloudy.CMS.UI.ScriptSupport
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
