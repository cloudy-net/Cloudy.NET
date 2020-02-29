using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.ScriptSupport
{
    public class AbsoluteScriptPathException : Exception
    {
        public AbsoluteScriptPathException(Type ownerType, string path) : base($"Type {ownerType} has defined a script with an absolute path ({path}). Script paths can never be absolute but rather always needs to be relative to their respective component to which they belong. (so remove the initial slash)") { }
    }
}
