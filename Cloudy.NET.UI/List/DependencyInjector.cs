using Cloudy.NET.DependencyInjectionSupport;
using Cloudy.NET.UI.List.Filter;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.NET.UI.List
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddScoped<IListColumnProvider, ListColumnProvider>();
            services.AddScoped<IListColumnCreator, ListColumnCreator>();
            services.AddScoped<IListFilterProvider, ListFilterProvider>();
            services.AddScoped<IListFilterCreator, ListFilterCreator>();
        }
    }
}
