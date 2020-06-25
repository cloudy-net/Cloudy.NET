using Cloudy.CMS.DocumentSupport.CacheSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.DocumentSupport.FileSupport
{
    public static class StartupExtensions
    {
        public static CloudyConfigurator AddFileBasedDocuments(this CloudyConfigurator cloudy, string path = "json")
        {
            cloudy.AddCachedDocuments<FileDataSource>();
            cloudy.Services.AddSingleton<IFileHandler, FileHandler>();
            cloudy.Services.AddSingleton<IFilePathProvider, FilePathProvider>();
            cloudy.Services.AddSingleton<IFileBasedDocumentOptions>(new FileBasedDocumentOptions(path));
            return cloudy;
        }
    }
}
