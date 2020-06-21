using Cloudy.CMS.DependencyInjectionSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.Routing
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IContentRouter, ContentRouter>();
            services.AddSingleton<IRootContentRouter, RootContentRouter>();
            services.AddSingleton<IRoutableRootContentProvider, RoutableRootContentProvider>();
            services.AddSingleton<IContentSegmentRouter, ContentSegmentRouter>();
            services.AddSingleton<IContentRouteCreator, ContentRouteCreator>();
            services.AddSingleton<IContentRouteProvider, ContentRouteProvider>();
            services.AddSingleton<IContentRouteMatcher, ContentRouteMatcher>();
        }
    }
}
