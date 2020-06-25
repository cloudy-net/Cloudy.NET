using Cloudy.CMS.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Cloudy.CMS;
using Cloudy.CMS.AspNetCore.DependencyInjectionSupport;
using Cloudy.CMS.DependencyInjectionSupport;
using Cloudy.CMS.InitializerSupport;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace Microsoft.AspNetCore.Builder
{
    public static class StartupExtensions
    {
        public static void AddCloudy(this IServiceCollection services)
        {
            var assembly = Assembly.GetCallingAssembly();
            AddCloudy(services, cloudy => cloudy.AddComponentAssembly(assembly));
        }

        public static void AddCloudy(this IServiceCollection services, Action<CloudyConfigurator> cloudy)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("contentroute", typeof(ContentRouteConstraint));
            });

            var options = new CloudyOptions();
            var configurator = new CloudyConfigurator(services, options);

            cloudy(configurator);

            configurator.AddComponent<CloudyAssemblyHandle>();

            if (Assembly.GetCallingAssembly() != Assembly.GetExecutingAssembly())
            {
                configurator.AddComponentAssembly(Assembly.GetCallingAssembly());
            }

            var AssemblyProvider = new AssemblyProvider(options.Assemblies.Select(a => new AssemblyWrapper(a)));
            services.AddSingleton<IAssemblyProvider>(AssemblyProvider);

            foreach (var injector in new DependencyInjectorProvider(new DependencyInjectorCreator(AssemblyProvider)).GetAll())
            {
                injector.InjectDependencies(services);
            }

            services.AddTransient<IStartupFilter, InitializerBootstrapper>();
        }
    }
}
