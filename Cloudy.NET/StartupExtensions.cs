﻿using Cloudy.NET.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Cloudy.NET;
using Cloudy.NET.DependencyInjectionSupport;
using Cloudy.NET.InitializerSupport;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using Cloudy.NET.ContextSupport;
using Cloudy.NET.AssemblySupport;
using Cloudy.NET.Licensing;
using Microsoft.AspNetCore.Http;

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

            var assemblyProvider = new AssemblyProvider(options.Assemblies.Select(a => new AssemblyWrapper(a)));
            var contextDescriptorProvider = new ContextDescriptorProvider(new ContextDescriptorCreator().Create(options.ContextTypes));

            services.AddHttpContextAccessor();
            services.AddSingleton<IAssemblyProvider>(assemblyProvider);
            services.AddSingleton<IContextDescriptorProvider>(contextDescriptorProvider);
            services.AddHttpClient<ILicenseValidator, LicenseValidator>(c => c.BaseAddress = new Uri("https://cloudy-licensing.azurewebsites.net/api"));
            services.AddSingleton<ILicenseProvider>(sp => new LicenseProvider(sp.GetService<ILicenseValidator>(), options.LicenseKey));
            
            foreach (var injector in new DependencyInjectorProvider(new DependencyInjectorCreator(assemblyProvider, contextDescriptorProvider)).GetAll())
            {
                injector.InjectDependencies(services);
            }

            services.AddTransient<IStartupFilter, InitializerBootstrapper>();
        }
    }
}
