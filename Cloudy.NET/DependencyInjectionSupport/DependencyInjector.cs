using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.NET.DependencyInjectionSupport
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IDependencyInjectorCreator, DependencyInjectorCreator>();
            services.AddSingleton<IDependencyInjectorProvider, DependencyInjectorProvider>();
            services.AddScoped<IInstantiator, Instantiator>();
        }
    }
}
