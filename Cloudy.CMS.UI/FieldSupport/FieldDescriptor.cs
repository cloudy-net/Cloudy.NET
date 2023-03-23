using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Cloudy.CMS.UI.FieldSupport
{
    [DebuggerDisplay("{Name}")]
    public record FieldDescriptor(
        string Name,
        [property: JsonIgnore] Type Type,
        string Label,
        string Description,
        string Partial,
        string ListPartial,
        bool? AutoGenerate,
        bool RenderChrome,
        string Tab,
        IDictionary<string, object> Settings,
        IDictionary<string, object> Validators
    );
}
