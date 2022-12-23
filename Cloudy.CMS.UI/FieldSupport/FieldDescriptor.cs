using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Cloudy.CMS.UI.FormSupport.FieldSupport
{
    [DebuggerDisplay("{Name}")]
    public record FieldDescriptor(
        string Name,
        [property: JsonIgnore] Type Type,
        string Label,
        string Partial,
        bool AutoGenerate,
        bool RenderChrome,
        string Tab
    );
}
