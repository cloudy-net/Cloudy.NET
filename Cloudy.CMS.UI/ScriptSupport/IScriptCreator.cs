using Poetry.ComponentSupport;
using Poetry.ComposableSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.ScriptSupport
{
    public interface IScriptCreator : IComposable
    {
        IEnumerable<ScriptDescriptor> Create();
    }
}
