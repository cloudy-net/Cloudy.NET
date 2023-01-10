using System;
using System.Diagnostics;

namespace Cloudy.CMS.EntityTypeSupport
{
    [DebuggerDisplay("{Name}")]
    public record EntityTypeDescriptor(
        string Name,
        Type Type,
        bool IsNameable = false,
        bool IsImageable = false,
        bool IsRoutable = false,
        bool IsSingleton = false,
        bool IsHierarchical = false
    );
}
