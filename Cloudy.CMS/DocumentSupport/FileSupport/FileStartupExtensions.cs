using Cloudy.CMS;
using Cloudy.CMS.DocumentSupport.CacheSupport;
using Cloudy.CMS.DocumentSupport.FileSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Builder
{
    public static class FileStartupExtensions
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
