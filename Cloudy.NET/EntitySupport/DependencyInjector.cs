using Cloudy.NET.DependencyInjectionSupport;
using Cloudy.NET.EntitySupport.PrimaryKey;
using Cloudy.NET.EntitySupport.Reference;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.NET.EntitySupport
{
    public class DependencyInjector : IDependencyInjector
    {
        public void InjectDependencies(IServiceCollection services)
        {
            services.AddScoped<IReferenceDeserializer, ReferenceDeserializer>();
            services.AddScoped<IReferenceSerializer, ReferenceSerializer>();

            services.AddScoped<IPrimaryKeyConverter, PrimaryKeyConverter>();
            services.AddScoped<IPrimaryKeyPropertyGetter, PrimaryKeyPropertyGetter>();
            services.AddScoped<IPrimaryKeyGetter, PrimaryKeyGetter>();
        }
    }
}
