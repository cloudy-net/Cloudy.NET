using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.NET.Routing
{
    public static class HttpContextExtensions
    {
        public static object GetContentFromContentRoute(this HttpContext instance)
        {
            if (!instance.Request.RouteValues.ContainsKey("contentFromContentRoute"))
            {
                return null;
            }

            return instance.Request.RouteValues["contentFromContentRoute"];
        }

        public static T GetContentFromContentRoute<T>(this HttpContext instance) where T : class
        {
            return GetContentFromContentRoute(instance) as T;
        }
    }
}
