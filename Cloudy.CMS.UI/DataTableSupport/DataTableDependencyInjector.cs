using Cloudy.CMS.UI.DataTableSupport.BackendSupport;
using Cloudy.CMS.DependencyInjectionSupport;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Cloudy.CMS.UI.DataTableSupport
{
    public class DataTableDependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IBackendCreator, BackendCreator>();
            services.AddSingleton<IBackendProvider, BackendProvider>();
        }
    }
}
