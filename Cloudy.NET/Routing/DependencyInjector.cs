﻿using Cloudy.NET.DependencyInjectionSupport;
using Cloudy.NET.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.NET.Routing
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddScoped<IContentRouter, ContentRouter>();
            services.AddScoped<IRootContentRouter, RootContentRouter>();
            services.AddScoped<IRoutableRootContentProvider, RoutableRootContentProvider>();
            services.AddScoped<IContentSegmentRouter, ContentSegmentRouter>();
            services.AddSingleton<IContentRouteCreator, ContentRouteCreator>();
            services.AddSingleton<IContentRouteProvider, ContentRouteProvider>();
            services.AddSingleton<IContentRouteMatcher, ContentRouteMatcher>();
            services.AddScoped<IUrlProvider, UrlProvider>();
        }
    }
}
