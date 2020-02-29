using Cloudy.CMS.ComponentSupport;
using Cloudy.CMS.ComposableSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.ScriptSupport
{
    public interface IScriptCreator : IComposable
    {
        IEnumerable<ScriptDescriptor> Create();
    }
}
