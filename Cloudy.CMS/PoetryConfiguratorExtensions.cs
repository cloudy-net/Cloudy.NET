using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Poetry.InitializerSupport;
using Poetry.DependencyInjectionSupport;
using Poetry.AspNetCore.DependencyInjectionSupport;
using System;

namespace Poetry.AspNetCore
{
    public static class PoetryConfiguratorExtensions
    {
        static bool HasInitialized { get; set; }

        /// <summary>
        /// Adds Poetry to your application.
        /// </summary>
        public static void AddPoetry(this IServiceCollection services, Action<PoetryConfigurator> configuratorFunction)
        {
            if (HasInitialized)
            {
                throw new AlreadyInitializedException();
            }

            HasInitialized = true;

            var container = new Container(services);

            container.RegisterSingleton<IInstantiator, Instantiator>();

            var configurator = new PoetryConfigurator(container);

            configuratorFunction(configurator);

            configurator.Done();
        }

        public static void UsePoetry(this IApplicationBuilder app)
        {
            foreach (var initializer in app.ApplicationServices.GetService<IInitializerProvider>().GetAll())
            {
                initializer.Initialize();
            }
        }
    }
}
