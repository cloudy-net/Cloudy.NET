using Cloudy.CMS.ContentTypeSupport;
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
        IContentTypeExpander ContentTypeExpander { get; }
        IEnumerable<ContentTypeDescriptor> Types { get; }

        public ContentRouteConstraint(IContentRouter contentRouter, IContentTypeExpander contentTypeExpander, string contentTypeorGroupId = null)
        {
            ContentRouter = contentRouter;
            ContentTypeExpander = contentTypeExpander;

            Types = contentTypeorGroupId != null ? ContentTypeExpander.Expand(contentTypeorGroupId) : Enumerable.Empty<ContentTypeDescriptor>();

            if(Types == null)
            {
                throw new Exception($"Routing could not use the constraint :contentroute({contentTypeorGroupId}) because it did not resolve to a content type or group");
            }
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var content = ContentRouter.RouteContent(values[routeKey]?.ToString().Split('/') ?? Enumerable.Empty<string>(), Types, null);

            values["contentFromContentRoute"] = content;

            return content != null;
        }
    }
}
