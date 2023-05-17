using Cloudy.NET.EntityTypeSupport;
using System.Collections.Generic;

namespace Cloudy.NET.Routing
{
    public interface IRootContentRouter
    {
        object Route(object root, IEnumerable<string> segments, IEnumerable<EntityTypeDescriptor> types);
    }
}