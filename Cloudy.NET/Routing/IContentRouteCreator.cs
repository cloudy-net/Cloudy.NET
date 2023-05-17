using System.Collections;
using System.Collections.Generic;

namespace Cloudy.CMS.Routing
{
    public interface IContentRouteCreator
    {
        IEnumerable<ContentRouteDescriptor> Create();
    }
}