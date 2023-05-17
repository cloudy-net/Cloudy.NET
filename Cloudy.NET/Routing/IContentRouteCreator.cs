using System.Collections;
using System.Collections.Generic;

namespace Cloudy.NET.Routing
{
    public interface IContentRouteCreator
    {
        IEnumerable<ContentRouteDescriptor> Create();
    }
}