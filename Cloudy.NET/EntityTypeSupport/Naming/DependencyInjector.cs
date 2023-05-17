using Cloudy.NET.DependencyInjectionSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.NET.EntityTypeSupport.Naming
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddSingleton<IEntityTypeNameProvider, EntityTypeNameProvider>();
            services.AddSingleton<IEntityTypeNameCreator, EntityTypeNameCreator>();
        }
    }
}
