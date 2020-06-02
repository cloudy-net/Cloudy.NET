using Cloudy.CMS.ComponentSupport;
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

namespace Microsoft.AspNetCore.Builder
{
    public static class StartupExtensions
    {
        public static void AddCloudy(this IServiceCollection services)
        {
            var assembly = Assembly.GetCallingAssembly();
            AddCloudy(services, cloudy => cloudy.AddComponentAssembly(assembly));
        }
        public static void AddCloudy(this IServiceCollection services, Action<CloudyConfigurator> configure)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("contentroute", typeof(ContentRouteConstraint));
            });

            var options = new CloudyOptions();
            var configurator = new CloudyConfigurator(services, options);

            configure(configurator);

            configurator.AddComponent<CloudyComponent>();

            if (Assembly.GetCallingAssembly() != Assembly.GetExecutingAssembly())
            {
                configurator.AddComponentAssembly(Assembly.GetCallingAssembly());
            }

            if (!options.HasDocumentProvider)
            {
                configurator.WithInMemoryDatabase();
            }

            var componentAssemblyProvider = new ComponentAssemblyProvider(options.ComponentAssemblies);
            services.AddSingleton<IComponentAssemblyProvider>(componentAssemblyProvider);

            var componentTypeProvider = new ComponentTypeProvider(options.Components);
            services.AddSingleton<IComponentTypeProvider>(componentTypeProvider);

            new CloudyDependencyInjector().InjectDependencies(services);

            foreach (var injector in new DependencyInjectorProvider(new DependencyInjectorCreator(componentAssemblyProvider, componentTypeProvider)).GetAll())
            {
                injector.InjectDependencies(services);
            }

            services.AddTransient<IStartupFilter, InitializerBootstrapper>();
        }
    }
}
