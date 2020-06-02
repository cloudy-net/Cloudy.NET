using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.DocumentSupport.MongoSupport
{
    public static class StartupExtensions
    {
        public static CloudyConfigurator AddMongo(this CloudyConfigurator configurator)
        {
            configurator.Services.AddSingleton<IContainerProvider, ContainerProvider>();
            configurator.Services.AddSingleton<IDatabaseProvider, DatabaseProvider>();
            configurator.Services.AddSingleton<IDatabaseConnectionStringNameProvider, NoDatabaseConnectionStringNameProvider>();
            configurator.Services.AddSingleton<IDocumentCreator, DocumentCreator>();
            configurator.Services.AddSingleton<IDocumentDeleter, DocumentDeleter>();
            configurator.Services.AddSingleton<IDocumentFinder, DocumentFinder>();
            configurator.Services.AddSingleton<IDocumentGetter, DocumentGetter>();
            configurator.Services.AddSingleton<IDocumentUpdater, DocumentUpdater>();
            configurator.Services.AddTransient<IDocumentFinderQueryBuilder, DocumentFinderQueryBuilder>();

            return configurator;
        }
    }
}
