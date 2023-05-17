using Cloudy.NET.DependencyInjectionSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.NET.EntitySupport.Serialization
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
