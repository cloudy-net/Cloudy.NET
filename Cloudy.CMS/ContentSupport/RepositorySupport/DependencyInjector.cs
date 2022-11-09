using Cloudy.CMS.ContentSupport.RepositorySupport.Methods;
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
            services.AddScoped<IChildrenGetter, ChildrenGetter>();
        }
    }
}
