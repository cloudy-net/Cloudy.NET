using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Cloudy.CMS.UI.StyleSupport
{
    [DebuggerDisplay("{Path}")]
    public class StyleDescriptor
    {
        public string Path { get; }

        public StyleDescriptor(string path)
        {
            Path = path;
        }
    }
}
