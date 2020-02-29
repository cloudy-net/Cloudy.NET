using Cloudy.CMS.ComponentSupport;
using Cloudy.CMS.DocumentSupport.MongoSupport;
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

namespace Cloudy.CMS
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

            var container = new Container(services);

            container.RegisterSingleton<IComponentAssemblyProvider>(new ComponentAssemblyProvider(options.ComponentAssemblies));
            container.RegisterSingleton<IComponentTypeProvider>(new ComponentTypeProvider(options.Components));

            new CloudyDependencyInjector().InjectDependencies(container);

            foreach (var injector in container.CreateResolver().Resolve<IDependencyInjectorProvider>().GetAll())
            {
                injector.InjectDependencies(container);
            }

            if (options.DatabaseConnectionString != null)
            {
                container.RegisterSingleton<IDatabaseConnectionStringNameProvider>(new DatabaseConnectionStringNameProvider(options.DatabaseConnectionString));
            }

            foreach (var initializer in services.BuildServiceProvider().GetRequiredService<IInitializerProvider>().GetAll())
            {
                initializer.Initialize();
            }
        }
    }
}
