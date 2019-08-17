using Cloudy.CMS.DocumentSupport.FileSupport;
using Cloudy.CMS.DocumentSupport.MongoSupport;
using Cloudy.CMS.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Cloudy.CMS
{
    public class CloudyConfigurator
    {
        public IServiceCollection Services { get; }
        CloudyOptions Options { get; }

        public CloudyConfigurator(IServiceCollection services, CloudyOptions options)
        {
            Services = services;
            Options = options;
        }

        public CloudyConfigurator WithFileBasedDocuments()
        {
            this.AddFileBased();

            return this;
        }

        public CloudyConfigurator AddContentRoute()
        {
            Services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("contentroute", typeof(ContentRouteConstraint));
            });

            return this;
        }

        public CloudyConfigurator WithMongoDatabaseConnectionStringNamed(string name)
        {
            if (name.Contains(":") || name.Contains("/"))
            {
                throw new ArgumentException("Connection strings have to be referenced by name from your appsettings.json. No direct URLs here. You'll thank me later!");
            }

            this.AddMongo();
            Options.DatabaseConnectionString = name;

            return this;
        }

        public CloudyConfigurator AddComponent<T>() where T : class
        {
            Options.Components.Add(typeof(T));

            return this;
        }
    }
}