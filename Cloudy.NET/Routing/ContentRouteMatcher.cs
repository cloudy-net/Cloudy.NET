using Cloudy.NET.EntitySupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.NET.Routing
{
    public record ContentRouteMatcher(IContentRouteProvider ContentRouteProvider) : IContentRouteMatcher
    {
        public IEnumerable<ContentRouteDescriptor> GetFor(Type type)
        {
            if (!type.IsAssignableTo(typeof(IRoutable)))
            {
                return Enumerable.Empty<ContentRouteDescriptor>();
            }

            var result = new List<ContentRouteDescriptor>();

            foreach(var contentRoute in ContentRouteProvider.GetAll())
            {
                if(!contentRoute.Types.Any(t => t == type))
                {
                    continue;
                }

                result.Add(contentRoute);
            }

            return result.AsReadOnly();
        }
    }
}
