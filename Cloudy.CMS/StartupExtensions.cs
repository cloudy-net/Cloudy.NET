using Cloudy.CMS.DocumentSupport;
using Microsoft.Extensions.DependencyInjection;
using Poetry;
using Poetry.AspNetCore.DependencyInjectionSupport;
using Poetry.ComponentSupport;
using Poetry.DependencyInjectionSupport;
using Poetry.InitializerSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS
{
    public static class StartupExtensions
    {
        public static void AddCloudy(this IServiceCollection services, Action<CloudyConfigurator> configure)
        {
            var options = new CloudyOptions();

            options.Components.Add(typeof(CloudyComponent));

            configure(new CloudyConfigurator(options));

            var container = new Container(services);

            container.RegisterSingleton<IComponentTypeProvider>(new ComponentTypeProvider(options.Components));

            new PoetryDependencyInjector().InjectDependencies(container);

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
