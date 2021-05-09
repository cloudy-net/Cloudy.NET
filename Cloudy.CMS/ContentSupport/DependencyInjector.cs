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

            services.AddSingleton<IPolymorphicDeserializer, PolymorphicDeserializer>();
            services.AddSingleton<IPolymorphicSerializer, PolymorphicSerializer>();
            services.AddSingleton<IPolymorphicCandidateProvider, PolymorphicCandidateProvider>();

            services.AddSingleton<IContentInstanceCreator, ContentInstanceCreator>();
        }
    }
}
