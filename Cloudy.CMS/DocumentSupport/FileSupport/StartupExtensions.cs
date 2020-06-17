using Cloudy.CMS.DocumentSupport.CacheSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.DocumentSupport.FileSupport
{
    public static class StartupExtensions
    {
        public static CloudyConfigurator AddFileBased(this CloudyConfigurator configurator, string path = "json")
        {
            configurator.AddCachedDocuments();
            configurator.Services.AddSingleton<IDataSource, FileDataSource>();
            configurator.Services.AddSingleton<IFileHandler, FileHandler>();
            configurator.Services.AddSingleton<IFilePathProvider, FilePathProvider>();
            configurator.Services.AddSingleton<IFileBasedDocumentOptions>(new FileBasedDocumentOptions(path));

            return configurator;
        }
    }
}
