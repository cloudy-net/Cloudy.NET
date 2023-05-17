using System;
using System.Diagnostics;

namespace Cloudy.NET.EntityTypeSupport
{
    [DebuggerDisplay("{Name}")]
    public record EntityTypeDescriptor(
        string Name,
        Type Type,
        bool IsIndependent = false,
        bool IsNameable = false,
        bool IsImageable = false,
        bool IsRoutable = false,
        bool IsSingleton = false,
        bool IsHierarchical = false
    );
}
