using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.DependencyInjectionSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IContentJsonConverterTypeProvider, ContentJsonConverterTypeProvider>();
            services.AddSingleton<IContentJsonConverterCreator, ContentJsonConverterCreator>();
            services.AddSingleton<IContentJsonConverterProvider, ContentJsonConverterProvider>();
        }
    }
}
