using Cloudy.CMS.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Cloudy.CMS
{
    public class CloudyConfigurator
    {
        public IServiceCollection Services { get; }
        public CloudyOptions Options { get; }

        public CloudyConfigurator(IServiceCollection services, CloudyOptions options)
        {
            Services = services;
            Options = options;
        }

        public CloudyConfigurator AddComponent<T>() where T : class
        {
            Options.Assemblies.Add(typeof(T).Assembly);

            return this;
        }

        public CloudyConfigurator AddComponentAssembly(Assembly assembly)
        {
            Options.Assemblies.Add(assembly);

            return this;
        }

        public CloudyConfigurator AddContext<T>() where T : class
        {
            Options.Contexts[typeof(T)] = typeof(T).GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

            return this;
        }
    }
}