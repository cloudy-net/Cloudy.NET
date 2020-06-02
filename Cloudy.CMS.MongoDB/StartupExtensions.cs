using Cloudy.CMS.DocumentSupport.MongoSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.MongoDB
{
    public static class StartupExtensions
    {
        public static CloudyConfigurator WithMongoDatabaseConnectionStringNamed(this CloudyConfigurator configurator, string name)
        {
            if (name.Contains(":") || name.Contains("/"))
            {
                throw new ArgumentException("Connection strings have to be referenced by name from your appsettings.json. No direct URLs here. You'll thank me later!");
            }

            configurator.AddMongo();
            configurator.Options.HasDocumentProvider = true;
            configurator.Services.AddSingleton<IDatabaseConnectionStringNameProvider>(new DatabaseConnectionStringNameProvider(name));

            return configurator;
        }
    }
}
