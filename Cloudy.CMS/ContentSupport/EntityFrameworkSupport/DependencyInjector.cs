using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.DependencyInjectionSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.EntityFrameworkSupport
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddScoped<IPrimaryKeyConverter, PrimaryKeyConverter>();
            services.AddScoped<IPrimaryKeyGetter, PrimaryKeyGetter>();
            services.AddScoped<IPrimaryKeySetter, PrimaryKeySetter>();

            services.AddScoped<IContentGetter, ContentGetter>();
            services.AddScoped<IContentInserter, ContentInserter>();
            services.AddScoped<IContentFinder, ContentFinder>();
            services.AddScoped<IContentChildrenCounter, ContentChildrenCounter>();
            services.AddScoped<IContentUpdater, ContentUpdater>();
            services.AddScoped<IContentCreator, ContentCreator>();

            services.AddScoped<IDbSetProvider, DbSetProvider>();
            services.AddScoped<IContextProvider, ContextProvider>();
            services.AddScoped<IContextCreator, ContextCreator>();
        }
    }
}
