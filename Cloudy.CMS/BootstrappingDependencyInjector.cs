using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.DependencyInjectionSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.ContentTypeSupport.PropertyMappingSupport;
using Cloudy.CMS.Core.ContentSupport.RepositorySupport;
using System;
using System.Collections.Generic;
using System.Text;
using Cloudy.CMS.Routing;
using Cloudy.CMS.SingletonSupport;
using Cloudy.CMS.ComponentSupport;
using Cloudy.CMS.ComposableSupport;
using Cloudy.CMS.ComponentSupport.MissingComponentAttributeCheckerSupport;
using Cloudy.CMS.ComponentSupport.DuplicateComponentIdCheckerSupport;
using Cloudy.CMS.ComponentSupport.MultipleComponentsInSingleAssemblyCheckerSupport;
using Cloudy.CMS.InitializerSupport;
using Cloudy.CMS.AspNetCore.DependencyInjectionSupport;
using Microsoft.Extensions.DependencyInjection;
using Cloudy.CMS.ContentTypeSupport.GroupSupport;
using Cloudy.CMS.DocumentSupport.CacheSupport;

namespace Cloudy.CMS
{
    public class BootstrappingDependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IPropertyDefinitionCreator, PropertyDefinitionCreator>();
            services.AddSingleton<ICoreInterfaceCreator, CoreInterfaceCreator>();
            services.AddSingleton<ICoreInterfaceProvider, CoreInterfaceProvider>();
            services.AddSingleton<IPropertyMappingCreator, PropertyMappingCreator>();
            services.AddSingleton<IPropertyMappingProvider, PropertyMappingProvider>();

            services.AddSingleton<IContentTypeCreator, ContentTypeCreator>();
            services.AddSingleton<IContentTypeProvider, ContentTypeProvider>();
            services.AddSingleton<IContentTypeGroupCreator, ContentTypeGroupCreator>();
            services.AddSingleton<IContentTypeGroupProvider, ContentTypeGroupProvider>();
            services.AddSingleton<IContentTypeGroupMatcher, ContentTypeGroupMatcher>();
            services.AddSingleton<IContentTypeCoreInterfaceProvider, ContentTypeCoreInterfaceProvider>();
            services.AddSingleton<IPropertyDefinitionProvider, PropertyDefinitionProvider>();
            services.AddSingleton<IDocumentPropertyPathProvider, DocumentPropertyPathProvider>();
            services.AddSingleton<IContentTypeExpander, ContentTypeExpander>();

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
