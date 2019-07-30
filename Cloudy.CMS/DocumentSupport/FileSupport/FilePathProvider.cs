using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Cloudy.CMS.DocumentSupport.FileSupport
{
    public class FilePathProvider : IFilePathProvider
    {
        IHostingEnvironment HostingEnvironment { get; }
        IFileBasedDocumentOptions FileBasedDocumentOptions { get; }

        public FilePathProvider(IHostingEnvironment hostingEnvironment, IFileBasedDocumentOptions fileBasedDocumentOptions)
        {
            HostingEnvironment = hostingEnvironment;
            FileBasedDocumentOptions = fileBasedDocumentOptions;
        }

        public string GetPathFor(string container)
        {
            return Path.Combine(HostingEnvironment.ContentRootPath, FileBasedDocumentOptions.Path, container);
        }

        public string GetPathFor(string container, string id)
        {
            return Path.Combine(HostingEnvironment.ContentRootPath, FileBasedDocumentOptions.Path, container, $"{id}.json");
        }
    }
}