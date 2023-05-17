using Cloudy.NET.DependencyInjectionSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.NET.EntityTypeSupport
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IEntityTypeCreator, EntityTypeCreator>();
            services.AddSingleton<IEntityTypeProvider, EntityTypeProvider>();
        }
    }
}
