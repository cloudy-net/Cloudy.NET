using Cloudy.CMS.ContentSupport;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.Routing
{
    public class ContentRoute : RouteBase
    {
        IRouter Target { get; }

        public ContentRoute(IRouter target, string routeName, string routeTemplate, RouteValueDictionary defaults, IDictionary<string, object> constraints, RouteValueDictionary dataTokens, IInlineConstraintResolver inlineConstraintResolver) :
            base(routeTemplate, routeName, inlineConstraintResolver, defaults, constraints, dataTokens)
        {
            Target = target;
        }

        public string RouteTemplate => ParsedTemplate.TemplateText;

        IContentRouteActionFinder ContentRouteActionFinder { get; set; }

        protected override Task OnRouteMatched(RouteContext context)
        {
            if (context.RouteData.Values.ContainsKey("controller") && context.RouteData.Values.ContainsKey("contentFromContentRoute") && !context.RouteData.Values.ContainsKey("action"))
            {
                if(ContentRouteActionFinder == null)
                {
                    ContentRouteActionFinder = (IContentRouteActionFinder)context.HttpContext.RequestServices.GetRequiredService(typeof(IContentRouteActionFinder));
                }

                var action = ContentRouteActionFinder.Find((string)context.RouteData.Values["controller"], (IContent)context.RouteData.Values["contentFromContentRoute"]);

                if (action != null)
                {
                    context.RouteData.Values["action"] = action.ActionName;
                }
            }

            context.RouteData.Routers.Add(Target);
            return Target.RouteAsync(context);
        }

        protected override VirtualPathData OnVirtualPathGenerated(VirtualPathContext context)
        {
            return Target.GetVirtualPath(context);
        }
    }
}
