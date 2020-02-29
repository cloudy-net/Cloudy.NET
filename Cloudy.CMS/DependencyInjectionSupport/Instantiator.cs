using Microsoft.Extensions.DependencyInjection;
using Cloudy.CMS.DependencyInjectionSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.AspNetCore.DependencyInjectionSupport
{
    public class Instantiator : IInstantiator
    {
        IServiceProvider ServiceProvider { get; }

        public Instantiator(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public object Instantiate(Type type)
        {
            return ActivatorUtilities.CreateInstance(ServiceProvider, type);
        }
    }
}
