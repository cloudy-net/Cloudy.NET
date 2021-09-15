using Cloudy.CMS.DependencyInjectionSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddScoped<IAncestorProvider, AncestorProvider>();
            services.AddScoped<IContentChildrenCounter, ContentChildrenCounter>();
            services.AddScoped<IChildrenGetter, ChildrenGetter>();
        }
    }
}
