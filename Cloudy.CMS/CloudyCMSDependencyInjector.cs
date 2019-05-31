using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.DocumentSupport;
using Poetry.DependencyInjectionSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.ContentTypeSupport.PropertyMappingSupport;
using Cloudy.CMS.Core.ContentSupport.RepositorySupport;
using System;
using System.Collections.Generic;
using System.Text;
using Cloudy.CMS.ContentControllerSupport;
using Cloudy.CMS.Routing;
using Cloudy.CMS.SingletonSupport;
using Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport;

namespace Cloudy.CMS
{
    public class CloudyCMSDependencyRegisteror : IDependencyInjector
    {
        public void InjectDependencies(IContainer container)
        {
            container.RegisterSingleton<IIdGenerator, IdGenerator>();
            container.RegisterSingleton<IContentTypeCreator, ContentTypeCreator>();
            container.RegisterSingleton<IContentTypeProvider, ContentTypeProvider>();
            container.RegisterSingleton<IContentSerializer, ContentSerializer>();
            container.RegisterSingleton<IContentDeserializer, ContentDeserializer>();
            container.RegisterSingleton<IContainerProvider, ContainerProvider>();

            container.RegisterSingleton<IContentGetter, ContentGetter>();
            container.RegisterSingleton<IContentDeleter, ContentDeleter>();
            container.RegisterSingleton<IContentCreator, ContentCreator>();
            container.RegisterSingleton<IContentInserter, ContentInserter>();
            container.RegisterSingleton<IContentUpdater, ContentUpdater>();
            container.RegisterSingleton<IChildLinkProvider, ChildLinkProvider>();
            container.RegisterSingleton<IAncestorLinkProvider, AncestorLinkProvider>();

            container.RegisterSingleton<IContainerSpecificContentGetter, ContainerSpecificContentGetter>();
            container.RegisterSingleton<IContainerSpecificContentDeleter, ContainerSpecificContentDeleter>();
            container.RegisterSingleton<IContainerSpecificContentCreator, ContainerSpecificContentCreator>();
            container.RegisterSingleton<IContainerSpecificContentUpdater, ContainerSpecificContentUpdater>();

            container.RegisterSingleton<IPropertyDefinitionCreator, PropertyDefinitionCreator>();
            container.RegisterSingleton<ICoreInterfaceCreator, CoreInterfaceCreator>();
            container.RegisterSingleton<IPropertyMappingCreator, PropertyMappingCreator>();
            container.RegisterSingleton<IPropertyMappingProvider, PropertyMappingProvider>();

            container.RegisterSingleton<IContentControllerFinder, ContentControllerFinder>();
            container.RegisterSingleton<IContentRouter, ContentRouter>();
            container.RegisterSingleton<IContentSegmentRouter, ContentSegmentRouter>();
            container.RegisterSingleton<ISingletonCreator, SingletonCreator>();
            container.RegisterSingleton<ISingletonGetter, SingletonGetter>();
            container.RegisterSingleton<ISingletonProvider, SingletonProvider>();
        }
    }
}
