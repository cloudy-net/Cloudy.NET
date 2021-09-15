using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport.RuntimeSupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.DependencyInjectionSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IIdGenerator, IdGenerator>();

            services.AddScoped<IPolymorphicDeserializer, PolymorphicDeserializer>();
            services.AddScoped<IPolymorphicSerializer, PolymorphicSerializer>();
            services.AddScoped<IPolymorphicCandidateProvider, PolymorphicCandidateProvider>();

            services.AddScoped<IContentInstanceCreator, ContentInstanceCreator>();

            services.AddScoped<IPrimaryKeyConverter, PrimaryKeyConverter>();
            services.AddScoped<IPrimaryKeyPropertyGetter, PrimaryKeyPropertyGetter>();
            services.AddScoped<IPrimaryKeyGetter, PrimaryKeyGetter>();
            services.AddScoped<IPrimaryKeySetter, PrimaryKeySetter>();

            services.AddScoped<IContentGetter, ContentGetter>();
            services.AddScoped<IContentInserter, ContentInserter>();
            services.AddScoped<IContentFinder, ContentFinder>();
            services.AddScoped<IContentUpdater, ContentUpdater>();
            services.AddScoped<IContentCreator, ContentCreator>();

            services.AddScoped<IDbSetProvider, DbSetProvider>();
            services.AddScoped<IContextProvider, ContextProvider>();
            services.AddScoped<IContextCreator, ContextCreator>();
        }
    }
}
