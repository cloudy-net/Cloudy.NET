using Cloudy.CMS.DocumentSupport.FileSupport;
using Cloudy.CMS.DocumentSupport.InMemorySupport;
using Cloudy.CMS.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
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

        public CloudyConfigurator WithFileBasedDocuments()
        {
            this.AddFileBased();
            Options.HasDocumentProvider = true;

            return this;
        }

        public CloudyConfigurator WithFileBasedDocuments(string jsonPath)
        {
            this.AddFileBased(jsonPath);
            Options.HasDocumentProvider = true;

            return this;
        }

        public CloudyConfigurator WithInMemoryDatabase()
        {
            this.AddInMemory();
            Options.HasDocumentProvider = true;

            return this;
        }

        public CloudyConfigurator AddComponent<T>() where T : class
        {
            Options.Components.Add(typeof(T));

            return this;
        }

        public CloudyConfigurator AddComponentAssembly(Assembly assembly)
        {
            Options.ComponentAssemblies.Add(assembly);

            return this;
        }
    }
}