using Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport.RuntimeSupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.Core.ContentSupport.RepositorySupport;
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
            services.AddSingleton<IContentGetter, ContentGetter>();
            services.AddSingleton<IContentDeleter, ContentDeleter>();
            services.AddSingleton<IContentCreator, ContentCreator>();
            services.AddSingleton<IContentInserter, ContentInserter>();
            services.AddSingleton<IContentUpdater, ContentUpdater>();
            services.AddSingleton<IChildLinkProvider, ChildLinkProvider>();
            services.AddSingleton<IChildrenGetter, ChildrenGetter>();
            services.AddSingleton<IAncestorLinkProvider, AncestorLinkProvider>();

            services.AddSingleton<IContentSerializer, ContentSerializer>();
            services.AddSingleton<IContentDeserializer, ContentDeserializer>();
            services.AddSingleton<IPolymorphicDeserializer, PolymorphicDeserializer>();
            services.AddSingleton<IPolymorphicSerializer, PolymorphicSerializer>();
            services.AddSingleton<IPolymorphicCandidateProvider, PolymorphicCandidateProvider>();

            services.AddSingleton<IContentInstanceCreator, ContentInstanceCreator>();

            services.AddSingleton<IContainerSpecificContentGetter, ContainerSpecificContentGetter>();
            services.AddSingleton<IContainerSpecificContentDeleter, ContainerSpecificContentDeleter>();
            services.AddSingleton<IContainerSpecificContentCreator, ContainerSpecificContentCreator>();
            services.AddSingleton<IContainerSpecificContentUpdater, ContainerSpecificContentUpdater>();
        }
    }
}
