﻿using Cloudy.CMS.ContentSupport.Serialization;
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
using Cloudy.CMS.DocumentSupport.MongoSupport;
using Cloudy.CMS.ComponentSupport;
using Cloudy.CMS.ComposableSupport;
using Cloudy.CMS.ComponentSupport.MissingComponentAttributeCheckerSupport;
using Cloudy.CMS.ComponentSupport.DuplicateComponentIdCheckerSupport;
using Cloudy.CMS.ComponentSupport.MultipleComponentsInSingleAssemblyCheckerSupport;
using Cloudy.CMS.InitializerSupport;
using Cloudy.CMS.AspNetCore.DependencyInjectionSupport;

namespace Cloudy.CMS
{
    public class CloudyDependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IContainer container)
        {
            container.RegisterSingleton<IIdGenerator, IdGenerator>();
            container.RegisterSingleton<IPropertyDefinitionCreator, PropertyDefinitionCreator>();
            container.RegisterSingleton<ICoreInterfaceCreator, CoreInterfaceCreator>();
            container.RegisterSingleton<ICoreInterfaceProvider, CoreInterfaceProvider>();
            container.RegisterSingleton<IPropertyMappingCreator, PropertyMappingCreator>();
            container.RegisterSingleton<IPropertyMappingProvider, PropertyMappingProvider>();

            container.RegisterSingleton<IContentTypeCreator, ContentTypeCreator>();
            container.RegisterSingleton<IContentTypeProvider, ContentTypeProvider>();
            container.RegisterSingleton<IContentTypeCoreInterfaceProvider, ContentTypeCoreInterfaceProvider>();
            container.RegisterSingleton<IPropertyDefinitionProvider, PropertyDefinitionProvider>();
            container.RegisterSingleton<IContentSerializer, ContentSerializer>();
            container.RegisterSingleton<IContentDeserializer, ContentDeserializer>();
            container.RegisterSingleton<IDocumentPropertyPathProvider, DocumentPropertyPathProvider>();

            container.RegisterSingleton<IContentGetter, ContentGetter>();
            container.RegisterSingleton<IContentDeleter, ContentDeleter>();
            container.RegisterSingleton<IContentCreator, ContentCreator>();
            container.RegisterSingleton<IContentInserter, ContentInserter>();
            container.RegisterSingleton<IContentUpdater, ContentUpdater>();
            container.RegisterSingleton<IChildLinkProvider, ChildLinkProvider>();
            container.RegisterSingleton<IChildrenGetter, ChildrenGetter>();
            container.RegisterSingleton<IAncestorLinkProvider, AncestorLinkProvider>();

            container.RegisterSingleton<IContainerSpecificContentGetter, ContainerSpecificContentGetter>();
            container.RegisterSingleton<IContainerSpecificContentDeleter, ContainerSpecificContentDeleter>();
            container.RegisterSingleton<IContainerSpecificContentCreator, ContainerSpecificContentCreator>();
            container.RegisterSingleton<IContainerSpecificContentUpdater, ContainerSpecificContentUpdater>();

            container.RegisterSingleton<IContentRouter, ContentRouter>();
            container.RegisterSingleton<IContentRouteActionFinder, ContentRouteActionFinder>();
            container.RegisterSingleton<IRootContentRouter, RootContentRouter>();
            container.RegisterSingleton<IRoutableRootContentProvider, RoutableRootContentProvider>();
            container.RegisterSingleton<IContentSegmentRouter, ContentSegmentRouter>();
            container.RegisterSingleton<ISingletonCreator, SingletonCreator>();
            container.RegisterSingleton<ISingletonGetter, SingletonGetter>();
            container.RegisterSingleton<ISingletonProvider, SingletonProvider>();

            container.RegisterSingleton<IComponentCreator, ComponentCreator>();
            container.RegisterSingleton<IComponentProvider, ComponentProvider>();
            container.RegisterSingleton<IComposableProvider, ComposableProvider>();
            container.RegisterSingleton<IMissingComponentAttributeChecker, MissingComponentAttributeChecker>();
            container.RegisterSingleton<IDuplicateComponentIdChecker, DuplicateComponentIdChecker>();
            container.RegisterSingleton<IMultipleComponentsInSingleAssemblyChecker, MultipleComponentsInSingleAssemblyChecker>();
            container.RegisterSingleton<IInitializerProvider, InitializerProvider>();
            container.RegisterSingleton<IInitializerCreator, InitializerCreator>();
            container.RegisterSingleton<IInstantiator, Instantiator>();
            container.RegisterSingleton<IDependencyInjectorCreator, DependencyInjectorCreator>();
            container.RegisterSingleton<IDependencyInjectorProvider, DependencyInjectorProvider>();
        }
    }
}