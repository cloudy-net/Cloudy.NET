using System;
using System.Diagnostics;

namespace Cloudy.CMS.BlockSupport
{
    [DebuggerDisplay("{Name}")]
    public record BlockTypeDescriptor(
        string Name,
        Type Type
    );
}
