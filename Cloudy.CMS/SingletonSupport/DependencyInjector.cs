using Cloudy.CMS.AssemblySupport;
using Cloudy.CMS.ContextSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.DependencyInjectionSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.SingletonSupport
{
    public record DependencyInjector(IAssemblyProvider AssemblyProvider, IContextDescriptorProvider ContextDescriptorProvider) : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddScoped<ISingletonGetter, SingletonGetter>();
        }
    }
}
