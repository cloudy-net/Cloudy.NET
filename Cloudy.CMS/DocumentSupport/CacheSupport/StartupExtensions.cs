using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Cloudy.CMS.DocumentSupport.CacheSupport
{
    public static class StartupExtensions
    {
        /// <summary>
        /// Adds document repository support with a 2nd level inmemory cache and the specified data source. All documents will be fetched on startup.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cloudy"></param>
        /// <returns></returns>
        public static CloudyConfigurator AddCachedDocuments<T>(this CloudyConfigurator cloudy) where T : class, IDataSource
        {
            cloudy.Services.AddSingleton<IDocumentGetter, CachedDocumentRepository>();
            cloudy.Services.AddSingleton<IDocumentCreator, CachedDocumentRepository>();
            cloudy.Services.AddSingleton<IDocumentUpdater, CachedDocumentRepository>();
            cloudy.Services.AddSingleton<IDocumentDeleter, CachedDocumentRepository>();
            cloudy.Services.AddSingleton<IDocumentFinder, CachedDocumentRepository>();
            cloudy.Services.AddSingleton<IDocumentLister, CachedDocumentRepository>();
            cloudy.Services.AddTransient<IDocumentFinderQueryBuilder, DocumentFinderQueryBuilder>();
            cloudy.Services.AddSingleton<IDocumentPropertyPathProvider, DocumentPropertyPathProvider>();
            cloudy.Services.AddSingleton<IDocumentPropertyFinder, DocumentPropertyFinder>();
            cloudy.Services.AddSingleton<IDataSource, T>();

            return cloudy;
        }
    }
}
