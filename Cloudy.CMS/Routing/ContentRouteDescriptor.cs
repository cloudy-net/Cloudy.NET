using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cloudy.CMS.Routing
{
    public record ContentRouteDescriptor(string Template, IEnumerable<Type> Types)
    {
        public string Apply(string contentRouteSegment)
        {
            return Template.Replace("{contentroute}", contentRouteSegment);
        }
    }
}