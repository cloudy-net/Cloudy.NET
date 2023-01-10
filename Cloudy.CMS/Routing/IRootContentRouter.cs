using Cloudy.CMS.EntityTypeSupport;
using System.Collections.Generic;

namespace Cloudy.CMS.Routing
{
    public interface IRootContentRouter
    {
        object Route(object root, IEnumerable<string> segments, IEnumerable<EntityTypeDescriptor> types);
    }
}