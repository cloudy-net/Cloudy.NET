using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Cloudy.CMS.DocumentSupport.CacheSupport
{
    public static class StartupExtensions
    {
        public static CloudyConfigurator AddCachedDocuments(this CloudyConfigurator instance)
        {
            instance.Services.AddSingleton<IDocumentGetter, CachedDocumentRepository>();
            instance.Services.AddSingleton<IDocumentCreator, CachedDocumentRepository>();
            instance.Services.AddSingleton<IDocumentUpdater, CachedDocumentRepository>();
            instance.Services.AddSingleton<IDocumentDeleter, CachedDocumentRepository>();
            instance.Services.AddSingleton<IDocumentFinder, CachedDocumentRepository>();
            instance.Services.AddSingleton<IDocumentLister, CachedDocumentRepository>();
            instance.Services.AddTransient<IDocumentFinderQueryBuilder, DocumentFinderQueryBuilder>();
            instance.Services.AddSingleton<IDocumentPropertyPathProvider, DocumentPropertyPathProvider>();
            instance.Services.AddSingleton<IDocumentPropertyFinder, DocumentPropertyFinder>();

            return instance;
        }
    }
}
