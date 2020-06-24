using Cloudy.CMS.DependencyInjectionSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.SingletonSupport
{
    public class DependencyInjector : IDependencyInjector
    {
        IAssemblyProvider AssemblyProvider { get; }

        public DependencyInjector(IAssemblyProvider assemblyProvider)
        {
            AssemblyProvider = assemblyProvider;
        }

        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<ISingletonCreator, SingletonCreator>();
            services.AddSingleton<ISingletonGetter, SingletonGetter>();
            services.AddSingleton<ISingletonProvider, SingletonProvider>();
        }
    }
}
