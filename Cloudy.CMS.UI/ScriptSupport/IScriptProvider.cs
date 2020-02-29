using Cloudy.CMS.ComponentSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.ScriptSupport
{
    public interface IScriptProvider
    {
        IEnumerable<ScriptDescriptor> GetAll();
    }
}
