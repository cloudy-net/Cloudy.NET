using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.RoutableResourceSupport
{
    public class AbsoluteRoutableResourcePathException : Exception
    {
        public AbsoluteRoutableResourcePathException(Type ownerType, string path) : base($"Type {ownerType} has defined a routable resource with an absolute path ({path}). Routable resource paths can never be absolute but rather always needs to be relative to their respective component to which they belong. (so remove the initial slash)") { }
    }
}
