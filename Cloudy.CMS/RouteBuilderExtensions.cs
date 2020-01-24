using Cloudy.CMS.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS
{
    public static class RouteBuilderExtensions
    {
        //public static IRouteBuilder MapContentControllerRoute(this IRouteBuilder routeBuilder, string name, string template, object defaults, object constraints, object dataTokens)
        //{
        //    routeBuilder.MapRoute(new ContentRoute(
        //        name,
        //        template,
        //        new RouteValueDictionary(defaults),
        //        new RouteValueDictionary(constraints),
        //        new RouteValueDictionary(dataTokens),
        //        routeBuilder.ServiceProvider.GetService<IInlineConstraintResolver>()));

        //    return routeBuilder;
        //}
    }
}
