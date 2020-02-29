using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.StyleSupport
{
    public class AbsoluteStylePathException : Exception
    {
        public AbsoluteStylePathException(Type ownerType, string path) : base($"Type {ownerType} has defined a style with an absolute path ({path}). Style paths can never be absolute but rather always needs to be relative to their respective component to which they belong. (so remove the initial slash)") { }
    }
}
