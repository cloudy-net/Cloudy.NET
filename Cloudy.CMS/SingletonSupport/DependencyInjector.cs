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
            services.AddScoped<ISingletonCreator, SingletonCreator>();
            services.AddScoped<ISingletonGetter, SingletonGetter>();
            services.AddScoped<ISingletonProvider, SingletonProvider>();

            var types = ContextDescriptorProvider.GetAll().SelectMany(c => c.DbSets.Select(p => p.Type)).ToList();

            foreach (var type in AssemblyProvider.Assemblies.SelectMany(a => a.Types))
            {
                var singletonAttribute = type.GetCustomAttribute<SingletonAttribute>();

                if (singletonAttribute == null)
                {
                    continue;
                }

                services.AddTransient(type, serviceProvider => serviceProvider.GetService<ISingletonGetter>().GetAsync(type).Result);
            }
        }
    }
}
