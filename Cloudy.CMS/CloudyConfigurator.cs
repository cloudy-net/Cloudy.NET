using Cloudy.CMS.LanguageSupport;
using Cloudy.CMS.Routing;
using Microsoft.AspNetCore.Routing;
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

        public CloudyConfigurator AddLanguage(string id)
        {
            AddLanguage(id, new CultureInfo(id).EnglishName);

            return this;
        }

        public CloudyConfigurator AddLanguage(string id, string name)
        {
            if (Options.Languages.Any(l => l.Id == id))
            {
                throw new Exception($"There is already a language added with the id {id}");
            }

            if (Options.Languages.Any(l => l.Name == name))
            {
                throw new Exception($"There is already a language added with the name {name}");
            }

            Options.Languages.Add(new LanguageDescriptor(id, name));

            return this;
        }
    }
}