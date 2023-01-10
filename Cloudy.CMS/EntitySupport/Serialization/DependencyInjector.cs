using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.DependencyInjectionSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.EntitySupport.Serialization
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IContentJsonConverterTypeProvider, EmbeddedBlockJsonConverterTypeProvider>();
            services.AddSingleton<IEmbeddedBlockJsonConverterCreator, EmbeddedBlockJsonConverterCreator>();
            services.AddSingleton<IEmbeddedBlockJsonConverterProvider, EmbeddedBlockJsonConverterProvider>();
        }
    }
}
