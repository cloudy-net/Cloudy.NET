using Cloudy.CMS.UI.ScriptSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.AppSupport
{
    public class AppModuleScriptCreator : IScriptCreator
    {
        IAppProvider AppProvider { get; }

        public AppModuleScriptCreator(IAppProvider appProvider)
        {
            AppProvider = appProvider;
        }

        public IEnumerable<ScriptDescriptor> Create()
        {
            var result = new List<ScriptDescriptor>();

            foreach(var app in AppProvider.GetAll())
            {
                result.Add(new ScriptDescriptor(app.ComponentId, app.ModulePath));
            }

            return result;
        }
    }
}
