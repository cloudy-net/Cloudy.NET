using Cloudy.CMS.ContextSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport.Methods;
using Cloudy.CMS.ContentSupport.RepositorySupport.PrimaryKey;
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

            services.AddScoped<IPrimaryKeyConverter, PrimaryKeyConverter>();
            services.AddScoped<IPrimaryKeyPropertyGetter, PrimaryKeyPropertyGetter>();
            services.AddScoped<IPrimaryKeyGetter, PrimaryKeyGetter>();
            services.AddScoped<IPrimaryKeySetter, PrimaryKeySetter>();
        }
    }
}
