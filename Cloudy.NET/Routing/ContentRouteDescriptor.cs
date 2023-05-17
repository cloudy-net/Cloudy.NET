using Cloudy.NET.EntityTypeSupport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cloudy.NET.Routing
{
    public record ContentRouteDescriptor(string Template, IEnumerable<Type> Types)
    {
        public string Apply(string contentRouteSegment)
        {
            return Template.Replace("{contentroute}", contentRouteSegment);
        }
    }
}