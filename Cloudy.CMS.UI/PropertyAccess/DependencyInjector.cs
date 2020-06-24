using Cloudy.CMS.DependencyInjectionSupport;
using Cloudy.CMS.PropertyAccess;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.PropertyAccess
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IPropertyGetter, PropertyGetter>();
            services.AddSingleton<IPropertySetter, PropertySetter>();
        }
    }
}
