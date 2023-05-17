using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Cloudy.NET.UI.FieldSupport
{
    [DebuggerDisplay("{Name}")]
    public record FieldDescriptor(
        string Name,
        [property: JsonIgnore] Type Type,
        string Label = null,
        string Description = null,
        string Partial = null,
        string ListPartial = null,
        bool? AutoGenerate = null,
        bool RenderChrome = false,
        string Tab = null,
        IDictionary<string, object> Settings = null,
        IDictionary<string, object> Validators = null
    );
}
