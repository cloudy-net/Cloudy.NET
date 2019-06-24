using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Poetry.UI.StyleSupport
{
    [DebuggerDisplay("{Path}")]
    public class StyleDescriptor
    {
        public string ComponentId { get; set; }
        public string Path { get; }

        public StyleDescriptor(string componentId, string path)
        {
            ComponentId = componentId;
            Path = path;
        }
    }
}
