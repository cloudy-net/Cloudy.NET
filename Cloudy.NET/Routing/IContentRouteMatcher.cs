using Cloudy.NET.EntityTypeSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.NET.Routing
{
    public interface IContentRouteMatcher
    {
        IEnumerable<ContentRouteDescriptor> GetFor(Type type);
    }
}
