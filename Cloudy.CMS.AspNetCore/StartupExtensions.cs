using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Poetry;
using Poetry.AspNetCore.DependencyInjectionSupport;
using Cloudy.CMS;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContentControllerSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.ContentTypeSupport.PropertyMappingSupport;
using Cloudy.CMS.Core;
using Cloudy.CMS.Core.ContentSupport.RepositorySupport;
using Cloudy.CMS.Mvc.Routing;
using Cloudy.CMS.Reflection;
using Cloudy.CMS.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Cloudy.CMS.AspNetCore.ContentControllerSupport;

namespace Cloudy.CMS.Mvc
{
    public static class StartupExtensions
    {
        static PoetryConfigurator PoetryConfigurator { get; set; }

        public static void AddCMS(this PoetryConfigurator poetryConfigurator, Action<CMSConfigurator> configuratorFunction)
        {
            PoetryConfigurator = poetryConfigurator;

            poetryConfigurator.AddComponent<CloudyCMSComponent>();
            poetryConfigurator.InjectSingleton<IMemberExpressionFromExpressionExtractor, MemberExpressionFromExpressionExtractor>();
            poetryConfigurator.InjectSingleton<IUrlGenerator, UrlGenerator>();
            poetryConfigurator.InjectSingleton<IControllerProvider, ControllerProvider>();
            poetryConfigurator.InjectSingleton<IContentControllerMatchCreator, ContentControllerMatchCreator>();

            configuratorFunction(new CMSConfigurator());
        }

        public static void AddContentRoute(this IRouteBuilder routes)
        {
            var resolver = PoetryConfigurator.Container.CreateResolver();
            routes.Routes.Add(new ContentRoute(routes.DefaultHandler, resolver.Resolve<IContentRouter>(), resolver.Resolve<IContentTypeProvider>(), resolver.Resolve<IContentControllerFinder>()));
        }
    }
}