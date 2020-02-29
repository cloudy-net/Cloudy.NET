using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport
{
    public class AbsoluteControlModulePathException : Exception
    {
        public AbsoluteControlModulePathException(Type ownerType, string path) : base($"Type {ownerType} has defined a control with an absolute module path ({path}). Module paths can never be absolute but rather always needs to be relative to their respective component to which they belong. (so remove the initial slash)") { }
    }
}
