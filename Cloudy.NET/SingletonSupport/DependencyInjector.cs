using Cloudy.NET.AssemblySupport;
using Cloudy.NET.ContextSupport;
using Cloudy.NET.EntityTypeSupport;
using Cloudy.NET.DependencyInjectionSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.NET.SingletonSupport
{
    public record DependencyInjector(IAssemblyProvider AssemblyProvider, IContextDescriptorProvider ContextDescriptorProvider) : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddScoped<ISingletonGetter, SingletonGetter>();
        }
    }
}
