using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.FieldSupport
{
    [DebuggerDisplay("{Name}")]
    public record FieldDescriptor(
        string Name,
        Type Type,
        IEnumerable<object> Attributes,
        string Label,
        string Partial,
        bool IsSortable,
        bool AutoGenerate,
        bool RenderChrome,
        string Tab
    );
}
