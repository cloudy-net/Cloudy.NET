using Cloudy.CMS.DocumentSupport.CacheSupport;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.DocumentSupport.FileSupport
{
    public class FileDataSource : IDataSource
    {
        IFilePathProvider FilePathProvider { get; }
        IFileHandler FileHandler { get; }

        public FileDataSource(IFilePathProvider filePathProvider, IFileHandler fileHandler)
        {
            FilePathProvider = filePathProvider;
            FileHandler = fileHandler;
        }

        public Task CreateDocumentAsync(string container, Document document)
        {
            var path = FilePathProvider.GetPathFor(container, document.Id);

            if (FileHandler.Exists(path))
            {
                throw new DocumentAlreadyExistsException(container, document.Id);
            }

            FileHandler.Create(path, JsonConvert.SerializeObject(document, Formatting.Indented));

            return Task.CompletedTask;
        }

        public Task DeleteAsync(string container, string id)
        {
            var path = FilePathProvider.GetPathFor(container, id);

            if (!FileHandler.Exists(path))
            {
                return Task.CompletedTask;
            }

            FileHandler.Delete(path);

            return Task.CompletedTask;
        }

        public Task<Document> GetAsync(string container, string id)
        {
            var path = FilePathProvider.GetPathFor(container, id);

            if (!FileHandler.Exists(path))
            {
                return Task.FromResult<Document>(null);
            }

            string contents = FileHandler.Get(path);

            return Task.FromResult(JsonConvert.DeserializeObject<Document>(contents));
        }

        public Task UpdateAsync(string container, string id, Document document)
        {
            var path = FilePathProvider.GetPathFor(container, id);

            if (!FileHandler.Exists(path))
            {
                throw new DocumentDoesNotExistException(container, id);
            }

            FileHandler.Update(path, JsonConvert.SerializeObject(document, Formatting.Indented));

            return Task.CompletedTask;
        }

        public Task<IEnumerable<Document>> ListAsync(string container)
        {
            var path = FilePathProvider.GetPathFor(container);

            if (!FileHandler.Exists(path))
            {
                return Task.FromResult(Enumerable.Empty<Document>());
            }

            var result = new List<Document>();

            foreach(var file in FileHandler.List(path))
            {
                result.Add(JsonConvert.DeserializeObject<Document>(file));
            }

            return Task.FromResult((IEnumerable<Document>)result.AsReadOnly());
        }
    }
}
