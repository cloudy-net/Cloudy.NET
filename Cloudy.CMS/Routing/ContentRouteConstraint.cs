using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.Routing
{
    public class ContentRouteConstraint : IRouteConstraint
    {
        IContentRouter ContentRouter { get; }

        public ContentRouteConstraint(IContentRouter contentRouter)
        {
            ContentRouter = contentRouter;
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var content = ContentRouter.RouteContent(values[routeKey]?.ToString().Split('/') ?? Enumerable.Empty<string>(), null);

            values["contentFromContentRoute"] = content;

            return content != null;
        }
    }
}
