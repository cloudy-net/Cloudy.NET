using Cloudy.CMS.DependencyInjectionSupport;
using Cloudy.CMS.UI.FormSupport.FieldSupport;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Cloudy.CMS.UI.FormSupport
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddScoped<IFieldCreator, FieldCreator>();
            services.AddScoped<IFieldProvider, FieldProvider>();
        }
    }
}
