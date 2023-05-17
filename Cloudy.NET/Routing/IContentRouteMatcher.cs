using Cloudy.CMS.EntityTypeSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.Routing
{
    public interface IContentRouteMatcher
    {
        IEnumerable<ContentRouteDescriptor> GetFor(Type type);
    }
}
