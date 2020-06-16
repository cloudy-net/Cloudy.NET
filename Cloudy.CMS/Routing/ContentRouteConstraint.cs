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
        string ContentTypeorGroupId { get; }

        public ContentRouteConstraint(IContentRouter contentRouter, IContentTypeExpander contentTypeExpander, string contentTypeorGroupId = null)
        {
            ContentRouter = contentRouter;
            ContentTypeExpander = contentTypeExpander;
            ContentTypeorGroupId = contentTypeorGroupId;
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var types = ContentTypeorGroupId != null ? ContentTypeExpander.Expand(ContentTypeorGroupId) : Enumerable.Empty<ContentTypeDescriptor>();

            if(types == null)
            {
                throw new Exception($"Routing could not use the constraint :contentroute({ContentTypeorGroupId}) because it did not resolve to a content type or group");
            }

            var content = ContentRouter.RouteContent(values[routeKey]?.ToString().Split('/') ?? Enumerable.Empty<string>(), types, null);

            values["contentFromContentRoute"] = content;

            return content != null;
        }
    }
}
