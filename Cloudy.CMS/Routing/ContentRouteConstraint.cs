using Cloudy.CMS.ContentTypeSupport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.Routing
{
    public class ContentRouteConstraint : IRouteConstraint
    {
        IContentTypeExpander ContentTypeExpander { get; }
        IEnumerable<ContentTypeDescriptor> Types { get; }

        public ContentRouteConstraint(IContentTypeExpander contentTypeExpander, IContentTypeProvider contentTypeProvider, string contentTypeorGroupIdOrTypeName = null)
        {
            ContentTypeExpander = contentTypeExpander;

            Types = contentTypeorGroupIdOrTypeName != null ? ContentTypeExpander.Expand(contentTypeorGroupIdOrTypeName) : contentTypeProvider.GetAll();

            if(Types == null)
            {
                throw new Exception($"Routing could not use the constraint :contentroute({contentTypeorGroupIdOrTypeName}) because it did not resolve to a content type or group or type name");
            }
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var contentRouter = (IContentRouter)httpContext.RequestServices.GetService(typeof(IContentRouter));

            var task = Task.Run(async () => await contentRouter.RouteContentAsync(values[routeKey]?.ToString().Split('/') ?? Enumerable.Empty<string>(), Types).ConfigureAwait(false));
            var content = task.WaitAndUnwrapException();

            values["contentFromContentRoute"] = content;

            return content != null;
        }
    }
}
