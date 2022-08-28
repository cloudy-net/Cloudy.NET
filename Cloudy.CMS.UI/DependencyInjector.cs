using Cloudy.CMS.Mvc.Routing;
using Cloudy.CMS.Reflection;
using Cloudy.CMS.UI.ContentAppSupport;
using Cloudy.CMS.UI.ContentAppSupport.ContentTypeActionSupport;
using Cloudy.CMS.UI.ContentAppSupport.ListActionSupport;
using Cloudy.CMS.DependencyInjectionSupport;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Cloudy.CMS.UI.ContentAppSupport.Controllers;

namespace Cloudy.CMS.UI
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IMemberExpressionFromExpressionExtractor, MemberExpressionFromExpressionExtractor>();
            services.AddScoped<IUrlProvider, UrlProvider>();
            services.AddSingleton<INameExpressionParser, NameExpressionParser>();
            services.AddSingleton<IImageExpressionParser, ImageExpressionParser>();
            services.AddSingleton<IListActionModuleCreator, ListActionModuleCreator>();
            services.AddSingleton<IListActionModuleProvider, ListActionModuleProvider>();
            services.AddSingleton<IContentTypeActionModuleCreator, ContentTypeActionModuleCreator>();
            services.AddSingleton<IContentTypeActionModuleProvider, ContentTypeActionModuleProvider>();
        }
    }
}
