using Cloudy.CMS.DependencyInjectionSupport;
using Cloudy.CMS.PropertyDefinitionSupport.PropertyMappingSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.PropertyDefinitionSupport
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IPropertyDefinitionCreator, PropertyDefinitionCreator>();
            services.AddSingleton<IPropertyMappingCreator, PropertyMappingCreator>();
            services.AddSingleton<IPropertyMappingProvider, PropertyMappingProvider>();

            services.AddSingleton<IPropertyDefinitionProvider, PropertyDefinitionProvider>();
        }
    }
}
