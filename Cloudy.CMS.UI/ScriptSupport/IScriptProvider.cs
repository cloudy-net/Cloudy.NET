using Poetry.ComponentSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.ScriptSupport
{
    public interface IScriptProvider
    {
        IEnumerable<ScriptDescriptor> GetAll();
    }
}
