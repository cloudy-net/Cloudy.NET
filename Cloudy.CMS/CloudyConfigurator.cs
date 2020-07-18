using Cloudy.CMS.DocumentSupport.FileSupport;
using Cloudy.CMS.DocumentSupport.InMemorySupport;
using Cloudy.CMS.LanguageSupport;
using Cloudy.CMS.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
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

        public CloudyConfigurator AddLanguage(string code)
        {
            Options.Languages.Add(new LanguageDescriptor(code, new CultureInfo(code).EnglishName));

            return this;
        }

        public CloudyConfigurator AddLanguage(string code, string name)
        {
            Options.Languages.Add(new LanguageDescriptor(code, name));

            return this;
        }
    }
}