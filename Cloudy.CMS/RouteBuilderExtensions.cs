using Cloudy.CMS.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS
{
    public static class RouteBuilderExtensions
    {
        public static IEndpointRouteBuilder MapContentRoute(this IEndpointRouteBuilder routeBuilder, string name, string template)
        {
            MapContentRoute(routeBuilder, name, template, defaults: null);
            return routeBuilder;
        }

        public static IEndpointRouteBuilder MapContentRoute(this IEndpointRouteBuilder routeBuilder, string name, string template, object defaults)
        {
            return MapContentRoute(routeBuilder, name, template, defaults, constraints: null);
        }

        public static IEndpointRouteBuilder MapContentRoute(this IEndpointRouteBuilder routeBuilder, string name, string template, object defaults, object constraints)
        {
            return MapContentRoute(routeBuilder, name, template, defaults, constraints, dataTokens: null);
        }

        public static IEndpointRouteBuilder MapContentRoute(this IEndpointRouteBuilder routeBuilder, string name, string template, object defaults, object constraints, object dataTokens)
        {
            routeBuilder.Routes.Add(new ContentRoute(
                routeBuilder.DefaultHandler,
                name,
                template,
                new RouteValueDictionary(defaults),
                new RouteValueDictionary(constraints),
                new RouteValueDictionary(dataTokens),
                routeBuilder.ServiceProvider.GetRequiredService<IInlineConstraintResolver>()));

            return routeBuilder;
        }
    }
}
