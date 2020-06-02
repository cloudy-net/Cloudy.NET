using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.DependencyInjectionSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.ContentTypeSupport.PropertyMappingSupport;
using Cloudy.CMS.Core.ContentSupport.RepositorySupport;
using System;
using System.Collections.Generic;
using System.Text;
using Cloudy.CMS.Routing;
using Cloudy.CMS.SingletonSupport;
using Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport;
using Cloudy.CMS.ComponentSupport;
using Cloudy.CMS.ComposableSupport;
using Cloudy.CMS.ComponentSupport.MissingComponentAttributeCheckerSupport;
using Cloudy.CMS.ComponentSupport.DuplicateComponentIdCheckerSupport;
using Cloudy.CMS.ComponentSupport.MultipleComponentsInSingleAssemblyCheckerSupport;
using Cloudy.CMS.InitializerSupport;
using Cloudy.CMS.AspNetCore.DependencyInjectionSupport;
using Microsoft.Extensions.DependencyInjection;

namespace Cloudy.CMS
{
    public class CloudyDependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IIdGenerator, IdGenerator>();
            services.AddSingleton<IPropertyDefinitionCreator, PropertyDefinitionCreator>();
            services.AddSingleton<ICoreInterfaceCreator, CoreInterfaceCreator>();
            services.AddSingleton<ICoreInterfaceProvider, CoreInterfaceProvider>();
            services.AddSingleton<IPropertyMappingCreator, PropertyMappingCreator>();
            services.AddSingleton<IPropertyMappingProvider, PropertyMappingProvider>();

            services.AddSingleton<IContentTypeCreator, ContentTypeCreator>();
            services.AddSingleton<IContentTypeProvider, ContentTypeProvider>();
            services.AddSingleton<IContentTypeCoreInterfaceProvider, ContentTypeCoreInterfaceProvider>();
            services.AddSingleton<IPropertyDefinitionProvider, PropertyDefinitionProvider>();
            services.AddSingleton<IContentSerializer, ContentSerializer>();
            services.AddSingleton<IContentDeserializer, ContentDeserializer>();
            services.AddSingleton<IDocumentPropertyPathProvider, DocumentPropertyPathProvider>();

            services.AddSingleton<IContentGetter, ContentGetter>();
            services.AddSingleton<IContentDeleter, ContentDeleter>();
            services.AddSingleton<IContentCreator, ContentCreator>();
            services.AddSingleton<IContentInserter, ContentInserter>();
            services.AddSingleton<IContentUpdater, ContentUpdater>();
            services.AddSingleton<IChildLinkProvider, ChildLinkProvider>();
            services.AddSingleton<IChildrenGetter, ChildrenGetter>();
            services.AddSingleton<IAncestorLinkProvider, AncestorLinkProvider>();

            services.AddSingleton<IContainerSpecificContentGetter, ContainerSpecificContentGetter>();
            services.AddSingleton<IContainerSpecificContentDeleter, ContainerSpecificContentDeleter>();
            services.AddSingleton<IContainerSpecificContentCreator, ContainerSpecificContentCreator>();
            services.AddSingleton<IContainerSpecificContentUpdater, ContainerSpecificContentUpdater>();

            services.AddSingleton<IContentRouter, ContentRouter>();
            services.AddSingleton<IContentRouteActionFinder, ContentRouteActionFinder>();
            services.AddSingleton<IRootContentRouter, RootContentRouter>();
            services.AddSingleton<IRoutableRootContentProvider, RoutableRootContentProvider>();
            services.AddSingleton<IContentSegmentRouter, ContentSegmentRouter>();
            services.AddSingleton<ISingletonCreator, SingletonCreator>();
            services.AddSingleton<ISingletonGetter, SingletonGetter>();
            services.AddSingleton<ISingletonProvider, SingletonProvider>();

            services.AddSingleton<IComponentCreator, ComponentCreator>();
            services.AddSingleton<IComponentProvider, ComponentProvider>();
            services.AddSingleton<IComposableProvider, ComposableProvider>();
            services.AddSingleton<IMissingComponentAttributeChecker, MissingComponentAttributeChecker>();
            services.AddSingleton<IDuplicateComponentIdChecker, DuplicateComponentIdChecker>();
            services.AddSingleton<IMultipleComponentsInSingleAssemblyChecker, MultipleComponentsInSingleAssemblyChecker>();
            services.AddSingleton<IInitializerProvider, InitializerProvider>();
            services.AddSingleton<IInitializerCreator, InitializerCreator>();
            services.AddSingleton<IInstantiator, Instantiator>();
            services.AddSingleton<IDependencyInjectorCreator, DependencyInjectorCreator>();
            services.AddSingleton<IDependencyInjectorProvider, DependencyInjectorProvider>();
        }
    }
}
