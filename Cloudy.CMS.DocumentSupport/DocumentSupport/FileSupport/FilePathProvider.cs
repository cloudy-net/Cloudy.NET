using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Cloudy.CMS.DocumentSupport.FileSupport
{
    public class FilePathProvider : IFilePathProvider
    {
        IWebHostEnvironment WebHostEnvironment { get; }
        IFileBasedDocumentOptions FileBasedDocumentOptions { get; }

        public FilePathProvider(IWebHostEnvironment webHostEnvironment, IFileBasedDocumentOptions fileBasedDocumentOptions)
        {
            WebHostEnvironment = webHostEnvironment;
            FileBasedDocumentOptions = fileBasedDocumentOptions;
        }

        public string GetPathFor(string container)
        {
            return Path.Combine(WebHostEnvironment.ContentRootPath, FileBasedDocumentOptions.Path, container);
        }

        public string GetPathFor(string container, string id)
        {
            return Path.Combine(WebHostEnvironment.ContentRootPath, FileBasedDocumentOptions.Path, container, $"{id}.json");
        }
    }
}