using System;
using System.Diagnostics;

namespace Cloudy.CMS.ContentTypeSupport
{
    [DebuggerDisplay("{Name}")]
    public record ContentTypeDescriptor(
        string Name,
        Type Type,
        bool IsNameable = false,
        bool IsImageable = false,
        bool IsRoutable = false,
        bool IsSingleton = false
    );
}
