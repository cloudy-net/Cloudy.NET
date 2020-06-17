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
            instance.Services.AddSingleton<IDocumentGetter, DocumentRepository>();
            instance.Services.AddSingleton<IDocumentCreator, DocumentRepository>();
            instance.Services.AddSingleton<IDocumentUpdater, DocumentRepository>();
            instance.Services.AddSingleton<IDocumentDeleter, DocumentRepository>();
            instance.Services.AddSingleton<IDocumentFinder, DocumentRepository>();
            instance.Services.AddSingleton<IDocumentLister, DocumentRepository>();
            instance.Services.AddSingleton<IDocumentFinderQueryBuilder, DocumentFinderQueryBuilder>();
            instance.Services.AddSingleton<IDocumentPropertyFinder, DocumentPropertyFinder>();
            return instance;
        }
    }
}
