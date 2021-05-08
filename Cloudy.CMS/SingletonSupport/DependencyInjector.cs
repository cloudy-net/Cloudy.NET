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

            foreach (var type in AssemblyProvider.GetAll().SelectMany(a => a.Types))
            {
                var contentTypeAttribute = type.GetCustomAttribute<ContentTypeAttribute>();

                if (contentTypeAttribute == null)
                {
                    continue;
                }

                var singletonAttribute = type.GetCustomAttribute<SingletonAttribute>();

                if (singletonAttribute == null)
                {
                    continue;
                }

                services.AddTransient(type, serviceProvider => serviceProvider.GetService<ISingletonGetter>().GetAsync(contentTypeAttribute.Id).Result);
            }
        }
    }
}
