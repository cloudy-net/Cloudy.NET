using Cloudy.CMS.DependencyInjectionSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.ListenerSupport
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddScoped<ISaveListenerCreator, SaveListenerCreator>();
            services.AddScoped<ISaveListenerProvider, SaveListenerProvider>();
        }
    }
}
