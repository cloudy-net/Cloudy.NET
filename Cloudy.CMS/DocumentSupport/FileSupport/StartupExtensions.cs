using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.DocumentSupport.FileSupport
{
    public static class StartupExtensions
    {
        public static CloudyConfigurator AddFileBased(this CloudyConfigurator configurator)
        {
            configurator.Services.AddSingleton<IFileBasedDocumentOptions>(new FileBasedDocumentOptions("json"));
            configurator.Services.AddSingleton<IFilePathProvider, FilePathProvider>();
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
