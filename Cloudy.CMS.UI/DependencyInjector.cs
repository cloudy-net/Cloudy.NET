using Cloudy.CMS.Mvc.Routing;
using Cloudy.CMS.UI.ContentAppSupport.ContentTypeActionSupport;
using Cloudy.CMS.UI.ContentAppSupport.ListActionSupport;
using Cloudy.CMS.DependencyInjectionSupport;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Cloudy.CMS.UI
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddScoped<IUrlProvider, UrlProvider>();
            services.AddSingleton<IListActionModuleCreator, ListActionModuleCreator>();
            services.AddSingleton<IListActionModuleProvider, ListActionModuleProvider>();
            services.AddSingleton<IContentTypeActionModuleCreator, ContentTypeActionModuleCreator>();
            services.AddSingleton<IContentTypeActionModuleProvider, ContentTypeActionModuleProvider>();
        }
    }
}
