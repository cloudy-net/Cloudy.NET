using Cloudy.CMS.DependencyInjectionSupport;
using Cloudy.CMS.EntitySupport.PrimaryKey;
using Cloudy.CMS.EntitySupport.Reference;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.EntitySupport
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
